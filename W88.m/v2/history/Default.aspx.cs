using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class History_Default : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var currentUrl = HttpContext.Current.Request.Url.PathAndQuery;

        if (currentUrl.ToLower().Contains("history"))
        {
            var header = (UserControl)Master.FindControl("HeaderLogo");
            var filter = (HyperLink)header.FindControl("filterHistory");
            filter.Visible = true;
        }
    }
}