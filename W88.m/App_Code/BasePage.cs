using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using customConfig;
using Helpers;
using Models;

public class BasePage : System.Web.UI.Page
{
    public Boolean isLoggedIn;
    public Boolean isPublic = true;
    public PageHeaders headers = new PageHeaders();
    public MemberSession.UserSessionInfo userInfo = new MemberSession.UserSessionInfo();
    private OperatorSettings _opsettings = new OperatorSettings("W88");

    protected override void OnPreInit(EventArgs e)
    {
        if (string.Compare(System.Configuration.ConfigurationManager.AppSettings.Get("ClearWebCache"), "true", true) == 0)
        {
            foreach (System.Collections.DictionaryEntry deCache in System.Web.HttpContext.Current.Cache)
            {
                HttpContext.Current.Cache.Remove(Convert.ToString(deCache.Key));
            }
        }

        base.OnPreInit(e);
    }

    protected override void OnInit(EventArgs e)
    {
        checkCDN();
        string CDN_Value = getCDNValue();
        string key = getCDNKey();

        commonCookie.Set("CDNCountryCode", GetCountryCode(CDN_Value, key), DateTime.Now.AddDays(1));

        if (string.IsNullOrWhiteSpace(commonCookie.CookieLanguage))
        {
            if (!string.IsNullOrEmpty(CDN_Value) && !string.IsNullOrEmpty(key))
            {
                commonVariables.SelectedLanguage = commonCountry.GetLanguageByCountry(GetCountryCode(CDN_Value, key));
            }
            else
            {
                Uri myUri = new Uri(System.Web.HttpContext.Current.Request.Url.ToString());
                string[] host = myUri.Host.Split('.');

                if (host.Count() > 1)
                {
                    commonVariables.SelectedLanguage = GetLanguageByDomain("." + host[1] + "." + host[2]);
                }
                else
                {
                    commonVariables.SelectedLanguage = GetLanguageByDomain("default");
                }

            }
        }

        base.OnInit(e);
    }

    protected override void OnPreLoad(EventArgs e)
    {
        base.OnPreLoad(e);

        UserSession.checkSession();
        var user = new Members();
        userInfo = user.MemberData();
    }

    protected override void OnLoad(EventArgs e)
    {
        var strIsApp = HttpContext.Current.Request.QueryString.Get("isApp");
        if (!string.IsNullOrEmpty(strIsApp))
        {
               commonCookie.CookieIsApp = (strIsApp == "1") ? strIsApp : null;
        }

        commonCookie.CookieSubPlatform(HttpContext.Current.Request.QueryString.Get("spfid"));

        var strLanguage = HttpContext.Current.Request.QueryString.Get("lang");

        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("languageCode")))
        {
            strLanguage = HttpContext.Current.Request.QueryString.Get("languageCode");
        }

        switch (strLanguage)
        {
            case "en":
                strLanguage = "en-us";
                break;

            case "id":
                strLanguage = "id-id";
                break;

            case "ja":
            case "jp":
                strLanguage = "ja-jp";
                break;

            case "km":
            case "kh":
                strLanguage = "km-kh";
                break;

            case "ko":
            case "kr":
                strLanguage = "ko-kr";
                break;

            case "th":
                strLanguage = "th-th";
                break;

            case "vi":
            case "vn":
                strLanguage = "vi-vn";
                break;

            case "cn":
            case "zh":
                strLanguage = "zh-cn";
                break;
        }

        if (!string.IsNullOrEmpty(strLanguage))
        {
            commonVariables.SelectedLanguage = strLanguage;

            var queryString = HttpUtility.ParseQueryString(Request.Url.Query);
            queryString.Remove("lang");
            queryString.Remove("languageCode");

            string redirectPath = queryString.Count > 0
                ? string.Format("{0}?{1}", Request.Url.LocalPath, queryString)
                : Request.Url.LocalPath;
            Response.Redirect(redirectPath);
        }

        if (!this.isPublic)
        {
            if (!UserSession.IsLoggedIn())
            {
                var redirectUrl = string.Format("/_Secure/Login.aspx?redirect={0}",
                    Uri.EscapeDataString(Request.Url.PathAndQuery));
                Response.Redirect(redirectUrl);
            }
        }

        base.OnLoad(e);

        CheckVipDomain();

        if (!userInfo.IsTestAccount)
        {
            CheckMaintenanceSettings();
        }
    }

    private void CheckMaintenanceSettings()
    {
        var pageLocation = HttpContext.Current.Request.Url.AbsoluteUri;

        var info = new MaintenanceInfo();

        if (pageLocation.Contains("/Deposit"))
            info.CurrentPage = MaintenanceModules.DPM;
        else if (pageLocation.Contains("/Withdrawal"))
            info.CurrentPage = MaintenanceModules.WPM;
        else if (pageLocation.Contains("/Profile/Rebates.aspx"))
            info.CurrentPage = MaintenanceModules.RS;
        else if (pageLocation.Contains("/FundTransfer"))
            info.CurrentPage = MaintenanceModules.FTM;
        else
            info.CurrentPage = MaintenanceModules.NonPage;

        info.MainSite = Convert.ToBoolean(_opsettings.Values.Get("MaintenanceAllPages"));
        info.Deposit = Convert.ToBoolean(_opsettings.Values.Get("MaintenanceDeposit"));
        info.Widrawal = Convert.ToBoolean(_opsettings.Values.Get("MaintenanceWidrawal"));
        info.FundTransfer = Convert.ToBoolean(_opsettings.Values.Get("MaintenanceFundTransfer"));
        info.Rebates = Convert.ToBoolean(_opsettings.Values.Get("MaintenanceRebates"));

        var maintenanceHelper = new MaintenanceHelper(info).CheckStatus();
        if (maintenanceHelper.AllSite)
        {
            Response.Clear();
            Response.Redirect(_opsettings.Values.Get("MaintenanceAllPage"), true);
        }
        else if (maintenanceHelper.PerModule)
        {
            Response.Clear();
            Response.Redirect(_opsettings.Values.Get("MaintenancePage"), true);

        }
    }

    private void CheckVipDomain()
    {
        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId) && !HttpContext.Current.Request.Url.AbsoluteUri.ToLower().Contains("/vip/"))
        {
            foreach (var v in ConfigurationManager.AppSettings.Get("VIP_Domains").ToLower().Split(new[] { '|' }).Where(v => v.Equals(HttpContext.Current.Request.Url.Host)))
            {
                commonCookie.CookieLanguage = "zh-cn";
                Response.Clear();
                Response.Redirect(ConfigurationManager.AppSettings.Get("VipUrl"), true);
            }
        }
    }

    protected bool CheckLogin()
    {
        if (UserSession.IsLoggedIn())
        {
            return true;
        }
        return false;
    }

    public string getCDNValue()
    {
        return this.headers.cdn;
    }

    public string getCDNKey()
    {
        return this.headers.key;
    }

    public void checkCDN()
    {
        if (!string.IsNullOrEmpty(this.GetValue<string>(Request.ServerVariables[commonCountry.HeaderKeys.HTTP_X_AKAMAI_EDGESCAPE])))
        {
            this.headers.cdn = Request.ServerVariables[commonCountry.HeaderKeys.HTTP_X_AKAMAI_EDGESCAPE].ToString();
            this.headers.key = commonCountry.HeaderKeys.HTTP_X_AKAMAI_EDGESCAPE;
        }

        if (!string.IsNullOrEmpty(this.GetValue<string>(Request.ServerVariables[commonCountry.HeaderKeys.HTTP_CF_IPCOUNTRY])))
        {
            this.headers.cdn = Request.ServerVariables[commonCountry.HeaderKeys.HTTP_CF_IPCOUNTRY].ToString();
            this.headers.key = commonCountry.HeaderKeys.HTTP_CF_IPCOUNTRY;
        }

        if (!string.IsNullOrEmpty(this.GetValue<string>(Request.ServerVariables[commonCountry.HeaderKeys.HTTP_GEO_COUNTRY])))
        {
            this.headers.cdn = Request.ServerVariables[commonCountry.HeaderKeys.HTTP_GEO_COUNTRY].ToString();
            this.headers.key = commonCountry.HeaderKeys.HTTP_GEO_COUNTRY;
        }

        if (!string.IsNullOrEmpty(this.GetValue<string>(Request.ServerVariables[commonCountry.HeaderKeys.HOST])))
        {
            this.headers.host = Request.ServerVariables[commonCountry.HeaderKeys.HOST].ToString();
        }

        if (!string.IsNullOrEmpty(this.GetValue<string>(Request.ServerVariables[commonCountry.HeaderKeys.TRUE_CLIENT_IP])))
        {
            this.headers.ip = Request.ServerVariables[commonCountry.HeaderKeys.TRUE_CLIENT_IP].ToString();
        }
    }

    public class PageHeaders
    {
        public string host;
        public string ip;
        public string cdn;
        public string key;
    }

    public T GetValue<T>(object obj)
    {
        if (obj == DBNull.Value || obj == null)
        {
            return default(T);
        }

        return (T)Convert.ChangeType(obj, typeof(T));
    }

    public string GetCountryCode(string CDN_Value, string key)
    {
        string CountryCode = string.Empty;

        if (key == commonCountry.HeaderKeys.HTTP_X_AKAMAI_EDGESCAPE)
        {
            string[] Values = new string[100];
            Values = CDN_Value.Split(',');
            CountryCode = Values[1].Split('=')[1];
        }
        if (key == commonCountry.HeaderKeys.HTTP_CF_IPCOUNTRY)
        {
            CountryCode = CDN_Value;
        }
        if (key == commonCountry.HeaderKeys.HTTP_GEO_COUNTRY)
        {
            CountryCode = CDN_Value;
        }
        return CountryCode;
    }

    public string GetLanguageByDomain(string Domain)
    {
        string Language = string.Empty;

        if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_CN].Contains(Domain))
        {
            Language = "zh-cn";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_VN].Contains(Domain))
        {
            Language = "vi-vn";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_TH].Contains(Domain))
        {
            Language = "th-th";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_ID].Contains(Domain))
        {
            Language = "id-id";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_MY].Contains(Domain))
        {
            Language = "en-us";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_KR].Contains(Domain))
        {
            Language = "ko-kr";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_JP].Contains(Domain))
        {
            Language = "ja-jp";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_KH].Contains(Domain))
        {
            Language = "km-kh";
        }
        else if (!string.IsNullOrWhiteSpace(commonVariables.SelectedLanguage))
        {
            Language = commonVariables.SelectedLanguage;
        }
        else
        {
            Language = "en-us";
        }

        return Language;
    }

    protected void SetTitle(string s)
    {
        if (Master == null) return;

        EnableLogoOnPage();

        var header = (UserControl)Master.FindControl("HeaderLogo");
        var title = (Literal)header.FindControl("ltrTitle");

        if (title == null) return;

        title.Text = HttpUtility.HtmlEncode(s);
        title.Visible = true;
    }

    protected void EnableLogoOnPage(bool cancel = false, bool back = false, bool logo = false)
    {
        if (Master == null) return;

        //var header = (UserControl)Master.FindControl("HeaderOnText");
        //header.Visible = false;

        var header = (UserControl)Master.FindControl("HeaderLogo");

        if (header == null) return;

        if (cancel)
        {
            var cancelButton = (HyperLink)header.FindControl("cancel");
            cancelButton.Visible = true;

        }

        if (back)
        {
            var backButton = (HyperLink)header.FindControl("aMenu");
            backButton.Visible = true;
        }

        if (logo)
        {
            var text = (Literal)header.FindControl("ltrTitle");
            text.Visible = false;
            var img = (Panel)header.FindControl("logo");
            img.Visible = true;
        }
        else
        {
            var text = (Literal)header.FindControl("ltrTitle");
            text.Visible = true;
            var img = (Panel)header.FindControl("logo");
            img.Visible = false;
        }
    }

    public string getAppSuffix()
    {
        return (commonCookie.CookieIsApp == "1") ? "_app" : "";
    }

    protected void CheckAgentAndRedirect(string url)
    {
        if (commonFunctions.isExternalPlatform())
        {
            Response.Redirect(url);
        }
    }
}