using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

public class BasePage : System.Web.UI.Page
{
    public Boolean isLoggedIn;
    public Boolean isPublic = true;
    protected string CDN_Value = string.Empty;
    protected string CDN_Key = string.Empty;

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
        return this.CDN_Value;
    }

    public string getCDNKey()
    {
        return this.CDN_Value;
    }

    public void checkCDN()
    {
        if (!string.IsNullOrEmpty(this.GetValue<string>(Request.ServerVariables[commonCountry.AkamaiKeys.HTTP_X_AKAMAI_EDGESCAPE])))
        {
            this.CDN_Value = Request.ServerVariables[commonCountry.AkamaiKeys.HTTP_X_AKAMAI_EDGESCAPE].ToString();
            this.CDN_Key = commonCountry.AkamaiKeys.HTTP_X_AKAMAI_EDGESCAPE;
        }

        if (!string.IsNullOrEmpty(this.GetValue<string>(Request.ServerVariables[commonCountry.AkamaiKeys.HTTP_CF_IPCOUNTRY])))
        {
            this.CDN_Value = Request.ServerVariables[commonCountry.AkamaiKeys.HTTP_CF_IPCOUNTRY].ToString();
            this.CDN_Key = commonCountry.AkamaiKeys.HTTP_CF_IPCOUNTRY;
        }

        if (!string.IsNullOrEmpty(this.GetValue<string>(Request.ServerVariables[commonCountry.AkamaiKeys.HTTP_GEO_COUNTRY])))
        {
            this.CDN_Value = Request.ServerVariables[commonCountry.AkamaiKeys.HTTP_GEO_COUNTRY].ToString();
            this.CDN_Key = commonCountry.AkamaiKeys.HTTP_GEO_COUNTRY;
        }
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

        if (key == commonCountry.AkamaiKeys.HTTP_X_AKAMAI_EDGESCAPE)
        {
            string[] Values = new string[100];
            Values = CDN_Value.Split(',');
            CountryCode = Values[1].Split('=')[1];
        }
        if (key == commonCountry.AkamaiKeys.HTTP_CF_IPCOUNTRY)
        {
            CountryCode = CDN_Value;
        }
        if (key == commonCountry.AkamaiKeys.HTTP_GEO_COUNTRY)
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
        else if (ConfigurationManager.AppSettings[commonCountry.AkamaiKeys.COUNTRY_DOMAIN_CN].Contains(Domain))
        {
            Language = "zh-cn";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.AkamaiKeys.COUNTRY_DOMAIN_VN].Contains(Domain))
        {
            Language = "vi-vn";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.AkamaiKeys.COUNTRY_DOMAIN_TH].Contains(Domain))
        {
            Language = "th-th";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.AkamaiKeys.COUNTRY_DOMAIN_ID].Contains(Domain))
        {
            Language = "id-id";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.AkamaiKeys.COUNTRY_DOMAIN_MY].Contains(Domain))
        {
            Language = "en-us";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.AkamaiKeys.COUNTRY_DOMAIN_KR].Contains(Domain))
        {
            Language = "ko-kr";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.AkamaiKeys.COUNTRY_DOMAIN_JP].Contains(Domain))
        {
            Language = "ja-jp";
        }
        else if (ConfigurationManager.AppSettings[commonCountry.AkamaiKeys.COUNTRY_DOMAIN_KH].Contains(Domain))
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