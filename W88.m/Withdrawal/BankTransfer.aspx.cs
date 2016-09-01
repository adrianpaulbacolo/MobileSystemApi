using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.XPath;
using Helpers;
using Newtonsoft.Json;
using svcPayMember;

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
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            base.InitialisePendingWithdrawals(sender.ToString().Contains("app"));
            this.InitialiseMemberBank();
        }
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
            // bank account name pre defined value
            var user = new Members();
            var userData = user.FetchMemberData(base.userInfo.CurrentSessionId);
            if (userData.Rows.Count > 0 && userData.Columns["Firstname"] != null)
            {
                txtAccountName.ReadOnly = true;
                var firstName = Convert.ToString(userData.Rows[0]["FirstName"]);
                var lastName = Convert.ToString(userData.Rows[0]["Lastname"]);
                if (string.IsNullOrWhiteSpace(lastName))
                {
                    txtAccountName.Text = firstName;
                }
                else
                {
                    switch (commonCookie.CookieCurrency)
                    {
                        case "RMB":
                        case "KRW":
                            txtAccountName.Text = lastName + firstName;
                            break;
                        case "VND":
                        case "JPY":
                            txtAccountName.Text = lastName + " " + firstName;
                            break;
                        case "USD":
                        case "MYR":
                        case "IDR":
                            txtAccountName.Text = firstName + " " + lastName;
                            break;
                        default:
                            txtAccountName.ReadOnly = false;
                            break;
                    }

                }

            }

            if (commonCookie.CookieCurrency != null && commonCookie.CookieCurrency.ToLower() == "vnd")
            {
                drpSecondaryBank.Items.Clear();
                MemberSecondaryBank[] banks = commonPaymentMethodFunc.GetSecondaryBanks();

                if (commonVariables.SelectedLanguageShort.ToLower() == "vn" && commonCookie.CookieCurrency.ToLower() == "vnd")
                    commonFunctions.BindDropDownList(drpSecondaryBank, banks, "bankNameNative", "bankId", true, strdrpBank);
                else
                    commonFunctions.BindDropDownList(drpSecondaryBank, banks, "bankName", "bankId", true, strdrpBank);

                drpSecondaryBank.Items.Insert(drpSecondaryBank.Items.Count, new ListItem(strdrpOtherBank, "OTHER"));
            }

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
        lblSecondBank.Text = strdrpOtherBank;
        lblBankLocation.Text = commonCulture.ElementValues.getResourceString("lblLocation", xeResources);
        lblBranch.Text = commonCulture.ElementValues.getResourceString("lblBankBranch", xeResources);

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
        decimal decMinLimit = commonValidation.isDecimal(strMinLimit) ? Convert.ToDecimal(strMinLimit) : 0;
        decimal decMaxLimit = commonValidation.isDecimal(strMaxLimit) ? Convert.ToDecimal(strMaxLimit) : 0;

        bool useV2 = false;
        long BankLocationId = 0;
        long BankBranchId = 0;

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

            var otherBankCode = drpSecondaryBank.SelectedItem != null ? drpSecondaryBank.SelectedItem.Value : "";
            if (strBankCode == "OTHER")
            {
                if (commonCookie.CookieCurrency.ToLower() == "vnd")
                {
                    if (otherBankCode == "-1")
                    {
                        status = base.GetErrors("/MissingSelectBankName");
                    }
                    else if (!commonValidation.isNumeric(hfBLId.Value) || commonValidation.isInjection(hfBLId.Value))
                    {
                             
                    }
                    else if (hfBLId.Value == "-1")
                    {
                        status = base.GetErrors("/MissingSelectBankLocation");
                    }
                    else if (hfBBId.Value == "-1")
                    {
                        status = base.GetErrors("/MissingSelectBankBranch");
                    }
                    else if (!commonValidation.isNumeric(hfBBId.Value) || commonValidation.isInjection(hfBBId.Value))
                    {
                        status = base.GetErrors("/InvalidBankBranch");
                    }
                }
            }
            else
            {
                if (string.IsNullOrEmpty(strBankBranch) && string.Compare(strCurrencyCode, "krw", true) != 0)
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
                else if (string.Compare(drpBank.SelectedValue, "OTHER", true) == 0 && string.IsNullOrEmpty(strBankNameInput))
                {
                    status = base.GetErrors("/MissingBankName");
                }
                else if (commonValidation.isInjection(strBankNameInput))
                {
                    status = base.GetErrors("/InvalidBankName");
                }
            }

            if (!status.IsProcessAbort)
            {
                if (commonCookie.CookieCurrency.ToLower() == "vnd" && !drpSecondaryBank.SelectedItem.Value.Equals("-1") && !drpSecondaryBank.SelectedItem.Value.Equals("OTHER"))
                {
                    strBankCode = drpSecondaryBank.SelectedValue;
                    strBankName = drpSecondaryBank.SelectedItem.Text;
                    strBankNameInput = drpSecondaryBank.SelectedItem.Text;
                    BankLocationId = Convert.ToInt64(hfBLId.Value);
                    BankBranchId = Convert.ToInt64(hfBBId.Value);
                    useV2 = true;
                }

                using (var svcInstance = new svcPayWithdrawal.WithdrawalClient())
                {
                    if (useV2)
                    {
                        xeResponse = svcInstance.createBankTransferTransactionV2(Convert.ToInt64(strOperatorId),
                            strMemberCode, Convert.ToInt64(commonVariables.WithdrawalMethod.BankTransfer),
                            strCurrencyCode, decWithdrawalAmount, strAccountName, strAccountNumber, BankLocationId,
                            BankBranchId, strBankCode, strBankName, strBankNameInput, strMyKad, string.Empty, false,
                            Convert.ToString(commonVariables.TransactionSource.Mobile));
                    }
                    else
                    {
                        xeResponse = svcInstance.createBankTransferTransactionV1(Convert.ToInt64(strOperatorId),
                            strMemberCode, Convert.ToInt64(commonVariables.WithdrawalMethod.BankTransfer),
                            strCurrencyCode, decWithdrawalAmount, strAccountName, strAccountNumber, strBankAddress,
                            strBankBranch, strBankCode, strBankName, strBankNameInput,
                            strMyKad, string.Empty, false, Convert.ToString(commonVariables.TransactionSource.Mobile));
                    }
                    
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

        string strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | WithdrawAmount: {3} | AccountName: {4} | AccountNumber: {5} | BankAddress: {6} | BankBranch: {7} | BankCode: {8} | BankName: {9} | BankNameInput: {10} | BankLocationId: {11} | BankBranchId: {12} | MyKad: {13} | Mobile: {14} | MinLimit: {15} | MaxLimit: {16} | TotalAllowed: {17} | DailyLimit: {18} | Response: {19}",
                                Convert.ToInt64(strOperatorId), strMemberCode, strCurrencyCode, strWithdrawalAmount, strAccountName, strAccountNumber, strBankAddress, strBankBranch, strBankCode, strBankName, strBankNameInput, BankLocationId, BankBranchId, strMyKad, string.Empty, decMinLimit, decMaxLimit, strTotalAllowed, strDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

        intProcessSerialId += 1;
        commonAuditTrail.appendLog("system", PageName, "InitiateWithdrawal", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);

        #endregion
    }

    private void InitialiseMemberBank()
    {
        MemberBank[] ArrMB = null;

        using (var svcInstance = new MemberClient())
        {
            string strProcessCode, strProcessText;
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

    [WebMethod]
    public static string GetBankLocation(string bankId)
    {
        var list = new List<object>();
        try
        {
            using (var client = new MemberClient())
            {
                string statusCode, statusText;
                var bankLocations = client.getBankLocations(Convert.ToInt64(bankId), out statusCode, out statusText);
                foreach (DataRow row in bankLocations.Rows)
                {
                    list.Add(new { name = row["description"], value = row["bankLocationId"], });
                }
            }
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", "Payment_BankTransfer", "GetBankLocation", string.Empty, string.Empty, string.Empty, "-99", "exception", ex.Message, string.Empty, string.Empty, true);
        }
        return JsonConvert.SerializeObject(list);
    }

    [WebMethod]
    public static string GetBankBranch(string bankId, string bankLocationId)
    {
        var list = new List<object>();
        try
        {

            using (var client = new MemberClient())
            {
                string statusCode, statusText;
                var bankBranches = client.getBankBranches(Convert.ToInt64(bankId), Convert.ToInt64(bankLocationId), out statusCode, out statusText);
                foreach (DataRow row in bankBranches.Rows)
                {
                    list.Add(new { name = row["description"], value = row["bankBranchId"], });
                }
            }
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", "Payment_BankTransfer", "GetBankBranch", string.Empty, string.Empty, string.Empty, "-99", "exception", ex.Message, string.Empty, string.Empty, true);
        }
        return JsonConvert.SerializeObject(list);
    }

  
}
