using System;
using System.Web;
using System.Web.UI;

public partial class Promotions : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected System.Xml.Linq.XElement xeResources = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        SetTitle(commonCulture.ElementValues.getResourceString("promotions", commonVariables.LeftMenuXML));
        xeErrors = commonVariables.ErrorsXML;
        commonCulture.appData.getRootResource("/Promotions.aspx", out xeResources);

        var affiliateId = HttpContext.Current.Request.QueryString.Get("AffiliateId");

        if (string.IsNullOrEmpty(affiliateId)) return;

        commonVariables.SetSessionVariable("AffiliateId", affiliateId);
        commonCookie.CookieAffiliateId = affiliateId;
    }
}