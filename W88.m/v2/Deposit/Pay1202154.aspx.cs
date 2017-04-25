using System;

public partial class v2_Deposit_Pay1202154 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.AloGatewayWeChat);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.AloGatewayWeChat);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }

}