using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Helpers;

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

    public static int checkSession(string memberSessionId = null)
    {
        var id = memberSessionId ?? commonVariables.CurrentMemberSessionId;
        if (!string.IsNullOrEmpty(id))
        {
            return new Members().CheckMemberSession(id);
        }
        return 10;
    }

    public static bool IsLoggedIn()
    {
        var isLoggedIn = !string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId);
        return isLoggedIn;
    }

    public static string GetSessionCode()
    {
        var strProcessCode = "10";
        if (!IsLoggedIn())
        {
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

    public static string MemberCode
    {
        get
        {
            var member = new Members();
            var info = member.MemberData();
            return info.MemberCode;
        }
    }
}