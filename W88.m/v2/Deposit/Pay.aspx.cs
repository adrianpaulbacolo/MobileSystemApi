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
            ? (int) Enum.Parse(typeof (commonVariables.AutoRouteMethod), Request.QueryString["MethodId"])
            : (int) Enum.Parse(typeof (commonVariables.DepositMethod), Request.QueryString["MethodId"]);

        switch (methodId)
        {
            case (int)commonVariables.DepositMethod.PaySec:
                GatewayFile = "paysec";
                break;

            case (int)commonVariables.DepositMethod.PayGo:
                GatewayFile = "paygo";
                break;

            case (int)commonVariables.DepositMethod.ECPSS:
                GatewayFile = "ecpsspay";
                break;

            case (int)commonVariables.DepositMethod.AllDebit:
                GatewayFile = "alldebit";
                break;

            case (int)commonVariables.DepositMethod.NganLuong:
                GatewayFile = "nganluong";
                break;

            case (int)commonVariables.DepositMethod.FastDeposit:
                GatewayFile = "fastdep";
                break;

            case (int)commonVariables.DepositMethod.BaokimScratchCard:
                GatewayFile = "baokimSc";
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

            case (int)commonVariables.AutoRouteMethod.AliPay:
            case (int)commonVariables.DepositMethod.NineVPayAlipay:
            case (int)commonVariables.DepositMethod.JuyPayAlipay:
            case (int)commonVariables.DepositMethod.JTPayAliPay:
            case (int)commonVariables.DepositMethod.JutaPay:
            case (int)commonVariables.DepositMethod.ShengPayAliPay:
                GatewayFile = "alipay";
                break;

            case (int)commonVariables.AutoRouteMethod.WeChat:
            case (int)commonVariables.DepositMethod.KexunPay:
            case (int)commonVariables.DepositMethod.JTPayWeChat:
            case (int)commonVariables.DepositMethod.KDPayWeChat:
                GatewayFile = "wechat";
                break;

            case (int)commonVariables.AutoRouteMethod.QuickOnline:
            case (int)commonVariables.DepositMethod.NextPay:
            case (int)commonVariables.DepositMethod.BofoPay:
                GatewayFile = "quickonline";
                break;

            case (int)commonVariables.AutoRouteMethod.TopUpCard:
            case (int)commonVariables.AutoRouteMethod.UnionPay:
                GatewayFile = "autoroute";
                break;
        }
    }
}