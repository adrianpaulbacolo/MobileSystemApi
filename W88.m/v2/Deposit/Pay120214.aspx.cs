using System;

public partial class v2_Deposit_Pay120214 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.Neteller);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.Neteller);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}