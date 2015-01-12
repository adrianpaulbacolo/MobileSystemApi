<%@ WebHandler Language="C#" Class="handlers_getlabel" %>

using System;
using System.Web;

public class handlers_getlabel : IHttpHandler
{    
    public void ProcessRequest (HttpContext context) 
    {
        string strPath = string.Empty;
        context.Response.Write(System.IO.File.ReadAllText(context.Server.MapPath("/") + @"/App_Data/products.json"));
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}