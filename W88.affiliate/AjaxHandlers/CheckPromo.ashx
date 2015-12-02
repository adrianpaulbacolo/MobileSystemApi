<%@ WebHandler Language="C#" Class="AjaxHandlers_ASHX_CheckPromo" %>

using System;
using System.Web;

public class AjaxHandlers_ASHX_CheckPromo : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    string strCheckPromoUrl = System.Configuration.ConfigurationManager.AppSettings.Get("CheckPromoUrl");
    
    public void ProcessRequest(HttpContext context)
    {
        string strWalletId = string.Empty;
        string strOperatorId = string.Empty;
        string strMemberCode = string.Empty;
        string strTransferAmount = string.Empty;
        string strPromoCode = string.Empty;

        strWalletId = context.Request.Params.Get("Wallet");
        strOperatorId = commonVariables.OperatorId;
        strMemberCode = commonVariables.GetSessionVariable("MemberCode");
        strTransferAmount = context.Request.Params.Get("Amount");
        strPromoCode = context.Request.Params.Get("Code");

        try
        {
            using (customConfig.WebClientWithTimeOut svcInstance = new customConfig.WebClientWithTimeOut(5000))
            {
                System.Collections.Specialized.NameValueCollection dataValues = new System.Collections.Specialized.NameValueCollection();
                dataValues.Add("walletId", strWalletId);
                dataValues.Add("operatorId", strOperatorId);
                dataValues.Add("memberCode", strMemberCode);
                dataValues.Add("transferAmount", strTransferAmount);
                dataValues.Add("promoCode", strPromoCode);
                dataValues.Add("dateTime", System.DateTime.Now.ToString(commonVariables.DateTimeFormat));

                svcInstance.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                svcInstance.Headers.Add("Accept", "text/xml");
                byte[] byteResponse = svcInstance.UploadValues(strCheckPromoUrl, "POST", dataValues);
                string strResponse = System.Text.Encoding.UTF8.GetString(byteResponse);

                context.Response.ContentType = "text/xml";
                context.Response.Write(strResponse);
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