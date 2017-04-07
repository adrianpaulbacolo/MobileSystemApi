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
            Response.Redirect("/_Secure/Login.aspx");
            return;
        }

        lblCurrentPassword.Text = GetTranslation("LABEL_CHANGEPASSWORD_CURRENT");
        lblNewPassword.Text = GetTranslation("LABEL_CHANGEPASSWORD_NEW");
        lblConfirmPassword.Text = GetTranslation("LABEL_CHANGEPASSWORD_CONFIRM");  
    }
}
