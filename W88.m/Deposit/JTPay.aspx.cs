using System;
using System.Web.UI;


public partial class Deposit_JTPay : PaymentBasePage
{
    protected string lblTransactionId;
    protected string resourceString;
    private string _redirection;

    protected void Page_Init(object sender, EventArgs e)
    {
        var type = this.RouteData.DataTokens["type"].ToString();
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        switch (type)
        {
            case "alipay":
                base.PageName = Convert.ToString(commonVariables.DepositMethod.JTPayAliPay);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.JTPayAliPay);
                resourceString = "dJTPayAliPay";
                hdnNoteVersion.Value = "1";
                break;
            case "wechat":
            default:
                base.PageName = Convert.ToString(commonVariables.DepositMethod.JTPayWeChat);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.JTPayWeChat);
                resourceString = "dJTPayWeChat";
                hdnNoteVersion.Value = "0";
                break;
        }

        _redirection = string.Concat(V2DepositPath, "Pay", PaymentMethodId, ".aspx");

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CheckAgentAndRedirect(V2DepositPath + _redirection);
            this.InitializeLabels();
        }
    }
    private void InitializeLabels()
    {
        lblDepositAmount.Text = base.strlblAmount;
        lblTransactionId = base.strlblTransactionId;
    }
}