using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class _Secure_Login : BasePage
{
    protected XElement xeErrors = null;
    protected string strRedirect = string.Empty;
    protected bool isSlotRedirect = false;

    protected void Page_Init(object sender, EventArgs e)
    {
        string strLanguage = string.Empty;

        strLanguage = Request.QueryString.Get("lang");

        commonVariables.SelectedLanguage = string.IsNullOrEmpty(strLanguage) ? (string.IsNullOrEmpty(commonVariables.SelectedLanguage) ? "en-us" : commonVariables.SelectedLanguage) : strLanguage;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        xeErrors = commonVariables.ErrorsXML;
        XElement xeResources = null;
        commonCulture.appData.getLocalResource(out xeResources);

        if (!string.IsNullOrEmpty(Request.QueryString.Get("token")))
        {
            try
            {
                var cipherKey = commonEncryption.Decrypt(ConfigurationManager.AppSettings.Get("PrivateKeyToken"));
                string strSessionId = commonEncryption.decryptToken(Request.QueryString.Get("token"), cipherKey);
                commonVariables.SetSessionVariable("MemberSessionId", strSessionId);

                var loginCode = UserSession.checkSession();

                if (loginCode != 1)
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
            if (string.IsNullOrEmpty(Request.QueryString.Get("redirect")))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["url"]))
                {
                    isSlotRedirect = true;

                    if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                    {
                        try
                        {
                            var link = new Uri(Server.UrlDecode(Request.QueryString["url"]));
                            NameValueCollection nvc = HttpUtility.ParseQueryString(link.Query);

                            var tokenArray = new string[] { "token", "s" };
                            bool isEmpty = true;

                            foreach (var item in tokenArray)
                            {
                                if (!string.IsNullOrEmpty(nvc[item]))
                                {
                                    isEmpty = false;

                                    if (nvc.AllKeys.Contains(item))
                                    {
                                        nvc.Remove(item);
                                        nvc.Add(item, commonVariables.CurrentMemberSessionId);
                                    }
                                }
                            }

                            if (isEmpty)
                            {
                                nvc.Add("s", commonVariables.CurrentMemberSessionId);
                            }

                            var domainArray = new string[] { "domainlink", "domain" };

                            foreach (var item in domainArray)
                            {
                                if (nvc.AllKeys.Contains(item))
                                {
                                    nvc.Remove(item);
                                    nvc.Add(item, (commonIp.DomainName).Trim(new char[] { '.' }));
                                }
                            }
                            if (link.Query.Length > 0)
                            {
                                link = new Uri(link.ToString().Replace(link.Query, ""));
                            }

                            Response.Redirect(link.ToString() + "?" + nvc.ToString(), false);
                            Response.End();
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }

                        Response.Redirect("/", true);
                    }
                }
                else
                {
                    strRedirect = "/Index.aspx?lang=" + commonVariables.SelectedLanguage;
                }
            }
            else
            {
                strRedirect = Request.QueryString.Get("redirect");

                if (string.IsNullOrWhiteSpace(strRedirect))
                {
                    UserSession.ClearSession();
                }
            }
        }

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
