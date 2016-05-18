using System;
using System.Web.UI;

public partial class Lottery : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;
        SetTitle(commonCulture.ElementValues.getResourceString("lottery", commonVariables.LeftMenuXML));

    }
}