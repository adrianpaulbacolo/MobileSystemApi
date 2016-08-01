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

public partial class Deposit_DaddyPay : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected System.Xml.Linq.XElement xeResources = null;
    protected System.Xml.Linq.XElement xeResource = null;

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
    private string strMemberId = string.Empty;

    protected string strMinAmount = string.Empty;
    protected string strMaxAmount = string.Empty;

    System.Xml.Linq.XElement xElement;

    string CurrentUrl = System.Web.HttpContext.Current.Request.Url.Host.ToString();

    protected void Page_Init(object sender, EventArgs e) { base.CheckLogin(); }

    protected void Page_Load(object sender, EventArgs e)
    {
        strOperatorId = commonVariables.OperatorId;
        strMemberId = base.userInfo.MemberId;
        strMemberCode = base.userInfo.MemberCode;
        strCurrencyCode = commonCookie.CookieCurrency;
        strCountryCode = commonVariables.GetSessionVariable("CountryCode");
        strRiskId = commonVariables.GetSessionVariable("RiskId");
        strPaymentGroup = commonVariables.GetSessionVariable("PaymentGroup");
        strSelectedLanguage = commonVariables.SelectedLanguage;

        xeErrors = commonVariables.ErrorsXML;
        commonCulture.appData.getRootResource("/Deposit/SDPay", out xeResources);
        commonCulture.appData.getRootResource("/Deposit/FastDeposit", out xeResource);


        if (!Page.IsPostBack)
        {
            lblMode.Text = commonCulture.ElementValues.getResourceString("lblMode", xeResources);
            txtMode.Text = string.Format(": {0}", commonCulture.ElementValues.getResourceString("txtMode", xeResources));
            lblMinMaxLimit.Text = commonCulture.ElementValues.getResourceString("lblMinMaxLimit", xeResources);
            lblDailyLimit.Text = commonCulture.ElementValues.getResourceString("lblDailyLimit", xeResources);
            lblTotalAllowed.Text = commonCulture.ElementValues.getResourceString("lblTotalAllowed", xeResources);


            bankDropDownList.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpBank", xeResource), "-1"));

            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);

            amount_txt.Attributes.Add("PLACEHOLDER", string.Format("{0} {1}", amount_txt.Text, strCurrencyCode));

            System.Threading.Tasks.Task t1 = System.Threading.Tasks.Task.Factory.StartNew(this.InitialisePaymentLimits);

            System.Threading.Tasks.Task.WaitAll(t1);

            string value = Request.QueryString["value"].ToString();
            if (value == "1")
            {
                account_txt.Visible = false;
                accountName_txt.Visible = false;
            }
            else if (value == "2")
            {
                account_txt.Visible = true; ;
                accountName_txt.Visible = true;
            }

            PopulateBankList();
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
                dtPaymentMethodLimits = svcInstance.getMethodLimits_Mobile(strOperatorId, strMemberCode, strMethodId, Convert.ToString(Convert.ToInt32(commonVariables.PaymentTransactionType.Deposit)), false, out strProcessCode, out strProcessText);
            }

            foreach (commonVariables.DepositMethod EnumMethod in Enum.GetValues(typeof(commonVariables.DepositMethod)))
            {
                if (dtPaymentMethodLimits.Select("[methodId] = " + Convert.ToInt32(EnumMethod)).Count() < 1)
                {
                    sbMethodsUnavailable.AppendFormat("{0}|", Convert.ToInt32(EnumMethod));
                }
            }

            strMethodsUnAvailable = Convert.ToString(sbMethodsUnavailable).TrimEnd('|');

        }

        HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");

        if (Request.QueryString["value"].ToString() == "1")
        {
            commonPaymentMethodFunc.GetDepositMethodList(strMethodsUnAvailable, depositTabs, "daddypay", sender.ToString().Contains("app"), strCurrencyCode);
        }
        else if (Request.QueryString["value"].ToString() == "2")
        {
            commonPaymentMethodFunc.GetDepositMethodList(strMethodsUnAvailable, depositTabs, "daddypayqr", sender.ToString().Contains("app"), strCurrencyCode);
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

        strOperatorId = commonVariables.OperatorId;

        System.Data.DataTable dtPaymentMethodLimits = null;
        System.Data.DataRow drPaymentMethodLimit = null;

        System.Text.StringBuilder sbMethodsUnavailable = new System.Text.StringBuilder();

        strMethodId = "0";

        using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
        {
            dtPaymentMethodLimits = svcInstance.getMethodLimits_Mobile(strOperatorId, strMemberCode, strMethodId, Convert.ToString(Convert.ToInt32(commonVariables.PaymentTransactionType.Deposit)), false, out strProcessCode, out strProcessText);
        }

        foreach (commonVariables.DepositMethod EnumMethod in Enum.GetValues(typeof(commonVariables.DepositMethod)))
        {
            if (dtPaymentMethodLimits.Select("[methodId] = " + Convert.ToInt32(EnumMethod)).Count() < 1)
            {
                sbMethodsUnavailable.AppendFormat("{0}|", Convert.ToInt32(EnumMethod));
            }
        }

        string value = Request.QueryString["value"].ToString();

        if (value == "1")
        {
            strMethodId = Convert.ToString(Convert.ToInt32(commonVariables.DepositMethod.DaddyPay));
        }
        else if (value == "2")
        {
            strMethodId = Convert.ToString(Convert.ToInt32(commonVariables.DepositMethod.DaddyPayQR));
        }

        if (dtPaymentMethodLimits.Select("[methodId] = " + strMethodId).Count() > 0)
        {
            drPaymentMethodLimit = dtPaymentMethodLimits.Select("[methodId] = " + strMethodId)[0];

            strMinLimit = Convert.ToDecimal(drPaymentMethodLimit["minDeposit"]).ToString(commonVariables.DecimalFormat);
            strMaxLimit = Convert.ToDecimal(drPaymentMethodLimit["maxDeposit"]).ToString(commonVariables.DecimalFormat);
            strTotalAllowed = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["totalAllowed"]) == 0 ? commonCulture.ElementValues.getResourceString("unlimited", xeResources) : Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["totalAllowed"]).ToString(commonVariables.DecimalFormat);
            strDailyLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["limitDaily"]) == 0 ? commonCulture.ElementValues.getResourceString("unlimited", xeResources) : Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["limitDaily"]).ToString(commonVariables.DecimalFormat);
            amount_txt.Text = commonCulture.ElementValues.getResourceString("lblAmount", xeResources);

            amount_txt.Attributes.Add("PLACEHOLDER", string.Format("{0} ({1})", amount_txt.Text, strCurrencyCode));

            Session["minLimit"] = strMinLimit;
            Session["maxLimit"] = strMaxLimit;

            txtMinMaxLimit.Text = string.Format(": {0} / {1}", strMinLimit, strMaxLimit);
            txtDailyLimit.Text = string.Format(": {0}", strDailyLimit);
            txtTotalAllowed.Text = string.Format(": {0}", strTotalAllowed);
        }

        strMethodsUnAvailable = Convert.ToString(sbMethodsUnavailable).TrimEnd('|');
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string value = Request.QueryString["value"].ToString();
            strOperatorId = commonVariables.OperatorId;

            if (!string.IsNullOrEmpty(amount_txt.Text))
            {

                if (Convert.ToDecimal(Session["minLimit"].ToString()) > Convert.ToDecimal(amount_txt.Text))
                {
                    Response.Write("<script>alert('金额小于最低限额');</script>");
                }
                else if (Convert.ToDecimal(Session["maxLimit"].ToString()) < Convert.ToDecimal(amount_txt.Text))
                {
                    Response.Write("<script>alert('金额大于最高限额');</script>");
                }
                else if (bankDropDownList.SelectedIndex.ToString() == "0")
                {
                    Response.Write("<script>alert('Please select a bank');</script>");
                }
                else if (string.IsNullOrEmpty(accountName_txt.Text) && value == "2")
                {
                    Response.Write("<script>alert('Please enter account name');</script>");
                }
                else if (string.IsNullOrEmpty(account_txt.Text) && value == "2")
                {
                    Response.Write("<script>alert('Please enter account number');</script>");
                }
                else
                {

                    DaddyPayDomain daddyPayDomain = new DaddyPayDomain();

                    DataTable dt = new DataTable();
                    string statusCode, statusText;

                    if (value == "1")
                    {
                        daddyPayDomain.depositMode = "2";

                        using (svcPayDeposit.DepositClient client = new svcPayDeposit.DepositClient())
                        {
                            xElement = client.createOnlineDepositTransactionV1(Convert.ToInt64(strOperatorId), long.Parse(strMemberId), strMemberCode, Convert.ToInt64(commonVariables.DepositMethod.DaddyPay), strCurrencyCode, Convert.ToDecimal(amount_txt.Text), DepositSource.Mobile, string.Empty);
                            client.Close();
                        }

                        using (svcPayMember.MemberClient client = new svcPayMember.MemberClient())
                        {
                            dt = client.getMethodLimits_Mobile(strOperatorId, strMemberCode, Convert.ToInt32(commonVariables.DepositMethod.DaddyPay).ToString(), "1", false, out statusCode, out statusText);
                            client.Close();
                        }
                    }
                    else if (value == "2")
                    {
                        daddyPayDomain.depositMode = "3";

                        using (svcPayDeposit.DepositClient client = new svcPayDeposit.DepositClient())
                        {
                            xElement = client.createOnlineDepositTransactionV1(Convert.ToInt64(strOperatorId), long.Parse(strMemberId), strMemberCode, Convert.ToInt64(commonVariables.DepositMethod.DaddyPayQR), strCurrencyCode, Convert.ToDecimal(amount_txt.Text), DepositSource.Mobile, string.Empty);
                            client.Close();
                        }

                        using (svcPayMember.MemberClient client = new svcPayMember.MemberClient())
                        {
                            dt = client.getMethodLimits_Mobile(strOperatorId, strMemberCode, Convert.ToInt32(commonVariables.DepositMethod.DaddyPay).ToString(), "1", false, out statusCode, out statusText);
                            client.Close();
                        }
                    }

                    if (xElement != null)
                    {
                        bool isDepositSuccessful = Convert.ToBoolean(commonCulture.ElementValues.getResourceString("result", xElement));

                        if (isDepositSuccessful)
                        {
                            DataRow dr = dt.Rows[0];

                            string config = Md5Hash(commonEncryption.decrypting("03WUpD2ff5AojnGcH/VL7tzEekc0XJp4X8x7F2IWVPQLECQgWSGhNMLgMioGWCI2"));

                            var builder = new StringBuilder();
                            builder.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}", config, Convert.ToString(dr["merchantId"]), bankDropDownList.SelectedValue.ToString(), Convert.ToDouble(amount_txt.Text).ToString("#.00"), xElement.Element("invId").Value.ToString(),
                                                 strMemberId, bankDropDownList.SelectedValue.ToString(), daddyPayDomain.depositMode, 0, CurrentUrl, string.Empty, (strMemberId + strMemberCode), 1);

                            string key = Md5Hash(builder.ToString());


                            daddyPayDomain.companyId = Convert.ToString(dr["merchantId"]);
                            daddyPayDomain.bankId = bankDropDownList.SelectedValue.ToString();
                            daddyPayDomain.amount = Convert.ToDouble(amount_txt.Text).ToString("#.00");
                            daddyPayDomain.companyOrderNum = xElement.Element("invId").Value.ToString();
                            daddyPayDomain.key = key;
                            daddyPayDomain.companyUser = strMemberId;
                            daddyPayDomain.estimatedPaymentBank = bankDropDownList.SelectedValue.ToString();
                            daddyPayDomain.groupId = "0";
                            daddyPayDomain.webUrl = CurrentUrl;
                            daddyPayDomain.memo = string.Empty;
                            daddyPayDomain.note = strMemberId + strMemberCode;
                            daddyPayDomain.noteModel = "1";
                            daddyPayDomain.terminal = "2";

                            byte[] responseBytes = UploadValues(daddyPayDomain);

                            string responseStr = Encoding.UTF8.GetString(responseBytes);

                            var s = new System.Web.Script.Serialization.JavaScriptSerializer();
                            List<myObject> obj = s.Deserialize<List<myObject>>("[" + responseStr + "]");

                            var status = obj[0].status;
                            var break_url = obj[0].break_url;


                            if (status == "1")
                            {
                                //Response.Redirect(break_url.ToString());
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + break_url.ToString() + "','_blank')", true);
                            }
                            else
                            {
                                //Response.Redirect(break_url.ToString());
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "popup", "window.open('" + break_url.ToString() + "','_blank')", true);
                            }
                        }
                    }
                }
            }
            else if (string.IsNullOrEmpty(amount_txt.Text))
            {
                Response.Write("<script>alert('Please enter a deposit amount');</script>");
            }
        }
        catch (Exception)
        { }
    }


    public byte[] UploadValues(DaddyPayDomain daddyPayDomain)
    {
        string postUrl = ConfigurationManager.AppSettings["DaddyPay_posturl"];

        DaddyPayDomain daddyPay = new DaddyPayDomain();

        daddyPay = daddyPayDomain;

        byte[] responseBytes = null;

        try
        {
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
        catch (Exception)
        {
            return responseBytes;
        }
    }


    public void PopulateBankList()
    {
        string lang = commonVariables.SelectedLanguage.ToLower();
        string BankList = string.Empty;

        if (lang == "zh-cn")
        {
            BankList = ConfigurationManager.AppSettings["BankList_cn"];
        }
        else
        {
            BankList = ConfigurationManager.AppSettings["BankList_en"];
        }

        string[] Bank = new string[100];
        Bank = BankList.Split(',');

        string flag = Request.QueryString["value"].ToString();

        if (flag == "1")
        {
            for (int x = 0; x < Bank.Count(); x++)
            {
                if (Bank[x].Split('-')[1] != "ALIPAYQR" && Bank[x].Split('-')[1] != "支付宝(二维码)")
                {
                    bankDropDownList.Items.Add(new ListItem(Bank[x].Split('-')[1], Bank[x].Split('-')[0]));
                }
            }
        }
        else if (flag == "2")
        {
            bankDropDownList.Items.Add(new ListItem(Bank[2].Split('-')[1], Bank[2].Split('-')[0]));
        }

    }


    public string decrypt(string str)
    {
        string privateKey = System.Configuration.ConfigurationManager.AppSettings.Get("privateKey_daddyPay");
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


    public static string Md5Hash(string input)
    {
        byte[] data = null;
        using (System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create())
        {
            data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        }
        StringBuilder sBuilder = new StringBuilder();

        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        return sBuilder.ToString();
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
}