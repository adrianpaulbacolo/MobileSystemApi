using System;

public partial class v2_Deposit_Pay1202114 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.KDPayWeChat);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.KDPayWeChat);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}