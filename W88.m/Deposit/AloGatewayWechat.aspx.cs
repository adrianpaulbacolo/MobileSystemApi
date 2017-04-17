using System;
using System.Web.UI;


public partial class Deposit_AloGatewayWechat : PaymentBasePage
{
    protected bool isWechat = false;
    private string _redirection;
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PageName = Convert.ToString(commonVariables.DepositMethod.AloGatewayWeChat);
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.AloGatewayWeChat);

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
    }
}