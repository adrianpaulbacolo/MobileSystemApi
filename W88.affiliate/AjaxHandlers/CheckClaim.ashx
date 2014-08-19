<%@ WebHandler Language="C#" Class="AjaxHandlers_ASHX_CheckClaim" %>

using System;
using System.Web;

public class AjaxHandlers_ASHX_CheckClaim : IHttpHandler
{    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        context.Response.Write("Hello World");
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}