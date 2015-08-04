using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.XPath;

public partial class Deposit_Default : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    private System.Xml.Linq.XElement xeResources = null;

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
        commonCulture.appData.getRootResource("/Deposit/FastDeposit", out xeResources);

        if (!Page.IsPostBack)
        {
            lblDepositAmount.Text = commonCulture.ElementValues.getResourceString("lblDepositAmount", xeResources);
            lblReferenceId.Text = commonCulture.ElementValues.getResourceString("lblReferenceId", xeResources);
            lblSystemAccount.Text = commonCulture.ElementValues.getResourceString("lblSystemAccount", xeResources);

            //drpDepositDateTime.Items.Add(new ListItem(commonCulture.ElementValues.getResourceString("drpDepositDateTime", xeResources), string.Empty, true));
            lblDepositChannel.Text = commonCulture.ElementValues.getResourceString("lblDepositChannel", xeResources);

            lblBank.Text = commonCulture.ElementValues.getResourceString("lblBank", xeResources);
            lblBankName.Text = commonCulture.ElementValues.getResourceString("lblBankName", xeResources);
            lblAccountName.Text = commonCulture.ElementValues.getResourceString("lblAccountName", xeResources);
            lblAccountNumber.Text = commonCulture.ElementValues.getResourceString("lblAccountNumber", xeResources);

            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);

            #region Placeholder
            txtDepositAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} {1}", lblDepositAmount.Text, strCurrencyCode));
            txtReferenceId.Attributes.Add("PLACEHOLDER", lblReferenceId.Text);
            txtBankName.Attributes.Add("PLACEHOLDER", lblBankName.Text);
            txtAccountName.Attributes.Add("PLACEHOLDER", lblAccountName.Text);
            txtAccountNumber.Attributes.Add("PLACEHOLDER", lblAccountNumber.Text);
            #endregion

            #region PopulateDropDownList

            System.Threading.Tasks.Task t1 = System.Threading.Tasks.Task.Factory.StartNew(this.InitialiseSystemBankAccounts);
            System.Threading.Tasks.Task t2 = System.Threading.Tasks.Task.Factory.StartNew(this.InitialiseMemberBank);
            System.Threading.Tasks.Task t3 = System.Threading.Tasks.Task.Factory.StartNew(this.InitialiseDepositChannel);
            System.Threading.Tasks.Task t4 = System.Threading.Tasks.Task.Factory.StartNew(this.InitialiseDepositDateTime);
            System.Threading.Tasks.Task t5 = System.Threading.Tasks.Task.Factory.StartNew(this.InitialisePaymentLimits);

            #endregion

            System.Threading.Tasks.Task.WaitAll(t1, t2, t3, t4, t5);

            drpSystemAccount.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpSystemAccount", xeResources), "-1"));
            drpDepositChannel.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpDepositChannel", xeResources), "-1"));
            drpBank.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpBank", xeResources), "-1"));
            drpBank.Items.Add(new ListItem(commonCulture.ElementValues.getResourceString("drpOtherBank", xeResources), "OTHER"));

            if (string.Compare(strCurrencyCode, "krw", true) == 0)
            {
                divDepositDateTime.Visible = false;
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsPageRefresh) { Response.Redirect(Request.Url.AbsoluteUri); }

        #region initialiseVariables
        int intProcessSerialId = 0;
        string strProcessId = Guid.NewGuid().ToString().ToUpper();
        string strPageName = "Deposit.FastDeposit";

        string strResultCode = string.Empty;
        string strResultDetail = string.Empty;
        string strErrorCode = string.Empty;
        string strErrorDetail = string.Empty;
        string strProcessRemark = string.Empty;
        bool isProcessAbort = false;
        bool isSystemError = false;

        long lngOperatorId = long.MinValue;
        string strDepositAmount = string.Empty;
        string strReferenceId = string.Empty;
        string strSystemAccount = string.Empty;
        string strDepositDate = string.Empty;
        //string strDepositDay = string.Empty;
        //string strDepositMonth = string.Empty;
        //string strDepositYear = string.Empty;
        string strDepositHour = string.Empty;
        string strDepositMinute = string.Empty;
        string strDepositChannel = string.Empty;
        string strBankCode = string.Empty;
        string strBankName = string.Empty;
        string strBankNameInput = string.Empty;
        string strAccountName = string.Empty;
        string strAccountNumber = string.Empty;

        decimal decMinLimit = decimal.Zero;
        decimal decMaxLimit = decimal.Zero;
        decimal decTotalAllowed = decimal.Zero;
        decimal decDailyLimit = decimal.Zero;

        System.DateTime dtDepositDateTime = System.DateTime.MinValue;
        System.Xml.Linq.XElement xeResponse = null;

        bool isDepositSuccessful = false;
        string strTransferId = string.Empty;
        #endregion

        #region populateVariables
        lngOperatorId = long.Parse(commonVariables.OperatorId);
        strMemberCode = commonVariables.GetSessionVariable("MemberCode");
        strCurrencyCode = commonVariables.GetSessionVariable("CurrencyCode");

        strDepositAmount = txtDepositAmount.Text;
        strReferenceId = txtReferenceId.Text;
        strSystemAccount = drpSystemAccount.SelectedValue;
        strDepositDate = drpDepositDate.SelectedValue;
        //strDepositDay = drpDay.SelectedValue;
        //strDepositMonth = drpMonth.SelectedValue;
        //strDepositYear = drpYear.SelectedValue;
        strDepositHour = drpHour.SelectedValue;
        strDepositMinute = drpMinute.SelectedValue;
        strDepositChannel = drpDepositChannel.SelectedValue;
        strBankCode = drpBank.SelectedValue;
        strBankName = drpBank.SelectedItem.Text;
        strBankNameInput = txtBankName.Text;
        strAccountName = txtAccountName.Text;
        strAccountNumber = txtAccountNumber.Text;

        if (string.Compare(strCurrencyCode, "krw", true) == 0) { dtDepositDateTime = System.DateTime.Now; }
        //else { dtDepositDateTime = System.DateTime.Parse(drpDepositDate.SelectedValue)}//new System.DateTime(Convert.ToInt32(strDepositYear), Convert.ToInt32(strDepositMonth), Convert.ToInt32(strDepositDay), Convert.ToInt32(strDepositHour), Convert.ToInt32(strDepositMinute), 0); }
        #endregion

        #region parametersValidation
        if (string.IsNullOrEmpty(strDepositAmount)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/MissingDepositAmount", xeErrors); return; }
        else if (string.IsNullOrEmpty(strAccountName)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/MissingAccountName", xeErrors); return; }
        else if (string.IsNullOrEmpty(strAccountNumber)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/MissingAccountNumber", xeErrors); return; }
        else if (string.Compare(drpBank.SelectedValue, "OTHER", true) == 0 && string.IsNullOrEmpty(strBankNameInput)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/MissingBankName", xeErrors); return; }
        else if (Convert.ToString(drpSystemAccount.SelectedValue) == "-1") { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/SelectSystemAccount", xeErrors); return; }
        else if (Convert.ToString(drpDepositChannel.SelectedValue) == "-1") { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/SelectDepositChannel", xeErrors); return; }
        else if (Convert.ToString(drpBank.SelectedValue) == "-1") { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/SelectBank", xeErrors); return; }
        else if (!commonValidation.isDecimal(strDepositAmount)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/InvalidDepositAmount", xeErrors); return; }
        else if (Convert.ToDecimal(strDepositAmount) <= 0) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/InvalidDepositAmount", xeErrors); return; }
        else if (commonValidation.isInjection(strDepositAmount)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/InvalidDepositAmount", xeErrors); return; }
        else if (commonValidation.isInjection(strReferenceId)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/InvalidReferenceId", xeErrors); return; }
        else if (commonValidation.isInjection(strBankNameInput)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/InvalidBankName", xeErrors); return; }
        else if (commonValidation.isInjection(strAccountName)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/InvalidAccountName", xeErrors); return; }
        else if (commonValidation.isInjection(strAccountNumber)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/InvalidAccountNumber", xeErrors); return; }
        else if (string.Compare(strCurrencyCode, "krw", true) != 0)
        {
            if (!string.IsNullOrEmpty(strDepositDate))
            {
                dtDepositDateTime = System.DateTime.Parse(strDepositDate).AddHours(double.Parse(strDepositHour)).AddMinutes(double.Parse(strDepositMinute));
                if ((dtDepositDateTime - System.DateTime.Now).TotalHours > 72 || (dtDepositDateTime - System.DateTime.Now).TotalHours < -72) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/InvalidDateTime", xeErrors); return; }
            }
            else if (string.IsNullOrEmpty(strDepositDate)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/InvalidDateTime", xeErrors); return; }
        }

        #endregion

        #region initialiseDeposit
        if (!isProcessAbort)
        {
            try
            {
                string strProcessCode = string.Empty;
                string strProcessText = string.Empty;

                System.Data.DataTable dtPaymentMethodLimits = null;

                using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
                {
                    dtPaymentMethodLimits = svcInstance.getMethodLimits(strOperatorId, strMemberCode, Convert.ToString(Convert.ToInt32(commonVariables.DepositMethod.FastDeposit)), Convert.ToString(Convert.ToInt32(commonVariables.PaymentTransactionType.Deposit)), false, out strProcessCode, out strProcessText);

                    if (dtPaymentMethodLimits.Rows.Count > 0)
                    {
                        decMinLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["minDeposit"]);
                        decMaxLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["maxDeposit"]);
                        decTotalAllowed = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["totalAllowed"]);
                        decDailyLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["limitDaily"]);

                        if (Convert.ToDecimal(strDepositAmount) < decMinLimit)
                        {
                            strAlertCode = "-1";
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/AmountMinLimit", xeErrors);
                            isProcessAbort = true;
                        }
                        else if (Convert.ToDecimal(strDepositAmount) > decMaxLimit) 
                        {
                            strAlertCode = "-1";
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/AmountMaxLimit", xeErrors);
                            isProcessAbort = true;
                        }
                        else if ((Convert.ToDecimal(strDepositAmount) > decTotalAllowed) && (decTotalAllowed != 0))
                        {
                            strAlertCode = "-1";
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/TotalAllowedExceeded", xeErrors);
                            isProcessAbort = true;
                        }
                    }
                }

                if (!isProcessAbort)
                {
                    using (svcPayDeposit.DepositClient svcInstance = new svcPayDeposit.DepositClient())
                    {
                        xeResponse = svcInstance.createFastDepositTransactionV1(lngOperatorId, strMemberCode, strDepositChannel, Convert.ToInt64(commonVariables.DepositMethod.FastDeposit),
                                    strCurrencyCode, Convert.ToDecimal(strDepositAmount), Convert.ToInt64(strSystemAccount), strAccountName, strAccountNumber, dtDepositDateTime,
                                    strReferenceId, strBankCode, strBankName, strBankNameInput, Convert.ToString(commonVariables.TransactionSource.Mobile));

                        if (xeResponse == null)
                        {
                            strAlertCode = "-1";
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/TransferFail", xeErrors);
                        }
                        else
                        {
                            isDepositSuccessful = Convert.ToBoolean(commonCulture.ElementValues.getResourceString("result", xeResponse));
                            strTransferId = commonCulture.ElementValues.getResourceString("invId", xeResponse);

                            if (isDepositSuccessful)
                            {
                                strAlertCode = "0";
                                strAlertMessage = string.Format("{0}\\n{1}: {2}", commonCulture.ElementValues.getResourceXPathString("/Deposit/TransferSuccess", xeErrors), commonCulture.ElementValues.getResourceString("lblTransactionId", xeResources), strTransferId);
                            }
                            else
                            {
                                strAlertCode = "-1";
                                strAlertMessage = string.Format("{0}\\n{1}", commonCulture.ElementValues.getResourceXPathString("/Deposit/TransferFail", xeErrors), commonCulture.ElementValues.getResourceXPathString("/Deposit/error" + strTransferId, xeErrors));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strAlertCode = "-1";
                strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/Exception", xeErrors);

                strErrorDetail = ex.Message;
            }

            strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | DepositAmount: {3} | DepositChannel: {4} | AccountName: {6} | AccountNumber: {6} | SystemAccount: {7} | BankCode: {8} | BankName: {9} | BankNameInput: {10} | ReferenceID: {11} | DepositDateTime: {12} | MinLimit: {13} | MaxLimit: {14} | TotalAllowed: {15} | DailyLimit: {16} | Response: {17}",
                lngOperatorId, strMemberCode, strCurrencyCode, strDepositAmount, strDepositChannel, strAccountName, strAccountNumber, strSystemAccount, strBankCode, strBankName, strBankNameInput, strReferenceId, dtDepositDateTime.ToString("yyyy-MM-dd HH:mm:ss"), decMinLimit, decMaxLimit, decTotalAllowed, decDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", strPageName, "InitiateDeposit", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        }
        #endregion
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

    private void InitialiseSystemBankAccounts()
    {
        string strProcessCode = string.Empty;
        string strProcessText = string.Empty;

        svcPayMS1.SystemBankAccount[] ArrSBA = null;

        using (svcPayMS1.MS1Client svcInstance = new svcPayMS1.MS1Client())
        {
            ArrSBA = svcInstance.getSystemBankAccount(Convert.ToInt64(strOperatorId), strMemberCode, out strProcessCode, out strProcessText);
        }

        drpSystemAccount.DataSource = ArrSBA;
        drpSystemAccount.DataValueField = "AccountId";
        drpSystemAccount.DataTextField = "descriptionExternal";
        drpSystemAccount.DataBind();
    }
    private void InitialiseMemberBank()
    {
        string strProcessCode = string.Empty;
        string strProcessText = string.Empty;

        svcPayMember.MemberBank[] ArrMB = null;

        using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
        {
            ArrMB = svcInstance.getBankAccounts(Convert.ToInt64(strOperatorId), strCurrencyCode, strCountryCode, out strProcessCode, out strProcessText);
        }

        drpBank.DataSource = ArrMB;

        if (xeResources.XPathSelectElement("BankNameNative/" + strSelectedLanguage.ToUpper() + "_" + strCurrencyCode.ToUpper()) != null) { drpBank.DataTextField = "bankNameNative"; } else { drpBank.DataTextField = "bankName"; }
        drpBank.DataValueField = "bankCode";
        //drpBank.DataTextField = "bankNameNative";
        drpBank.DataBind();
    }
    private void InitialiseDepositChannel()
    {
        var links = from link in xeResources.Element("DepositChannel").Descendants() select new { dataValue = link.Name, dataText = link.Value };
        drpDepositChannel.DataSource = links;
        drpDepositChannel.DataValueField = "dataValue";
        drpDepositChannel.DataTextField = "dataText";
        drpDepositChannel.DataBind();
    }
    private void InitialiseDepositDateTime()
    {
        #region DepositDateTime

        drpDepositDate.Items.Add(new ListItem(commonCulture.ElementValues.getResourceString("drpDepositDateTime", xeResources), string.Empty));
        for (System.DateTime dtDepositDateTime = System.DateTime.Today.AddHours(-72); dtDepositDateTime < System.DateTime.Today.AddHours(72); dtDepositDateTime = dtDepositDateTime.AddHours(24))
        {
            drpDepositDate.Items.Add(new ListItem(dtDepositDateTime.ToString("dd / MMM / yyyy"), dtDepositDateTime.ToString("yyyy-MM-dd")));
        }

        for (int intHour = 0; intHour < 24; intHour++) { drpHour.Items.Add(new ListItem((intHour).ToString("0#"), Convert.ToString(intHour))); }
        for (int intMinute = 0; intMinute < 60; intMinute++) { drpMinute.Items.Add(new ListItem((intMinute).ToString("0#"), Convert.ToString(intMinute))); }
        #endregion
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

        strMethodId = Convert.ToString(Convert.ToInt32(commonVariables.DepositMethod.FastDeposit));

        if (dtPaymentMethodLimits.Select("[methodId] = " + strMethodId).Count() > 0)
        {
            drPaymentMethodLimit = dtPaymentMethodLimits.Select("[methodId] = " + strMethodId)[0];

            strMinLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["minDeposit"]).ToString(commonVariables.DecimalFormat);
            strMaxLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["maxDeposit"]).ToString(commonVariables.DecimalFormat);
            strTotalAllowed = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["totalAllowed"]) == 0 ? commonCulture.ElementValues.getResourceString("unlimited", xeResources) : Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["totalAllowed"]).ToString(commonVariables.DecimalFormat);
            strDailyLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["limitDaily"]).ToString(commonVariables.DecimalFormat);
            txtDepositAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} {1}({2} / {3})", lblDepositAmount.Text, strCurrencyCode, strMinLimit, strMaxLimit));
            lblDailyLimit.Text = string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("lblDailyLimit", xeResources), strDailyLimit);
            lblTotalAllowed.Text = string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("lblTotalAllowed", xeResources), strTotalAllowed);
        }

        strMethodsUnAvailable = Convert.ToString(sbMethodsUnavailable).TrimEnd('|');
    }
}