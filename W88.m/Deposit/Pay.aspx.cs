using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Deposit_Pay : PaymentBasePage
{
    public string GatewayFile;

    protected void Page_Load(object sender, EventArgs e)
    {
        commonVariables.DepositMethod PaymentMethodId = (commonVariables.DepositMethod)Enum.Parse(typeof(commonVariables.DepositMethod), Request.QueryString["MethodId"]);

        switch (PaymentMethodId)
        {
            case commonVariables.DepositMethod.PaySec:
                GatewayFile = "paysecpay";
                break;
            case commonVariables.DepositMethod.ECPSS:
                GatewayFile = "ecpsspay";
                break;
            case commonVariables.DepositMethod.KexunPay:
                GatewayFile = "kexunpay";
                break;
        }
    }
}