<%@ WebHandler Language="C#" Class="ASHX_Secure_AjaxHandlers_KeysGet" %>

using System;
using System.Web;

public class ASHX_Secure_AjaxHandlers_KeysGet : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{    
    public void ProcessRequest (HttpContext context) {
        string strOperatorCode = string.Empty;
        string strFunUrl = string.Empty;
        string strRealUrl = string.Empty;
        string strGetUrl = string.Empty;
        string strSlotSessionId = string.Empty;
        string strResponse = string.Empty;
        System.Xml.Linq.XElement xResults = null;
        
        try
        {
            strOperatorCode = System.Configuration.ConfigurationManager.AppSettings.Get("OperatorCode");
            strFunUrl = System.Configuration.ConfigurationManager.AppSettings.Get("SlotKeyFunUrl");
            strRealUrl = System.Configuration.ConfigurationManager.AppSettings.Get("SlotKeyRealUrl");

            using (customConfig.WebClientWithTimeOut wsUrlGet = new customConfig.WebClientWithTimeOut(5000))
            {
                System.Collections.Specialized.NameValueCollection dataValues = new System.Collections.Specialized.NameValueCollection();
                wsUrlGet.Headers.Add("Accept", "text/xml");

                strGetUrl = string.Format("{0}?iid={1}", strFunUrl, strOperatorCode);
                byte[] byteResponse = wsUrlGet.DownloadData(strGetUrl);
                strResponse = System.Text.Encoding.UTF8.GetString(byteResponse);

                xResults = System.Xml.Linq.XElement.Parse(xmlData.RemoveCarriageReturn(xmlData.RemoveAllNamespaces(strResponse).Replace("&", "&amp;")));
                strSlotSessionId = xResults.Value;
                context.Session.Add("SID", strSlotSessionId);                
                context.Response.ContentType = "text/xml";
                //context.Response.Write(strResponse);
                context.ApplicationInstance.CompleteRequest();
            }
        }
        catch (Exception) { }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}