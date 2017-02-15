using System;


public partial class Deposit_JuyPayAlipay : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.JuyPayAlipay);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.JuyPayAlipay);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CheckAgentAndRedirect(V2DepositPath + "Pay1202113.aspx");
            this.InitializeLabels();
        }
    }

    private void InitializeLabels()
    {
        lblDepositAmount.Text = base.strlblAmount;
    }
}