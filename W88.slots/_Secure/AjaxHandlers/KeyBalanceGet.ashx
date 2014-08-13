<%@ WebHandler Language="C#" Class="ASHX_Secure_AjaxHandlers_KeyBalanceGet" %>

using System;
using System.Web;

public class ASHX_Secure_AjaxHandlers_KeyBalanceGet : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        bool isProcessAbort = false;
        string strOperatorCode = string.Empty;
        string strFunUrl = string.Empty;
        string strGetUrl = string.Empty;
        string strBalanceUrl = string.Empty;
        string strResponse = string.Empty;
        System.Xml.Linq.XElement xResults = null;

        string strType = string.Empty;

        System.Xml.Linq.XElement xReturn = null;
        xReturn = new System.Xml.Linq.XElement("KEYBALANCE");

        strType = context.Request.Form.Get("type");

        string strStatusCode = "1";
        string strSlotSessionId = string.Empty;

        if (string.IsNullOrEmpty(strType)) { strType = "false"; }

        if (Boolean.Parse(strType))
        {
            try
            {
                string strAuthSessionUrl = string.Empty;
                string strMemberSessionId = string.Empty;

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
                    xResults = System.Xml.Linq.XElement.Parse(strResponse);
                    strStatusCode = xmlData.getNodeText(xResults, "STATUSCODE");
                    if (string.Compare(strStatusCode, "0", true) == 0)
                    {
                        strSlotSessionId = xmlData.getNodeText(xResults, "TOKEN");
                        context.Session.Add("SID", strSlotSessionId);
                        xReturn.Add(new System.Xml.Linq.XElement("REALMODE", "1"));
                        xReturn.Add(xResults.Elements());
                    }
                    else
                    {
                        isProcessAbort = true;
                        xReturn = xResults;
                    }
                }
            }
            catch (Exception) { }
        }
        else
        {
            try
            {
                /* fun play */
                strOperatorCode = System.Configuration.ConfigurationManager.AppSettings.Get("OperatorCode");
                strFunUrl = System.Configuration.ConfigurationManager.AppSettings.Get("SlotKeyFunUrl");

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
                    xReturn.Add(new System.Xml.Linq.XElement("REALMODE", "0"), new System.Xml.Linq.XElement("STATUSCODE", "0"));
                }
            }
            catch (Exception) { }
        }

        
        string strAuthLoginUrl = string.Empty;
        string strLogin = "656321";
        string strPassword = "2ffe3d4a97f843cead55fc88b7df1bb2";
        string strVendorId = "1";

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
        

        /* login via s cookie */
        /*
        try
        {
            string strAuthSessionUrl = string.Empty;
            string strMemberSessionId = string.Empty;

            strAuthSessionUrl = System.Configuration.ConfigurationManager.AppSettings.Get("SlotAuthSessionUrl");
            strOperatorCode = System.Configuration.ConfigurationManager.AppSettings.Get("OperatorCode");
            strMemberSessionId = commonCookie.CookieS;

            using (customConfig.WebClientWithTimeOut wsUrlGet = new customConfig.WebClientWithTimeOut(5000))
            {
                System.Collections.Specialized.NameValueCollection dataValues = new System.Collections.Specialized.NameValueCollection();
                wsUrlGet.Headers.Add("Accept", "text/xml");

                strGetUrl = string.Format("{0}?iid={1}&token={2}", strOperatorCode, strMemberSessionId);
                byte[] byteResponse = wsUrlGet.DownloadData(strGetUrl);
                strResponse = System.Text.Encoding.UTF8.GetString(byteResponse);
                xResults = System.Xml.Linq.XElement.Parse(xmlData.RemoveCarriageReturn(xmlData.RemoveAllNamespaces(strResponse).Replace("&", "&amp;")));
                strSlotSessionId = xResults.Value;
                context.Session.Add("SID", strSlotSessionId);
            }
        }
        catch (Exception) { }
        */
        if (!isProcessAbort)
        {
            try
            {
                strBalanceUrl = System.Configuration.ConfigurationManager.AppSettings.Get("SlotBalanceUrl");

                using (customConfig.WebClientWithTimeOut wsUrlGet = new customConfig.WebClientWithTimeOut(5000))
                {
                    System.Collections.Specialized.NameValueCollection dataValues = new System.Collections.Specialized.NameValueCollection();
                    wsUrlGet.Headers.Add("Accept", "text/xml");

                    strGetUrl = string.Format("{0}?key={1}", strBalanceUrl, strSlotSessionId);
                    byte[] byteResponse = wsUrlGet.DownloadData(strGetUrl);
                    strResponse = System.Text.Encoding.UTF8.GetString(byteResponse);

                    xResults = System.Xml.Linq.XElement.Parse(strResponse);
                    xReturn.Add(xResults);
                }
            }
            catch (Exception) { }
        }
        context.Response.ContentType = "text/xml";//"text/plain";
        context.Response.Write(Convert.ToString(xReturn));
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