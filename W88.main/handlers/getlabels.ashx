<%@ WebHandler Language="C#" Class="handlers_getlabels" %>

using System;
using System.Web;

public class handlers_getlabels : IHttpHandler
{    
    public void ProcessRequest (HttpContext context) 
    {
        string strPath = string.Empty;
        string strExtension = string.Empty;
        string strFilePath = string.Empty;
        
        strPath = context.Request.Form.Get("path");
        strExtension = context.Request.Form.Get("ext");
        strFilePath = string.Format(@"{0}/App_Data/{1}.{2}", context.Server.MapPath("/"), strPath, strExtension);

        if (System.IO.File.Exists(strFilePath))
        {
            if (!string.IsNullOrWhiteSpace(strPath))
            {
                context.Response.Write(System.IO.File.ReadAllText(strFilePath));
                context.Response.Flush();                
            }
        }

        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}