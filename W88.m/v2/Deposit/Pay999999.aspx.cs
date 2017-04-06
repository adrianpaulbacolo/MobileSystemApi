using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_Deposit_Pay999999 : PaymentBasePage
{
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
        }
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