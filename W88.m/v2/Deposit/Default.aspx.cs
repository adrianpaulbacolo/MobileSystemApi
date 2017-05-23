using System;
using Models;

public partial class v2_Deposit_Default : FundsBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
    }

    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", W88Constant.PageNames.Funds);
        base.OnLoad(e);
    }
}