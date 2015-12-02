using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    protected string strAlertMessage = string.Empty;
    protected System.Xml.Linq.XElement xeErrors = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        xeErrors = commonVariables.ErrorsXML;
        bool login = false;

        string strSelectedLanguage = string.Empty;

        strSelectedLanguage = commonVariables.SelectedLanguage;

        #region Logout
        if (string.Compare(Convert.ToString(this.RouteData.DataTokens["logout"]), "true", true) == 0) { login = true; commonVariables.ClearSessionVariables(); commonCookie.ClearCookies(); Response.Redirect(string.Format("/Index?Lang={0}", strSelectedLanguage)); }
        if (string.Compare(Convert.ToString(this.RouteData.DataTokens["expire"]), "true", true) == 0) { login = true; commonVariables.ClearSessionVariables(); commonCookie.ClearCookies(); strAlertMessage = commonCulture.ElementValues.getResourceString("SessionExpired", xeErrors); Response.Redirect(string.Format("/Index?Lang={0}", strSelectedLanguage)); }
        if (string.Compare(Convert.ToString(this.RouteData.DataTokens["invalid"]), "true", true) == 0) { login = true; commonVariables.ClearSessionVariables(); commonCookie.ClearCookies(); strAlertMessage = commonCulture.ElementValues.getResourceString("SessionExpired", xeErrors); Response.Redirect(string.Format("/Index?Lang={0}", strSelectedLanguage)); }
        #endregion

        customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");

       string strSplashUrl = opSettings.Values.Get("SplashUrl");
        /*
        if (!string.IsNullOrEmpty(strSplashUrl))
        {
            Response.Status = "301 Moved Permanently";
            Response.AddHeader("Location", strSplashUrl);
            Response.End();
        }
        else 
        {
            Response.Redirect(string.Format("/Index?Lang={0}", strSelectedLanguage));
        }
        */
        string arrStrLanguageSelection = opSettings.Values.Get("LanguageSelection");
        List<string> lstLanguageSelection = arrStrLanguageSelection.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();

        System.Text.StringBuilder sbLanguageHTML = new System.Text.StringBuilder();
        foreach (string language in lstLanguageSelection)
        {
            string strLanguage = language.Trim();
            //if (string.IsNullOrEmpty(commonCookie.CookieS) || login) { sbLanguageHTML.AppendFormat("<a data-theme='b' href='/_Secure/Login.aspx?lang={0}' data-transition='slide'  data-rel='dialog' data-transition='slidedown' data-inline='true'><div id='div{1}' class='divLangImg'></div></a>", strLanguage, strLanguage); }
            //else { sbLanguageHTML.AppendFormat("<a data-theme='b' data-ajax='false' href='/Index.aspx?lang={0}' data-inline='true'><div id='div{1}' class='divLangImg'></div></a>", strLanguage, strLanguage); }

            sbLanguageHTML.AppendFormat("<a data-theme='b' data-ajax='false' href='/Index.aspx?lang={0}' data-inline='true'><div id='div{1}' class='divLangImg'></div></a>", strLanguage, strLanguage);
        }
        divLanguageContainer.InnerHtml = Convert.ToString(sbLanguageHTML);

        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("AffiliateId"))) { commonVariables.SetSessionVariable("AffiliateId", HttpContext.Current.Request.QueryString.Get("AffiliateId")); }
    }
}