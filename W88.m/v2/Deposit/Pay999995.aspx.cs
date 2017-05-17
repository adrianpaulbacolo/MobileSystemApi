using System;

public partial class v2_Deposit_Pay999995 : FundsBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.AutoRouteMethod.WeChat);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.AutoRouteMethod.WeChat);
    }
}