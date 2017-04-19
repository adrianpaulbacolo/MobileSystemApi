using System;

public partial class v2_Deposit_Pay : PaymentBasePage
{
    public string GatewayFile;

    protected void Page_Load(object sender, EventArgs e)
    {
        var isAutoRoute = false;

        if (Request.QueryString["AutoRoute"] != null)
        {
            isAutoRoute = Boolean.TryParse(Request.QueryString["AutoRoute"], out isAutoRoute);
        }

        var methodId = (isAutoRoute)
             ? (int)Enum.Parse(typeof(commonVariables.AutoRouteMethod), Request.QueryString["MethodId"])
             : (int)Enum.Parse(typeof(commonVariables.DepositMethod), Request.QueryString["MethodId"]);

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
                GatewayFile = "baokimscratchcard";
                break;
            
            case (int)commonVariables.DepositMethod.DinPayTopUp:
                GatewayFile = "dinpaytopup";
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
                GatewayFile = "sdapay";
                break;

            case (int)commonVariables.AutoRouteMethod.AliPay:
            case (int)commonVariables.DepositMethod.NineVPayAlipay:
            case (int)commonVariables.DepositMethod.JuyPayAlipay:
            case (int)commonVariables.DepositMethod.JTPayAliPay:
            case (int)commonVariables.DepositMethod.JutaPay:
            case (int)commonVariables.DepositMethod.ShengPayAliPay:
            case (int)commonVariables.DepositMethod.AifuAlipay:
                GatewayFile = "alipay";
                break;

            case (int)commonVariables.AutoRouteMethod.WeChat:
            case (int)commonVariables.DepositMethod.KexunPayWeChat:
            case (int)commonVariables.DepositMethod.JTPayWeChat:
            case (int)commonVariables.DepositMethod.KDPayWeChat:
            case (int)commonVariables.DepositMethod.AifuWeChat:
                GatewayFile = "wechat";
                break;

            case (int)commonVariables.AutoRouteMethod.QuickOnline:
            case (int)commonVariables.DepositMethod.NextPay:
            case (int)commonVariables.DepositMethod.BofoPay:
            case (int)commonVariables.DepositMethod.NganLuong:
            case (int)commonVariables.DepositMethod.Help2Pay:
            case (int)commonVariables.DepositMethod.ECPSS:
            case (int)commonVariables.DepositMethod.EGHL:
                GatewayFile = "quickonline";
                break;

            case (int)commonVariables.AutoRouteMethod.TopUpCard:
            case (int)commonVariables.AutoRouteMethod.UnionPay:
                GatewayFile = "autoroute";
                break;
        }
    }
}