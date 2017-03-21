using System;
using System.Web.UI;


public partial class Deposit_JTPay : PaymentBasePage
{
    protected bool isWechat = false;
    protected void Page_Init(object sender, EventArgs e)
    {
        var type = this.RouteData.DataTokens["type"].ToString();
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        switch (type)
        {
            case "alipay":
                base.PageName = Convert.ToString(commonVariables.DepositMethod.JTPayAliPay);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.JTPayAliPay);
                isWechat = false;
                break;
            case "wechat":
            default:
                base.PageName = Convert.ToString(commonVariables.DepositMethod.JTPayWeChat);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.JTPayWeChat);
                isWechat = true;
                break;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.InitializeLabels();
        }
    }
    private void InitializeLabels()
    {
        lblDepositAmount.Text = base.strlblAmount;
    }
}