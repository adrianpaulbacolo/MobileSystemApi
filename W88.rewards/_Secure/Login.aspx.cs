using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Xml.Linq;
using W88.BusinessLogic.Shared.Helpers;

public partial class _Secure_Login : BasePage
{
    protected XElement XeErrors = null;
    protected XElement LeftMenu = null;
    protected string RedirectUri = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        string language = Request.QueryString.Get("lang");
        LanguageHelpers.SelectedLanguage = string.IsNullOrEmpty(language) ? 
            (string.IsNullOrEmpty(LanguageHelpers.SelectedLanguage) ? "en-us" : LanguageHelpers.SelectedLanguage) 
            : language;
        btnSubmit.Visible = !HasSession;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            XeErrors = CultureHelpers.AppData.GetRootResource("Errors");
            LeftMenu = CultureHelpers.AppData.GetRootResource("leftMenu");

            try
            {

                var queryString = HttpContext.Current.Request.QueryString;
                if (queryString.Count > 0 && !string.IsNullOrEmpty(queryString["redirect"]))
                {
                    RedirectUri = GetRedirectUriFromQueryString(queryString);
                }
                else
                {
                    RedirectUri = "/Index.aspx?lang=" + LanguageHelpers.SelectedLanguage;
                    if (HasSession)
                    {
                        Response.Redirect(RedirectUri);                                   
                    }
                }
            }
            catch (Exception ex)
            {

            }

            XElement xeLogin = CultureHelpers.AppData.GetRootResource(@"_Secure/Login.aspx");
            lblUsername.Text = CultureHelpers.ElementValues.GetResourceString("lblUsername", xeLogin);
            lblPassword.Text = CultureHelpers.ElementValues.GetResourceString("lblPassword", xeLogin);
            lblCaptcha.Text = CultureHelpers.ElementValues.GetResourceString("lblCaptcha", xeLogin);
            btnSubmit.Text = CultureHelpers.ElementValues.GetResourceString("btnLogin", xeLogin);
            txtUsername.Focus();
            lblRegister.Text = CultureHelpers.ElementValues.GetResourceString("lblRegister", xeLogin);
        }
    }

    private string GetRedirectUriFromQueryString(NameValueCollection queryStrings)
    {
        var allKeys = queryStrings.AllKeys;
        var stringBuilder1 = new StringBuilder();
        for (int index = 0; index < allKeys.Length; index++)
        {
            var key = allKeys[index];
            if (!key.Equals("redirect"))
            {
                stringBuilder1.Append(key)
                    .Append("=")
                    .Append(queryStrings[key]);
                if (index < allKeys.Length - 1)
                {
                    stringBuilder1.Append("&");
                }
            }
        }
        var stringBuilder2 = new StringBuilder();
        stringBuilder2.Append(queryStrings["redirect"])
            .Append("?")
            .Append(stringBuilder1);
        return  stringBuilder2.ToString();
    }
}
