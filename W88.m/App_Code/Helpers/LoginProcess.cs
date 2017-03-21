using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using customConfig;
using Helpers;
using Models;

/// <summary>
/// Summary description for LoginProcess
/// </summary>
public class LoginProcess
{
    private readonly LoginInfo _loginInfo;

	public LoginProcess(LoginInfo info)
	{
	    _loginInfo = info;
	}

    public void CheckResult(ref ProcessCode process, DataTable dTable, ref int serialId, string processId )
    {
        switch (process.Code)
        {
            case "0":
                process.Message = commonCulture.ElementValues.getResourceString("Exception", _loginInfo.XeErrors);
                break;
            case "1":
                SetSessions(dTable, _loginInfo.Password);

                if (IsResetPassword())
                {
                    process.Code = "resetPassword";
                }

                //CheckIovation(Convert.ToString(dTable.Rows[0]["lastLoginIP"]), ref serialId, processId);

                process.Message = string.Empty;
                break;
            case "21":
                process.Message = commonCulture.ElementValues.getResourceXPathString("Login/InvalidUsername", _loginInfo.XeErrors);
                break;
            case "22":
                process.Message = commonCulture.ElementValues.getResourceXPathString("Login/InactiveAccount", _loginInfo.XeErrors);
                break;
            case "23":
                process.Message = commonCulture.ElementValues.getResourceXPathString("Login/InvalidPassword", _loginInfo.XeErrors);
                break;
        }
    }

    public ProcessCode ValidateData()
    {
        var msg = new ProcessCode();

        if (string.IsNullOrEmpty(_loginInfo.Username))
        {
            msg.Code = "-1";
            msg.Message = commonCulture.ElementValues.getResourceXPathString("Login/MissingUsername", _loginInfo.XeErrors);
            msg.IsAbort = true;
        }
        else if (string.IsNullOrEmpty(_loginInfo.Password))
        {
            msg.Code = "-1";
            msg.Message = commonCulture.ElementValues.getResourceXPathString("Login/MissingPassword", _loginInfo.XeErrors);
            msg.IsAbort = true;
        }
        else if (commonValidation.isInjection(_loginInfo.Username))
        {
            msg.Code = "-1";
            msg.Message = commonCulture.ElementValues.getResourceXPathString("Login/InvalidUsername", _loginInfo.XeErrors);
            msg.IsAbort = true;
        }
        else if (commonValidation.isInjection(_loginInfo.Password))
        {
            msg.Code = "-1";
            msg.Message = commonCulture.ElementValues.getResourceXPathString("Login/InvalidPassword", _loginInfo.XeErrors);
            msg.IsAbort = true;
        }
        else if (!string.IsNullOrEmpty(_loginInfo.Captcha) && !string.IsNullOrEmpty(_loginInfo.SessionCaptcha))
        {
            if (string.IsNullOrEmpty(_loginInfo.Captcha))
            {
                msg.Code = "-1";
                msg.Message = commonCulture.ElementValues.getResourceString("MissingVCode", _loginInfo.XeErrors);
                msg.IsAbort = true;
            }
            else if (commonValidation.isInjection(_loginInfo.Captcha))
            {
                msg.Code = "-1";
                msg.Message = commonCulture.ElementValues.getResourceXPathString("Register/InvalidVCode",
                    _loginInfo.XeErrors);
                msg.IsAbort = true;
            }
            if (_loginInfo.Captcha != commonEncryption.decrypting(_loginInfo.SessionCaptcha))
            {
                msg.Code = "-1";
                msg.Message = commonCulture.ElementValues.getResourceXPathString("Register/IncorrectVCode",
                    _loginInfo.XeErrors);
                msg.IsAbort = true;
            }
        }
        

        return msg;
    }

    private void SetSessions(DataTable dTable, string password)
    {
        var memberSessionId = dTable.Rows[0]["memberSessionId"];
        var riskId = dTable.Rows[0]["riskId"].ToString();
        HttpContext.Current.Session.Add("MemberSessionId", memberSessionId);
        HttpContext.Current.Session.Add("MemberId", dTable.Rows[0]["memberId"]);
        HttpContext.Current.Session.Add("MemberCode", dTable.Rows[0]["memberCode"]);
        HttpContext.Current.Session.Add("CountryCode", dTable.Rows[0]["countryCode"]);
        HttpContext.Current.Session.Add("CurrencyCode", dTable.Rows[0]["currency"]);
        HttpContext.Current.Session.Add("LanguageCode", dTable.Rows[0]["languageCode"]);
        HttpContext.Current.Session.Add("RiskId", riskId);
        HttpContext.Current.Session.Add("PaymentGroup", dTable.Rows[0]["paymentGroup"]);
        HttpContext.Current.Session.Add("PartialSignup", dTable.Rows[0]["partialSignup"]);
        HttpContext.Current.Session.Add("ResetPassword", dTable.Rows[0]["resetPassword"]);

        commonCookie.CookieS = Convert.ToString(memberSessionId);
        commonCookie.CookieG = Convert.ToString(memberSessionId);
        commonCookie.CookiePalazzo = password;

        var opSettings = new OperatorSettings("W88");
        foreach (var v in opSettings.Values.Get("VIP_Allowed").ToUpper().Split(new[] { '|' }).Where(v => v.Equals(riskId)))
        {
            commonCookie.CookieVip = "true";
        }
    }

    private bool IsResetPassword()
    {
        return Convert.ToBoolean((HttpContext.Current.Session["ResetPassword"] == null) ? 0 : HttpContext.Current.Session["ResetPassword"]);
    }

    private string CheckMemberSession(int signInreturnValue)
    {
        string returnMsg = string.Empty;
        using (var svcInstance = new wsMemberMS1.memberWSSoapClient())
        {
            DataSet dsMember = svcInstance.MemberSessionCheck(commonVariables.CurrentMemberSessionId, commonIp.UserIP);

            if (dsMember.Tables[0].Rows.Count > 0)
            {
                switch (signInreturnValue)
                {
                    case 0:
                        returnMsg = commonCulture.ElementValues.getResourceString("Exception", _loginInfo.XeErrors);
                        break;
                    case 1:
                        HttpContext.Current.Session.Add("MemberName",
                            Convert.ToString(dsMember.Tables[0].Rows[0]["lastName"]) +
                            Convert.ToString(dsMember.Tables[0].Rows[0]["firstName"]));
                        break;
                }
            }
        }

        return returnMsg;
    }
     
    private void CheckIovation(string lastLoginIp, ref int serialId, string processId)
    {
        var runIovation = false;
        if (HttpContext.Current.Request.Cookies[_loginInfo.Username] == null)
        {
            runIovation = true;
        }
        else if (HttpContext.Current.Request.Cookies[_loginInfo.Username] != null && string.Compare(lastLoginIp, commonIp.UserIP, true) != 0)
        {
            runIovation = true;
        }
        if (runIovation)
        {
            new Iovation().IovationSubmit(ref serialId, processId, commonIp.UserIP, _loginInfo);
        }
    }
}