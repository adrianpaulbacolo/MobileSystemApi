using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_Funds : PaymentBasePage
{
    protected override void OnLoad(EventArgs e)
    {
        Page.Title = commonCulture.ElementValues.getResourceString("fundmanagement", commonVariables.LeftMenuXML);
        base.OnLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}