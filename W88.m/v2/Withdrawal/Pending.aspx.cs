using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_Withdrawal_Pending : System.Web.UI.Page
{
    protected override void OnLoad(EventArgs e)
    {
        Page.Items.Add("Parent", "/v2/Funds.aspx");
        base.OnLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}