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
                _redirection = "Pay1202122.aspx";
                
                break;
            case "wechat":
            default:
                _redirection = "Pay1202123.aspx";
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