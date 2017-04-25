using System;

public partial class Slots : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        CheckAgent();
        if (Page.IsPostBack) return;
        SetTitle(commonCulture.ElementValues.getResourceString("slots", commonVariables.LeftMenuXML));
    }

    private void CheckAgent()
    {
        var userAgent = Request.UserAgent.ToString();
        if (userAgent.ToLower().Contains("clubw"))
        {
            Response.Redirect("/_static/v2new/slots.html");
        }
    }
}