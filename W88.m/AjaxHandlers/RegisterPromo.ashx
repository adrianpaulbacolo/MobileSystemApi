<%@ WebHandler Language="C#" Class="AjaxHandlers_ASHX_RegisterPromo" %>

using System;
using System.Web;

public class AjaxHandlers_ASHX_RegisterPromo : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{    
    public void ProcessRequest (HttpContext context) {
        
        long lngOperatorId = long.MinValue;
        long lngMemberId = long.MinValue;
        string strSubjectCode = string.Empty;
        string strComment = string.Empty;
        int intResult = int.MinValue;

        lngOperatorId = Convert.ToInt64(string.IsNullOrEmpty(commonVariables.OperatorId) ? "-1" : commonVariables.OperatorId);
        lngMemberId = Convert.ToInt64(string.IsNullOrEmpty(commonVariables.GetSessionVariable("MemberId")) ? "-1" : commonVariables.GetSessionVariable("MemberId"));
        strSubjectCode = context.Request.Form.Get("SCode");
        strComment = context.Request.Form.Get("Comment");

        switch (strComment.ToLower()) 
        {
            case "asports":
                strComment = "SB";
                break;
            case "esports":
                strComment = "SB2";
                break;
            case "isports":
                strComment = "SB3";
                break;
            case "usports":
                strComment = "SB4";
                break;
            case "bravado":
                strComment = "slot";
                break;
            case "crescendo":
                strComment = "ctxm";
                break;
            case "divino":
                strComment = "betsoft";
                break;
            case "palazzo":
                strComment = "playtech";
                break;
            case "massimo":
                strComment = "vanguard";
                break;
            case "apollo":
                strComment = "ags";
                break;
            case "casino":
            case "ilotto":
            case "keno":
            default:
                break;
        }

        intResult = 1;  
        using (wsMemberMS1.memberWSSoapClient wsInstance = new wsMemberMS1.memberWSSoapClient()) 
        {
            intResult = wsInstance.MemberPromotionRegistration(lngOperatorId, lngMemberId, strSubjectCode, strComment);

            switch (intResult) 
            {
                case 1:
                    break;
                case 10:
                    break;
                default:
                    break; 
            }
        }

        context.Response.Write(intResult);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}