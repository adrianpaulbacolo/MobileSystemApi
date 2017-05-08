using System;

public partial class v2_Deposit_Pay1202133 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.AifuWeChat);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.AifuWeChat);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}