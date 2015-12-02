using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected string strRedirectPathUrl = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strRouteURL = string.Empty;
        string strAbsoluteRouteURL = string.Empty;

        if (!string.IsNullOrEmpty(this.RouteData.DataTokens["reroute"] as string))
        {
            strRouteURL = Convert.ToString(this.RouteData.DataTokens["reroute"]);
            strAbsoluteRouteURL = Convert.ToString(this.RouteData.DataTokens["absoluteURL"]);
            strRedirectPathUrl = (string.IsNullOrEmpty(strAbsoluteRouteURL) ? strRouteURL : strAbsoluteRouteURL) + (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString)) ? "?" + Convert.ToString(Request.QueryString) : "");
        }        
    }
}