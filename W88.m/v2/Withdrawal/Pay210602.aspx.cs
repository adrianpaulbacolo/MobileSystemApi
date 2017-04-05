using System;

public partial class v2_Withdrawal_Pay210602 : PaymentBasePage
{
    
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.WithdrawalMethod.BankTransfer);
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.WithdrawalMethod.BankTransfer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}