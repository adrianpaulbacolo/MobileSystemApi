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
using svcPayDeposit;
using System.Web.Services;
using System.Xml.Linq;

public partial class Deposit_DaddyPay : PaymentBasePage
{
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected bool isDaddyPayQR = false;

    private static string weChatNickNameNotAvailable;
    private static string weChatNickNamePendingDeposit;
    private static string serverError;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = "DaddyPay";
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;

        if (Request.QueryString["value"].ToString() == "1")
        {
            base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.DaddyPay);
        }
        else if (Request.QueryString["value"].ToString() == "2")
        {
            isDaddyPayQR = true;
            base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.DaddyPayQR);
        }

        base.CheckLogin();
        base.InitialiseVariables();

        base.InitialisePaymentLimits();

        base.GetMainWalletBalance("0");

        string bank = isDaddyPayQR ? "DaddyPayQRBank" : "DaddyPayBank";
        drpBank.Items.AddRange(base.InitializeBank(bank).ToArray());

        this.InitializeWeChatDenominations();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CancelUnexpectedRePost();

        base.PageName = isDaddyPayQR ? base.PageName + "QR" : base.PageName;
        HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");
        commonPaymentMethodFunc.getDepositMethodList(strMethodsUnAvailable, depositTabs, base.PageName, sender.ToString().Contains("app"));

        if (!Page.IsPostBack)
        {
            lblMode.Text = commonCulture.ElementValues.getResourceString("lblMode", xeResources);
            txtMode.Text = string.Format(": {0}", commonCulture.ElementValues.getResourceString("txtMode", xeResources));
            lblMinMaxLimit.Text = commonCulture.ElementValues.getResourceString("lblMinMaxLimit", xeResources);
            lblDailyLimit.Text = commonCulture.ElementValues.getResourceString("lblDailyLimit", xeResources);
            lblTotalAllowed.Text = commonCulture.ElementValues.getResourceString("lblTotalAllowed", xeResources);
            lblDepositAmount.Text = commonCulture.ElementValues.getResourceString("lblDepositAmount", xeResources);

            lblAccountName.Text = commonCulture.ElementValues.getResourceString("lblAccountName", xeResources);
            lblAccountNumber.Text = commonCulture.ElementValues.getResourceString("lblAccountNumber", xeResources);

            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);

            txtDepositAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} ({1})", lblDepositAmount.Text, strCurrencyCode));

            txtMinMaxLimit.Text = string.Format(": {0} / {1}", strMinLimit, strMaxLimit);
            txtDailyLimit.Text = string.Format(": {0}", strDailyLimit);
            txtTotalAllowed.Text = string.Format(": {0}", strTotalAllowed);

            accountName.Visible = isDaddyPayQR;
            accountNo.Visible = isDaddyPayQR;

            weChatNickNameNotAvailable = commonCulture.ElementValues.getResourceString("wechatNickNameNA", xeResources);
            weChatNickNamePendingDeposit = commonCulture.ElementValues.getResourceString("wechatPendingDeposit", xeResources);
            serverError = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/error" + "1", xeErrors);
        }
    }

    private void InitializeWeChatDenominations()
    {
        List<ListItem> denoms = new List<ListItem>();
        XElement denom = xeResources.Element("denoms");

        denoms.AddRange(denom.Elements("denom").Select(x => new ListItem(x.Value, x.Attribute("id").Value)));

        drpDepositAmount.Items.AddRange(denoms.ToArray());
    }

    [WebMethod]
    public static string ProcessWeChatNickname(string action, string nickname)
    {
        XElement processResult;

        using (svcPayMember.MemberClient client = new svcPayMember.MemberClient())
        {
            processResult = client.processMemberWeChatNickName(Convert.ToInt64(commonVariables.OperatorId), commonVariables.GetSessionVariable("MemberCode"), action, nickname);
        }

        if (processResult.Element("error") != null)
        {
            string error = processResult.Element("error").Value;

            if (action == "changeNickname" && error == "NA")
            {
                return weChatNickNameNotAvailable;
            }

            if (action == "changeNickname" && error == "PendingDeposit")
            {
                return weChatNickNamePendingDeposit;
            }
        }

        if (processResult.Element("result") != null) //only for success cases will return with result element
        {
            return processResult.Element("result").Value;
        }
        else
        {
            return serverError;
        }
    }

    private void ValidateWeChatNickName(string strAccountName)
    {
        string strWCNickname = hfWCNickname.Value;

        if (!strAccountName.Equals(strWCNickname, StringComparison.OrdinalIgnoreCase))
        {
            string result = ProcessWeChatNickname("changeNickname", strAccountName);

            if (!result.Equals("allow", StringComparison.OrdinalIgnoreCase))
            {
                strAlertCode = "-1";
                strAlertMessage = result;
                isProcessAbort = true;
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsPageRefresh)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        string selectedBank = drpBank.SelectedValue;
        string strDepositAmount = selectedBank.Equals("40") ? drpDepositAmount.SelectedValue : txtDepositAmount.Text.Trim();
        string strAccountName = txtAccountName.Text.Trim();
        string strAccountNo = txtAccountNo.Text.Trim();

        decimal decDepositAmount = commonValidation.isDecimal(strDepositAmount) ? Convert.ToDecimal(strDepositAmount) : 0;
        decimal decMinLimit = Convert.ToDecimal(strMinLimit);
        decimal decMaxLimit = Convert.ToDecimal(strMaxLimit);

        if (!isProcessAbort)
        {
            try
            {
                ValidateDaddyPay(isDaddyPayQR, selectedBank, decDepositAmount, decMinLimit, decMaxLimit, strAccountName, strAccountNo);

                ValidateWeChatNickName(strAccountName);

                if (!isProcessAbort)
                {
                    InitiateDeposit(selectedBank, strDepositAmount);
                }

            }
            catch (Exception ex)
            {
                strAlertCode = "-1";
                strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/Exception", xeErrors);

                strErrorDetail = ex.Message;
            }

            string strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | DepositAmount: {3} | BankName: {4} | AccountName: {5} | AccountNumber: {6} | MinLimit: {7} | MaxLimit: {8} | TotalAllowed: {9} | DailyLimit: {10} | Response: {11}",
              Convert.ToInt64(strOperatorId), strMemberCode, strCurrencyCode, strDepositAmount, drpBank.SelectedValue, strAccountName, strAccountNo, decMinLimit, decMaxLimit, strTotalAllowed, strDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", PageName, "InitiateDeposit", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        }
    }

    private void ValidateDaddyPay(bool isDaddyPayQR, string selectedBank, decimal decDepositAmount, decimal decMinLimit, decimal decMaxLimit, string strAccountName, string strAccountNo)
    {
        if (decDepositAmount == 0)
        {
            strAlertCode = "-1";
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/MissingDepositAmount", xeErrors);
            isProcessAbort = true;
        }
        else if (selectedBank == "-1")
        {
            strAlertCode = "-1";
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/SelectBank", xeErrors);
            isProcessAbort = true;
        }
        else if (decDepositAmount < decMinLimit)
        {
            strAlertCode = "-1";
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/AmountMinLimit", xeErrors);
            isProcessAbort = true;
        }
        else if (decDepositAmount > decMaxLimit)
        {
            strAlertCode = "-1";
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/AmountMaxLimit", xeErrors);
            isProcessAbort = true;
        }
        else if ((strTotalAllowed != commonCulture.ElementValues.getResourceString("unlimited", xeResources)) && (decDepositAmount > Convert.ToDecimal(strTotalAllowed)) && Convert.ToDecimal(strTotalAllowed) > 0)
        {
            strAlertCode = "-1";
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TotalAllowedExceeded", xeErrors);
            isProcessAbort = true;
        }

        if (isDaddyPayQR)
        {
            ValidateDaddyPayQR(strAccountName, strAccountNo, selectedBank);
        }
    }

    private void ValidateDaddyPayQR(string strAccountName, string strAccountNo, string selectedBank)
    {
        if (string.IsNullOrEmpty(strAccountName))
        {
            strAlertCode = "-1";
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/InvalidAccountName", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strAccountNo) && !selectedBank.Equals("40"))
        {
            strAlertCode = "-1";
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/InvalidAccountNumber", xeErrors);
            isProcessAbort = true;
        }
    }

    private void InitiateDeposit(string selectedBank, string strDepositAmount)
    {
        using (svcPayDeposit.DepositClient client = new svcPayDeposit.DepositClient())
        {
            xeResponse = client.createOnlineDepositTransactionV1(Convert.ToInt64(strOperatorId), Convert.ToInt64(strMemberID), strMemberCode, Convert.ToInt64(base.PaymentMethodId), strCurrencyCode, Convert.ToDecimal(strDepositAmount), svcPayDeposit.DepositSource.Mobile, string.Empty);

            if (xeResponse == null)
            {
                strAlertCode = "-1";
                strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferFail", xeErrors);
            }
            else
            {
                bool isTransactionSuccessful = Convert.ToBoolean(commonCulture.ElementValues.getResourceString("result", xeResponse));
                string transferId = commonCulture.ElementValues.getResourceString("invId", xeResponse);

                if (isTransactionSuccessful)
                {
                    string config = commonEncryption.Md5Hash(commonEncryption.decrypting(ConfigurationManager.AppSettings["privateKey_daddyPay"]));

                    strDepositAmount = Convert.ToDouble(strDepositAmount).ToString("#.00");
                    string strMemberIDCode = strMemberID + strMemberCode;
                    string strCurrentUrl = System.Web.HttpContext.Current.Request.Url.Host.ToString();

                    StringBuilder builder = new StringBuilder();

                    string depositMode = isDaddyPayQR ? "3" : "2";

                    builder.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}", config, strMerchantId, selectedBank, strDepositAmount, transferId,
                                         strMemberID, selectedBank, depositMode, 0, strCurrentUrl, string.Empty, strMemberIDCode, 1);

                    string key = commonEncryption.Md5Hash(builder.ToString());

                    DaddyPayDomain daddyPayDomain = new DaddyPayDomain();

                    daddyPayDomain.depositMode = depositMode;
                    daddyPayDomain.companyId = strMerchantId;
                    daddyPayDomain.bankId = drpBank.SelectedValue.ToString();
                    daddyPayDomain.amount = strDepositAmount;
                    daddyPayDomain.companyOrderNum = transferId;
                    daddyPayDomain.key = key;
                    daddyPayDomain.companyUser = strMemberID;
                    daddyPayDomain.estimatedPaymentBank = selectedBank;
                    daddyPayDomain.groupId = "0";
                    daddyPayDomain.webUrl = strCurrentUrl;
                    daddyPayDomain.memo = string.Empty;
                    daddyPayDomain.note = strMemberIDCode;
                    daddyPayDomain.noteModel = "1";
                    daddyPayDomain.terminal = "2";

                    byte[] responseBytes = UploadValues(daddyPayDomain);

                    string responseStr = Encoding.UTF8.GetString(responseBytes);

                    var s = new System.Web.Script.Serialization.JavaScriptSerializer();
                    List<myObject> obj = s.Deserialize<List<myObject>>("[" + responseStr + "]");


                    string status = obj[0].status;
                    string break_url = obj[0].break_url;

                    if (status.Equals("1") && !string.IsNullOrWhiteSpace(break_url))
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + break_url.ToString() + "','_blank')", true);
                    }
                    else
                    {
                        strAlertCode = "-1";
                        strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/Exception", xeErrors);
                        strErrorDetail = obj[0].error_msg;
                    }
                }
            }
        }
    }

    public byte[] UploadValues(DaddyPayDomain daddyPayDomain)
    {
        string postUrl = ConfigurationManager.AppSettings["DaddyPay_posturl"];

        DaddyPayDomain daddyPay = new DaddyPayDomain();

        daddyPay = daddyPayDomain;

        byte[] responseBytes = null;

        NameValueCollection postData = new NameValueCollection();
        postData["company_id"] = daddyPay.companyId;
        postData["bank_id"] = daddyPay.bankId;
        postData["amount"] = daddyPay.amount;
        postData["company_order_num"] = daddyPay.companyOrderNum;
        postData["company_user"] = daddyPay.companyUser;
        postData["key"] = daddyPay.key;
        postData["estimated_payment_bank"] = daddyPay.estimatedPaymentBank;
        postData["deposit_mode"] = daddyPay.depositMode;
        postData["group_id"] = daddyPay.groupId;
        postData["web_url"] = daddyPay.webUrl;
        postData["memo"] = daddyPay.memo;
        postData["note"] = daddyPay.note;
        postData["note_model"] = daddyPay.noteModel;
        postData["terminal"] = daddyPay.terminal;

        using (WebClient wc = new WebClient())
        {
            responseBytes = wc.UploadValues(postUrl, "POST", postData);
        }

        return responseBytes;
    }
}

public class DaddyPayDomain
{
    public string companyId { get; set; }
    public string bankId { get; set; }
    public string amount { get; set; }
    public string companyOrderNum { get; set; }
    public string companyUser { get; set; }
    public string key { get; set; }
    public string estimatedPaymentBank { get; set; }
    public string depositMode { get; set; }
    public string groupId { get; set; }
    public string webUrl { get; set; }
    public string memo { get; set; }
    public string note { get; set; }
    public string noteModel { get; set; }
    public string terminal { get; set; }

    public DaddyPayDomain()
    {
        this.companyId = string.Empty;
        this.bankId = string.Empty;
        this.amount = string.Empty;
        this.companyOrderNum = string.Empty;
        this.companyUser = string.Empty;
        this.key = string.Empty;
        this.estimatedPaymentBank = string.Empty;
        this.depositMode = string.Empty;
        this.groupId = string.Empty;
        this.webUrl = string.Empty;
        this.memo = string.Empty;
        this.note = string.Empty;
        this.noteModel = string.Empty;
        this.terminal = string.Empty;
    }
}

public class myObject
{
    public string bank_card_num { get; set; }
    public string bank_acc_name { get; set; }
    public string amount { get; set; }
    public string email { get; set; }
    public string company_order_num { get; set; }
    public string datetime { get; set; }
    public string note { get; set; }
    public string mownecum_order_num { get; set; }
    public string status { get; set; }
    public string mode { get; set; }
    public string issuing_bank_address { get; set; }
    public string break_url { get; set; }
    public string deposit_mode { get; set; }
    public string collection_bank_id { get; set; }
    public string key { get; set; }
    public string error_msg { get; set; }
}