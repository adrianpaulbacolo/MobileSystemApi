<%@ WebHandler Language="C#" Class="MemberSessionCheck" %>

using System.IO;
using System.Threading.Tasks;
using System.Web;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.Utilities;
using Members = W88.BusinessLogic.Accounts.Helpers.Members;

public class MemberSessionCheck : HttpTaskAsyncHandler, System.Web.SessionState.IReadOnlySessionState
{
    public override async Task ProcessRequestAsync(HttpContext context)
    {
        if (!context.Request.ContentType.Contains("json")) return;
        
        var jsonString = new StreamReader(context.Request.InputStream).ReadToEnd();
        var memberSession = Common.DeserializeObject<MemberSession>(jsonString);
        var process = await (new Members()).MembersSessionCheck(memberSession.Token);
        
        switch (process.Code)
        {
            case 0:
                process.Message = RewardsHelper.GetTranslation(TranslationKeys.Errors.Exception);
                break;
            case 1:
                process.Message = "Has Session";
                break;
            case 22:
                process.Message = RewardsHelper.GetTranslation(TranslationKeys.Errors.InactiveAccount);
                break;
            default:
                process.Message = RewardsHelper.GetTranslation(TranslationKeys.Errors.SessionExpired);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.Write(Common.SerializeObject(process));
        context.Response.End();
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }
}