using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.XPath;

public partial class Withdrawal_WingMoney : PaymentBasePage
{
    protected string strStatusCode = string.Empty;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.WithdrawalMethod.WingMoney);
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.WithdrawalMethod.WingMoney);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack) base.InitialisePendingWithdrawals(sender.ToString().Contains("app"));

        HtmlGenericControl withdrawalTabs = (HtmlGenericControl)FindControl("withdrawalTabs");
        commonPaymentMethodFunc.GetWithdrawalMethodList(strMethodsUnAvailable, withdrawalTabs, base.PageName, sender.ToString().Contains("app"));

        if (!Page.IsPostBack)
        {
            InitializeLabels();
        }
    }

    private void InitializeLabels()
    {
        lblMode.Text = base.strlblMode;
        txtMode.Text = base.strtxtMode;

        lblMinMaxLimit.Text = base.strlblMinMaxLimit;
        txtMinMaxLimit.Text = base.strtxtMinMaxLimit;

        lblDailyLimit.Text = base.strlblDailyLimit;
        txtDailyLimit.Text = base.strtxtDailyLimit;

        lblTotalAllowed.Text = base.strlblTotalAllowed;
        txtTotalAllowed.Text = base.strtxtTotalAllowed;

        lblWithdrawAmount.Text = base.strlblAmount;

        lblAccountName.Text = base.strlblAccountName;
        lblAccountNumber.Text = base.strlblAccountNumber;

        btnSubmit.Text = base.strbtnSubmit;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        string strWithdrawalAmount = txtWithdrawAmount.Text;
        string strAccountName = txtAccountName.Text;
        string strAccountNumber = txtAccountNumber.Text;

        decimal decWithdrawalAmount = commonValidation.isDecimal(strWithdrawalAmount) ? Convert.ToDecimal(strWithdrawalAmount) : 0;
        decimal decMinLimit = commonValidation.isDecimal(strMinLimit) ? Convert.ToDecimal(strMinLimit) : 0;
        decimal decMaxLimit = commonValidation.isDecimal(strMaxLimit) ? Convert.ToDecimal(strMaxLimit) : 0;

        #region initialiseWithdrawal
        if (!isProcessAbort)
        {
            try
            {
                if (decWithdrawalAmount == 0)
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/MissingWithdrawalAmount", xeErrors);
                    isProcessAbort = true;
                }
                else if (commonValidation.isInjection(strWithdrawalAmount))
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/InvalidWithdrawAmount", xeErrors);
                    isProcessAbort = true;
                }
                else if (string.IsNullOrEmpty(strAccountName))
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/MissingAccountName", xeErrors);
                    isProcessAbort = true;
                }
                else if (commonValidation.isInjection(strAccountName))
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/InvalidAccountName", xeErrors);
                    isProcessAbort = true;
                }
                else if (string.IsNullOrEmpty(strAccountNumber))
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/MissingAccountNumber", xeErrors);
                    isProcessAbort = true;
                }
                else if (commonValidation.isInjection(strAccountNumber))
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/InvalidAccountNumber", xeErrors);
                    isProcessAbort = true;
                }
                else if (decWithdrawalAmount < decMinLimit)
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/AmountMinLimit", xeErrors);
                    isProcessAbort = true;
                }
                else if (decWithdrawalAmount > decMaxLimit)
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/AmountMaxLimit", xeErrors);
                    isProcessAbort = true;
                }
                else if ((strTotalAllowed != commonCulture.ElementValues.getResourceString("unlimited", xeResources)) && (decWithdrawalAmount > Convert.ToDecimal(strTotalAllowed)) && Convert.ToDecimal(strTotalAllowed) > 0)
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TotalAllowedExceeded", xeErrors);
                    isProcessAbort = true;
                }

                if (!isProcessAbort)
                {
                    using (svcPayWithdrawal.WithdrawalClient svcInstance = new svcPayWithdrawal.WithdrawalClient())
                    {
                        xeResponse = svcInstance.createWingMoneyTransactionV1(Convert.ToInt64(strOperatorId), strMemberCode, Convert.ToInt64(commonVariables.WithdrawalMethod.WingMoney),
                                        strCurrencyCode, decWithdrawalAmount, strAccountName, strAccountNumber, string.Empty, Convert.ToString(commonVariables.TransactionSource.Mobile));

                        if (xeResponse == null)
                        {
                            strAlertCode = "-1";
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() +  "/TransferFail", xeErrors);
                        }
                        else
                        {
                            bool isWithdrawSuccessful = Convert.ToBoolean(commonCulture.ElementValues.getResourceString("result", xeResponse));
                            string strTransferId = commonCulture.ElementValues.getResourceString("invId", xeResponse);

                            if (isWithdrawSuccessful)
                            {
                                strAlertCode = "0";
                                strAlertMessage = string.Format("{0}\\n{1}: {2}", commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferSuccess", xeErrors), strlblTransactionId, strTransferId);
                            }
                            else
                            {
                                strAlertCode = "-1";
                                strAlertMessage = string.Format("{0}\\n{1}", commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferFail", xeErrors), commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() +  "/error" + strTransferId, xeErrors));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strAlertCode = "-1";
                strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() +  "/Exception", xeErrors);

                strErrorDetail = ex.Message;
            }

            string strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | WithdrawAmount: {3} | AccountName: {4} | AccountNumber: {5} | Mobile: {6} | MinLimit: {7} | MaxLimit: {8} | TotalAllowed: {9} | DailyLimit: {10} | Response: {11}",
                                     Convert.ToInt64(strOperatorId), strMemberCode, strCurrencyCode, strWithdrawalAmount, strAccountName, strAccountNumber, string.Empty, decMinLimit, decMaxLimit, strTotalAllowed, strDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", PageName, "InitiateWithdrawal", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        }
        #endregion
    }
}
