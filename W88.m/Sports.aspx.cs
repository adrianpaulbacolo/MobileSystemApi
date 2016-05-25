using System;
using System.Web.UI;

public partial class Sports :BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;
        SetTitle(commonCulture.ElementValues.getResourceString("sports", commonVariables.LeftMenuXML));

    }
}