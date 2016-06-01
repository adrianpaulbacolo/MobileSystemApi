using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_Login : System.Web.UI.Page
{
    protected System.Xml.Linq.XElement xeErrors = null;
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
        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getLocalResource(out xeResources);

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
