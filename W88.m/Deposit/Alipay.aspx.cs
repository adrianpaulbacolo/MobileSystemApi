using System;


public partial class Deposit_Alipay : PaymentBasePage
{
    protected string lblTransactionId;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.AutoRouteMethod.AliPay);
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
        lblTransactionId = base.strlblTransactionId;

    }
}