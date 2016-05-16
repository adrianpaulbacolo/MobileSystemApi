using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Promotions : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected System.Xml.Linq.XElement xeResources = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        xeErrors = commonVariables.ErrorsXML;
        commonCulture.appData.getRootResource("/Promotions.aspx", out xeResources);

        string affiliateId = HttpContext.Current.Request.QueryString.Get("AffiliateId");

        if (!string.IsNullOrEmpty(affiliateId))
        {
            commonVariables.SetSessionVariable("AffiliateId", affiliateId);

            commonCookie.CookieAffiliateId = affiliateId;
        }
    }
}