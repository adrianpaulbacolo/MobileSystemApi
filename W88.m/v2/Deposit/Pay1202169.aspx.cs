﻿using System;

public partial class v2_Deposit_Pay1202169 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.AllDebitAlipay);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.AllDebitAlipay);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
    }
}