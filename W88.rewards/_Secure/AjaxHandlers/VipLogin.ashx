<%@ WebHandler Language="C#" Class="VipLogin" %>

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

public class VipLogin : HttpTaskAsyncHandler, System.Web.SessionState.IReadOnlySessionState
{
    public override async Task ProcessRequestAsync(HttpContext context)
    {
        if (!context.Request.ContentType.Contains("json")) return;
        
        var jsonString = new StreamReader(context.Request.InputStream).ReadToEnd();
        var memberSession = Common.DeserializeObject<MemberSession>(jsonString);
        var process = await (new Members()).MembersSessionCheck(memberSession.Token);
        
        switch (process.Code)
        {
            case 1:
                memberSession = process.Data as MemberSession;
                var riskId = memberSession == null ? string.Empty : memberSession.RiskId;
                foreach (var allowed in Common.GetAppSetting<string>("VIP_Allowed").ToUpper().Split(new[] { '|' }).Where(allowed => allowed.Equals(riskId)))
                {
                    CookieHelpers.CookieVip = "true";
                }
                var vipCookie = CookieHelpers.CookieVip;
                bool isVip;
                bool.TryParse(vipCookie, out isVip);
                process.Code = isVip ? (int)Constants.StatusCode.Success : (int)Constants.StatusCode.Error;
                process.Data = isVip ? process.Data : null;
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
    
    public bool IsReusable {
        get {
            return false;
        }
    }
}