using System;
using System.Configuration;
using Helpers;
using wsMemberMS1;

public partial class _Secure_AutoLogin : System.Web.UI.Page
{
    private string _username = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            using (var client = new memberWSSoapClient())
            {
                commonCookie.CookieIsApp = "1";
                var palazzoPrefix = ConfigurationManager.AppSettings.Get("palazzo_account_prefix");
                var rawUsername = Request.QueryString["username"];
                var token = Request.QueryString["token"];
                _username = Request.QueryString["username"].StartsWith(palazzoPrefix)
                    ? Request.QueryString["username"].Remove(0, palazzoPrefix.Length)
                    : Request.QueryString["username"];

                var dsData = client.MemberAutoSigninFromApp(Convert.ToInt64(commonVariables.OperatorId), _username,
                    rawUsername, token,
                    Request.Url.ToString(), Request.UserHostAddress, Session.SessionID);

                if (dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
                {
                    var result = int.Parse(dsData.Tables[0].Rows[0]["RETURN_VALUE"].ToString());
                    if (result == 1)
                    {
                        new Members().SetSessions(dsData.Tables[0], null);

                        Response.Redirect(
                            !string.IsNullOrEmpty(Request.QueryString.Get("redirect"))
                                ? Request.QueryString.Get("redirect")
                                : "/Funds.aspx", false);
                    }
                    else
                    {
                         Response.Redirect("./?" + Request.QueryString, false);
                    }

                }
                else
                {
                    Response.Redirect("./?" + Request.QueryString, false);
                }
            }
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog(_username, "_Secure_AutoLogin", "Page_Load", string.Empty, string.Empty,
                string.Empty, "-99", ex.InnerException.ToString(), ex.Message, string.Empty, string.Empty, true);
            Response.Redirect("./?" + Request.QueryString, false);
        }
    }
}