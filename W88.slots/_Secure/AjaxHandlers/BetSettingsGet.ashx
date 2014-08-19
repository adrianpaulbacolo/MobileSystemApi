<%@ WebHandler Language="C#" Class="ASHX_Secure_AjaxHandlers_BetSettingsGet" %>

using System;
using System.Web;

public class ASHX_Secure_AjaxHandlers_BetSettingsGet : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{    
    public void ProcessRequest (HttpContext context) {
        string strBetUrl = string.Empty;
        string strGetUrl = string.Empty;
        string strSlotSessionId = string.Empty;
        string strResponse = string.Empty;
        System.Xml.Linq.XElement xResults = null;
        
        try
        {
            strSlotSessionId = commonVariables.GetSessionVariable("SID");
            strBetUrl = System.Configuration.ConfigurationManager.AppSettings.Get("SlotBetUrl");            

            using (customConfig.WebClientWithTimeOut wsUrlGet = new customConfig.WebClientWithTimeOut(5000))
            {
                System.Collections.Specialized.NameValueCollection dataValues = new System.Collections.Specialized.NameValueCollection();
                wsUrlGet.Headers.Add("Accept", "text/xml");

                strGetUrl = string.Format("{0}?key={1}", strBetUrl, strSlotSessionId);
                byte[] byteResponse = wsUrlGet.DownloadData(strGetUrl);
                strResponse = System.Text.Encoding.UTF8.GetString(byteResponse);

                xResults = System.Xml.Linq.XElement.Parse(xmlData.RemoveCarriageReturn(xmlData.RemoveAllNamespaces(strResponse).Replace("&", "&amp;")));
                context.Response.ContentType = "text/xml";
                context.Response.Write(strResponse);
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