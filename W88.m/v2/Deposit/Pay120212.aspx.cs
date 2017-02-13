using System;

public partial class v2_Deposit_Pay120212 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.NganLuong);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.NganLuong);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }
}