using System;
using System.Web.UI;


public partial class Deposit_Tonghui : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        var type = this.RouteData.DataTokens["type"].ToString();
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        switch (type)
        {
            case "alipay":
                base.PageName = Convert.ToString(commonVariables.DepositMethod.TongHuiAlipay);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.TongHuiAlipay);
                break;
            case "wechat":
                base.PageName = Convert.ToString(commonVariables.DepositMethod.TongHuiWeChat);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.TongHuiWeChat);
                break;
            default:
                base.PageName = Convert.ToString(commonVariables.DepositMethod.TongHuiPay);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.TongHuiPay);
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
        lblDepositAmount.Text = base.strlblAmount;
    }
}