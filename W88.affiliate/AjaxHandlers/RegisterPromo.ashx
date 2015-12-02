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