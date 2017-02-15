using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_Deposit_Pay : PaymentBasePage
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

            case commonVariables.DepositMethod.ECPSS:
                GatewayFile = "ecpsspay";
                break;

            case commonVariables.DepositMethod.AllDebit:
                GatewayFile = "alldebit";
                break;

            case commonVariables.DepositMethod.NganLuong:
                GatewayFile = "nganluong";
                break;

            case commonVariables.DepositMethod.FastDeposit:
                GatewayFile = "fastdep";
                break;

            case commonVariables.DepositMethod.BaokimScratchCard:
                GatewayFile = "baokimSc";
                break;

            case commonVariables.DepositMethod.NineVPayAlipay:
            case commonVariables.DepositMethod.JuyPayAlipay:
            case commonVariables.DepositMethod.JTPayAliPay:
            case commonVariables.DepositMethod.JutaPay:
            case commonVariables.DepositMethod.ShengPayAliPay:
                GatewayFile = "alipay";
                break;

            case commonVariables.DepositMethod.KexunPay:
            case commonVariables.DepositMethod.JTPayWeChat:
            case commonVariables.DepositMethod.KDPayWeChat:
                GatewayFile = "wechat";
                break;
        }
    }
}