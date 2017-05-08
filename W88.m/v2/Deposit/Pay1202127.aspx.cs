using System;

public partial class v2_Deposit_Pay1202127 : FundsBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.KexunPayWeChat);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.KexunPayWeChat);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}