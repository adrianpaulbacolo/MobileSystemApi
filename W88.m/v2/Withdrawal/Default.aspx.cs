using System;

public partial class v2_Withdrawal_Default : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}