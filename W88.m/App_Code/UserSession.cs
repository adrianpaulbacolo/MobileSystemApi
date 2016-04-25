using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for UserSession
/// </summary>
public class UserSession
{
    public static void ClearSession()
    {
        commonVariables.ClearSessionVariables();
        commonCookie.ClearCookies(); 
    }

    public static void checkSession()
    {
        if(!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)){
            try
            {
                using (wsMemberMS1.memberWSSoapClient svcInstance = new wsMemberMS1.memberWSSoapClient())
                {
                    System.Data.DataSet dsSignin = null;
                    dsSignin = svcInstance.MemberSessionCheck(commonVariables.CurrentMemberSessionId, commonIp.UserIP);

                    if (dsSignin.Tables[0].Rows.Count > 0)
                    {
                        var strProcessCode = Convert.ToString(dsSignin.Tables[0].Rows[0]["RETURN_VALUE"]);
                        if (strProcessCode == "1")
                        {
                            // re-assign user session variable
                            HttpContext.Current.Session.Add("MemberSessionId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberSessionId"]));
                            HttpContext.Current.Session.Add("MemberId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberId"]));
                            HttpContext.Current.Session.Add("MemberCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberCode"]));
                            HttpContext.Current.Session.Add("CountryCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["countryCode"]));
                            HttpContext.Current.Session.Add("CurrencyCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["currencyCode"]));
                            HttpContext.Current.Session.Add("LanguageCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["languageCode"]));
                            HttpContext.Current.Session.Add("RiskId", Convert.ToString(dsSignin.Tables[0].Rows[0]["riskId"]));
                            HttpContext.Current.Session.Add("PaymentGroup", Convert.ToString(dsSignin.Tables[0].Rows[0]["paymentGroup"]));
                            HttpContext.Current.Session.Add("PartialSignup", Convert.ToString(dsSignin.Tables[0].Rows[0]["partialSignup"]));
                            HttpContext.Current.Session.Add("ResetPassword", Convert.ToString(dsSignin.Tables[0].Rows[0]["resetPassword"]));

                            commonCookie.CookieS = commonVariables.CurrentMemberSessionId;
                            commonCookie.CookieG = commonVariables.CurrentMemberSessionId;

                        }
                    }

                }
            }catch(Exception ex){
                //do nothing
            }
        }
    }

    public static bool IsLoggedIn()
    {
        var isLoggedIn = !string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId);
        return isLoggedIn;
    }

    public static string GetSessionCode()
    {
        var strProcessCode = "10";
        if(!IsLoggedIn()){
            return strProcessCode;
        }
        System.Data.DataSet dsSignin = null;
        try
        {
            using (wsMemberMS1.memberWSSoapClient svcInstance = new wsMemberMS1.memberWSSoapClient())
            {
                dsSignin = svcInstance.MemberSessionCheck(commonVariables.CurrentMemberSessionId, commonIp.UserIP);

                if (dsSignin.Tables[0].Rows.Count > 0)
                {
                    strProcessCode = Convert.ToString(dsSignin.Tables[0].Rows[0]["RETURN_VALUE"]);

                }
            }
        }
        catch (Exception e)
        {
        }
        return strProcessCode;
    }
}