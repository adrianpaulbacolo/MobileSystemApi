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
    protected string lblTransactionId;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.SDAPayAlipay);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.SDAPayAlipay);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            drpBank.Items.AddRange(base.InitializeBank("SDAPayAlipayBank").Where(bank => bank.Value.Equals("alipay", StringComparison.OrdinalIgnoreCase)).ToArray());

            this.InitializeLabels();

            drpBank.SelectedValue = "alipay";
        }
    }

    private void InitializeLabels()
    {
        lblAmount.Text = base.strlblAmount;

        lblIndicatorMessage.Text = commonCulture.ElementValues.getResourceString("lblIndicatorMessage", xeResources);

        lblTransactionId = base.strlblTransactionId;
    }
}
