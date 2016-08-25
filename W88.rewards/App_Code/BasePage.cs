using System;
using System.Configuration;
using System.Web;
using System.Web.UI;

public class BasePage : Page
{
    public Boolean isLoggedIn;
    public Boolean isPublic = true;
    public PageHeaders headers = new PageHeaders();

    protected override void OnPreInit(EventArgs e)
    {
        if (string.Compare(ConfigurationManager.AppSettings.Get("ClearWebCache"), "true", true) == 0)
        {
            foreach (System.Collections.DictionaryEntry deCache in System.Web.HttpContext.Current.Cache)
            {
                HttpContext.Current.Cache.Remove(Convert.ToString(deCache.Key));
            }
        }

        base.OnPreInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        UserSession.checkSession();

        if (!isPublic)
        {
            if (!UserSession.IsLoggedIn())
            {
                Response.Redirect("/Index.aspx");
            }
        }

        base.OnLoad(e);
    }

    protected bool CheckLogin()
    {
        if (UserSession.IsLoggedIn())
        {
            return true;
        }
        return false;
    }

    public string GetCdnValue()
    {
        return headers.cdn;
    }

    public string GetCdnKey()
    {
        return headers.key;
    }

    public void CheckCdn()
    {
        if (!string.IsNullOrEmpty(GetValue<string>(Request.ServerVariables[commonCountry.HeaderKeys.HTTP_X_AKAMAI_EDGESCAPE])))
        {
            headers.cdn = Request.ServerVariables[commonCountry.HeaderKeys.HTTP_X_AKAMAI_EDGESCAPE];
            headers.key = commonCountry.HeaderKeys.HTTP_X_AKAMAI_EDGESCAPE;
        }

        if (!string.IsNullOrEmpty(GetValue<string>(Request.ServerVariables[commonCountry.HeaderKeys.HTTP_CF_IPCOUNTRY])))
        {
            headers.cdn = Request.ServerVariables[commonCountry.HeaderKeys.HTTP_CF_IPCOUNTRY];
            headers.key = commonCountry.HeaderKeys.HTTP_CF_IPCOUNTRY;
        }

        if (!string.IsNullOrEmpty(GetValue<string>(Request.ServerVariables[commonCountry.HeaderKeys.HTTP_GEO_COUNTRY])))
        {
            headers.cdn = Request.ServerVariables[commonCountry.HeaderKeys.HTTP_GEO_COUNTRY];
            headers.key = commonCountry.HeaderKeys.HTTP_GEO_COUNTRY;
        }

        if (!string.IsNullOrEmpty(GetValue<string>(Request.ServerVariables[commonCountry.HeaderKeys.HOST])))
        {
            headers.host = Request.ServerVariables[commonCountry.HeaderKeys.HOST];
        }

        if (!string.IsNullOrEmpty(GetValue<string>(Request.ServerVariables[commonCountry.HeaderKeys.TRUE_CLIENT_IP])))
        {
            headers.ip = Request.ServerVariables[commonCountry.HeaderKeys.TRUE_CLIENT_IP];
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

    public string GetLanguageByDomain(string domain)
    {
        string language;

        if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_CN].Contains(domain))
        {
            language = "zh-cn";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_VN].Contains(domain))
        {
            language = "vi-vn";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_TH].Contains(domain))
        {
            language = "th-th";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_ID].Contains(domain))
        {
            language = "id-id";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_MY].Contains(domain))
        {
            language = "en-us";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_KR].Contains(domain))
        {
            language = "ko-kr";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_JP].Contains(domain))
        {
            language = "ja-jp";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_KH].Contains(domain))
        {
            language = "km-kh";
        }
        else if (!string.IsNullOrWhiteSpace(commonVariables.SelectedLanguage))
        {
            language = commonVariables.SelectedLanguage;
        }
        else
        {
            language = "en-us";
        }

        return language;
    }
}