using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;


public partial class Deposit_Help2Pay : BasePage
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

    protected string strMinAmount = string.Empty;
    protected string strMaxAmount = string.Empty;

    protected void Page_Init(object sender, EventArgs e) { base.CheckLogin(); }

    protected void Page_Load(object sender, EventArgs e)
    {
        CancelUnexpectedRePost();

        strOperatorId = commonVariables.OperatorId;
        strMemberCode = commonVariables.GetSessionVariable("MemberCode");
        strCurrencyCode = commonVariables.GetSessionVariable("CurrencyCode");
        strCountryCode = commonVariables.GetSessionVariable("CountryCode");
        strRiskId = commonVariables.GetSessionVariable("RiskId");
        strPaymentGroup = commonVariables.GetSessionVariable("PaymentGroup");
        strSelectedLanguage = commonVariables.SelectedLanguage;

        xeErrors = commonVariables.ErrorsXML;
        commonCulture.appData.getRootResource("/Deposit/Help2Pay", out xeResources);

        if (!Page.IsPostBack)
        {
            lblMode.Text = commonCulture.ElementValues.getResourceString("lblMode", xeResources);
            txtMode.Text = string.Format(": {0}", commonCulture.ElementValues.getResourceString("txtMode", xeResources));
            lblMinMaxLimit.Text = commonCulture.ElementValues.getResourceString("lblMinMaxLimit", xeResources);
            lblDailyLimit.Text = commonCulture.ElementValues.getResourceString("lblDailyLimit", xeResources);
            lblTotalAllowed.Text = commonCulture.ElementValues.getResourceString("lblTotalAllowed", xeResources);
            lblDepositAmount.Text = commonCulture.ElementValues.getResourceString("lblDepositAmount", xeResources);
        
            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);

            txtDepositAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} {1}", lblDepositAmount.Text, strCurrencyCode));
            
            System.Threading.Tasks.Task t1 = System.Threading.Tasks.Task.Factory.StartNew(this.InitialisePaymentLimits);
            System.Threading.Tasks.Task t2 = System.Threading.Tasks.Task.Factory.StartNew(this.InitializeBank);
        
            System.Threading.Tasks.Task.WaitAll(t1, t2);

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

        strMethodId = Convert.ToString(Convert.ToInt32(commonVariables.DepositMethod.Help2Pay));

        if (dtPaymentMethodLimits.Select("[methodId] = " + strMethodId).Count() > 0)
        {
            drPaymentMethodLimit = dtPaymentMethodLimits.Select("[methodId] = " + strMethodId)[0];

            strMinLimit = Convert.ToDecimal(drPaymentMethodLimit["minDeposit"]).ToString(commonVariables.DecimalFormat);
            strMaxLimit = Convert.ToDecimal(drPaymentMethodLimit["maxDeposit"]).ToString(commonVariables.DecimalFormat);
            strTotalAllowed = Convert.ToDecimal(drPaymentMethodLimit["totalAllowed"]).ToString(commonVariables.DecimalFormat);
            strDailyLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["limitDaily"]) == 0 ? commonCulture.ElementValues.getResourceString("unlimited", xeResources) : Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["limitDaily"]).ToString(commonVariables.DecimalFormat);

            txtDepositAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} ({1})", lblDepositAmount.Text, strCurrencyCode));

            txtMinMaxLimit.Text = string.Format(": {0} / {1}", strMinLimit, strMaxLimit);
            txtDailyLimit.Text = string.Format(": {0}", strDailyLimit);
            txtTotalAllowed.Text = string.Format(": {0}", strTotalAllowed); 
            strMinAmount = drPaymentMethodLimit["minDeposit"].ToString();
            strMaxAmount = drPaymentMethodLimit["maxDeposit"].ToString();
        }

        strMethodsUnAvailable = Convert.ToString(sbMethodsUnavailable).TrimEnd('|');
    }

    private void InitializeBank()
    {
        try
        {

            XElement xElementBank = null;

            commonCulture.appData.getRootResource("/Deposit/Help2PayBank", out xElementBank);

            XElement xElementBankPath = xElementBank.Element(commonVariables.GetSessionVariable("CurrencyCode"));
            var banks = from bank in xElementBankPath.Elements("bank") select new { value = bank.Attribute("id").Value, text = bank.Value };

            drpBank.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpBank", xeResources), "-1"));

            foreach (var b in banks)
            {
                drpBank.Items.Add(new ListItem(b.text, b.value));
            }
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", "help2pay", "InitializeBank", string.Empty, string.Empty, string.Empty, "-99", "exception", ex.Message, string.Empty, string.Empty, true);
        }
    }

    

}