using System;

public partial class Deposit_Pay : PaymentBasePage
{
    public string GatewayFile;

    protected void Page_Load(object sender, EventArgs e)
    {
        commonVariables.DepositMethod PaymentMethodId = (commonVariables.DepositMethod)Enum.Parse(typeof(commonVariables.DepositMethod), Request.QueryString["MethodId"]);

        switch (PaymentMethodId)
        {
            case commonVariables.DepositMethod.PaySec:
                GatewayFile = "paysec";
                break;
            case commonVariables.DepositMethod.NineVPayAlipay:
                GatewayFile = "ninevpay";
                break;
            case commonVariables.DepositMethod.JuyPayAlipay:
                GatewayFile = "juypay";
                break;
            case commonVariables.DepositMethod.ECPSS:
                GatewayFile = "ecpsspay";
                break;
            case commonVariables.DepositMethod.IWallet:
                GatewayFile = "iwallet";
                break;
            case commonVariables.DepositMethod.KexunPay:
                GatewayFile = "kexunpay";
                break;
            case commonVariables.DepositMethod.Help2Pay:
                GatewayFile = "help2pay";
                break;
        }
    }
}