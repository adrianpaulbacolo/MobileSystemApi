<%@ WebHandler Language="C#" Class="_Secure_AjaxHandlers_MemberSessionCheck" %>

using System;
using System.Web;

public class _Secure_AjaxHandlers_MemberSessionCheck : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    public void ProcessRequest (HttpContext context)
    {
        System.Xml.Linq.XElement xeErrors = commonVariables.ErrorsXML;
        string json = HttpContext.Current.Request.Form["json"];
        var js = new System.Web.Script.Serialization.JavaScriptSerializer();
        var response = new SessionCheckResponse();
        response.code = UserSession.GetSessionCode();

        switch (UserSession.GetSessionCode())
        {
            case "0":
                response.message = commonCulture.ElementValues.getResourceString("Exception", xeErrors);
                break;
            case "1":
                response.message = "Has Session";
                break;
            case "22":
                response.message = commonCulture.ElementValues.getResourceXPathString("InactiveAccount", xeErrors);
                break;
            case "10":
            case "13":
            default:
                response.message = commonCulture.ElementValues.getResourceXPathString("SessionExpired", xeErrors);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.Write(js.Serialize(response));
        context.Response.End();
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}

public class SessionCheckResponse
{
    public string code;
    public string message;
}