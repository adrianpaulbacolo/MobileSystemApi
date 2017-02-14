using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.XPath;

public partial class Deposit_FastDesposit : PaymentBasePage
{
    protected string lblTransactionId;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.FastDeposit);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.FastDeposit);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CheckAgentAndRedirect(V2DepositPath + "Pay110101.aspx");
            this.InitialiseSystemBankAccounts();
            this.InitialiseMemberBank();
            this.InitialiseDepositChannel();
            this.InitialiseDepositDateTime();
        }

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

        lblAccountName.Text = base.strlblAccountName;
        lblAccountNumber.Text = base.strlblAccountNumber;

        lblBank.Text = base.strlblBank;
        lblBankName.Text = base.strlblBankName;

        btnSubmit.Text = base.strbtnSubmit;

        lblReferenceId.Text = commonCulture.ElementValues.getResourceString("lblReferenceId", xeResources);

        lblDepositChannel.Text = commonCulture.ElementValues.getResourceString("lblDepositChannel", xeResources);

        lblSystemAccount.Text = commonCulture.ElementValues.getResourceString("lblSystemAccount", xeResources);

        lblDepositDateTime.Text = commonCulture.ElementValues.getResourceString("drpDepositDateTime", xeResources);

        if (commonVariables.SelectedLanguageShort.ToLower() == "vn" && commonCookie.CookieCurrency.ToLower() == "vnd")
        {
            var sbNote = new StringBuilder();
            sbNote.Append("<li class='row'><div class='col'>");
            sbNote.Append(string.Format("<p style='color:#ff0000'>{0}</p>", commonCulture.ElementValues.getResourceString("Note", xeResources)));
            sbNote.Append("</div></li>");
            ltlNote.Text = sbNote.ToString();
        }

        lblTransactionId = base.strlblTransactionId;
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

        for (System.DateTime dtDepositDateTime = System.DateTime.Today.AddHours(-72); dtDepositDateTime <= System.DateTime.Today.AddHours(72); dtDepositDateTime = dtDepositDateTime.AddHours(24))
        {
            drpDepositDate.Items.Add(new ListItem(dtDepositDateTime.ToString("dd / MM / yyyy"), dtDepositDateTime.ToString("yyyy-MM-dd")));
        }

        drpDepositDate.SelectedValue = DateTime.Now.ToString("yyyy-MM-dd");

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
}