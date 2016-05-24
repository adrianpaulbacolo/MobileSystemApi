using System;
using System.Web.UI;

public partial class Slots : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;
        SetTitle(commonCulture.ElementValues.getResourceString("slots", commonVariables.LeftMenuXML));
    }
}