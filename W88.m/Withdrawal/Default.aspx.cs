using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.XPath;

public partial class Withdrawal_Default : PaymentBasePage
{
    protected string strStatusCode = string.Empty;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = "BankTransfer";
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.WithdrawalMethod.BankTransfer);

        base.CheckLogin();
        base.InitialiseVariables();

        base.InitialisePaymentLimits();

        base.GetMainWalletBalance("0");

        base.InitialisePendingWithdrawals();

        this.InitialiseMemberBank();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CancelUnexpectedRePost();

        HtmlGenericControl withdrawalTabs = (HtmlGenericControl)FindControl("withdrawalTabs");
        commonPaymentMethodFunc.getWithdrawalMethodList(strMethodsUnAvailable, withdrawalTabs, base.PageName, sender.ToString().Contains("app") || Request.QueryString["source"] == "app");

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

            drpBank.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpBank", xeResources), "-1"));
            drpBank.Items.Add(new ListItem(commonCulture.ElementValues.getResourceString("drpOtherBank", xeResources), "OTHER"));

            txtWithdrawAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} {1}({2} / {3})", lblWithdrawAmount.Text, strCurrencyCode, strMinLimit, strMaxLimit));
            lblDailyLimit.Text = string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("lblDailyLimit", xeResources), strDailyLimit);
            lblTotalAllowed.Text = string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("lblTotalAllowed", xeResources), strTotalAllowed);

            if (string.Compare(strCurrencyCode, "krw", true) == 0)
            {
                divBankBranch.Visible = false;
                divAddress.Visible = false;
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsPageRefresh)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        string strWithdrawalAmount = txtWithdrawAmount.Text.Trim();
        string strBankCode = drpBank.SelectedValue;
        string strBankName = drpBank.SelectedItem.Text;
        string strBankNameInput = txtBankName.Text;
        string strBankAddress = txtAddress.Text;
        string strBankBranch = txtBankBranch.Text;
        string strAccountName = txtAccountName.Text;
        string strAccountNumber = txtAccountNumber.Text;
        string strMyKad = txtMyKad.Text;

        decimal decWithdrawalAmount = commonValidation.isDecimal(strWithdrawalAmount) ? Convert.ToDecimal(strWithdrawalAmount) : 0;
        decimal decMinLimit = Convert.ToDecimal(strMinLimit);
        decimal decMaxLimit = Convert.ToDecimal(strMaxLimit);

        #region initialiseWithdrawal
        if (!isProcessAbort)
        {
            try
            {
                if (string.Compare(strCurrencyCode, "myr", true) == 0)
                {
                    if (string.IsNullOrEmpty(strMyKad))
                    {
                        strAlertCode = "-1";
                        strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/MissingMyKad", xeErrors);
                        isProcessAbort = true;
                    }
                    else if (commonValidation.isInjection(strMyKad))
                    {
                        strAlertCode = "-1";
                        strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/InvalidMyKad", xeErrors);
                        isProcessAbort = true;
                    }
                }
                else if (decWithdrawalAmount == 0)
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
                else if (Convert.ToString(drpBank.SelectedValue) == "-1")
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/SelectBank", xeErrors);
                    isProcessAbort = true;
                }
                else if (string.Compare(drpBank.SelectedValue, "OTHER", true) == 0 && string.IsNullOrEmpty(strBankNameInput))
                {
                    strAlertCode = "-1"; 
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/MissingBankName", xeErrors);
                    isProcessAbort = true;
                }
                else if (commonValidation.isInjection(strBankNameInput))
                {
                    strAlertCode = "-1"; 
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/InvalidBankName", xeErrors);
                    isProcessAbort = true;
                }
                else if (string.IsNullOrEmpty(strBankBranch) && string.Compare(strCurrencyCode, "krw", true) != 0)
                {
                    strAlertCode = "-1"; 
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/MissingBankBranch", xeErrors);
                    isProcessAbort = true;
                }
                else if (commonValidation.isInjection(strBankBranch))
                {
                    strAlertCode = "-1"; 
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/InvalidBankBranch", xeErrors);
                    isProcessAbort = true;
                }
                else if (string.IsNullOrEmpty(strBankAddress) && string.Compare(strCurrencyCode, "krw", true) != 0)
                {
                    strAlertCode = "-1"; 
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/MissingBankAddress", xeErrors);
                    isProcessAbort = true;
                }
                else if (commonValidation.isInjection(strBankAddress))
                {
                    strAlertCode = "-1"; 
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/InvalidBankAddress", xeErrors);
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
                        xeResponse = svcInstance.createBankTransferTransactionV1(Convert.ToInt64(strOperatorId), strMemberCode, Convert.ToInt64(commonVariables.WithdrawalMethod.BankTransfer),
                                        strCurrencyCode, decWithdrawalAmount, strAccountName, strAccountNumber, strBankAddress, strBankBranch, strBankCode, strBankName, strBankNameInput,
                                        strMyKad, string.Empty, false, Convert.ToString(commonVariables.TransactionSource.Mobile));

                        if (xeResponse == null)
                        {
                            strAlertCode = "-1";
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferFail", xeErrors);
                        }
                        else
                        {
                            bool isTransactionSuccessful = Convert.ToBoolean(commonCulture.ElementValues.getResourceString("result", xeResponse));
                            string strTransferId = commonCulture.ElementValues.getResourceString("invId", xeResponse);

                            if (isTransactionSuccessful)
                            {
                                strAlertCode = "0";
                                strAlertMessage = string.Format("{0}\\n{1}: {2}", commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferSuccess", xeErrors), strlblTransactionId, strTransferId);
                            }
                            else
                            {
                                strAlertCode = "-1";
                                strAlertMessage = string.Format("{0}\\n{1}", commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferFail", xeErrors), commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/error" + strTransferId, xeErrors));
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                strAlertCode = "-1";
                strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/Exception", xeErrors);

                strErrorDetail = ex.Message;
            }

            string strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | WithdrawAmount: {3} | AccountName: {4} | AccountNumber: {5} | BankAddress: {6} | BankBranch: {7} | BankCode: {8} | BankName: {9} | BankNameInput: {10} | MyKad: {11} | Mobile: {12} | MinLimit: {13} | MaxLimit: {14} | TotalAllowed: {15} | DailyLimit: {16} | Response: {17}",
                                    Convert.ToInt64(strOperatorId), strMemberCode, strCurrencyCode, strWithdrawalAmount, strAccountName, strAccountNumber, strBankAddress, strBankBranch, strBankCode, strBankName, strBankNameInput, strMyKad, string.Empty, decMinLimit, decMaxLimit, strTotalAllowed, strDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", PageName, "InitiateWithdrawal", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        }
        #endregion
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
