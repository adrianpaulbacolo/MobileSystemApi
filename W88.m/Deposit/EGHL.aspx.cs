using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;


public partial class Deposit_EGHL : PaymentBasePage
{
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;
    protected string strResponse = string.Empty;
    protected string strPageTitle = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.EGHL);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.EGHL);

        base.CheckLogin();
        base.InitialiseVariables();

        base.InitialisePaymentLimits();

        base.GetMainWalletBalance("0");

        drpBank.Items.AddRange(base.InitializeBank("EGHLBank").ToArray());
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        strPageTitle = strCurrencyCode.Equals("IDR", StringComparison.OrdinalIgnoreCase) ? "ATM Online" : commonCulture.ElementValues.getResourceString("dEGHL", commonVariables.PaymentMethodsXML);

        HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");
        commonPaymentMethodFunc.GetDepositMethodList(strMethodsUnAvailable, depositTabs, base.PageName, sender.ToString().Contains("app"), base.strCurrencyCode);

        if (!Page.IsPostBack)
        {
            this.InitializeLabels();
        }
    }

    private void InitializeLabels()
    {
        lblMode.Text = base.strlblMode;
        txtMode.Text = base.strtxtMode;
        lblMinMaxLimit.Text = base.strlblMinMaxLimit;
        lblDailyLimit.Text = base.strlblDailyLimit;
        lblTotalAllowed.Text = base.strlblTotalAllowed;
        lblDepositAmount.Text = base.strlblAmount;
        lblBank.Text = base.strlblBank;
        lblMessage.Text = commonCulture.ElementValues.getResourceString("bankNotice", xeResources);

        btnSubmit.Text = base.strbtnSubmit;

        txtDepositAmount.Attributes.Add("PLACEHOLDER", base.strtxtAmount);

        txtMinMaxLimit.Text = base.strtxtMinMaxLimit;
        txtDailyLimit.Text = base.strtxtDailyLimit;
        txtTotalAllowed.Text = base.strtxtTotalAllowed;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string strDepositAmount = txtDepositAmount.Text.Trim();
        string selectedBank = drpBank.SelectedItem.Value != "-1" ? drpBank.SelectedItem.Value : string.Empty;

        decimal decDepositAmount = commonValidation.isDecimal(strDepositAmount) ? Convert.ToDecimal(strDepositAmount) : 0;
        decimal decMinLimit = Convert.ToDecimal(strMinLimit);
        decimal decMaxLimit = Convert.ToDecimal(strMaxLimit);

        CommonStatus status = new CommonStatus();

        try
        {
            if (decDepositAmount == 0)
            {
                status = base.GetErrors("/MissingDepositAmount");
            }
            else if (decDepositAmount < decMinLimit)
            {
                status = base.GetErrors("/AmountMinLimit");
            }
            else if (decDepositAmount > decMaxLimit)
            {
                status = base.GetErrors("/AmountMaxLimit");
            }
            else if ((strTotalAllowed != strUnlimited) && (decDepositAmount > Convert.ToDecimal(strTotalAllowed)) && Convert.ToDecimal(strTotalAllowed) > 0)
            {
                status = base.GetErrors("/TotalAllowedExceeded");
            }

            if (!status.IsProcessAbort)
            {
                using (svcPayDeposit.DepositClient client = new svcPayDeposit.DepositClient())
                {
                    xeResponse = client.createOnlineDepositTransactionV3(Convert.ToInt64(strOperatorId), strMemberCode, Convert.ToInt64(base.PaymentMethodId), strMerchantId, strCurrencyCode, decDepositAmount, svcPayDeposit.DepositSource.Mobile, selectedBank, string.Empty);

                    if (xeResponse == null)
                    {
                        status = base.GetErrors("/TransferFail");
                    }
                    else
                    {
                        bool isTransactionSuccessful = Convert.ToBoolean(commonCulture.ElementValues.getResourceString("result", xeResponse));
                        string strTransferId = commonCulture.ElementValues.getResourceString("invId", xeResponse);

                        if (isTransactionSuccessful)
                        {
                            status.AlertCode = "0";
                            status.AlertMessage = string.Format("{0}\\n{1}: {2}", commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferSuccess", xeErrors), strlblTransactionId, strTransferId);

                            litForm.Text = GetForm(strTransferId, decDepositAmount);
                        }
                        else
                        {
                            status = GetErrors("/TransferFail", strTransferId, "/error");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            status = base.GetErrors("/Exception");

            strErrorDetail = ex.Message;
        }

        strAlertCode = status.AlertCode;
        strAlertMessage = status.AlertMessage;

        string strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | DepositAmount: {3} | MinLimit: {4} | MaxLimit: {5} | TotalAllowed: {6} | DailyLimit: {7} | Response: {8}",
           Convert.ToInt64(strOperatorId), strMemberCode, strCurrencyCode, strDepositAmount, decMinLimit, decMaxLimit, strTotalAllowed, strDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

        intProcessSerialId += 1;
        commonAuditTrail.appendLog("system", PageName, "InitiateDeposit", string.Empty, strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
    }

    private string GetForm(string invId, decimal amount)
    {
        string postUrl = ConfigurationManager.AppSettings["EGHL_posturl"];

        string amt = strCurrencyCode.Equals("IDR") ? (amount * 1000).ToString("#.00") : amount.ToString("#.00");
        //string email = (strCurrencyCode.Equals("IDR") ? "GVV" + invId.Substring(invId.Length - 4) : strMemberID) + "@qq.com";
        string email = (strCurrencyCode.Equals("IDR") ? "GVV" + base.strMemberCode : strMemberID) + "@qq.com";

        var requestUrl = HttpContext.Current.Request.Url;
        string callbackUrl = requestUrl.Scheme + "://" + requestUrl.Host + base.ThankYouPage;

        var request = (HttpWebRequest)WebRequest.Create(postUrl);

        string postData = "merID=" + strMerchantId;
        postData += ("&e-inv=" + invId);
        postData += ("&amt=" + amt);
        postData += ("&p_Name=" + base.PageName);
        postData += ("&c_Email=" + email);
        postData += ("&respURL=" + callbackUrl);

        var data = Encoding.ASCII.GetBytes(postData);

        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        var response = (HttpWebResponse)request.GetResponse();

        var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        return responseString.Replace("form name", @"form target=""_blank"" name").Replace("setTimeout('delayer()', 5000)", "setTimeout('delayer()', 1000)");
    }
}