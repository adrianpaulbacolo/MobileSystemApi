<%@ WebHandler Language="C#" Class="ASHX_Secure_AjaxHandlers_WheelSpin" %>

using System;
using System.Web;

public class ASHX_Secure_AjaxHandlers_WheelSpin : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{    
    public void ProcessRequest (HttpContext context) {
        string strGetBetsUrl = System.Configuration.ConfigurationManager.AppSettings.Get("SlotSpinUrl");
        string strSlotSessionId = commonVariables.GetSessionVariable("SID");
        string strGetUrl = string.Empty;
        System.Xml.Linq.XElement xResults = null;
        string strBetAmount = string.Empty;
        string strBetLines = string.Empty;
        string strMultiply = string.Empty;

        string strOperatorCode = System.Configuration.ConfigurationManager.AppSettings.Get("OperatorCode");
        string strFunUrl = System.Configuration.ConfigurationManager.AppSettings.Get("SlotKeyFunUrl");
        string strRealUrl = System.Configuration.ConfigurationManager.AppSettings.Get("SlotKeyRealUrl");
        string strResponse = string.Empty;
        
        strBetAmount = context.Request.Form.Get("Amount");
        strBetLines = context.Request.Form.Get("Lines");
        strMultiply = context.Request.Form.Get("Multiply");

        strBetAmount = "0.20";
        strBetLines = "30";
        strMultiply = "20";

        if (string.IsNullOrEmpty(strSlotSessionId))
        {
            try
            {
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
        else
        {
            try
            {
                using (customConfig.WebClientWithTimeOut wsUrlGet = new customConfig.WebClientWithTimeOut(5000))
                {
                    System.Collections.Specialized.NameValueCollection dataValues = new System.Collections.Specialized.NameValueCollection();
                    wsUrlGet.Headers.Add("Accept", "text/xml");

                    strGetUrl = string.Format("{0}?key={1}&bet={2}&lineBet={3}&multiply={4}", strGetBetsUrl, strSlotSessionId, strBetAmount, strBetLines, strMultiply);
                    byte[] byteResponse = wsUrlGet.DownloadData(strGetUrl);
                    strResponse = System.Text.Encoding.UTF8.GetString(byteResponse);

                    xResults = System.Xml.Linq.XElement.Parse(xmlData.RemoveCarriageReturn(xmlData.RemoveAllNamespaces(strResponse).Replace("&", "&amp;")));

                    context.Response.ContentType = "text/xml";
                    context.Response.Write(strResponse);
                }
            }

            catch (Exception) { }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}