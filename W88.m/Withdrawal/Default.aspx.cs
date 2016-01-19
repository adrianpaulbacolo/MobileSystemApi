using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.XPath;

public partial class Withdrawal_Default : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected System.Xml.Linq.XElement xeWallet = null;
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

    protected void Page_Init(object sender, EventArgs e)
    {
        base.CheckLogin();

        System.Web.UI.WebControls.Literal litScript = (System.Web.UI.WebControls.Literal)Page.FindControl("litScript");

        strOperatorId = commonVariables.OperatorId;
        strMemberCode = commonVariables.GetSessionVariable("MemberCode");

        string strStatusCode = string.Empty;
        string strStatusText = string.Empty;

        svcPayMember.PendingWithdrawal[] arrPending = null;

        using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
        {
            arrPending = svcInstance.getPendingWithdrawal(Convert.ToInt64(strOperatorId), strMemberCode, out strStatusCode, out strStatusText);
        }

        if (arrPending != null && arrPending.Length > 0)
        {
            if (Request.QueryString["source"] == "app")
                Response.Redirect("/Withdrawal/Pending_app.aspx");
            else
                Response.Redirect("/Withdrawal/Pending.aspx");
        }
    }

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
        commonCulture.appData.getRootResource("/Withdrawal/BankTransfer", out xeResources);

        if (!Page.IsPostBack)
        {
            lblWithdrawAmount.Text = commonCulture.ElementValues.getResourceString("lblAmount", xeResources);

            lblBank.Text = commonCulture.ElementValues.getResourceString("lblBank", xeResources);
            lblBankName.Text = commonCulture.ElementValues.getResourceString("lblBankName", xeResources);
            lblBankBranch.Text = commonCulture.ElementValues.getResourceString("lblBankBranch", xeResources);
            lblAddress.Text = commonCulture.ElementValues.getResourceString("lblAddress", xeResources);
            lblAccountName.Text = commonCulture.ElementValues.getResourceString("lblAccountName", xeResources);
            lblAccountNumber.Text = commonCulture.ElementValues.getResourceString("lblAccountNumber", xeResources);
            lblMyKad.Text = commonCulture.ElementValues.getResourceString("lblMyKad", xeResources);
            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);

            #region PopulateDropDownList

            System.Threading.Tasks.Task t1 = System.Threading.Tasks.Task.Factory.StartNew(this.InitialisePaymentLimits);
            System.Threading.Tasks.Task t2 = System.Threading.Tasks.Task.Factory.StartNew(this.InitialiseMemberBank);
            //System.Threading.Tasks.Task t3 = System.Threading.Tasks.Task.Factory.StartNew(this.InitialiseDepositChannel);
            //System.Threading.Tasks.Task t4 = System.Threading.Tasks.Task.Factory.StartNew(this.InitialiseDepositDateTime);
            #endregion

            System.Threading.Tasks.Task.WaitAll(t1, t2);

            drpBank.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpBank", xeResources), "-1"));
            drpBank.Items.Add(new ListItem(commonCulture.ElementValues.getResourceString("drpOtherBank", xeResources), "OTHER"));

            if (string.Compare(strCurrencyCode, "krw", true) == 0)
            {
                divBankBranch.Visible = false;
                divAddress.Visible = false;
            }

            getMainWalletBalance("0");

        }

        HtmlGenericControl withdrawalTabs = (HtmlGenericControl)FindControl("withdrawalTabs");
        commonPaymentMethodFunc.getWithdrawalMethodList(strMethodsUnAvailable, withdrawalTabs, "default", sender.ToString().Contains("app"));
    }

    private void getMainWalletBalance(string walletId)
    {
        string strOperatorId = commonVariables.OperatorId;
        string strMemberCode = commonVariables.GetSessionVariable("MemberCode");
        string strSiteUrl = commonVariables.SiteUrl;

        string strProductCurrency = string.Empty;

        if (!string.IsNullOrEmpty(strMemberCode) && !string.IsNullOrEmpty(strOperatorId))
        {
            using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
            {
                Session["MAIN"] = svcInstance.getWalletBalance(strOperatorId, strSiteUrl, strMemberCode, walletId, out strProductCurrency);
            }
        }
        else
        {
            Session["MAIN"] = "0.00";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsPageRefresh) { Response.Redirect(Request.Url.AbsoluteUri); }

        #region initialiseVariables
        int intProcessSerialId = 0;
        string strProcessId = Guid.NewGuid().ToString().ToUpper();
        string strPageName = "Withdrawal.BankTransfer";

        string strResultCode = string.Empty;
        string strResultDetail = string.Empty;
        string strErrorCode = string.Empty;
        string strErrorDetail = string.Empty;
        string strProcessRemark = string.Empty;
        bool isProcessAbort = false;
        bool isSystemError = false;

        long lngOperatorId = long.MinValue;
        string strWithdrawAmount = string.Empty;
        string strBankCode = string.Empty;
        string strBankName = string.Empty;
        string strBankNameInput = string.Empty;
         string strBankBranch = string.Empty;
        string strBankAddress = string.Empty;
        string strAccountName = string.Empty;
        string strAccountNumber = string.Empty;
        string strMyKad = string.Empty;
        string strMobileNumber = string.Empty;
        bool MobileNotify = false;

        decimal decMinLimit = decimal.Zero;
        decimal decMaxLimit = decimal.Zero;
        decimal decTotalAllowed = decimal.Zero;
        decimal decDailyLimit = decimal.Zero;

        System.Xml.Linq.XElement xeResponse = null;

        bool isWithdrawSuccessful = false;
        string strTransferId = string.Empty;
        #endregion

        #region populateVariables
        lngOperatorId = long.Parse(commonVariables.OperatorId);
        strMemberCode = commonVariables.GetSessionVariable("MemberCode");
        strCurrencyCode = commonVariables.GetSessionVariable("CurrencyCode");

        strWithdrawAmount = txtWithdrawAmount.Text;
        strBankCode = drpBank.SelectedValue;
        strBankName = drpBank.SelectedItem.Text;
        strBankNameInput = txtBankName.Text;
        strBankAddress = txtAddress.Text;
        strBankBranch = txtBankBranch.Text;
        strAccountName = txtAccountName.Text;
        strAccountNumber = txtAccountNumber.Text;
        strMyKad = txtMyKad.Text;

        System.Text.RegularExpressions.Regex regxATM = new System.Text.RegularExpressions.Regex("([0-9]{16})$");

        #endregion

        #region parametersValidation
        if (string.IsNullOrEmpty(strWithdrawAmount)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/MissingWithdrawalAmount", xeErrors); return; }
        else if (string.IsNullOrEmpty(strAccountName)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/MissingAccountName", xeErrors); return; }
        else if (string.IsNullOrEmpty(strAccountNumber)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/MissingAccountNumber", xeErrors); return; }
        else if (string.Compare(drpBank.SelectedValue, "OTHER", true) == 0 && string.IsNullOrEmpty(strBankNameInput)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/MissingBankName", xeErrors); return; }
        //else if (string.Compare(drpBank.SelectedValue, "VIETIN", true) == 0 && (!commonValidation.isNumeric(strAccountNumber) || !regxATM.IsMatch(strAccountNumber))) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/InvalidAccountNumber", xeErrors); return; }
        else if (string.IsNullOrEmpty(strBankBranch) && string.Compare(strCurrencyCode, "krw", true) != 0) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/MissingBankBranch", xeErrors); return; }
        else if (string.IsNullOrEmpty(strBankAddress) && string.Compare(strCurrencyCode, "krw", true) != 0) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/MissingBankAddress", xeErrors); return; }
        else if (string.Compare(strCurrencyCode, "myr", true) == 0) { if (string.IsNullOrEmpty(strMyKad)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/MissingAccountNumber", xeErrors); return; } }
        else if (Convert.ToString(drpBank.SelectedValue) == "-1") { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/SelectBank", xeErrors); return; }
        else if (!commonValidation.isDecimal(strWithdrawAmount)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/InvalidWithdrawAmount", xeErrors); return; }
        else if (Convert.ToDecimal(strWithdrawAmount) <= 0) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/InvalidWithdrawAmount", xeErrors); return; }
        else if (commonValidation.isInjection(strWithdrawAmount)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/InvalidWithdrawAmount", xeErrors); return; }
        else if (commonValidation.isInjection(strBankNameInput)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/InvalidBankName", xeErrors); return; }
        else if (commonValidation.isInjection(strAccountName)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/InvalidAccountName", xeErrors); return; }
        else if (commonValidation.isInjection(strAccountNumber)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/InvalidAccountNumber", xeErrors); return; }
        else if (commonValidation.isInjection(strBankBranch)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/InvalidBankBranch", xeErrors); return; }
        else if (commonValidation.isInjection(strBankAddress)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/InvalidBankAddress", xeErrors); return; }
        else if (string.Compare(strCurrencyCode, "myr", true) == 0) { if (commonValidation.isInjection(strMyKad)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/MissingMyKad", xeErrors); return; } }
        #endregion

        #region initialiseWithdrawal
        if (!isProcessAbort)
        {
            try
            {
                string strProcessCode = string.Empty;
                string strProcessText = string.Empty;

                System.Data.DataTable dtPaymentMethodLimits = null;

                using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
                {
                    dtPaymentMethodLimits = svcInstance.getMethodLimits(strOperatorId, strMemberCode, Convert.ToString(Convert.ToInt32(commonVariables.WithdrawalMethod.BankTransfer)), Convert.ToString(Convert.ToInt32(commonVariables.PaymentTransactionType.Withdrawal)), false, out strProcessCode, out strProcessText);

                    if (dtPaymentMethodLimits.Rows.Count > 0)
                    {
                        decMinLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["minWithdrawal"]);
                        decMaxLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["maxWithdrawal"]);
                        decTotalAllowed = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["totalAllowed"]);
                        decDailyLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["limitDaily"]);

                        if (Convert.ToDecimal(strWithdrawAmount) < decMinLimit)
                        {
                            strAlertCode = "-1";
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/AmountMinLimit", xeErrors);
                            isProcessAbort = true;
                        }
                        else if (Convert.ToDecimal(strWithdrawAmount) > decMaxLimit)
                        {
                            strAlertCode = "-1";
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/AmountMaxLimit", xeErrors);
                            isProcessAbort = true;
                        }
                        else if (Convert.ToDecimal(strWithdrawAmount) > decTotalAllowed)
                        {
                            strAlertCode = "-1";
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/TotalAllowedExceeded", xeErrors);
                            isProcessAbort = true;
                        }
                    }
                }

                if (!isProcessAbort)
                {
                    using (svcPayWithdrawal.WithdrawalClient svcInstance = new svcPayWithdrawal.WithdrawalClient())
                    {
                        xeResponse = svcInstance.createBankTransferTransactionV1(lngOperatorId, strMemberCode, Convert.ToInt64(commonVariables.WithdrawalMethod.BankTransfer),
                                        strCurrencyCode, Convert.ToDecimal(strWithdrawAmount), strAccountName, strAccountNumber, strBankAddress, strBankBranch, strBankCode, strBankName, strBankNameInput,
                                        strMyKad, strMobileNumber, MobileNotify, Convert.ToString(commonVariables.TransactionSource.Mobile));

                        if (xeResponse == null)
                        {
                            strAlertCode = "-1";
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/TransferFail", xeErrors);
                        }
                        else
                        {
                            isWithdrawSuccessful = Convert.ToBoolean(commonCulture.ElementValues.getResourceString("result", xeResponse));
                            strTransferId = commonCulture.ElementValues.getResourceString("invId", xeResponse);

                            if (isWithdrawSuccessful)
                            {
                                strAlertCode = "0";
                                strAlertMessage = string.Format("{0}\\n{1}: {2}", commonCulture.ElementValues.getResourceXPathString("/Withdrawal/TransferSuccess", xeErrors), commonCulture.ElementValues.getResourceString("lblTransactionId", xeResources), strTransferId);
                            }
                            else
                            {
                                strAlertCode = "-1";
                                strAlertMessage = string.Format("{0}\\n{1}", commonCulture.ElementValues.getResourceXPathString("/Withdrawal/TransferFail", xeErrors), commonCulture.ElementValues.getResourceXPathString("/Withdrawal/error" + strTransferId, xeErrors));
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                strAlertCode = "-1";
                strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/Exception", xeErrors);

                strErrorDetail = ex.Message;
            }

            strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | WithdrawAmount: {3} | AccountName: {4} | AccountNumber: {5} | BankAddress: {6} | BankBranch: {7} | BankCode: {8} | BankName: {9} | BankNameInput: {10} | MyKad: {11} | Mobile: {12} | MinLimit: {13} | MaxLimit: {14} | TotalAllowed: {15} | DailyLimit: {16} | Response: {17}",
                                    lngOperatorId, strMemberCode, strCurrencyCode, strWithdrawAmount, strAccountName, strAccountNumber, strBankAddress, strBankBranch, strBankCode, strBankName, strBankNameInput, strMyKad, strMobileNumber, decMinLimit, decMaxLimit, decTotalAllowed, decDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", strPageName, "InitiateWithdrawal", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        }
        getMainWalletBalance("0");
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
            dtPaymentMethodLimits = svcInstance.getMethodLimits(strOperatorId, strMemberCode, strMethodId, Convert.ToString(Convert.ToInt32(commonVariables.PaymentTransactionType.Withdrawal)), false, out strProcessCode, out strProcessText);
        }

        foreach (commonVariables.WithdrawalMethod EnumMethod in Enum.GetValues(typeof(commonVariables.WithdrawalMethod)))
        {
            if (dtPaymentMethodLimits.Select("[methodId] = " + Convert.ToInt32(EnumMethod)).Count() < 1)
            {
                sbMethodsUnavailable.AppendFormat("{0}|", Convert.ToInt32(EnumMethod));
            }
        }

        strMethodId = Convert.ToString(Convert.ToInt32(commonVariables.WithdrawalMethod.BankTransfer));

        if (dtPaymentMethodLimits.Select("[methodId] = " + strMethodId).Count() > 0)
        {
            drPaymentMethodLimit = dtPaymentMethodLimits.Select("[methodId] = " + strMethodId)[0];

            strMinLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["minDeposit"]).ToString(commonVariables.DecimalFormat);
            strMaxLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["maxDeposit"]).ToString(commonVariables.DecimalFormat);
            strTotalAllowed = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["totalAllowed"]).ToString(commonVariables.DecimalFormat);
            strDailyLimit = Convert.ToDecimal(dtPaymentMethodLimits.Rows[0]["limitDaily"]).ToString(commonVariables.DecimalFormat);

            txtWithdrawAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} {1}({2} / {3})", lblWithdrawAmount.Text, strCurrencyCode, strMinLimit, strMaxLimit));
            lblDailyLimit.Text = string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("lblDailyLimit", xeResources), strDailyLimit);
            lblTotalAllowed.Text = string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("lblTotalAllowed", xeResources), strTotalAllowed);
        }
        //else { }

        strMethodsUnAvailable = Convert.ToString(sbMethodsUnavailable).TrimEnd('|');
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
}
