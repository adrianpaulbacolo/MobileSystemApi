using System;
using System.Web.UI;

public partial class VSports : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;
        SetTitle(commonCulture.ElementValues.getResourceXPathString("Products/VSports/Label", commonVariables.ProductsXML));
    }
}