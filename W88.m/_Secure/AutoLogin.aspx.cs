using System;
using System.Configuration;
using System.Xml.Linq;
using Helpers;
using wsMemberMS1;

public partial class _Secure_AutoLogin : BasePage
{
    protected XElement xeErrors = null;
    System.Xml.Linq.XElement _xeLoginResources;
    private string _username = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        xeErrors = commonVariables.ErrorsXML;
        commonCulture.appData.getRootResource("/_Secure/Login.aspx", out _xeLoginResources);

        lblRegister.Text = commonCulture.ElementValues.getResourceString("lblRegister2", _xeLoginResources);
        lblRegNote.Text = commonCulture.ElementValues.getResourceString("lblMsgNote", _xeLoginResources);
        lblUsername.Text = commonCulture.ElementValues.getResourceString("lblUsername", _xeLoginResources);
        lblPassword.Text = commonCulture.ElementValues.getResourceString("lblPassword", _xeLoginResources);
        lblCaptcha.Text = commonCulture.ElementValues.getResourceString("lblCaptcha", _xeLoginResources);
        hfLoginTranslation.Value = commonCulture.ElementValues.getResourceString("btnLogin", _xeLoginResources);

        try
        {
            if (Request.QueryString["username"] != null && Request.QueryString["code"] != null)
            {
                using (var client = new memberWSSoapClient())
                {
                    commonAuditTrail.appendLog(_username, "_Secure_AutoLogin", "Params", string.Empty, string.Empty,
                        string.Empty, "-99", string.Empty, Request.Url.ToString(),
                        string.Empty, string.Empty, true);

                    commonCookie.CookieIsApp = "1";
                    var palazzoPrefix = ConfigurationManager.AppSettings.Get("palazzo_account_prefix");
                    var rawUsername = Request.QueryString["username"];
                    var token = Request.QueryString["code"];
                    _username = Request.QueryString["username"].StartsWith(palazzoPrefix)
                        ? Request.QueryString["username"].Remove(0, palazzoPrefix.Length)
                        : Request.QueryString["username"];

                    commonAuditTrail.appendLog(_username, "_Secure_AutoLogin", "Params", string.Empty, string.Empty,
                        string.Empty, "-99", string.Empty, string.Format("{0}|{1}|{2}", rawUsername, _username, token),
                        string.Empty, string.Empty, true);

                    var dsData = client.MemberAutoSigninFromApp(Convert.ToInt64(commonVariables.OperatorId), _username,
                        rawUsername, token,
                        Request.Url.ToString(), Request.UserHostAddress, Session.SessionID);

                    commonAuditTrail.appendLog(_username, "_Secure_AutoLogin", "Response", string.Empty, string.Empty, string.Empty, "-99", string.Empty, dsData.Tables.Count.ToString(), string.Empty, string.Empty, true);

                    if (dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
                    {
                        
                        var result = int.Parse(dsData.Tables[0].Rows[0]["RETURN_VALUE"].ToString());
                        commonAuditTrail.appendLog(_username, "_Secure_AutoLogin", "ReturnValue", string.Empty, string.Empty, string.Empty, "-99", string.Empty, result.ToString(), string.Empty, string.Empty, true);

                        if (result == 1)
                        {
                            new Members().SetSessions(dsData.Tables[0], null);

                            Response.Redirect(
                                !string.IsNullOrEmpty(Request.QueryString.Get("redirect"))
                                    ? Request.QueryString.Get("redirect")
                                    : "/Funds.aspx", false);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog(_username, "_Secure_AutoLogin", "Page_Load", string.Empty, string.Empty,
                string.Empty, "-99", ex.InnerException.ToString(), ex.Message, string.Empty, string.Empty, true);
            Response.Redirect(
                               !string.IsNullOrEmpty(Request.QueryString.Get("redirect"))
                                   ? Request.QueryString.Get("redirect")
                                   : "/Funds.aspx", false);
        }
    }
}