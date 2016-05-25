using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_Login : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected string strRedirect = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        string strLanguage = string.Empty;

        strLanguage = Request.QueryString.Get("lang");

        commonVariables.SelectedLanguage = string.IsNullOrEmpty(strLanguage) ? (string.IsNullOrEmpty(commonVariables.SelectedLanguage) ? "en-us" : commonVariables.SelectedLanguage) : strLanguage;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        xeErrors = commonVariables.ErrorsXML;
        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getLocalResource(out xeResources);

        if (!string.IsNullOrEmpty(Request.QueryString.Get("token")))
        {
            try
            {
                var cipherKey = commonEncryption.Decrypt(ConfigurationManager.AppSettings.Get("PrivateKeyToken"));
                string strSessionId = commonEncryption.decryptToken(Request.QueryString.Get("token"), cipherKey);
                commonVariables.SetSessionVariable("MemberSessionId", strSessionId);

                var loginCode = UserSession.checkSession();

                if (loginCode != "1")
                {
                    UserSession.ClearSession();
                }
                else
                {
                    Response.Redirect("/Deposit/Default_app.aspx", false);
                }
            }
            catch (Exception ex)
            {
                UserSession.ClearSession();
            }
        }
        else
        {
            UserSession.ClearSession();
        }

        if (string.IsNullOrEmpty(Request.QueryString.Get("redirect"))) { strRedirect = "/Index.aspx?lang=" + commonVariables.SelectedLanguage; }
        else { strRedirect = Request.QueryString.Get("redirect"); }

        if (!Page.IsPostBack)
        {
            lblUsername.Text = commonCulture.ElementValues.getResourceString("lblUsername", xeResources);
            lblPassword.Text = commonCulture.ElementValues.getResourceString("lblPassword", xeResources);
            lblCaptcha.Text = commonCulture.ElementValues.getResourceString("lblCaptcha", xeResources);
            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnLogin", xeResources);

            txtUsername.Focus();

            lblRegister.Text = commonCulture.ElementValues.getResourceString("btnRegister", xeResources);
        }
    }
}
