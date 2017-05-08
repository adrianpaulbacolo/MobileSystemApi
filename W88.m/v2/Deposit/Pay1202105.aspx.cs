using System;

public partial class v2_Deposit_Pay1202105 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.NineVPayAlipay);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.NineVPayAlipay);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}