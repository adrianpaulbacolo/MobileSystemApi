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

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = "EGHL";
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.EGHL);

        base.CheckLogin();
        base.InitialiseVariables();

        base.InitialisePaymentLimits();

        base.GetMainWalletBalance("0");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CancelUnexpectedRePost();

        HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");
        commonPaymentMethodFunc.getDepositMethodList(strMethodsUnAvailable, depositTabs, "eghl", sender.ToString().Contains("app"));

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
        lblDepositAmount.Text = base.strlblDepositAmount;

        btnSubmit.Text = base.strbtnSubmit;

        txtDepositAmount.Attributes.Add("PLACEHOLDER", base.strtxtDepositAmount);

        txtMinMaxLimit.Text = base.strtxtMinMaxLimit;
        txtDailyLimit.Text = base.strtxtDailyLimit;
        txtTotalAllowed.Text = base.strtxtTotalAllowed;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsPageRefresh)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        string strDepositAmount = txtDepositAmount.Text.Trim();

        decimal decDepositAmount = commonValidation.isDecimal(strDepositAmount) ? Convert.ToDecimal(strDepositAmount) : 0;
        decimal decMinLimit = Convert.ToDecimal(strMinLimit);
        decimal decMaxLimit = Convert.ToDecimal(strMaxLimit);

        CommonStatus status = new CommonStatus();

        if (!status.IsProcessAbort)
        {
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
                        xeResponse = client.createOnlineDepositTransactionV2(Convert.ToInt64(strOperatorId), strMemberCode, Convert.ToInt64(base.PaymentMethodId), strMerchantId, strCurrencyCode, decDepositAmount, string.Empty);

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

                                litForm.Text = GetForm(strTransferId, decDepositAmount.ToString("#.00"));
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
    }

    private string GetForm(string invId, string amount)
    {
        string postUrl = ConfigurationManager.AppSettings["EGHL_posturl"];
        string email = strMemberID + "@qq.com";

        var request = (HttpWebRequest)WebRequest.Create(postUrl);
        
        string postData = "merID=" + strMerchantId;
        postData += ("&e-inv=" + invId);
        postData += ("&amt=" + amount);
        postData += ("&p_Name=" + base.PageName);
        postData += ("&c_Email=" + email);

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