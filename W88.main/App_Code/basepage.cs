using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class basepage : System.Web.UI.Page
{
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
        string strMemberSessionId = string.Empty;
        string strLanguage = HttpContext.Current.Request.QueryString.Get("lang");

        strMemberSessionId = common.variables.CurrentMemberSessionId;

        if (!string.IsNullOrEmpty(strLanguage)) { common.variables.SelectedLanguage = strLanguage; }

        if (string.IsNullOrEmpty(common.variables.GetSessionVariable("LoginStatus")) && !string.IsNullOrEmpty(strMemberSessionId))
        {
            //Response.Redirect("/_Secure/ProcessLoginBySessionId.html" + (!string.IsNullOrEmpty(strLanguage) ? "?lang=" + strLanguage : ""), true);
        }
        else if (string.IsNullOrEmpty(strMemberSessionId) && string.Compare(common.variables.GetSessionVariable("LoginStatus"), "success", true) == 0)
        {
            //Response.Redirect("/Expire", true);
            Response.Write("<script type='text/javascript'>window.location.replace('/Expire');</script>");
        }
        else if (string.IsNullOrEmpty(strMemberSessionId))
        {
            //Response.Redirect("/Default.aspx", true);
            //Response.Write("<script type='text/javascript'>window.location.replace('/Expire');</script>");
        }

        base.OnLoad(e);
    }

    //protected bool CheckLogin()
    //{
    //    string strMemberSessionId = string.Empty;

    //    strMemberSessionId = common.variables.CurrentMemberSessionId;

    //    if (string.IsNullOrEmpty(strMemberSessionId))
    //    {
    //        base.Context.Response.Redirect("/Invalid");
    //    }

    //    return string.IsNullOrEmpty(strMemberSessionId);
    //}
}