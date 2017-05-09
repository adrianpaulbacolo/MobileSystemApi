using System;

public partial class v2_Deposit_Pay999996 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.AutoRouteMethod.AliPay);
    }


    protected void Page_Load(object sender, EventArgs e)
    {

    }
}