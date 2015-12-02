<%@ WebHandler Language="C#" Class="ASHX_Secure_AjaxHandlers_BetsGet" %>

using System;
using System.Web;

public class ASHX_Secure_AjaxHandlers_BetsGet : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string strGetBetsUrl = string.Empty;
        string strSlotSessionId = string.Empty;
        string strGetUrl = string.Empty;
        string strResponse = string.Empty;
        System.Xml.Linq.XElement xResults = null;
        
        try
        {
            strGetBetsUrl = System.Configuration.ConfigurationManager.AppSettings.Get("SlotBetUrl");
            strSlotSessionId = commonVariables.GetSessionVariable("SID");

            using (customConfig.WebClientWithTimeOut wsUrlGet = new customConfig.WebClientWithTimeOut(5000))
            {
                System.Collections.Specialized.NameValueCollection dataValues = new System.Collections.Specialized.NameValueCollection();
                wsUrlGet.Headers.Add("Accept", "text/xml");

                strGetUrl = string.Format("{0}?key={1}", strGetBetsUrl, strSlotSessionId);
                byte[] byteResponse = wsUrlGet.DownloadData(strGetUrl);
                strResponse = System.Text.Encoding.UTF8.GetString(byteResponse);

                xResults = System.Xml.Linq.XElement.Parse(xmlData.RemoveCarriageReturn(xmlData.RemoveAllNamespaces(strResponse).Replace("&", "&amp;")));
            }
        }
        catch (Exception) { }

        context.Response.ContentType = "text/xml";
        context.Response.Write(strResponse);
        context.ApplicationInstance.CompleteRequest();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}