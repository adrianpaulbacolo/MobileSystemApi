<%@ WebHandler Language="C#" Class="AjaxHandlers_ASHX_GetFXRate" %>

using System;
using System.Web;

public class AjaxHandlers_ASHX_GetFXRate : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context) {

        string strCFrom = string.Empty;
        string strCTo = string.Empty;
        string strMethod = string.Empty;
        string strSessionCurrency = string.Empty;
        string strCurrencyRate = string.Empty;

        //if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["CurrencyCode"] as string)) { strSessionCurrency = Convert.ToString(System.Web.HttpContext.Current.Session["CurrencyCode"]); }

        strSessionCurrency = commonVariables.GetSessionVariable("CurrencyCode");        
        strMethod = HttpContext.Current.Request.HttpMethod;
        
        strCFrom = string.Compare(strMethod, "POST", true) == 0 ? HttpContext.Current.Request.Form.Get("CurrencyFrom") : HttpContext.Current.Request.QueryString.Get("CurrencyFrom");
        strCTo = string.Compare(strMethod, "POST", true) == 0 ? HttpContext.Current.Request.Form.Get("CurrencyTo") : HttpContext.Current.Request.QueryString.Get("CurrencyTo");

        strCFrom = string.IsNullOrEmpty(strCTo) ? strSessionCurrency : strCFrom;
        strCTo = string.IsNullOrEmpty(strCTo) ? "USD" : strCTo;

        using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
        {
            strCurrencyRate = svcInstance.getCurrencyForeignRate(strCFrom, strCTo);
        }

        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        context.Response.ContentType = "text/plain";
        context.Response.Write(strCurrencyRate);
        context.Response.End();
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}