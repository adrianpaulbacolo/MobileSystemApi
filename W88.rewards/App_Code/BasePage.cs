using System;
using System.Threading.Tasks;
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
    protected static readonly IpHelper IpHelper = new IpHelper();
    
    protected string ContentLanguage
    {
        get
        { 
            if (!IsDebugMode)
            {
                return new RewardsHelper(Language).ContentLanguage;
            }

            switch (CountryCode)
            {
                case "my":
                    switch (Language)
                    {
                        case "en-us":
                            return "en-my";
                        case "zh-cn":
                            return "zh-my";
                    }
                    return Language;
                default:
                    return Language;
            }
        }
    }

    public static string CountryCode
    {
        get
        {
            var debugCountryCode = Common.GetAppSetting<string>("debugCountryCode");
            if (IsDebugMode && !string.IsNullOrEmpty(debugCountryCode)) return debugCountryCode.Trim().ToLower();
            return RewardsHelper.CountryCode.ToLower();
    	}
    }

    public static bool IsDebugMode
    {
	    get
	    {
            bool isDebugMode;
            Boolean.TryParse(Common.GetAppSetting<string>("isDebugMode"), out isDebugMode);
            return isDebugMode;
        }
    }

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

    public static bool IsVipPage
    {
        get
        {
            var host = HttpContext.Current.Request.Url.Host.Split('.');
            return Common.GetAppSetting<string>("VIP_Domains").ToLower().Contains(string.Format("{0}.{1}", host[1], host[2]));
        }
    }

    public static string Token 
    {
        get 
        { 
            var cookie = HttpContext.Current.Request.Cookies.Get("s");
            return cookie == null ? string.Empty : cookie.Value;
        }
        set
        {
            var cookie = HttpContext.Current.Request.Cookies.Get("s") ?? new HttpCookie("s");
            cookie.Value = value;
            if (!string.IsNullOrEmpty(IpHelper.DomainName)) { cookie.Domain = IpHelper.DomainName; }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }   
    }

    protected override async void OnPreInit(EventArgs e)
    {
        base.OnPreInit(e);        
        var language = HttpContext.Current.Request.QueryString.Get("lang");
        Language = !string.IsNullOrEmpty(language) ? language : LanguageHelpers.SelectedLanguage;
        HasSession = await CheckSession();
        if (!IsUnderMaintenance)
        {
            return;
       	}
        // Check if site is under maintenance and allow only certain users to have access
        var isAllowedAccess = false;
        var allowedUsers = Common.GetAppSetting<string>("allowedUsers");
        if (!string.IsNullOrEmpty(allowedUsers) && HasSession
            && Array.IndexOf(allowedUsers.ToLower().Split('|'), UserSessionInfo.MemberCode.ToLower()) >= 0)
            isAllowedAccess = true;
        if (!isAllowedAccess)
        {
            Response.Redirect("/_Static/Pages/enhancement-all.aspx", false);
        }
    }

    protected async Task<bool> CheckSession()
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
                    token = Token;
                }
                if (string.IsNullOrEmpty(token)) return false;
            }
            var process = await MembersHelper.MembersSessionCheck(token);
            if (!(process.Code == 1 && !string.IsNullOrEmpty(process.Data.Token))) return false;
            var sessionInfo = await MembersHelper.GetMemberInfo(token);
            if (string.IsNullOrEmpty(sessionInfo.MemberCode)) return false;
            UserSessionInfo = sessionInfo;
            MemberSession = process.Data;
            SetMemberRewardsInfo();
        }
        catch (Exception)
        { 
            return false;
        }
        return true;
    }

    protected async void SetMemberRewardsInfo()
    {
        if (MemberSession == null || UserSessionInfo == null) return;
        MemberRewardsInfo = new MemberRewardsInfo();
        MemberRewardsInfo.CurrentPoints = await MembersHelper.GetRewardsPoints(UserSessionInfo);
        MemberRewardsInfo.CurrentPointLevel = await RewardsHelper.GetPointLevel(MemberSession.MemberId);
    }

    protected string GetTranslation(string key, string fileName = "")
    {
        return CultureHelpers.GetTranslation(key, 
            string.IsNullOrEmpty(Language) ? LanguageHelpers.SelectedLanguage : Language, 
            string.Format("contents/{0}", string.IsNullOrEmpty(fileName) ? "translations" : fileName));
    }
}