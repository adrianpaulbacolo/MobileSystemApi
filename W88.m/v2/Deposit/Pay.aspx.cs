using System;

public partial class v2_Deposit_Pay : PaymentBasePage
{
    public string GatewayFile;

    protected void Page_Load(object sender, EventArgs e)
    {
        var methodId = Convert.ToInt32(Request.QueryString["MethodId"]);
        if (methodId < 0)
            return;

        if (Enum.IsDefined(typeof(commonVariables.AutoRouteMethod), methodId))
            methodId = (int)Enum.Parse(typeof(commonVariables.AutoRouteMethod), methodId.ToString());
        else
        {
            if (Enum.IsDefined(typeof(commonVariables.DepositMethod), methodId))
                methodId = (int)Enum.Parse(typeof(commonVariables.DepositMethod), methodId.ToString());
            else
                return;
        }

        switch (methodId)
        {
            case (int)commonVariables.DepositMethod.PaySec:
                GatewayFile = "paysec";
                break;

            case (int)commonVariables.DepositMethod.PayGo:
            case (int)commonVariables.DepositMethod.WingMoney:
            case (int)commonVariables.DepositMethod.TrueMoney:
                GatewayFile = "moneytransfer";
                break;

            case (int)commonVariables.DepositMethod.AllDebit:
                GatewayFile = "alldebit";
                break;

            case (int)commonVariables.DepositMethod.FastDeposit:
                GatewayFile = "banktransfer";
                break;

            case (int)commonVariables.DepositMethod.BaokimScratchCard:
            case (int)commonVariables.DepositMethod.DinPayTopUp:
                GatewayFile = "topup";
                break;

            case (int)commonVariables.DepositMethod.IWallet:
                GatewayFile = "iwallet";
                break;

            case (int)commonVariables.DepositMethod.Neteller:
                GatewayFile = "neteller";
                break;

            case (int)commonVariables.DepositMethod.VenusPoint:
                GatewayFile = "venuspoint";
                break;

            case (int)commonVariables.DepositMethod.Baokim:
                GatewayFile = "baokim";
                break;

            case (int)commonVariables.DepositMethod.SDPay:
                GatewayFile = "unionpay";
                break;

            case (int)commonVariables.DepositMethod.DaddyPay:
            case (int)commonVariables.DepositMethod.DaddyPayQR:
                GatewayFile = "daddypay";
                break;

            case (int)commonVariables.DepositMethod.SDAPayAlipay:
            case (int)commonVariables.DepositMethod.AlipayTransfer:
                GatewayFile = "alipaytransfer";
                break;

            case (int)commonVariables.DepositMethod.Cubits:
                GatewayFile = "cubits";
                break;

            case (int)commonVariables.AutoRouteMethod.AliPay:
            case (int)commonVariables.DepositMethod.NineVPayAlipay:
            case (int)commonVariables.DepositMethod.JuyPayAlipay:
            case (int)commonVariables.DepositMethod.JTPayAliPay:
            case (int)commonVariables.DepositMethod.JutaPay:
            case (int)commonVariables.DepositMethod.ShengPayAliPay:
            case (int)commonVariables.DepositMethod.AifuAlipay:
            case (int)commonVariables.DepositMethod.AllDebitAlipay:
                GatewayFile = "alipay";
                break;

            case (int)commonVariables.AutoRouteMethod.WeChat:
            case (int)commonVariables.DepositMethod.KexunPayWeChat:
            case (int)commonVariables.DepositMethod.JTPayWeChat:
            case (int)commonVariables.DepositMethod.TongHuiWeChat:
            case (int)commonVariables.DepositMethod.KDPayWeChat:
            case (int)commonVariables.DepositMethod.AifuWeChat:
            case (int)commonVariables.DepositMethod.AloGatewayWeChat:
            case (int)commonVariables.DepositMethod.AllDebitWeChat:
                GatewayFile = "wechat";
                break;

            case (int)commonVariables.AutoRouteMethod.QuickOnline:
            case (int)commonVariables.DepositMethod.NextPay:
            case (int)commonVariables.DepositMethod.BofoPay:
            case (int)commonVariables.DepositMethod.NganLuong:
            case (int)commonVariables.DepositMethod.Help2Pay:
            case (int)commonVariables.DepositMethod.ECPSS:
            case (int)commonVariables.DepositMethod.EGHL:
            case (int)commonVariables.DepositMethod.NextPayGV:
            case (int)commonVariables.DepositMethod.PayTrust:
            case (int)commonVariables.DepositMethod.AllDebitB2C:
                GatewayFile = "quickonline";
                break;

            case (int)commonVariables.AutoRouteMethod.TopUpCard:
            case (int)commonVariables.AutoRouteMethod.UnionPay:
                GatewayFile = "autoroute";
                break;
        }
    }
}