<%@ WebHandler Language="C#" Class="AjaxHandlers_ASHX_GetBalance" %>

using System;
using System.Web;
using Helpers;

public class AjaxHandlers_ASHX_GetBalance : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        string strOperatorId = commonVariables.OperatorId;
        string strMemberCode = string.Empty;
        string strSiteUrl = commonVariables.SiteUrl;

        string processCode = string.Empty;
        string processText = string.Empty;
                
        string strWalletId = string.Empty;
        string strWalletAmount = string.Empty;
        string strProductCurrency = string.Empty;

        var user = new Members();
        var userInfo = user.MemberData();

        System.Text.StringBuilder sbWalletBalance = new System.Text.StringBuilder();

        strWalletId = context.Request.Form.Get("Wallet");
        //if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["MemberCode"] as string)) { strMemberCode = Convert.ToString(System.Web.HttpContext.Current.Session["MemberCode"]); }
        strMemberCode = userInfo.MemberCode;
        #region productWalletBalance

        if (!string.IsNullOrEmpty(strMemberCode) && !string.IsNullOrEmpty(strOperatorId))
        {
            using (svcPayMember.MemberClient svcInstance = new svcPayMember.MemberClient())
            {
                strWalletAmount = svcInstance.getWalletBalance(strOperatorId, strSiteUrl, strMemberCode, strWalletId, out strProductCurrency);
                sbWalletBalance.AppendFormat("<walletsBalance><balance id=\"{0}\">{1}</balance></walletsBalance>", strWalletId, strWalletAmount);
            }
        }
        else { strWalletAmount = "0"; }
        
        #endregion

        context.Response.ContentEncoding = System.Text.Encoding.UTF8;
        //context.Response.ContentType = "text/xml";
        //context.Response.Write(Convert.ToString(sbWalletBalance));
        context.Response.Write(strWalletAmount);
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