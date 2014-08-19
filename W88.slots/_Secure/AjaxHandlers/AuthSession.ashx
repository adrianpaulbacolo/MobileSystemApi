<%@ WebHandler Language="C#" Class="ASHX_Secure_AjaxHandlers_AuthSession" %>

using System;
using System.Web;

public class ASHX_Secure_AjaxHandlers_AuthSession : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string strAuthSessionUrl = string.Empty;
        string strSlotSessionId = string.Empty;
        string strGetUrl = string.Empty;
        string strResponse = string.Empty;
        System.Xml.Linq.XElement xResults = null;
        string strOperatorCode = string.Empty;
        string strMemberSessionId = string.Empty;
        
        try
        {
            strAuthSessionUrl = System.Configuration.ConfigurationManager.AppSettings.Get("SlotAuthSessionUrl");
            strOperatorCode = System.Configuration.ConfigurationManager.AppSettings.Get("OperatorCode");
            strMemberSessionId = commonCookie.CookieS;

            using (customConfig.WebClientWithTimeOut wsUrlGet = new customConfig.WebClientWithTimeOut(5000))
            {
                System.Collections.Specialized.NameValueCollection dataValues = new System.Collections.Specialized.NameValueCollection();
                wsUrlGet.Headers.Add("Accept", "text/xml");

                strGetUrl = string.Format("{0}?iid={1}&token={2}", strAuthSessionUrl, strOperatorCode, strMemberSessionId);
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