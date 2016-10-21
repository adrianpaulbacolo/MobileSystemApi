using System;
using System.Web;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Shared.Helpers;

public partial class _Secure_ChangePassword : BasePage
{
    protected string RedirectUri = string.Empty;
    protected string Language = string.Empty;
    protected const string TranslationsPath = "contents/translations";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }

        var language = HttpContext.Current.Request.QueryString.Get("lang");
        Language = string.IsNullOrEmpty(language) ? LanguageHelpers.SelectedLanguage : language;

        if (!HasSession)
        {
            Response.Redirect(string.Format(@"/_Secure/Login.aspx?lang={0}", language));
            return;
        }

        lblCurrentPassword.Text = RewardsHelper.GetTranslation("LABEL_CHANGEPASSWORD", Language, TranslationsPath);
        lblNewPassword.Text = RewardsHelper.GetTranslation("LABEL_CHANGEPASSWORD_NEW", Language, TranslationsPath);
        lblConfirmPassword.Text = RewardsHelper.GetTranslation("LABEL_CHANGEPASSWORD_CONFIRM", Language, TranslationsPath);  
    }
}
