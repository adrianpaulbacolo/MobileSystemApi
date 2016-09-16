using System;
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
        if (HasSession)
        {
            btnSubmit.Visible = false;
        }
        else
        {
            btnSubmit.Visible = true;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            XeErrors = CultureHelpers.AppData.GetRootResource("Errors");
            LeftMenu = CultureHelpers.AppData.GetRootResource("leftMenu");

            try
            {
                RedirectUri = "/Index.aspx?lang=" + LanguageHelpers.SelectedLanguage;
                if (HasSession)
                {
                    Response.Redirect(RedirectUri);                                   
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
}
