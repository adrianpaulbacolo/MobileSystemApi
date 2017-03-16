using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : BasePage
{
    protected string strAlertMessage = string.Empty;
    protected System.Xml.Linq.XElement xeErrors = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        xeErrors = commonVariables.ErrorsXML;

        string strSelectedLanguage = commonVariables.SelectedLanguage;

        #region Logout
        if (string.Compare(Convert.ToString(this.RouteData.DataTokens["logout"]), "true", true) == 0) { 
            UserSession.ClearSession();
            FormsAuthentication.SignOut();
            Session.Abandon();
            Response.Redirect(string.Format("/Index?Lang={0}", strSelectedLanguage)); 
        }
        if (string.Compare(Convert.ToString(this.RouteData.DataTokens["expire"]), "true", true) == 0) {
            UserSession.ClearSession();
            FormsAuthentication.SignOut();
            Session.Abandon();
            strAlertMessage = commonCulture.ElementValues.getResourceString("SessionExpired", xeErrors); 
            Response.Redirect(string.Format("/Index?Lang={0}", strSelectedLanguage)); 
        }
        if (string.Compare(Convert.ToString(this.RouteData.DataTokens["invalid"]), "true", true) == 0) {
            UserSession.ClearSession();
            FormsAuthentication.SignOut();
            Session.Abandon();
            strAlertMessage = commonCulture.ElementValues.getResourceString("SessionExpired", xeErrors); 
            Response.Redirect(string.Format("/Index?Lang={0}", strSelectedLanguage)); 
        }
        #endregion

        customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");

        string strSplashUrl = opSettings.Values.Get("SplashUrl");

        string arrStrLanguageSelection = opSettings.Values.Get("LanguageSelection");
        List<string> lstLanguageSelection = arrStrLanguageSelection.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();

        System.Text.StringBuilder sbLanguageHTML = new System.Text.StringBuilder();
        foreach (string language in lstLanguageSelection)
        {
            string strLanguage = language.Trim();

            if (base.CDNCountryCode.Equals("MY", StringComparison.OrdinalIgnoreCase) || commonCookie.CookieLanguage.Equals("zh-my", StringComparison.OrdinalIgnoreCase))
            {
                if (strLanguage.Equals("en-us", StringComparison.OrdinalIgnoreCase) || strLanguage.Equals("zh-cn", StringComparison.OrdinalIgnoreCase))
                    continue;
            }
            else
            {
                if (strLanguage.Equals("en-my", StringComparison.OrdinalIgnoreCase) || strLanguage.Equals("zh-my", StringComparison.OrdinalIgnoreCase))
                    continue;
            }

            sbLanguageHTML.AppendFormat("<li><a data-ajax='false' href='/Index.aspx?lang={0}' data-inline='true' id='div{0}' class='divLangImg div{0}'></a></li>", strLanguage);
        }
        divLanguageContainer.InnerHtml = Convert.ToString(sbLanguageHTML);

        string affiliateId = HttpContext.Current.Request.QueryString.Get("AffiliateId");

        if (!string.IsNullOrEmpty(affiliateId))
        {
            commonVariables.SetSessionVariable("AffiliateId", affiliateId);

            commonCookie.CookieAffiliateId = affiliateId;
        }
    }
}
