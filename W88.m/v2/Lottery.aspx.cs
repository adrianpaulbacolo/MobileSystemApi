using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Helpers;

public partial class v2_Lottery : BasePage
{
    protected override void OnLoad(EventArgs e)
    {
        Page.Title = commonCulture.ElementValues.getResourceString("lottery", commonVariables.LeftMenuXML);
        Page.Items.Add("Parent", Pages.Dashboard);
        base.OnLoad(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}