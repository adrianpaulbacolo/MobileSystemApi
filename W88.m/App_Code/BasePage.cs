using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

public class BasePage : System.Web.UI.Page
{
    public Boolean isLoggedIn;
    public Boolean isPublic = true;
    public PageHeaders headers = new PageHeaders();

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

    protected override void OnLoad(EventArgs e)
    {
        UserSession.checkSession();

        string strLanguage = HttpContext.Current.Request.QueryString.Get("lang");

        if (!string.IsNullOrEmpty(strLanguage)) { 
            commonVariables.SelectedLanguage = strLanguage; 
        }

        if (!this.isPublic)
        {
            if (!UserSession.IsLoggedIn())
            {
                Response.Redirect("/Index");
            }
        }

        System.Web.UI.WebControls.Literal litScript = (System.Web.UI.WebControls.Literal)Page.FindControl("litScript");
        if (litScript != null) { }
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

        if (!string.IsNullOrWhiteSpace(commonVariables.SelectedLanguage))
        {
            Language = commonVariables.SelectedLanguage;
        }
        else if (ConfigurationManager.AppSettings[commonCountry.HeaderKeys.COUNTRY_DOMAIN_CN].Contains(Domain))
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
        else
        {
            Language = "en-us";
        }

        return Language;
    }

}