using System;
using System.Web.UI;


public partial class Deposit_Aifu : PaymentBasePage
{
    protected bool isWechat = false;
    private string _redirection;

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

        _redirection = string.Concat(V2DepositPath, "Pay", PaymentMethodId, ".aspx");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CheckAgentAndRedirect(_redirection);
            this.InitializeLabels();
        }
    }
    private void InitializeLabels()
    {
        lblAmount.Text = base.strlblAmount;
    }
}