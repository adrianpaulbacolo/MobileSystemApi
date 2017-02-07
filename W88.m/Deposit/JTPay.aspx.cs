using System;
using System.Web.UI;


public partial class Deposit_JTPay : PaymentBasePage
{
    protected string lblTransactionId;
    protected string resourceString;
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
        lblTransactionId = base.strlblTransactionId;
    }
}