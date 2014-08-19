<%@ WebHandler Language="C#" Class="https" %>

using System;
using System.Web;

public class https : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
	foreach (string key in context.Request.ServerVariables.AllKeys) 
            {
                context.Response.Write(key + " : <b>" + context.Request.ServerVariables[key] + "</b><br />");
            }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}