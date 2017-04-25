using System;
using System.Web.UI;


public partial class Deposit_AllDebit : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        var type = this.RouteData.DataTokens["type"].ToString();
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        switch (type)
        {
            case "alipay":
                base.PageName = Convert.ToString(commonVariables.DepositMethod.AllDebitAlipay);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.AllDebitAlipay);
                break;
            case "wechat":
                base.PageName = Convert.ToString(commonVariables.DepositMethod.AllDebitWeChat);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.AllDebitWeChat);
                break;
            default:
                base.PageName = Convert.ToString(commonVariables.DepositMethod.AllDebitB2C);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.AllDebitB2C);
                break;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CheckAgentAndRedirect(string.Concat(V2DepositPath, "Pay", PaymentMethodId, ".aspx"));
            this.InitializeLabels();
        }
    }
    private void InitializeLabels()
    {
        lblAmount.Text = base.strlblAmount;
        lblBank.Text = base.strlblBank;
    }
}