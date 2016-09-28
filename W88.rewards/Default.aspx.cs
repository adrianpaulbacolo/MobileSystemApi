using System;
using System.Web;
using System.Xml.Linq;
using W88.BusinessLogic.Shared.Helpers;
using W88.Utilities;

public partial class _Default : BasePage
{
    protected string AlertMessage = string.Empty;
    protected XElement XeErrors = null;
    protected XElement LeftMenu = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        XeErrors = CultureHelpers.AppData.GetRootResource("Errors");
        LeftMenu = CultureHelpers.AppData.GetRootResource("leftMenu");
        string selectedLanguage = LanguageHelpers.SelectedLanguage;

        #region Logout
        if (string.Compare(Convert.ToString(RouteData.DataTokens["logout"]), "true", true) == 0) 
        {
            // Do logout logic here
            ClearCookies();
            Response.Redirect("/Default.aspx", false);
        }
        if (string.Compare(Convert.ToString(RouteData.DataTokens["expire"]), "true", true) == 0)
        {
            AlertMessage = CultureHelpers.ElementValues.GetResourceString("SessionExpired", XeErrors); 
        }
        if (string.Compare(Convert.ToString(RouteData.DataTokens["invalid"]), "true", true) == 0)
        {
            AlertMessage = CultureHelpers.ElementValues.GetResourceString("SessionExpired", XeErrors); 
        }
        #endregion

        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("AffiliateId"))) { Common.SetSessionVariable("AffiliateId", HttpContext.Current.Request.QueryString.Get("AffiliateId")); }

        if (HasSession)
        {
            Response.Redirect("/Index.aspx?lang=" + LanguageHelpers.SelectedLanguage, false);
        }
        if (string.IsNullOrEmpty(selectedLanguage))
        {
            Response.Redirect("/Lang.aspx", false);
        }
    }

    private void ClearCookies()
    {
        var keys = HttpContext.Current.Request.Cookies.AllKeys;
        foreach (var key in keys)
        {
            var cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Value = string.Empty;
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
        }
    }
}