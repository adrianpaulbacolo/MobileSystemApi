<%@ WebHandler Language="C#" Class="GetWalletBalance" %>

using System;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using Models;

public class GetWalletBalance : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    
    public void ProcessRequest (HttpContext context) {
        
        if (!context.Request.ContentType.Contains("json")) return;

        var sJson = new StreamReader(context.Request.InputStream).ReadToEnd();
        var objMember = commonFunctions.Deserialize<WalletInfo>(sJson);
        var balance = commonPaymentMethodFunc.GetWalletBalanceAsync(objMember.Id);

        var serializer = new JavaScriptSerializer();
        var json = serializer.Serialize(balance.Result.getWalletBalanceResult);
        context.Response.ContentType = "text/json";
        context.Response.Write(json);
        
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}