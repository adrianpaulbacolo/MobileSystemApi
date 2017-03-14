using System;
using System.Web.UI;


public partial class Deposit_Aifu : PaymentBasePage
{
    protected bool isWechat = false;
    protected void Page_Init(object sender, EventArgs e)
    {
        var type = this.RouteData.DataTokens["type"].ToString();
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        switch (type)
        {
            case "alipay":
                base.PageName = Convert.ToString(commonVariables.DepositMethod.AifuAlipay);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.AifuAlipay);
                isWechat = false;
                break;
            case "wechat":
            default:
                base.PageName = Convert.ToString(commonVariables.DepositMethod.AifuWeChat);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.AifuWeChat);
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
        lblAmount.Text = base.strlblAmount;
    }
}