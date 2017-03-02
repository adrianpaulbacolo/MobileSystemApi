using System;

public partial class v2_Deposit_Pay1202123 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.JTPayWeChat);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.JTPayWeChat);

    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}