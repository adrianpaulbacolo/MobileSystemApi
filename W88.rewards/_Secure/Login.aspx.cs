using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.BusinessLogic.Shared.Helpers;

public partial class _Secure_Login : BasePage
{
    protected string RedirectUri = string.Empty;
    
    protected void Page_Init(object sender, EventArgs e)
    {
        btnSubmit.Visible = !HasSession;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }
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
                    Response.Redirect(RedirectUri, false);                                   
                }
            }
        }
        catch (Exception ex)
        {

        }

        lblUsername.Text = RewardsHelper.GetTranslation(TranslationKeys.Label.Username);
        lblPassword.Text = RewardsHelper.GetTranslation(TranslationKeys.Label.Password);
        lblCaptcha.Text = RewardsHelper.GetTranslation(TranslationKeys.Label.Captcha);
        btnSubmit.Text = RewardsHelper.GetTranslation(TranslationKeys.Label.Login);
        txtUsername.Focus();
        lblRegister.Text = HttpUtility.HtmlDecode(RewardsHelper.GetTranslation(TranslationKeys.Label.Register));      
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
