using System;

public partial class v2_Deposit_Pay120254 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.SDAPayAlipay);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.SDAPayAlipay);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}