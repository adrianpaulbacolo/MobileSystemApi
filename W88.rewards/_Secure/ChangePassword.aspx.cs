using System;
using System.Web;
using W88.BusinessLogic.Shared.Helpers;

public partial class _Secure_ChangePassword : BasePage
{
    protected string RedirectUri = string.Empty;
    protected const string TranslationsPath = "contents/translations";
    protected const string MessagesPath = "contents/messages";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }

        if (!HasSession)
        {
            Response.Redirect(string.Format(@"/_Secure/Login.aspx?lang={0}", Language));
            return;
        }

        lblCurrentPassword.Text = CultureHelpers.GetTranslation("LABEL_CHANGEPASSWORD_CURRENT", Language, TranslationsPath);
        lblNewPassword.Text = CultureHelpers.GetTranslation("LABEL_CHANGEPASSWORD_NEW", Language, TranslationsPath);
        lblConfirmPassword.Text = CultureHelpers.GetTranslation("LABEL_CHANGEPASSWORD_CONFIRM", Language, TranslationsPath);  
    }
}
