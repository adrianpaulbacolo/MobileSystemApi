using System;
using System.Web;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.Utilities;

public partial class _Default : BasePage
{
    protected string AlertMessage = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        #region Logout
        if (string.Compare(Convert.ToString(RouteData.DataTokens["logout"]), "true", true) == 0) 
        {
            // Do logout logic here
            CookieHelpers.ClearCookies();
            Response.Redirect(string.Format("/Default.aspx?lang={0}", LanguageHelpers.SelectedLanguage), false);
            return;
        }
        if (string.Compare(Convert.ToString(RouteData.DataTokens["expire"]), "true", true) == 0)
        {
            AlertMessage = RewardsHelper.GetTranslation(TranslationKeys.Errors.SessionExpired); 
        }
        if (string.Compare(Convert.ToString(RouteData.DataTokens["invalid"]), "true", true) == 0)
        {
            AlertMessage = RewardsHelper.GetTranslation(TranslationKeys.Errors.SessionExpired); 
        }
        #endregion

        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("AffiliateId"))) { Common.SetSessionVariable("AffiliateId", HttpContext.Current.Request.QueryString.Get("AffiliateId")); }

        if (HasSession)
        {
            Response.Redirect(string.Format("/Index.aspx?lang={0}", LanguageHelpers.SelectedLanguage), false);
        }

        if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["lang"]))
        {
            Response.Redirect("/Lang.aspx", false);
        }
    }
}