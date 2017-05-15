using System;

public partial class v2_Deposit_Default : FundsBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
    }

    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", "/v2/Funds.aspx");
        base.OnLoad(e);
    }
}