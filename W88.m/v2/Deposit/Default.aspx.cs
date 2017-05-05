using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_Deposit_Default : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
    }

    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", "/v2/Funds.aspx");
        base.OnLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}