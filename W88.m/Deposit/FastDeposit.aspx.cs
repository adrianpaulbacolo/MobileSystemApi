using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.XPath;

public partial class Deposit_FastDesposit : PaymentBasePage
{
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = "FastDeposit";
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.FastDeposit);

        base.CheckLogin();
        base.InitialiseVariables();

        base.InitialisePaymentLimits();

        base.GetMainWalletBalance("0");

        this.InitialiseSystemBankAccounts();
        this.InitialiseMemberBank();
        this.InitialiseDepositChannel();
        this.InitialiseDepositDateTime();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CancelUnexpectedRePost();

        HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");
        commonPaymentMethodFunc.GetDepositMethodList(strMethodsUnAvailable, depositTabs, base.PageName, sender.ToString().Contains("app"));

        if (strCurrencyCode.Equals("MYR", StringComparison.OrdinalIgnoreCase) && drpDepositChannel.Text.Equals("CDM", StringComparison.OrdinalIgnoreCase))
        {
            txtAccountNumber.Visible = false;
            lblAccountNumber.Visible = false;
        }
        else
        {
            txtAccountNumber.Visible = true;
            lblAccountNumber.Visible = true;
        }

        if (string.Compare(strCurrencyCode, "krw", true) == 0)
        {
            divDepositDateTime.Visible = false;
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

        lblDepositAmount.Text = base.strlblAmount;
        txtDepositAmount.Attributes.Add("PLACEHOLDER", base.strtxtAmount);

        lblAccountName.Text = base.strlblAccountName;
        lblAccountNumber.Text = base.strlblAccountNumber;

        lblBank.Text = base.strlblBank;
        lblBankName.Text = base.strlblBankName;

        btnSubmit.Text = base.strbtnSubmit;

        lblReferenceId.Text = commonCulture.ElementValues.getResourceString("lblReferenceId", xeResources);

        lblDepositChannel.Text = commonCulture.ElementValues.getResourceString("lblDepositChannel", xeResources);

        lblSystemAccount.Text = commonCulture.ElementValues.getResourceString("lblSystemAccount", xeResources);
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

        drpSystemAccount.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpSystemAccount", xeResources), "-1"));
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

    private void InitialiseDepositChannel()
    {
        var links = from link in xeResources.Element("DepositChannel").Descendants() select new { dataValue = link.Name, dataText = link.Value };

        drpDepositChannel.DataSource = links;
        drpDepositChannel.DataValueField = "dataValue";
        drpDepositChannel.DataTextField = "dataText";
        drpDepositChannel.DataBind();

        drpDepositChannel.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpDepositChannel", xeResources), "-1"));
    }

    private void InitialiseDepositDateTime()
    {
        #region DepositDateTime

        drpDepositDate.Items.Add(new ListItem(commonCulture.ElementValues.getResourceString("drpDepositDateTime", xeResources), string.Empty));

        for (System.DateTime dtDepositDateTime = System.DateTime.Today.AddHours(-72); dtDepositDateTime < System.DateTime.Today.AddHours(72); dtDepositDateTime = dtDepositDateTime.AddHours(24))
        {
            drpDepositDate.Items.Add(new ListItem(dtDepositDateTime.ToString("dd / MMM / yyyy"), dtDepositDateTime.ToString("yyyy-MM-dd")));
        }

        for (int intHour = 0; intHour < 24; intHour++)
        {
            drpHour.Items.Add(new ListItem((intHour).ToString("0#"), Convert.ToString(intHour)));
        }
        for (int intMinute = 0; intMinute < 60; intMinute++)
        {
            drpMinute.Items.Add(new ListItem((intMinute).ToString("0#"), Convert.ToString(intMinute)));
        }
        #endregion
    }
     
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsPageRefresh)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        string strSystemAccount = drpSystemAccount.SelectedItem.Value;
        string strDepositChannel = drpDepositChannel.SelectedItem.Value;

        string strBankCode = drpBank.SelectedItem.Value;
        string strBankName = drpBank.SelectedItem.Text;
        string strBankNameInput = txtBankName.Text;

        string strDepositAmount = txtDepositAmount.Text.Trim();
        string strReferenceId = txtReferenceId.Text;
        string strAccountName = txtAccountName.Text;
        string strAccountNumber = txtAccountNumber.Text;
        string strDepositDate = drpDepositDate.SelectedValue;
        string strDepositHour = drpHour.SelectedValue;
        string strDepositMinute = drpMinute.SelectedValue;

        DateTime dtDepositDateTime = string.Compare(strCurrencyCode, "krw", true) == 0 ? DateTime.Now : DateTime.MinValue;

        decimal decDepositAmount = commonValidation.isDecimal(strDepositAmount) ? Convert.ToDecimal(strDepositAmount) : 0;
        decimal decMinLimit = Convert.ToDecimal(strMinLimit);
        decimal decMaxLimit = Convert.ToDecimal(strMaxLimit);

        CommonStatus status = new CommonStatus();

        if (!status.IsProcessAbort)
        {
            try
            {
                if (string.Compare(strCurrencyCode, "krw", true) != 0 && !string.IsNullOrEmpty(strDepositDate))
                {
                    dtDepositDateTime = divDepositDateTime.Visible ? DateTime.Parse(strDepositDate).AddHours(double.Parse(strDepositHour)).AddMinutes(double.Parse(strDepositMinute)) : DateTime.MinValue;
                }

                status = ValidateDeposit(dtDepositDateTime, strDepositDate, strDepositHour, strDepositMinute, strSystemAccount, strDepositChannel, strBankCode, strBankName, strBankNameInput, strDepositAmount,
                                            strReferenceId, strAccountName, strAccountNumber, decDepositAmount, decMinLimit, decMaxLimit);

                if (!status.IsProcessAbort)
                {
                    using (svcPayDeposit.DepositClient client = new svcPayDeposit.DepositClient())
                    {
                        xeResponse = client.createFastDepositTransactionV1(Convert.ToInt64(strOperatorId), strMemberCode, strDepositChannel, Convert.ToInt64(base.PaymentMethodId), strCurrencyCode, decDepositAmount, Convert.ToInt64(strSystemAccount),
                                            strAccountName, strAccountNumber, dtDepositDateTime, strReferenceId, strBankCode, strBankName, strBankNameInput, Convert.ToString(svcPayDeposit.DepositSource.Mobile));

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

            string strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | DepositAmount: {3} | DepositChannel: {4} | AccountName: {6} | AccountNumber: {6} | SystemAccount: {7} | BankCode: {8} | BankName: {9} | BankNameInput: {10} | ReferenceID: {11} | DepositDateTime: {12} | MinLimit: {13} | MaxLimit: {14} | TotalAllowed: {15} | DailyLimit: {16} | Response: {17}",
                Convert.ToInt64(strOperatorId), strMemberCode, strCurrencyCode, strDepositAmount, strDepositChannel, strAccountName, strAccountNumber, strSystemAccount, drpBank.SelectedValue, strBankName, strBankNameInput, strReferenceId, dtDepositDateTime.ToString("yyyy-MM-dd HH:mm:ss"), decMinLimit, decMaxLimit, strTotalAllowed, strDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", PageName, "InitiateDeposit", string.Empty, strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        }
    }

    private CommonStatus ValidateDeposit(DateTime dtDepositDateTime, string strDepositDate, string strDepositHour, string strDepositMinute, string strSystemAccount, string strDepositChannel, string strBankCode,
        string strBankName, string strBankNameInput, string strDepositAmount, string strReferenceId, string strAccountName, string strAccountNumber, decimal decDepositAmount, decimal decMinLimit, decimal decMaxLimit)
    {
        CommonStatus status = new CommonStatus();

        if (decDepositAmount == 0)
        {
            status = base.GetErrors("/MissingDepositAmount");
        }
        else if (decDepositAmount <= 0)
        {
            status = base.GetErrors("/InvalidDepositAmount");
        }
        else if (commonValidation.isInjection(strDepositAmount))
        {
            status = base.GetErrors("/InvalidDepositAmount");
        }
        else if (commonValidation.isInjection(strReferenceId))
        {
            status = base.GetErrors("/InvalidReferenceId");
        }
        else if (string.Compare(drpBank.SelectedValue, "OTHER", true) == 0 && string.IsNullOrEmpty(strBankNameInput))
        {
            status = base.GetErrors("/MissingBankName");
        }
        else if (commonValidation.isInjection(strBankNameInput))
        {
            status = base.GetErrors("/InvalidBankName");
        }
        else if (strSystemAccount == "-1")
        {
            status = base.GetErrors("/SelectSystemAccount");
        }
        else if (strDepositChannel == "-1")
        {
            status = base.GetErrors("/SelectDepositChannel");
        }
        else if (strBankCode == "-1")
        {
            status = base.GetErrors("/SelectBank");
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
        else if (decDepositAmount < decMinLimit)
        {
            status = base.GetErrors("/AmountMinLimit");
        }
        else if (decDepositAmount > decMaxLimit)
        {
            status = base.GetErrors("/AmountMaxLimit");
        }
        else if ((strTotalAllowed != strUnlimited) && (decDepositAmount > Convert.ToDecimal(strTotalAllowed)) && Convert.ToDecimal(strTotalAllowed) > 0)
        {
            status = base.GetErrors("/TotalAllowedExceeded");
        }
        else if (string.Compare(strCurrencyCode, "krw", true) != 0)
        {
            if (string.IsNullOrEmpty(strDepositDate))
            {
                status = base.GetErrors("/InvalidDateTime");
            }
            else
            {
                if ((dtDepositDateTime - DateTime.Now).TotalHours > 72 || (dtDepositDateTime - DateTime.Now).TotalHours < -72)
                {
                    status = base.GetErrors("/InvalidDateTime");
                }
            }
        }

        return status;
    }
}