using System;
using System.Web.UI;

public partial class Casino : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;
        SetTitle(commonCulture.ElementValues.getResourceString("liveCasinoTitle", commonVariables.LeftMenuXML));
    }
}