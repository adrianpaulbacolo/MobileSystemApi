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


public partial class Deposit_BofoPay : PaymentBasePage
{
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;
    protected string strRedirectUrl = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.BofoPay);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.BofoPay);

        base.CheckLogin();
        base.InitialiseVariables();

        base.InitialisePaymentLimits();

        base.GetMainWalletBalance("0");

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CancelUnexpectedRePost();

        HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");
        commonPaymentMethodFunc.GetDepositMethodList(strMethodsUnAvailable, depositTabs, base.PageName, sender.ToString().Contains("app"));

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

        btnSubmit.Text = base.strbtnSubmit;

        txtDepositAmount.Attributes.Add("PLACEHOLDER", base.strtxtAmount);

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
                    xeResponse = client.createOnlineDepositTransactionV3(Convert.ToInt64(strOperatorId), strMemberCode, Convert.ToInt64(base.PaymentMethodId), strMerchantId, strCurrencyCode, decDepositAmount, svcPayDeposit.DepositSource.Mobile, string.Empty, string.Empty);

                    if (xeResponse == null)
                    {
                        status = base.GetErrors("/TransferFail");
                    }
                    else
                    {
                        bool isSuccess = Convert.ToBoolean(commonCulture.ElementValues.getResourceString("result", xeResponse));
                        string strTransferId = commonCulture.ElementValues.getResourceString("invId", xeResponse);

                        if (isSuccess)
                        {
                            status.AlertCode = "0";
                            status.AlertMessage = string.Format("{0}\\n{1}: {2}", commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferSuccess", xeErrors), strlblTransactionId, strTransferId);

                            //litForm.Text = CallVendor(strTransferId, decDepositAmount);

                            strRedirectUrl = CallVendor(strTransferId, decDepositAmount);
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
        commonAuditTrail.appendLog("system", base.PageName, "InitiateDeposit", string.Empty, strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
    }


    private string CallVendor(string strTransferId, decimal decDepositAmount)
    {
        string[] infoArray = base.GetPaymentGatewayMerchantSetting(commonVariables.DepositMethod.BofoPay).Split('|');

        string payID = string.Empty; //will direct to Bofo payment page to make bank selection
        string tradeDate = DateTime.Now.ToString("yyyyMMddHHmmss");
        string orderMoney = (decDepositAmount * 100).ToString();
        string productName = string.Empty;
        string amount = "1"; //quantity
        string userName = string.Empty;
        string additionalInfo = string.Empty;
        string pageUrl = string.Empty;
        string returnUrl = ConfigurationManager.AppSettings["BofoPay_serverreturnurl"]; //server to receive response from Baofo
        string noticeType = "0"; //0 - no redirection to pageUrl after successful payment, 1 - redirect to pageUrl after payment


        string md5Key = infoArray[0];
        string terminalID = infoArray[1];
        string interfaceVersion = ConfigurationManager.AppSettings["Baofo_InterfaceVersion"];
        string keyType = "1"; //1 - MD5

        var builder = new StringBuilder();
        builder.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}", strMemberID, payID, tradeDate, strTransferId, orderMoney, pageUrl, returnUrl, noticeType, md5Key);

        string signInfo = commonEncryption.GetMd5Hash(builder.ToString()).ToUpper();

        string postUrl = ConfigurationManager.AppSettings["BofoPay_postUrl"];

        var request = (HttpWebRequest)WebRequest.Create(postUrl);
        string postData = "MemberID=" + strMerchantId;
        postData += ("&TerminalID=" + terminalID);
        postData += ("&InterfaceVersion=" + interfaceVersion);
        postData += ("&KeyType=" + keyType);
        postData += ("&PayID=" + payID);
        postData += ("&TradeDate=" + tradeDate);
        postData += ("&TransID=" + strTransferId);
        postData += ("&OrderMoney=" + orderMoney); //deposit amount
        postData += ("&ProductName=" + productName);
        postData += ("&Amount=" + amount); //quantity
        postData += ("&Username=" + userName);
        postData += ("&AdditionalInfo=" + additionalInfo);
        postData += ("&PageUrl=" + pageUrl);
        postData += ("&ReturnUrl=" + returnUrl);
        postData += ("&Signature=" + signInfo);
        postData += ("&NoticeType=" + noticeType);

        var data = Encoding.ASCII.GetBytes(postData);

        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }

        var response = (HttpWebResponse)request.GetResponse();

        return response.ResponseUri.ToString();

        //var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

        //return responseString.Replace("form ", @"form target=""_blank"" ").Replace("setTimeout('delayer()', 5000)", "setTimeout('delayer()', 1000)");
    }
}