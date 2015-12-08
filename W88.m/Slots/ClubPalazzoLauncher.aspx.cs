using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Slots_ClubPalazzo: BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    private System.Xml.Linq.XElement xeResources = null;
    public string javascriptLogin = "";
    public string javascriptToken = "";
    public string link = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        string strLanguageCode = string.Empty;

        switch (commonVariables.SelectedLanguage)
        {
            case "ko-kr":
                strLanguageCode = "ko";
                break;
            case "th-th":
                strLanguageCode = "th";
                break;
            case "zh-cn":
                strLanguageCode = "zh-cn";
                break;
            default:
                strLanguageCode = "en";
                break;
        }

        if (!string.IsNullOrEmpty((string)System.Web.HttpContext.Current.Session["MemberSessionID"]) && Request.QueryString["mode"] == "real")
        {
            if (!string.IsNullOrEmpty(Request.QueryString["type"]) && !string.IsNullOrEmpty(Request.QueryString["mode"]) && !string.IsNullOrEmpty(Request.QueryString["name"]))
            {
                string type = Request.QueryString["type"];
                string name = Request.QueryString["name"];
                string mode = Request.QueryString["mode"];
                string userName = ConfigurationManager.AppSettings["palazzo_account_prefix"] + ((string)Session["MemberCode"]).ToUpper();
                    
                javascriptLogin = string.Format("var result = iapiLogin(\"{0}\", \"{1}\" , 1, \"{2}\");",
                    userName,
                    commonEncryption.Decrypt(System.Web.HttpContext.Current.Request.Cookies["palazzo"].Value),
                    strLanguageCode);

                javascriptToken = string.Format("iapiRequestTemporaryToken(1, '427', 'GamePlay');");

                link += string.Format("\"{0}", string.Format(ConfigurationManager.AppSettings["palazzo_url_real"], name, userName, strLanguageCode));
            }
        }
        else if (Request.QueryString["mode"] == "fun")
        {
            //Response.Redirect(string.Format(AppVar.palazzo_url_fun, Request.QueryString["game"], ((string)Session["language"] == "zh-cn") ? "zh-cn" : "en"));
            javascriptLogin = string.Format("location.href= '{0}'", string.Format(ConfigurationManager.AppSettings["palazzo_url_fun"], Request.QueryString["game"], strLanguageCode));
            javascriptToken = string.Format("iapiRequestTemporaryToken(0, '427', 'GamePlay');");
        }
        else
        {
            //close window, parent redirect to login
            javascriptLogin = "window.close();";
            javascriptToken = "";
        }
        
    }
}
