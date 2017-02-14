using System;

public partial class Deposit_Pay : PaymentBasePage
{
    public string GatewayFile;

    protected void Page_Load(object sender, EventArgs e)
    {
        commonVariables.DepositMethod PaymentMethodId = (commonVariables.DepositMethod)Enum.Parse(typeof(commonVariables.DepositMethod), Request.QueryString["MethodId"]);

        switch (PaymentMethodId)
        {
            case commonVariables.DepositMethod.ECPSS:
                GatewayFile = "ecpsspay";
                break;
            case commonVariables.DepositMethod.KexunPay:
                GatewayFile = "kexunpayV2";
                break;

            case commonVariables.DepositMethod.AllDebit:
                GatewayFile = "alldebit";
                break;

            case commonVariables.DepositMethod.BaokimScratchCard:
                GatewayFile = "baokimSc";
                break;

            case commonVariables.DepositMethod.NganLuong:
                GatewayFile = "nganluong";
                break;

            case commonVariables.DepositMethod.FastDeposit:
                GatewayFile = "fastdeposit";
                break;
        }
    }
}