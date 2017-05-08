using System;

public partial class v2_Deposit_Pay1202122 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.JTPayAliPay);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.JTPayAliPay);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}