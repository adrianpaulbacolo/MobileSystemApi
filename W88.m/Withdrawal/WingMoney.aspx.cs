using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.XPath;

public partial class Withdrawal_WingMoney : BasePage
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
            if (litScript != null) { litScript.Text += "<script type='text/javascript'>window.location.replace('/Withdrawal/Pending.aspx');</script>"; }
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
        commonCulture.appData.getRootResource("/Withdrawal/WingMoney", out xeResources);

        if (!Page.IsPostBack)
        {
            lblWithdrawAmount.Text = commonCulture.ElementValues.getResourceString("lblAmount", xeResources);

            lblAccountName.Text = commonCulture.ElementValues.getResourceString("lblAccountName", xeResources);
            lblAccountNumber.Text = commonCulture.ElementValues.getResourceString("lblAccountNumber", xeResources);
            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);

            // #region Placeholder
            // txtWithdrawAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} {1}", lblWithdrawAmount.Text, strCurrencyCode));
            // txtAccountName.Attributes.Add("PLACEHOLDER", lblAccountName.Text);
            // txtAccountNumber.Attributes.Add("PLACEHOLDER", lblAccountNumber.Text);
            // #endregion

            #region PopulateDropDownList
            System.Threading.Tasks.Task t1 = System.Threading.Tasks.Task.Factory.StartNew(this.InitialisePaymentLimits);
            #endregion

            System.Threading.Tasks.Task.WaitAll(t1);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsPageRefresh) { Response.Redirect(Request.Url.AbsoluteUri); }

        #region initialiseVariables
        int intProcessSerialId = 0;
        string strProcessId = Guid.NewGuid().ToString().ToUpper();
        string strPageName = "Withdrawal.WingMoney";

        string strResultCode = string.Empty;
        string strResultDetail = string.Empty;
        string strErrorCode = string.Empty;
        string strErrorDetail = string.Empty;
        string strProcessRemark = string.Empty;
        bool isProcessAbort = false;
        bool isSystemError = false;

        long lngOperatorId = long.MinValue;
        string strWithdrawAmount = string.Empty;
        string strAccountName = string.Empty;
        string strAccountNumber = string.Empty;
        string strMobileNumber = string.Empty;

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
        strAccountName = txtAccountName.Text;
        strAccountNumber = txtAccountNumber.Text;

        System.Text.RegularExpressions.Regex regxATM = new System.Text.RegularExpressions.Regex("([0-9]{16})$");

        #endregion

        #region parametersValidation
        if (string.IsNullOrEmpty(strWithdrawAmount)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/MissingWithdrawalAmount", xeErrors); return; }
        else if (string.IsNullOrEmpty(strAccountName)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/MissingAccountName", xeErrors); return; }
        else if (string.IsNullOrEmpty(strAccountNumber)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/MissingAccountNumber", xeErrors); return; }
        else if (!commonValidation.isDecimal(strWithdrawAmount)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/InvalidWithdrawAmount", xeErrors); return; }
        else if (Convert.ToDecimal(strWithdrawAmount) <= 0) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/InvalidWithdrawAmount", xeErrors); return; }
        else if (commonValidation.isInjection(strWithdrawAmount)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/InvalidWithdrawAmount", xeErrors); return; }
        else if (commonValidation.isInjection(strAccountName)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/InvalidAccountName", xeErrors); return; }
        else if (commonValidation.isInjection(strAccountNumber)) { strAlertCode = "-1"; strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Withdrawal/InvalidAccountNumber", xeErrors); return; }
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
                    dtPaymentMethodLimits = svcInstance.getMethodLimits(strOperatorId, strMemberCode, Convert.ToString(Convert.ToInt32(commonVariables.WithdrawalMethod.WingMoney)), Convert.ToString(Convert.ToInt32(commonVariables.PaymentTransactionType.Withdrawal)), false, out strProcessCode, out strProcessText);

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
                        xeResponse = svcInstance.createWingMoneyTransactionV1(lngOperatorId, strMemberCode, Convert.ToInt64(commonVariables.WithdrawalMethod.WingMoney),
                                        strCurrencyCode, Convert.ToDecimal(strWithdrawAmount), strAccountName, strAccountNumber, strMobileNumber, Convert.ToString(commonVariables.TransactionSource.Mobile));

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

            strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | WithdrawAmount: {3} | AccountName: {4} | AccountNumber: {5} | Mobile: {6} | MinLimit: {7} | MaxLimit: {8} | TotalAllowed: {9} | DailyLimit: {10} | Response: {11}",
                                    lngOperatorId, strMemberCode, strCurrencyCode, strWithdrawAmount, strAccountName, strAccountNumber, strMobileNumber, decMinLimit, decMaxLimit, decTotalAllowed, decDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", strPageName, "InitiateWithdrawal", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
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

        strMethodId = Convert.ToString(Convert.ToInt32(commonVariables.WithdrawalMethod.WingMoney));

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
}
