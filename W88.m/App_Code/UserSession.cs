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

    public static bool IsLoggedIn()
    {
        return !string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId);
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