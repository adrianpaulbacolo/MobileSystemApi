using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_Login : System.Web.UI.Page
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

        if (string.IsNullOrEmpty(Request.QueryString.Get("redirect"))) { strRedirect = "/Index.aspx"; }
        else { strRedirect = Request.QueryString.Get("redirect"); }

        if (!Page.IsPostBack)
        {
            lblUsername.Text = commonCulture.ElementValues.getResourceString("lblUsername", xeResources);
            lblPassword.Text = commonCulture.ElementValues.getResourceString("lblPassword", xeResources);
            lblCaptcha.Text = commonCulture.ElementValues.getResourceString("lblCaptcha", xeResources);
            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnLogin", xeResources);

            txtUsername.Attributes.Add("PLACEHOLDER", lblUsername.Text);
            txtPassword.Attributes.Add("PLACEHOLDER", lblPassword.Text);
            txtCaptcha.Attributes.Add("PLACEHOLDER", lblCaptcha.Text);

            txtUsername.Focus();
        }
    }
}