using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class Deposit_SDAPay : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        var type = this.RouteData.DataTokens["type"].ToString();
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        switch (type)
        {
            case "sdapayalipay":
                base.PageName = Convert.ToString(commonVariables.DepositMethod.SDAPayAlipay);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.SDAPayAlipay);
                break;
            case "alipaytransfer":
            default:
                base.PageName = Convert.ToString(commonVariables.DepositMethod.AlipayTransfer);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.AlipayTransfer);
                break;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CheckAgentAndRedirect(string.Concat(V2DepositPath, "Pay", PaymentMethodId, ".aspx"));
            this.InitializeLabels();
        }
    }

    private void InitializeLabels()
    {
        lblAmount.Text = base.strlblAmount;
    }
}
