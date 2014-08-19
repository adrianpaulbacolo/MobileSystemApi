<%@ WebHandler Language="C#" Class="XmlMessagesGet" %>

using System;
using System.Web;

public class XmlMessagesGet : IHttpHandler, System.Web.SessionState.IReadOnlySessionState {
    
    public void ProcessRequest (HttpContext context) {
        System.Xml.Linq.XElement xeXML = commonCulture.appData.getRootResource("Message");
        context.Response.ContentType = "text/xml";
        context.Response.Write(xeXML.ToString());
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}