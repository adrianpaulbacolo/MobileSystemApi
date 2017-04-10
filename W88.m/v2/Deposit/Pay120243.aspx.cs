using System;

public partial class v2_Deposit_Pay120243 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.DaddyPay);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.DaddyPay);

    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}