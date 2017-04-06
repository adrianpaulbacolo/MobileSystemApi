using System;

public partial class v2_Deposit_Pay120231 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.BofoPay);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.BofoPay);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}