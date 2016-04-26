using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class BasePage : System.Web.UI.Page
{
    public Boolean isLoggedIn;
    public Boolean isPublic = true;
    protected override void OnPreInit(EventArgs e)
    {
        if (string.Compare(System.Configuration.ConfigurationManager.AppSettings.Get("ClearWebCache"), "true", true) == 0)
        {
            foreach (System.Collections.DictionaryEntry deCache in System.Web.HttpContext.Current.Cache)
            {
                HttpContext.Current.Cache.Remove(Convert.ToString(deCache.Key));
            }
        }

        base.OnPreInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {

        UserSession.checkSession();

        string strLanguage = HttpContext.Current.Request.QueryString.Get("lang");

        if (!string.IsNullOrEmpty(strLanguage)) { 
            commonVariables.SelectedLanguage = strLanguage; 
        }

        if (!this.isPublic)
        {
            if (!UserSession.IsLoggedIn())
            {
                Response.Redirect("/Index");
            }
        }

        System.Web.UI.WebControls.Literal litScript = (System.Web.UI.WebControls.Literal)Page.FindControl("litScript");
        if (litScript != null) { }
        base.OnLoad(e);
    }

    protected bool CheckLogin()
    {
        if (UserSession.IsLoggedIn())
        {
            return true;
        }
        return false;
    }
}