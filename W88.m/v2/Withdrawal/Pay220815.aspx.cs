using System;


public partial class v2_Withdrawal_Pay220815 : PaymentBasePage
{

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.WithdrawalMethod.Neteller);
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.WithdrawalMethod.Neteller);
    }
}