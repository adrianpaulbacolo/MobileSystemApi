<%@ WebHandler Language="C#" Class="_secure_handlers_login_process" %>

using System;
using System.Web;

public class _secure_handlers_login_process : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        long lngOperatorId = long.MinValue;
        string strInputUsername = string.Empty;
        string strInputPassword = string.Empty;
        string strMemberSessionId = string.Empty;
        string strSiteURL = string.Empty;
        string strLoginIP = string.Empty;
        string strDeviceID = string.Empty;

        string strUserPassword = string.Empty;
        var v_dictionary = new System.Collections.Generic.Dictionary<string, string>();
        bool _signin = true;
        bool _passwordincorrect = true;

        string strErrorCode = string.Empty;
        string strReturnVal = string.Empty;

        System.Data.DataSet dsMember = null;
        System.Data.DataSet dsSignin = null;

        lngOperatorId = Convert.ToInt64(common.variables.OperatorId);
        strMemberSessionId = common.variables.CurrentMemberSessionId;
        strDeviceID = context.Request.UserAgent;
        strLoginIP = string.IsNullOrEmpty(context.Request.Form.Get("ipadd")) ? common.ips.userip : context.Request.Form.Get("ipadd");

        if (string.IsNullOrWhiteSpace(strMemberSessionId))
        {
            // login via username / password 
            strInputUsername = Convert.ToString(context.Request.Form.Get("username"));
            strInputPassword = Convert.ToString(context.Request.Form.Get("password"));

            if (string.IsNullOrWhiteSpace(strInputUsername)) { _signin = false; }
            else if (string.IsNullOrWhiteSpace(strInputPassword)) { _signin = false; }
            else if (common.validation._isinjection(strInputUsername)) { _signin = false; }
            else if (common.validation._isinjection(strInputPassword)) { _signin = false; }

            try
            {
                // get member info and check password
                using (wsMemberCoreMobile.mws_member wsinstance = new wsMemberCoreMobile.mws_member())
                {
                    wsinstance.Timeout = 10000;

                    dsMember = wsinstance.GetMemberInfoByMemberCode(lngOperatorId, strInputUsername);

                    if (dsMember == null) { }
                    else if (dsMember.Tables.Count < 1) { }
                    else if (dsMember.Tables[0].Rows.Count < 1) { }
                    else
                    {
                        strUserPassword = Convert.ToString(dsMember.Tables[0].Rows[0]["password"]);
                    }
                    string strDecryptedPwd = common.encryption.Decrypt(strUserPassword);
                    if (string.Compare(strInputPassword, common.encryption.GetMd5Hash(strDecryptedPwd), true) != 0)
                    {
                        v_dictionary.Add("error", "1");
                        v_dictionary.Add("alert", "incorrect_password");
                        //context.Response.Write("");
                        //return;

                        string strLoginCount = common.variables.GetSessionVariable("login_count");

                        if (string.IsNullOrWhiteSpace(strLoginCount)) { strLoginCount = "0"; }

                        HttpContext.Current.Session.Add("login_count", Convert.ToInt32(strLoginCount) + 1);
                    }
                    else { _passwordincorrect = false; HttpContext.Current.Session.Remove("login_count"); }
                }

                //member signin
                if (_signin)
                {
                    using (wsMemberCore.memberWS wsinstance = new wsMemberCore.memberWS())
                    {
                        wsinstance.Timeout = 10000;
                        if (_passwordincorrect) { strUserPassword = strInputPassword; }

                        dsSignin = null;
                        dsSignin = wsinstance.MemberSignin(lngOperatorId, strInputUsername, strUserPassword, strSiteURL, strLoginIP, strDeviceID);

                        if (dsSignin.Tables[0].Rows.Count > 0)
                        {
                            strReturnVal = Convert.ToString(dsSignin.Tables[0].Rows[0]["RETURN_VALUE"]);
                            v_dictionary.Add("error", "1");

                            switch (strReturnVal)
                            {
                                case "0":
                                    //strProcessMessage = commonCulture.ElementValues.getResourceString("Exception", xeErrors);
                                    break;
                                case "1":
                                    v_dictionary.Remove("error");
                                    v_dictionary.Add("error", "0");
                                    v_dictionary.Add("alert", "success");

                                    strMemberSessionId = Convert.ToString(dsSignin.Tables[0].Rows[0]["memberSessionId"]);
                                    HttpContext.Current.Session.Add("MemberSessionId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberSessionId"]));
                                    HttpContext.Current.Session.Add("MemberId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberId"]));
                                    HttpContext.Current.Session.Add("MemberCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberCode"]));
                                    HttpContext.Current.Session.Add("CountryCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["countryCode"]));
                                    HttpContext.Current.Session.Add("CurrencyCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["currency"]));
                                    HttpContext.Current.Session.Add("LanguageCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["languageCode"]));
                                    HttpContext.Current.Session.Add("RiskId", Convert.ToString(dsSignin.Tables[0].Rows[0]["riskId"]));
                                    //HttpContext.Current.Session.Add("PaymentGroup", "A"); //Convert.ToString(dsSignin.Tables[0].Rows[0]["paymentGroup"]));
                                    HttpContext.Current.Session.Add("PartialSignup", Convert.ToString(dsSignin.Tables[0].Rows[0]["partialSignup"]));
                                    HttpContext.Current.Session.Add("ResetPassword", Convert.ToString(dsSignin.Tables[0].Rows[0]["resetPassword"]));

                                    common.cookies.cookie_s = strMemberSessionId;
                                    common.cookies.cookie_g = strMemberSessionId;
                                    HttpContext.Current.Session.Add("LoginStatus", "success");

                                    //strLastLoginIP = Convert.ToString(dsSignin.Tables[0].Rows[0]["lastLoginIP"]);
                                    //if (HttpContext.Current.Request.Cookies[strMemberCode] == null) { runIovation = true; }
                                    //else if (HttpContext.Current.Request.Cookies[strMemberCode] != null && string.Compare(strLastLoginIP, strLoginIp, true) != 0) { runIovation = true; }
                                    //if (runIovation) { this.IovationSubmit(ref intProcessSerialId, strProcessId, strPageName, strMemberCode, strLoginIp, strPermission); }
                                    break;
                                case "21":
                                    v_dictionary.Add("alert", "invalid_user");
                                    break;
                                case "22":
                                    v_dictionary.Add("alert", "inactive_account");
                                    break;
                                case "23":
                                    //strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/InvalidPassword", xeErrors);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { }
        }
        else
        {
            // login via session id 
            using (wsMemberCore.memberWS wsInstance = new wsMemberCore.memberWS())
            {
                dsSignin = null;
                dsSignin = wsInstance.MemberSessionCheck(strMemberSessionId, strLoginIP);

                if (dsSignin.Tables[0].Rows.Count > 0)
                {
                    strReturnVal = Convert.ToString(dsSignin.Tables[0].Rows[0]["RETURN_VALUE"]);
                    v_dictionary.Add("error", "1");

                    switch (strReturnVal)
                    {
                        case "0":
                            //strProcessMessage = "Exception";
                            break;
                        case "1":
                            v_dictionary.Remove("error");
                            v_dictionary.Add("error", "0");
                            v_dictionary.Add("alert", "success");

                            strMemberSessionId = Convert.ToString(dsSignin.Tables[0].Rows[0]["memberSessionId"]);
                            HttpContext.Current.Session.Add("MemberSessionId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberSessionId"]));
                            HttpContext.Current.Session.Add("MemberId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberId"]));
                            HttpContext.Current.Session.Add("MemberCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberCode"]));
                            HttpContext.Current.Session.Add("CountryCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["countryCode"]));
                            HttpContext.Current.Session.Add("CurrencyCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["currencyCode"]));
                            HttpContext.Current.Session.Add("LanguageCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["languageCode"]));
                            HttpContext.Current.Session.Add("RiskId", Convert.ToString(dsSignin.Tables[0].Rows[0]["riskId"]));
                            //HttpContext.Current.Session.Add("PaymentGroup", "A"); //Convert.ToString(dsSignin.Tables[0].Rows[0]["paymentGroup"]));
                            HttpContext.Current.Session.Add("PartialSignup", Convert.ToString(dsSignin.Tables[0].Rows[0]["partialSignup"]));
                            HttpContext.Current.Session.Add("ResetPassword", Convert.ToString(dsSignin.Tables[0].Rows[0]["resetPassword"]));

                            common.cookies.cookie_s = strMemberSessionId;
                            common.cookies.cookie_g = strMemberSessionId;
                            HttpContext.Current.Session.Add("LoginStatus", "success");
                            break;
                        case "10":
                            //strProcessMessage = "NotLogin";
                            common.variables.ClearSessionVariables();
                            common.cookies.clearAll();
                            break;
                        case "21":
                            v_dictionary.Add("alert", "invalid_user");
                            break;
                        case "22":
                            v_dictionary.Add("alert", "inactive_account");
                            break;
                        case "23":
                            //strProcessMessage = "InvalidPassword";
                            break;
                    }
                }
            }
        }

        //context.Response.ContentType = "text/plain";
        //context.Response.Write("Hello World");
        //context.Response.Write(common.encryption.GetMd5Hash("email@dot.com"));
        //v_dictionary.Add("error_code", strErrorCode);

        context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(v_dictionary));
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}