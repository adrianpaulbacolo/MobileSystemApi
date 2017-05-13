<%@ WebHandler Language="C#" Class="Login" %>

using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities;
using Members = W88.BusinessLogic.Accounts.Helpers.Members;

public class Login : HttpTaskAsyncHandler, System.Web.SessionState.IReadOnlySessionState
{
    public override async Task ProcessRequestAsync(HttpContext context)
    {
        var requestToken = context.Request.Headers.Get("token");
        bool isVipLogin;
        bool.TryParse(context.Request.Headers.Get("isVipLogin"), out isVipLogin);
              
        var process = await (new Members()).MembersSessionCheck(requestToken); 
        var memberSession = process.Data as MemberSession;
        var token = memberSession == null ? string.Empty : memberSession.Token;
        switch (process.Code)
        {
            case 1:
                if (isVipLogin)
                {
                    var riskId = memberSession == null ? string.Empty : memberSession.RiskId;
                    foreach (var allowed in Common.GetAppSetting<string>("VIP_Allowed").ToUpper().Split(new[] { '|' }).Where(allowed => allowed.Equals(riskId)))
                    {
                        CookieHelpers.CookieVip = "true";
                    }
                    var vipCookie = CookieHelpers.CookieVip;
                    bool isVip;
                    bool.TryParse(vipCookie, out isVip);
                    if (!isVip)
                    {
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Data = null;
                    }
                    else
                    {
                        SetToken(token, ref process);
                    }  
                }
                else
                {
                    SetToken(token, ref process);
                }
                context.Response.ContentType = "application/json";
                context.Response.Write(Common.SerializeObject(process));
                context.Response.End();  
                break;
            default:
                process.Message = RewardsHelper.GetTranslation(TranslationKeys.Errors.SessionExpired);
                context.Response.ContentType = "application/json";
                context.Response.Write(Common.SerializeObject(process));
                context.Response.End();
                break;
        }
    }

    private static void SetToken(string token, ref ProcessCode process)
    {
        BasePage.Token = token;
        process.Code = !string.IsNullOrEmpty(BasePage.Token) ? (int)Constants.StatusCode.Success : (int)Constants.StatusCode.Error;
        process.Data = process.Code == (int)Constants.StatusCode.Success ? process.Data : null;
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }
}