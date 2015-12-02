<%@ WebHandler Language="C#" Class="_Secure_AjaxHandlers_CancelWithdrawal" %>

using System;
using System.Web;

public class _Secure_AjaxHandlers_CancelWithdrawal : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{    
    public void ProcessRequest (HttpContext context) {

        string strInvId = string.Empty;
        string strPayMethodId = string.Empty;
        string strOperatorId = string.Empty;
        string strMemberCode = string.Empty;
        string strStatusCode = string.Empty;
        string strStatusText = string.Empty;
        string strReturnValue = string.Empty;
       
        bool WithdrawalCancelled = false;
        
        strOperatorId = commonVariables.OperatorId;
        strMemberCode = commonVariables.GetSessionVariable("MemberCode");
        strPayMethodId = context.Request.Form.Get("MethodId");
        strInvId = context.Request.Form.Get("TrxId");
        
        using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient()) { WithdrawalCancelled = svcInstance.cancelWithdrawal(Convert.ToInt64(strInvId), Convert.ToInt64(strPayMethodId), Convert.ToInt64(strOperatorId), strMemberCode, out strStatusCode, out strStatusText); }

        if (WithdrawalCancelled) { strReturnValue = "0"; } else { strReturnValue = strStatusCode; }

        context.Response.ContentType = "text/plain";
        context.Response.Write(strReturnValue);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}