<%@ WebHandler Language="C#" Class="https" %>

using System;
using System.Web;

public class https : IHttpHandler, System.Web.SessionState.IReadOnlySessionState {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/html";
        System.Text.RegularExpressions.Regex rxDomains_CN = new System.Text.RegularExpressions.Regex(commonVariables.ChinaDomain);

        if (rxDomains_CN.IsMatch(context.Request.ServerVariables["SERVER_NAME"]))
        {
            //commonVariables.SelectedLanguage = "zh-cn";
        }

        context.Response.Write(rxDomains_CN.IsMatch(context.Request.ServerVariables["SERVER_NAME"]) + "<br />");
        context.Response.Write(commonVariables.SelectedLanguage);
        
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