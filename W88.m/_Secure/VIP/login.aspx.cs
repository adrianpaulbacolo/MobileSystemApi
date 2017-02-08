using System;
using System.Web.UI;
using System.Xml.Linq;

public partial class _Secure_VIP_login : BasePage
{
    protected XElement xeErrors = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        xeErrors = commonVariables.ErrorsXML;
        XElement xeResources = null;
        commonCulture.appData.getLocalResource(out xeResources);

        if (commonCookie.CookieVip != null && commonCookie.CookieVip == "true")
        {
            Response.Redirect("/index", true);
        }
    }
}