using System;
using W88.BusinessLogic.Shared.Helpers;

public partial class _Secure_VIP_login : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        if (IsVip())
        {
            Response.Redirect("/Index", true);
        }
    }

    protected string GetTranslation(string key)
    {
        return CultureHelpers.GetTranslation(key, LanguageHelpers.SelectedLanguage, "contents/translations");
    }
}