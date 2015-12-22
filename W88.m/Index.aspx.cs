using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class _Index : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;

    protected void Page_Init(object sender, EventArgs e) 
    {
        string priorityVIP = commonVariables.GetSessionVariable("PriorityVIP");

        string CDN_Value = string.Empty;
        string key = string.Empty;

        if (!string.IsNullOrEmpty(this.GetValue<string>(Request.ServerVariables[Keys.HTTP_X_AKAMAI_EDGESCAPE])))
        {
            CDN_Value = Request.ServerVariables[Keys.HTTP_X_AKAMAI_EDGESCAPE].ToString();
            key = Keys.HTTP_X_AKAMAI_EDGESCAPE;
        }

        if (!string.IsNullOrEmpty(this.GetValue<string>(Request.ServerVariables[Keys.HTTP_CF_IPCOUNTRY])))
        {
            CDN_Value = Request.ServerVariables[Keys.HTTP_CF_IPCOUNTRY].ToString();
            key = Keys.HTTP_CF_IPCOUNTRY;
        }

        if (!string.IsNullOrEmpty(this.GetValue<string>(Request.ServerVariables[Keys.HTTP_GEO_COUNTRY])))
        {
            CDN_Value = Request.ServerVariables[Keys.HTTP_GEO_COUNTRY].ToString();
            key = Keys.HTTP_GEO_COUNTRY;
        }


        if(!string.IsNullOrEmpty(CDN_Value) && !string.IsNullOrEmpty(key))
        {
            commonVariables.SelectedLanguage = GetLanguageByCountry(GetCountryCode(CDN_Value,key));
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

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Web.UI.WebControls.Literal litScript = (System.Web.UI.WebControls.Literal)Page.FindControl("litScript");
        
        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("lang"))) { commonVariables.SelectedLanguage = HttpContext.Current.Request.QueryString.Get("lang"); }

        xeErrors = commonVariables.ErrorsXML;

        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getRootResource("/Index.aspx", out xeResources);

        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("Error")) && !string.IsNullOrEmpty(commonVariables.GetSessionVariable("Error")))
            {
                Session.Remove("Error");
                if (litScript != null) { litScript.Text += string.Format("<script type='text/javascript'>alert('{0}');</script>", HttpContext.Current.Request.QueryString.Get("Error")); }
            }
        }

        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("AffiliateId"))) { commonVariables.SetSessionVariable("AffiliateId", HttpContext.Current.Request.QueryString.Get("AffiliateId")); }

        DetectDownloadLinks(DetectMobileDevice());
    }




    public T GetValue<T>(object obj)
    {
        if (obj == DBNull.Value || obj == null)
        {
            return default(T);
        }

        return (T)Convert.ChangeType(obj, typeof(T));
    }


    private class Keys
    {
        public const string HTTP_X_AKAMAI_EDGESCAPE = "HTTP_X_AKAMAI_EDGESCAPE";
        public const string HTTP_CF_IPCOUNTRY = "HTTP_CF_IPCOUNTRY";
        public const string HTTP_GEO_COUNTRY = "HTTP_GEO_COUNTRY";
        public const string COUNTRY_DOMAIN_CN = "country_domain_cn";
        public const string COUNTRY_DOMAIN_VN = "country_domain_vn";
        public const string COUNTRY_DOMAIN_TH = "country_domain_th";
        public const string COUNTRY_DOMAIN_ID = "country_domain_id";
        public const string COUNTRY_DOMAIN_MY = "country_domain_my";
        public const string COUNTRY_DOMAIN_KR = "country_domain_kr";
        public const string COUNTRY_DOMAIN_JP = "country_domain_jp";
        public const string COUNTRY_DOMAIN_KH = "country_domain_kh";
    }


    public string GetCountryCode(string CDN_Value,string key)
    {
        string CountryCode = string.Empty;

        if (key == Keys.HTTP_X_AKAMAI_EDGESCAPE)
        {
            string[] Values = new string[100];
            Values = CDN_Value.Split(',');
            CountryCode = Values[1].Split('=')[1];
        }
        if(key == Keys.HTTP_CF_IPCOUNTRY)
        {
            CountryCode = CDN_Value;
        }
        if(key == Keys.HTTP_GEO_COUNTRY)
        {
            CountryCode = CDN_Value;
        }
        return CountryCode;
    }


    public string GetLanguageByCountry(string CountryCode)
    {
        switch (CountryCode.ToLower())
        {
            case "us":
                return "en-us";
            case "id":
                return "id-id";
            case "kh":
                return "km-kh";
            case "kr":
                return "ko-kr";
            case "th":
                return "th-th";
            case "vn":
                return "vi-vn";
            case "cn":
                return "zh-cn";
            case "jp":
                return "ja-jp";
            default:
                return "en-us";
        }
    }

    public string GetLanguageByDomain(string Domain)
    {
        string Language = string.Empty;

        if (!string.IsNullOrWhiteSpace(commonVariables.SelectedLanguage))
        {
            Language = commonVariables.SelectedLanguage;
        }
        else if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_CN].Contains(Domain))
        {
            Language = "zh-cn";
        }
        else if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_VN].Contains(Domain))
        {
            Language = "vi-vn";
        }
        else if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_TH].Contains(Domain))
        {
            Language = "th-th";
        }
        else if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_ID].Contains(Domain))
        {
            Language = "id-id";
        }
        else if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_MY].Contains(Domain))
        {
            Language = "en-us";
        }
        else if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_KR].Contains(Domain))
        {
            Language = "ko-kr";
        }
        else if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_JP].Contains(Domain))
        {
            Language = "ja-jp";
        }
        else if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_KH].Contains(Domain))
        {
            Language = "km-kh";
        }
        else
        {
            Language = "en-us";
        }

        return Language;
    }

    public int DetectMobileDevice()
    {
        string strUserAgent = Request.UserAgent.ToString().ToLower();

        int responseCode = 0;

        if (strUserAgent != null)
        {
            if (strUserAgent.Contains("mac"))
            {
                pokerAndroid_link.Visible = false;
                pokerIOS_link.Visible = true;
                responseCode = 1;
                Session["responseCode"] = "1";
            }
            else if (strUserAgent.Contains("android"))
            {
                pokerAndroid_link.Visible = true;
                pokerIOS_link.Visible = false;
                responseCode = 2;
                Session["responseCode"] = "2";
            }
            else
            {
                pokerAndroid_link.Visible = true;
                pokerIOS_link.Visible = true;
                responseCode = 3;
                Session["responseCode"] = "3";
            }
        }
        return responseCode;
    }

    public void DetectDownloadLinks(int responseCode)
    {
        if(commonVariables.SelectedLanguage == "en-us")
        {
           if(responseCode == 1)
           {
               pokerIOS.Attributes.Remove("href");
               pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);
               Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
           }
           else if(responseCode == 2)
           {
               pokerAndroid.Attributes.Remove("href");
               pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);
               Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
           }
           else if(responseCode == 3)
           {
               pokerIOS.Attributes.Remove("href");
               pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);

               pokerAndroid.Attributes.Remove("href");
               pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);

               Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
               Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
           }
        }
        if (commonVariables.SelectedLanguage == "km-kh")
        {
            if (responseCode == 1)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);
                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
            }
            else if (responseCode == 2)
            {
                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
            else if (responseCode == 3)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);

                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);

                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
        }
        if (commonVariables.SelectedLanguage == "ja-jp")
        {
            if (responseCode == 1)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_JA"]);
                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_JA"];
            }
            else if (responseCode == 2)
            {
                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_JA"]);
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_JA"];
            }
            else if (responseCode == 3)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_JA"]);

                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_JA"]);

                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_JA"];
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_JA"];
            }
        }
        if (commonVariables.SelectedLanguage == "ko-kr")
        {
            if (responseCode == 1)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);
                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
            }
            else if (responseCode == 2)
            {
                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
            else if (responseCode == 3)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);

                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);

                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
        }
        if (commonVariables.SelectedLanguage == "id-id")
        {
            if (responseCode == 1)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_ID"]);
                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_ID"];
            }
            else if (responseCode == 2)
            {
                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_ID"]);
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_ID"];
            }
            else if (responseCode == 3)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_ID"]);

                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_ID"]);

                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_ID"];
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_ID"];
            }
        }
        if (commonVariables.SelectedLanguage == "th-th")
        {
            if (responseCode == 1)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);
                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
            }
            else if (responseCode == 2)
            {
                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
            else if (responseCode == 3)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);

                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);

                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
        }
        if (commonVariables.SelectedLanguage == "vi-vn")
        {
            if (responseCode == 1)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);
                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
            }
            else if (responseCode == 2)
            {
                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
            else if (responseCode == 3)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);

                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);

                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
        }
        if (commonVariables.SelectedLanguage == "zh-cn")
        {
            if (responseCode == 1)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_CN"]);
                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_CN"];
            }
            else if (responseCode == 2)
            {
                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_CN"]);
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_CN"];
            }
            else if (responseCode == 3)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_CN"]);

                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_CN"]);

                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_CN"];
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_CN"];

            }
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("Casino.aspx");
    }
}