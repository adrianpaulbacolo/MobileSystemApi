using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Models;
using Newtonsoft.Json;
using wsMemberMS1;

public partial class _Secure_ForgotPassword : BasePage
{
    protected System.Xml.Linq.XElement XeErrors = null;
    protected string AlertCode = string.Empty;
    protected string AlertMessage = string.Empty;
    protected System.Xml.Linq.XElement XeResources;
    System.Xml.Linq.XElement _xeRegisterResources;

    protected void Page_Load(object sender, EventArgs e)
    {
        XeErrors = commonVariables.ErrorsXML;
        commonCulture.appData.getRootResource("/_Secure/ForgotPassword", out XeResources);
        commonCulture.appData.getRootResource("/_Secure/Register.aspx", out _xeRegisterResources);

        if (IsPostBack) return;
        SetTitle(commonCulture.ElementValues.getResourceString("Title", XeResources));

        SetQuestions();

        lblUsername.Text = commonCulture.ElementValues.getResourceString("lblUsername", _xeRegisterResources);
        lblEmail.Text = commonCulture.ElementValues.getResourceString("lblEmailAddress", _xeRegisterResources);
        btnStep1.Text = btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", _xeRegisterResources);
    }

    private void SetQuestions()
    {
        commonCulture.appData.getRootResource("/_Secure/UpdateProfile", out XeResources);
        lblSecurityQuestion.Text = commonCulture.ElementValues.getResourceString("lblSecurityQuestion", XeResources);

        string securityQuestion = null;
        foreach (System.Xml.Linq.XElement xeSq in XeResources.Element("drpSecurityQuestion").Elements())
        {
            if (string.Compare(Convert.ToString(xeSq.Name).Substring(2), "0", true) != 0)
            {
                drpSecurityQuestion.Items.Add(new ListItem(xeSq.Value, Convert.ToString(xeSq.Name).Substring(2)));
            }
            else if (string.IsNullOrEmpty(securityQuestion) || Convert.ToInt32(securityQuestion) < 1)
            {
                drpSecurityQuestion.Items.Add(new ListItem(xeSq.Value, Convert.ToString(xeSq.Name).Substring(2)));
            }
        }
        drpSecurityQuestion.SelectedIndex = Convert.ToInt32(securityQuestion);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var email = txtEmail.Text;
        var username = txtUsername.Text;
        var secureAnswer = txtSecurityAnswer.Text;

        var lastSentTime = DateTime.MinValue;
        if (!string.IsNullOrEmpty((string)HttpContext.Current.Session["forgot_login"]))
        {
            lastSentTime = DateTime.ParseExact((string)HttpContext.Current.Session["forgot_login"], "yyyy-MM-dd HH:mm:ss", null);
        }
        if (HttpContext.Current.Request.Cookies["forgot_login"] != null)
        {
            var cookieTime = DateTime.ParseExact((string)HttpContext.Current.Request.Cookies["forgot_login"].Value, "yyyy-MM-dd HH:mm:ss", null);
            if (cookieTime > lastSentTime)
            {
                lastSentTime = cookieTime;
            }
        }

        if (DateTime.Now.AddMinutes(-1) < lastSentTime)
        {
            var ts = lastSentTime - DateTime.Now.AddMinutes(-1);
            AlertMessage = commonCulture.ElementValues.getResourceString("Resent", XeResources).Replace("[min]", ts.Minutes.ToString()).Replace("[sec]", ts.Seconds.ToString());
            AlertCode = "-1";
        }
        else if (string.IsNullOrEmpty(username))
        {
            AlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingUsername", XeErrors);
            AlertCode = "-1";
        }
        else if (string.IsNullOrEmpty(email) || commonValidation.isInjection(email))
        {
            AlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingEmail", XeErrors);
            AlertCode = "-1";
        }
        else if (commonValidation.isInjection(secureAnswer))
        {
            AlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingUsername", XeErrors);
            AlertCode = "-1";
        }
        else
        {
            using (var memberWs = new memberWSSoapClient())
            {

                int secQue;
                int.TryParse(drpSecurityQuestion.SelectedValue, out secQue);

                var result = secQue == 0 ? memberWs.MemberForgotPasswordPartial(long.Parse(commonVariables.OperatorId), username, email) : memberWs.MemberForgotPassword(long.Parse(commonVariables.OperatorId), username, email, secQue, secureAnswer);

                switch (result)
                {
                    case 1:
                        var myCookie = new HttpCookie("forgot_login") { Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") };
                        HttpContext.Current.Response.Cookies.Add(myCookie);
                        AlertMessage = commonCulture.ElementValues.getResourceString("Success", XeResources);
                        AlertCode = "1";
                        break;
                    case 11:
                        AlertMessage = commonCulture.ElementValues.getResourceXPathString("ForgotPassword/NotExist", XeErrors);
                        AlertCode = "0";
                        break;
                    case 12:
                    case 13:
                        AlertMessage = commonCulture.ElementValues.getResourceXPathString("ForgotPassword/IncorrectSecurity", XeErrors);
                        AlertCode = "-1";
                        break;
                    default:
                        AlertMessage = commonCulture.ElementValues.getResourceXPathString("ForgotPassword/Other", XeErrors);
                        AlertCode = "0";
                        break;
                }
            }
        }
    }

    [WebMethod]
    public static int CheckIfMemberPartial(string username, string email)
    {
        try
        {
            using (var member = new memberWSSoapClient())
            {
                return member.MemberPartialRegistration(long.Parse(commonVariables.OperatorId), username, email);
            }
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", "Forgot Password", "CheckIfMemberPartial", string.Empty, string.Empty, string.Empty, "-99", "exception", ex.Message, string.Empty, string.Empty, true);
        }

        return 0;
    }
}