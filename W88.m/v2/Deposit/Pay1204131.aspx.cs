using System;

public partial class v2_Deposit_Pay1204131 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.AlipayTransfer);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.AlipayTransfer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}