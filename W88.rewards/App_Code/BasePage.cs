using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using W88.BusinessLogic.Accounts.Helpers;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.Utilities;
using W88.Utilities.Geo;


public class BasePage : Page
{
    protected bool HasSession = false;
    protected MemberSession MemberSession = null;
    protected UserSessionInfo UserSessionInfo = null;
    protected MemberRewardsInfo MemberRewardsInfo = null;
    protected Members MembersHelper = new Members();
    protected RewardsHelper RewardsHelper = new RewardsHelper();
    protected string Language = string.Empty;

    protected bool IsUnderMaintenance
    {
        get
        {
            bool isUnderMaintenance;
            Boolean.TryParse(Common.GetAppSetting<string>("isUnderMaintenance"), out isUnderMaintenance);
            if (!isUnderMaintenance)
                return false;

            var maintenanceModules = Common.GetAppSetting<string>("maintenanceModules");
            if (string.IsNullOrEmpty(maintenanceModules))
                return true;

            return !(Array.IndexOf(maintenanceModules.Split('|'), Request.Url.AbsolutePath.ToLower()) < 0);
        }
    }

    protected override void OnPreInit(EventArgs e)
    {
        try
        {
            if (bool.Parse(Common.GetAppSetting<string>("ClearWebCache")))
            {
                foreach (System.Collections.DictionaryEntry deCache in HttpContext.Current.Cache)
                {
                    HttpContext.Current.Cache.Remove(Convert.ToString(deCache.Key));
                }
            }
        }
        finally
        {
            var language = HttpContext.Current.Request.QueryString.Get("lang");
            Language = !string.IsNullOrEmpty(language) ? language : LanguageHelpers.SelectedLanguage;
            CheckSession();
        }
    }

    protected async void CheckSession()
    {
        try
        {
            var token = HttpContext.Current.Request.QueryString.Get("token");
            if (!string.IsNullOrEmpty(token))
            {                
                token = Encryption.Decrypt(W88.Utilities.Constant.EncryptionType.TripleDESCS, token);               
            }

            if (string.IsNullOrEmpty(token))
            {
                token = HttpContext.Current.Request.Headers.Get("token");
                if (string.IsNullOrEmpty(token))
                {
                    var cookie = HttpContext.Current.Request.Cookies["user"];
                    if (cookie == null) return;
                    var keyValuePairs = cookie.Value.Replace("{", string.Empty).Replace("}", string.Empty).Replace("\"", string.Empty).Split(':');
                    for (var index = keyValuePairs.Length - 1; index > 0; index--)
                    {
                        token = Regex.Match(keyValuePairs[index], "^[{(]?[0-9A-F]{8}[-]?([0-9A-F]{4}[-]?){3}[0-9A-F]{12}[)}]?$", RegexOptions.ECMAScript).Value;
                        if (!string.IsNullOrEmpty(token)) break;
                    }
                }
                if (string.IsNullOrEmpty(token)) return false;
            }
            var process = await MembersHelper.MembersSessionCheck(token);
            HasSession = process.Code == 1 && !string.IsNullOrEmpty(process.Data.Token);
            if (!HasSession) return;
            MemberSession = process.Data;
            UserSessionInfo = await MembersHelper.GetMemberInfo(token);

            if (IsUnderMaintenance)
            {
                // Check if site is under maintenance and allow only certain users to have access
                var isAllowedAccess = false;
                var allowedUsers = Common.GetAppSetting<string>("allowedUsers");
                if (!string.IsNullOrEmpty(allowedUsers) && !string.IsNullOrEmpty(UserSessionInfo.MemberCode) 
                    && Array.IndexOf(allowedUsers.ToLower().Split('|'), UserSessionInfo.MemberCode.ToLower()) >= 0)
                    isAllowedAccess = true;

                if (!isAllowedAccess)
                {
                    Response.Redirect("/_Static/Pages/enhancement-all.aspx", false);
                    return;
                }
            }
         
            SetMemberRewardsInfo();
        }
        catch (Exception)
        {

        }
    }

    protected async void SetMemberRewardsInfo()
    {
        if (MemberSession == null || UserSessionInfo == null) return;
        MemberRewardsInfo = new MemberRewardsInfo();
        MemberRewardsInfo.CurrentPoints = await MembersHelper.GetRewardsPoints(UserSessionInfo);
        MemberRewardsInfo.CurrentPointLevel = await RewardsHelper.GetPointLevel(MemberSession.MemberId);
    }

    protected bool IsVip
    {     
        get
        {
            var vipCookie = CookieHelpers.CookieVip;
            bool isVip;
            bool.TryParse(vipCookie, out isVip);
            return isVip;
        }
    }

    protected string GetTranslation(string key, string fileName = "")
    {
        return CultureHelpers.GetTranslation(key, 
            string.IsNullOrEmpty(Language) ? LanguageHelpers.SelectedLanguage : Language, 
            string.Format("contents/{0}", string.IsNullOrEmpty(fileName) ? "translations" : fileName));
    }
}