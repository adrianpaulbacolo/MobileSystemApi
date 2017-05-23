using System;
using Models;

public partial class v2_Withdrawal_Default : FundsBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
    }

    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", W88Constant.PageNames.Funds);
        base.OnLoad(e);
    }
}