using System;

public partial class Slots : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckAgentAndRedirect("/_static/v2new/slots.html");
        if (Page.IsPostBack) return;
        SetTitle(commonCulture.ElementValues.getResourceString("slots", commonVariables.LeftMenuXML));
    }
}