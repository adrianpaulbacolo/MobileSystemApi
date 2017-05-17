using System;

public partial class v2_Deposit_Pay1202175 : PaymentBasePage
{

    protected string lblTransactionId;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.HebaoWeChat);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.HebaoWeChat);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

}