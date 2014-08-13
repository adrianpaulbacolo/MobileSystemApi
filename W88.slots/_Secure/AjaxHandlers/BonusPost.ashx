<%@ WebHandler Language="C#" Class="ASHX_Secure_AjaxHandlers_BonusPost" %>

using System;
using System.Web;

public class ASHX_Secure_AjaxHandlers_BonusPost : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{    
    public void ProcessRequest (HttpContext context) {
        string strBonusUrl = System.Configuration.ConfigurationManager.AppSettings.Get("SlotBonusUrl");
        string strSlotSessionId = commonVariables.GetSessionVariable("SID");
        string strGetUrl = string.Empty;
        string strBonusKey = string.Empty;
        string strChoice = string.Empty; 
        string strStep = string.Empty;
        string strResponse = string.Empty;
        System.Xml.Linq.XElement xResults = null;
        try
        {
            strBonusUrl = System.Configuration.ConfigurationManager.AppSettings.Get("SlotBonusUrl");
            strSlotSessionId = commonVariables.GetSessionVariable("SID");
            strBonusKey = context.Request.Form.Get("key");
            strChoice = context.Request.Form.Get("choice");
            strStep = context.Request.Form.Get("step");
            using (customConfig.WebClientWithTimeOut wsUrlGet = new customConfig.WebClientWithTimeOut(5000))
            {
                System.Collections.Specialized.NameValueCollection dataValues = new System.Collections.Specialized.NameValueCollection();
                wsUrlGet.Headers.Add("Accept", "text/xml");

                strGetUrl = string.Format("{0}?key={1}&bonus={2}&step={3}&param={4}", strBonusUrl, strSlotSessionId, strBonusKey, strStep, strChoice);
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
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}