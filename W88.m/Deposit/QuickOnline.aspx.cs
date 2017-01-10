using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;


public partial class Deposit_QuickOnline : PaymentBasePage
{
    protected string lblTransactionId;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.AutoRouteMethod.QuickOnline);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitializeBanks(autoRouteBanks);
            this.InitializeLabels();
        }
    }

    private void InitializeLabels()
    {
        lblMode.Text = base.strlblMode;
        txtMode.Text = base.strtxtMode;

        lblMinMaxLimit.Text = base.strlblMinMaxLimit;
        txtMinMaxLimit.Text = base.strtxtMinMaxLimit;

        lblDepositAmount.Text = base.strlblAmount;

        lblBank.Text = base.strlblBank;

        btnSubmit.Text = base.strbtnSubmit;

        lblTransactionId = base.strlblTransactionId;
    }

    private void InitializeBanks(DataTable banksTable)
    {
        bool isNameNative = false;
        if ((commonVariables.SelectedLanguage.Equals("ZH-CN", StringComparison.OrdinalIgnoreCase) && strCurrencyCode.Equals("RMB", StringComparison.OrdinalIgnoreCase)) ||
            (commonVariables.SelectedLanguage.Equals("VI-VN", StringComparison.OrdinalIgnoreCase) && strCurrencyCode.Equals("VND", StringComparison.OrdinalIgnoreCase)) ||
            (commonVariables.SelectedLanguage.Equals("TH-TH", StringComparison.OrdinalIgnoreCase) && strCurrencyCode.Equals("THB", StringComparison.OrdinalIgnoreCase)) ||
            (commonVariables.SelectedLanguage.Equals("ID-ID", StringComparison.OrdinalIgnoreCase) && strCurrencyCode.Equals("IDR", StringComparison.OrdinalIgnoreCase)) ||
            (commonVariables.SelectedLanguage.Equals("KM-KH", StringComparison.OrdinalIgnoreCase) && strCurrencyCode.Equals("USD", StringComparison.OrdinalIgnoreCase)) ||
            (commonVariables.SelectedLanguage.Equals("KO-KR", StringComparison.OrdinalIgnoreCase) && strCurrencyCode.Equals("KRW", StringComparison.OrdinalIgnoreCase)) ||
            (commonVariables.SelectedLanguage.Equals("JA-JP", StringComparison.OrdinalIgnoreCase) && strCurrencyCode.Equals("JPY", StringComparison.OrdinalIgnoreCase)))
        {
            isNameNative = true;
        }

        drpBank.DataSource = banksTable;
        drpBank.DataValueField = "bankCode";
        drpBank.DataTextField = isNameNative ? "bankNameNative" : "bankName";
        drpBank.DataBind();
        drpBank.Items.Insert(0, new ListItem(strdrpBank, "-1"));
    }
}