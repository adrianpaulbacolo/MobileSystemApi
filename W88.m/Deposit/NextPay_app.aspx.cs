using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Deposit_NextPay : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected System.Xml.Linq.XElement xeResources = null;

    protected string strStatusCode = string.Empty;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;
    protected string strMethodsUnAvailable = string.Empty;

    private Boolean IsPageRefresh = false;

    private string strOperatorId = string.Empty;
    private string strMemberCode = string.Empty;
    private string strCurrencyCode = string.Empty;
    private string strCountryCode = string.Empty;
    private string strRiskId = string.Empty;
    private string strPaymentGroup = string.Empty;
    private string strSelectedLanguage = string.Empty;
    private string strMemberID = string.Empty;

    protected string strMinAmount = string.Empty;
    protected string strMaxAmount = string.Empty;

    protected void Page_Init(object sender, EventArgs e) { base.CheckLogin(); }

    protected void Page_Load(object sender, EventArgs e)
    {
        CancelUnexpectedRePost();

        strOperatorId = commonVariables.OperatorId;
        strMemberCode = commonVariables.GetSessionVariable("MemberCode");
        strMemberID = commonVariables.GetSessionVariable("memberId");
        strCurrencyCode = commonVariables.GetSessionVariable("CurrencyCode");
        strCountryCode = commonVariables.GetSessionVariable("CountryCode");
        strRiskId = commonVariables.GetSessionVariable("RiskId");
        strPaymentGroup = commonVariables.GetSessionVariable("PaymentGroup");
        strSelectedLanguage = commonVariables.SelectedLanguage;

        xeErrors = commonVariables.ErrorsXML;
        commonCulture.appData.getRootResource("/Deposit/SDPay", out xeResources);

        if (!Page.IsPostBack)
        {
            lblMode.Text = commonCulture.ElementValues.getResourceString("lblMode", xeResources);
            txtMode.Text = string.Format(": {0}", commonCulture.ElementValues.getResourceString("txtMode", xeResources));
            lblMinMaxLimit.Text = commonCulture.ElementValues.getResourceString("lblMinMaxLimit", xeResources);
            lblDailyLimit.Text = commonCulture.ElementValues.getResourceString("lblDailyLimit", xeResources);
            lblTotalAllowed.Text = commonCulture.ElementValues.getResourceString("lblTotalAllowed", xeResources);
            lblDepositAmount.Text = commonCulture.ElementValues.getResourceString("lblDepositAmount", xeResources);
            //lblNoticeDownload.Text = commonCulture.ElementValues.getResourceString("lblNoticeDownload", xeResources);

            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);

            txtDepositAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} {1}", lblDepositAmount.Text, strCurrencyCode));

            System.Threading.Tasks.Task t1 = System.Threading.Tasks.Task.Factory.StartNew(this.InitialisePaymentLimits);

            System.Threading.Tasks.Task.WaitAll(t1);
        }
        else
        {
            string strProcessCode = string.Empty;
            string strProcessText = string.Empty;
            string strMethodId = string.Empty;
            strMethodId = "0";
            System.Data.DataTable dtPaymentMethodLimits = null;
            System.Text.StringBuilder sbMethodsUnavailable = new System.Text.StringBuilder();

            using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
            {
                dtPaymentMethodLimits = svcInstance.getMethodLimits(strOperatorId, strMemberCode, strMethodId, Convert.ToString(Convert.ToInt32(commonVariables.PaymentTransactionType.Deposit)), false, out strProcessCode, out strProcessText);
            }

            foreach (commonVariables.DepositMethod EnumMethod in Enum.GetValues(typeof(commonVariables.DepositMethod)))
            {
                if (dtPaymentMethodLimits.Select("[methodId] = " + Convert.ToInt32(EnumMethod)).Count() < 1)
                {
                    sbMethodsUnavailable.AppendFormat("{0}|", Convert.ToInt32(EnumMethod));
                }
            }

            strMethodsUnAvailable = Convert.ToString(sbMethodsUnavailable).TrimEnd('|');

            //bankDropDownList.Items.Clear();

            //PopulateBankList();
        }
    }

    private void CancelUnexpectedRePost()
    {
        if (!IsPostBack)
        {
            ViewState["postids"] = System.Guid.NewGuid().ToString();
            Session["postid"] = ViewState["postids"].ToString();
        }
        else
        {
            if (string.IsNullOrEmpty(ViewState["postids"] as string)) { IsPageRefresh = true; }
            else
            {
                if (string.IsNullOrEmpty(Session["postid"] as string)) { IsPageRefresh = true; }
                else if (ViewState["postids"].ToString() != Session["postid"].ToString()) { IsPageRefresh = true; }
            }
            Session["postid"] = System.Guid.NewGuid().ToString();
            ViewState["postids"] = Session["postid"];
            //System.Web.HttpContext.Current.Request.RawUrl
        }
    }

    private void InitialisePaymentLimits()
    {
        string strProcessCode = string.Empty;
        string strProcessText = string.Empty;
        string strMinLimit = string.Empty;
        string strMaxLimit = string.Empty;
        string strTotalAllowed = string.Empty;
        string strDailyLimit = string.Empty;
        string strMethodId = string.Empty;

        System.Data.DataTable dtPaymentMethodLimits = null;
        System.Data.DataRow drPaymentMethodLimit = null;

        System.Text.StringBuilder sbMethodsUnavailable = new System.Text.StringBuilder();

        strMethodId = "0";

        using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
        {
            dtPaymentMethodLimits = svcInstance.getMethodLimits(strOperatorId, strMemberCode, strMethodId, Convert.ToString(Convert.ToInt32(commonVariables.PaymentTransactionType.Deposit)), false, out strProcessCode, out strProcessText);
        }

        foreach (commonVariables.DepositMethod EnumMethod in Enum.GetValues(typeof(commonVariables.DepositMethod)))
        {
            if (dtPaymentMethodLimits.Select("[methodId] = " + Convert.ToInt32(EnumMethod)).Count() < 1)
            {
                sbMethodsUnavailable.AppendFormat("{0}|", Convert.ToInt32(EnumMethod));
            }
        }

        strMethodId = Convert.ToString(Convert.ToInt32(commonVariables.DepositMethod.NextPay));

        if (dtPaymentMethodLimits.Select("[methodId] = " + strMethodId).Count() > 0)
        {
            drPaymentMethodLimit = dtPaymentMethodLimits.Select("[methodId] = " + strMethodId)[0];

            //strMinAmount = Convert.ToString(dtPaymentMethodLimits.Rows[0]["minDeposit"]);
            //strMaxAmount = Convert.ToString(dtPaymentMethodLimits.Rows[0]["maxDeposit"]);

            strMinLimit = Convert.ToDecimal(drPaymentMethodLimit["minDeposit"]).ToString(commonVariables.DecimalFormat);
            strMaxLimit = Convert.ToDecimal(drPaymentMethodLimit["maxDeposit"]).ToString(commonVariables.DecimalFormat);
            strTotalAllowed = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["totalAllowed"]) == 0 ? commonCulture.ElementValues.getResourceString("unlimited", xeResources) : Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["totalAllowed"]).ToString(commonVariables.DecimalFormat);
            strDailyLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["limitDaily"]) == 0 ? commonCulture.ElementValues.getResourceString("unlimited", xeResources) : Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["limitDaily"]).ToString(commonVariables.DecimalFormat);

            txtDepositAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} ({1})", lblDepositAmount.Text, strCurrencyCode));

            txtMinMaxLimit.Text = string.Format(": {0} / {1}", strMinLimit, strMaxLimit);
            txtDailyLimit.Text = string.Format(": {0}", strDailyLimit);
            txtTotalAllowed.Text = string.Format(": {0}", strTotalAllowed);

            Session["minLimit"] = strMinLimit;
            Session["maxLimit"] = strMaxLimit;
        }

        strMethodsUnAvailable = Convert.ToString(sbMethodsUnavailable).TrimEnd('|');
    }



    private string GetForm(string invId, string amount)
    {
        string merchantId = decrypting(ConfigurationManager.AppSettings["NextPay_merchantid"]);
        string callbackUrl = ConfigurationManager.AppSettings["NextPay_callbackurl"];
        string postUrl = ConfigurationManager.AppSettings["NextPay_posturl"];

        var request = (HttpWebRequest)WebRequest.Create(postUrl);
        string postData = "merchantID=" + merchantId;
        postData += ("&inv=" + invId);
        postData += ("&amt=" + amount);
        postData += ("&returnURL=" + callbackUrl);
        postData += ("&bm=" + bankDropDownList.SelectedValue.ToLower());
        postData += ("&cID=" + strMemberID);
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


    //private string GetForm(string invId, string amount)
    //{
    //    string merchantId = decrypting(ConfigurationManager.AppSettings["NextPay_merchantid"]);
    //    string callbackUrl = ConfigurationManager.AppSettings["NextPay_callbackurl"];
    //    string postUrl = ConfigurationManager.AppSettings["NextPay_posturl"];

    //    StringBuilder sb = new StringBuilder();
    //    sb.Append(@"<form id=""theForm"" name=""theForm"" target=""_blank"" method=""post"" action='" + postUrl + "'>");
    //    sb.Append(@"<input type=""hidden"" name=""merchantID"" value='" + merchantId + "'/>");
    //    sb.Append(@"<input type=""hidden"" name=""inv"" value='" + invId + "'/>");
    //    sb.Append(@"<input type=""hidden"" name=""amt"" value='" + amount + "'/>");
    //    sb.Append(@"<input type=""hidden"" name=""returnURL"" value='" + callbackUrl + "'/>");
    //    sb.Append(@"<Input Type=""hidden"" name=""bm"" value='" + bankDropDownList.SelectedValue.ToLower().ToString() + "'/>");
    //    sb.Append(@"<input type=""hidden"" name=""cID"" value='" + (string)Session["user_MemberID"] + "'/>");
    //    sb.Append(@"</form>");
    //    return sb.ToString();
    //}


    //private string GetScript(long invId)
    //{
    //    try
    //    {
    //        string CurrentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
    //        StringBuilder sb = new StringBuilder();
    //        sb.Append(@"<script type='text/javascript'>");
    //        sb.Append(@"var ctlForm = document.forms.namedItem('theForm');");
    //        sb.Append(@"ctlForm.submit();");
    //        sb.Append("setTimeout(function() {window.location = '" + CurrentUrl + "'}, 5000);");
    //        sb.Append(@"</script>");
    //        return sb.ToString();
    //    }
    //    catch (Exception ex)
    //    {
    //        return string.Empty;
    //    }
    //}



    public void DoHttpPost_NextPay()
    {
        try
        {
            string depositAmount = txtDepositAmount.Text.Trim();
            svcPayDeposit.DepositClient client = new svcPayDeposit.DepositClient();
            System.Xml.Linq.XElement xElement = client.createOnlineDepositTransaction(Convert.ToInt64(strOperatorId), strMemberCode, Convert.ToInt64(commonVariables.DepositMethod.NextPay), strCurrencyCode, Convert.ToDecimal(depositAmount), string.Empty);
            client.Close();

            if (xElement == null)
            {
                return;
            }
            bool result = Convert.ToBoolean(xElement.Element("result").Value);
            long invId = Convert.ToInt64(xElement.Element("invId").Value);

            if (!result)
            {
                return;
            }

            litForm.Text = GetForm(invId.ToString(), depositAmount);
            //LiteralScript.Text = GetScript(invId);
        }
        catch (Exception) { }
    }


    public static string decrypting(string str)
    {
        string privateKey = System.Configuration.ConfigurationManager.AppSettings.Get("PrivateKey_nextPay");
        string functionReturnValue = null;
        if (!string.IsNullOrEmpty(str))
        {
            encryption_manager.encryption decrypt = new encryption_manager.encryption();
            decrypt.private_key = privateKey;
            decrypt.message = str;
            functionReturnValue = decrypt.decrypting();
            decrypt = null;
        }
        else { functionReturnValue = string.Empty; }
        return functionReturnValue;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (Convert.ToDecimal(Session["minLimit"].ToString()) > Convert.ToDecimal(txtDepositAmount.Text.Trim()))
            {
                Response.Write("<script>alert('Deposit amount below minimum.');</script>");
            }
            else if (Convert.ToDecimal(Session["maxLimit"].ToString()) < Convert.ToDecimal(txtDepositAmount.Text.Trim()))
            {
                Response.Write("<script>alert('Deposit amount above maximum.');</script>");
            }
            else
            {
                DoHttpPost_NextPay();
            }
        }
        catch(Exception)
        {
            Response.Write("<script>alert('Invalid deposit amount.');</script>");
        }
    }
}