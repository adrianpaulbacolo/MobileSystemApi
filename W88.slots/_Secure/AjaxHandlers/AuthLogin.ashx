<%@ WebHandler Language="C#" Class="ASHX_Secure_AjaxHandlers_AuthLogin" %>

using System;
using System.Web;

public class ASHX_Secure_AjaxHandlers_AuthLogin : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string strAuthLoginUrl = string.Empty;
        string strSlotSessionId = string.Empty;
        string strLogin = "656321";
        string strPassword = "2ffe3d4a97f843cead55fc88b7df1bb2";
        string strVendorId = "1";
        string strGetUrl = string.Empty;
        string strResponse = string.Empty;
        System.Xml.Linq.XElement xResults = null;
        
        try
        {
            strAuthLoginUrl = System.Configuration.ConfigurationManager.AppSettings.Get("SlotAuthLoginUrl");

            using (customConfig.WebClientWithTimeOut wsUrlGet = new customConfig.WebClientWithTimeOut(5000))
            {
                System.Collections.Specialized.NameValueCollection dataValues = new System.Collections.Specialized.NameValueCollection();
                wsUrlGet.Headers.Add("Accept", "text/xml");

                strGetUrl = string.Format("{0}?login={1}&password={2}&vendorid={3}", strAuthLoginUrl, strLogin, strPassword, strVendorId);
                byte[] byteResponse = wsUrlGet.DownloadData(strGetUrl);
                strResponse = System.Text.Encoding.UTF8.GetString(byteResponse);
                xResults = System.Xml.Linq.XElement.Parse(xmlData.RemoveCarriageReturn(xmlData.RemoveAllNamespaces(strResponse).Replace("&", "&amp;")));
                strSlotSessionId = xResults.Value;
                context.Session.Add("SID", strSlotSessionId);
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