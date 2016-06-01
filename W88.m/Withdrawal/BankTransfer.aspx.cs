using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.XPath;

public partial class Withdrawal_BankTransfer : PaymentBasePage
{
    protected string strStatusCode = string.Empty;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.WithdrawalMethod.BankTransfer);
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.WithdrawalMethod.BankTransfer);

        base.CheckLogin();
        base.InitialiseVariables();

        base.InitialisePaymentLimits();

        base.GetMainWalletBalance("0");

        base.InitialisePendingWithdrawals(sender.ToString().Contains("app"));

        this.InitialiseMemberBank();
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        HtmlGenericControl withdrawalTabs = (HtmlGenericControl)FindControl("withdrawalTabs");
        commonPaymentMethodFunc.GetWithdrawalMethodList(strMethodsUnAvailable, withdrawalTabs, base.PageName, sender.ToString().Contains("app"));

        if (string.Compare(strCurrencyCode, "krw", true) == 0)
        {
            divBankBranch.Visible = false;
            divAddress.Visible = false;
        }

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
        txtMinMaxLimit.Text = base.strtxtMinMaxLimit;

        lblDailyLimit.Text = base.strlblDailyLimit;
        txtDailyLimit.Text = base.strtxtDailyLimit;

        lblTotalAllowed.Text = base.strlblTotalAllowed;
        txtTotalAllowed.Text = base.strtxtTotalAllowed;

        lblWithdrawAmount.Text = base.strlblAmount;
        txtWithdrawAmount.Attributes.Add("PLACEHOLDER", base.strtxtAmount);

        lblAccountName.Text = base.strlblAccountName;
        lblAccountNumber.Text = base.strlblAccountNumber;

        lblBank.Text = base.strlblBank;
        lblBankName.Text = base.strlblBankName;
        lblBankBranch.Text = commonCulture.ElementValues.getResourceString("lblBankBranch", xeResources);

        lblAddress.Text = commonCulture.ElementValues.getResourceString("lblAddress", xeResources);

        btnSubmit.Text = base.strbtnSubmit;
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        string strWithdrawalAmount = txtWithdrawAmount.Text.Trim();
        string strBankCode = drpBank.SelectedValue;
        string strBankName = drpBank.SelectedItem.Text;
        string strBankNameInput = txtBankName.Text;
        string strBankAddress = txtAddress.Text;
        string strBankBranch = txtBankBranch.Text;
        string strAccountName = txtAccountName.Text;
        string strAccountNumber = txtAccountNumber.Text;
        string strMyKad = string.Empty; //txtMyKad.Text;

        decimal decWithdrawalAmount = commonValidation.isDecimal(strWithdrawalAmount) ? Convert.ToDecimal(strWithdrawalAmount) : 0;
        decimal decMinLimit = Convert.ToDecimal(strMinLimit);
        decimal decMaxLimit = Convert.ToDecimal(strMaxLimit);

        #region initialiseWithdrawal

        CommonStatus status = new CommonStatus();

        try
        {
            //if (string.Compare(strCurrencyCode, "myr", true) == 0)
            //{
            //    if (string.IsNullOrEmpty(strMyKad))
            //    {
            //        status = base.GetErrors("/MissingMyKad");
            //    }
            //    else if (commonValidation.isInjection(strMyKad))
            //    {
            //        status = base.GetErrors("/InvalidMyKad");
            //    }
            //} else
            if (decWithdrawalAmount == 0)
            {
                status = base.GetErrors("/MissingWithdrawAmount");
            }
            else if (commonValidation.isInjection(strWithdrawalAmount))
            {
                status = base.GetErrors("/InvalidWithdrawAmount");
            }
            else if (string.IsNullOrEmpty(strAccountName))
            {
                status = base.GetErrors("/MissingAccountName");
            }
            else if (commonValidation.isInjection(strAccountName))
            {
                status = base.GetErrors("/InvalidAccountName");
            }
            else if (string.IsNullOrEmpty(strAccountNumber))
            {
                status = base.GetErrors("/MissingAccountNumber");
            }
            else if (commonValidation.isInjection(strAccountNumber))
            {
                status = base.GetErrors("/InvalidAccountNumber");
            }
            else if (Convert.ToString(drpBank.SelectedValue) == "-1")
            {
                status = base.GetErrors("/SelectBank");
            }
            else if (string.Compare(drpBank.SelectedValue, "OTHER", true) == 0 && string.IsNullOrEmpty(strBankNameInput))
            {
                status = base.GetErrors("/MissingBankName");
            }
            else if (commonValidation.isInjection(strBankNameInput))
            {
                status = base.GetErrors("/InvalidBankName");
            }
            else if (string.IsNullOrEmpty(strBankBranch) && string.Compare(strCurrencyCode, "krw", true) != 0)
            {
                status = base.GetErrors("/MissingBankBranch");
            }
            else if (commonValidation.isInjection(strBankBranch))
            {
                status = base.GetErrors("/InvalidBankBranch");
            }
            else if (string.IsNullOrEmpty(strBankAddress) && string.Compare(strCurrencyCode, "krw", true) != 0)
            {
                status = base.GetErrors("/MissingBankAddress");
            }
            else if (commonValidation.isInjection(strBankAddress))
            {
                status = base.GetErrors("/InvalidBankAddress");
            }
            else if (decWithdrawalAmount < decMinLimit)
            {
                status = base.GetErrors("/AmountMinLimit");
            }
            else if (decWithdrawalAmount > decMaxLimit)
            {
                status = base.GetErrors("/AmountMaxLimit");
            }
            else if ((strTotalAllowed != commonCulture.ElementValues.getResourceString("unlimited", xeResources)) && (decWithdrawalAmount > Convert.ToDecimal(strTotalAllowed)) && Convert.ToDecimal(strTotalAllowed) > 0)
            {
                status = base.GetErrors("/TotalAllowedExceeded");
            }

            if (!status.IsProcessAbort)
            {
                using (svcPayWithdrawal.WithdrawalClient svcInstance = new svcPayWithdrawal.WithdrawalClient())
                {
                    xeResponse = svcInstance.createBankTransferTransactionV1(Convert.ToInt64(strOperatorId), strMemberCode, Convert.ToInt64(commonVariables.WithdrawalMethod.BankTransfer),
                                    strCurrencyCode, decWithdrawalAmount, strAccountName, strAccountNumber, strBankAddress, strBankBranch, strBankCode, strBankName, strBankNameInput,
                                    strMyKad, string.Empty, false, Convert.ToString(commonVariables.TransactionSource.Mobile));

                    if (xeResponse == null)
                    {
                        status = base.GetErrors("/TransferFail");
                    }
                    else
                    {
                        bool isTransactionSuccessful = Convert.ToBoolean(commonCulture.ElementValues.getResourceString("result", xeResponse));
                        string strTransferId = commonCulture.ElementValues.getResourceString("invId", xeResponse);

                        if (isTransactionSuccessful)
                        {
                            status.AlertCode = "0";
                            status.AlertMessage = string.Format("{0}\\n{1}: {2}", commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferSuccess", xeErrors), strlblTransactionId, strTransferId);
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

        string strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | WithdrawAmount: {3} | AccountName: {4} | AccountNumber: {5} | BankAddress: {6} | BankBranch: {7} | BankCode: {8} | BankName: {9} | BankNameInput: {10} | MyKad: {11} | Mobile: {12} | MinLimit: {13} | MaxLimit: {14} | TotalAllowed: {15} | DailyLimit: {16} | Response: {17}",
                                Convert.ToInt64(strOperatorId), strMemberCode, strCurrencyCode, strWithdrawalAmount, strAccountName, strAccountNumber, strBankAddress, strBankBranch, strBankCode, strBankName, strBankNameInput, strMyKad, string.Empty, decMinLimit, decMaxLimit, strTotalAllowed, strDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

        intProcessSerialId += 1;
        commonAuditTrail.appendLog("system", PageName, "InitiateWithdrawal", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        
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
        if (xeResources.XPathSelectElement("BankNameNative/" + strSelectedLanguage.ToUpper() + "_" + strCurrencyCode.ToUpper()) != null)
        {
            drpBank.DataTextField = "bankNameNative";
        }
        else
        {
            drpBank.DataTextField = "bankName";
        }

        drpBank.DataValueField = "bankCode";
        drpBank.DataBind();

        drpBank.Items.Insert(0, new ListItem(base.strdrpBank, "-1"));
        drpBank.Items.Add(new ListItem(base.strdrpOtherBank, "OTHER"));
    }
}
