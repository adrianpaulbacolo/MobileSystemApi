using System;
using System.Configuration;
using System.Web;


public class commonVariables
{
    private static System.Text.RegularExpressions.Regex rxDomains_CN = new System.Text.RegularExpressions.Regex(ChinaDomain);
    public static System.Xml.Linq.XElement LeftMenuXML { get { if (HttpContext.Current.Cache.Get("leftMenuXML_" + SelectedLanguage) != null) { return HttpContext.Current.Cache.Get("leftMenuXML_" + SelectedLanguage) as System.Xml.Linq.XElement; } else { System.Xml.Linq.XElement xcMenu = commonCulture.appData.getRootResource("/leftMenu"); HttpContext.Current.Cache.Add("leftMenuXML_" + SelectedLanguage, xcMenu, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 15, 0), System.Web.Caching.CacheItemPriority.AboveNormal, null); return xcMenu; } } }
    public static System.Xml.Linq.XElement ProductsXML { get { if (HttpContext.Current.Cache.Get("ProductsXML_" + SelectedLanguage) != null) { return HttpContext.Current.Cache.Get("ProductsXML_" + SelectedLanguage) as System.Xml.Linq.XElement; } else { System.Xml.Linq.XElement xcMenu = commonCulture.appData.getRootResource("/Products"); HttpContext.Current.Cache.Add("ProductsXML_" + SelectedLanguage, xcMenu, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 15, 0), System.Web.Caching.CacheItemPriority.AboveNormal, null); return xcMenu; } } }
    public static System.Xml.Linq.XElement ContactUsXML { get { if (HttpContext.Current.Cache.Get("ContactUsXML_" + SelectedLanguage) != null) { return HttpContext.Current.Cache.Get("ContactUsXML_" + SelectedLanguage) as System.Xml.Linq.XElement; } else { System.Xml.Linq.XElement xcMenu = commonCulture.appData.getRootResource("/ContactUs.aspx"); HttpContext.Current.Cache.Add("ContactUsXML_" + SelectedLanguage, xcMenu, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 15, 0), System.Web.Caching.CacheItemPriority.AboveNormal, null); return xcMenu; } } }

    public static System.Xml.Linq.XElement ErrorsXML {
        get {
            if (HttpContext.Current.Cache.Get("errorsXML_" + SelectedLanguage) != null)
            { return HttpContext.Current.Cache.Get("errorsXML_" + SelectedLanguage) as System.Xml.Linq.XElement; } 
            else { System.Xml.Linq.XElement xcErrors = commonCulture.appData.getRootResource("/Errors"); 
                HttpContext.Current.Cache.Add("errorsXML_" + SelectedLanguage, xcErrors, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 15, 0), System.Web.Caching.CacheItemPriority.AboveNormal, null);
                return xcErrors; } } }

    public static string SiteUrl { get { return HttpContext.Current.Request.ServerVariables["SERVER_NAME"]; } }
    public static string DisplayDateFormat { get { return ConfigurationManager.AppSettings.Get("DisplayDateFormat"); } }
    public static string DisplayDateTimeFormat { get { return ConfigurationManager.AppSettings.Get("DisplayDateTimeFormat"); } }
    public static string DateTimeFormat { get { return ConfigurationManager.AppSettings.Get("DateTimeFormat"); } }
    public static string DecimalFormat { get { return ConfigurationManager.AppSettings.Get("DecimalFormat"); } }
    public static string SelectedLanguage
    {
        get
        {
            if (!string.IsNullOrEmpty(commonCookie.CookieLanguage))
            {
                return commonCookie.CookieLanguage;
            }
            if (!string.IsNullOrWhiteSpace(HttpContext.Current.Session["SelectedLanguage"] as string))
            {
                return Convert.ToString(HttpContext.Current.Session["SelectedLanguage"]);
            }
            return rxDomains_CN.IsMatch(HttpContext.Current.Request.ServerVariables["SERVER_NAME"]) ? "zh-cn" : "en-us";            
        }
        set
        {
            commonCookie.CookieLanguage = value;
            SetSessionVariable("SelectedLanguage", value);
        }
    }

    public static string SelectedLanguageShort
    {
        get
        {
            switch (SelectedLanguage.ToLower())
            {
                case "en-us":
                    return "en";
                case "id-id":
                    return "id";
                case "km-kh":
                    return "kh";
                case "ko-kr":
                    return "kr";
                case "th-th":
                    return "th";
                case "vi-vn":
                    return "vn";
                case "zh-cn":
                    return "cn";
                case "ja-jp":
                    return "jp";
                default:
                    return "en";
            }
        }
    }

    public static string VIPCategoryId { get { return ConfigurationManager.AppSettings.Get("vipCategoryId"); } }

    public static string CurrentMemberSessionId
    {
        get
        {
            return (!string.IsNullOrEmpty(commonCookie.CookieS)) ? commonCookie.CookieS : "";
        }
    }

    public static string OperatorId
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings(ConfigurationManager.AppSettings.Get("Operator"));
            return opSettings.Values.Get("OperatorId");
        }
    }
    public static string OperatorCode
    {
        get
        {
            return ConfigurationManager.AppSettings.Get("Operator");
        }
    }

    public static string GetSessionVariable(string key)
    { return string.IsNullOrEmpty(HttpContext.Current.Session[key] as string) ? "" : Convert.ToString(HttpContext.Current.Session[key]); }
    public static void SetSessionVariable(string key, string value) { HttpContext.Current.Session.Add(key, value); }

    public static void ClearSessionVariables()
    {
        string strLanguage = string.Empty;
        string strVCode = string.Empty;

        strLanguage = SelectedLanguage;
        strVCode = GetSessionVariable("vCode");

        HttpContext.Current.Session.Clear();
        HttpContext.Current.Session.Abandon();

        SelectedLanguage = strLanguage;
        SetSessionVariable("vCode", strVCode);
    }

    internal enum TransferWallet
    {
        undefined = -1,
        main = 0,
        lottery = 1,
        oneworks = 2,
        casino = 3,
        playtech = 4,
        marketpulse = 5,
        rhymepoker = 6,
        sbtech = 7,
        pmahjong = 8,
        wft = 9
    }

    internal enum operatorCode
    {
        W88 = 1,
        BET8 = 3
    }

    public enum DepositMethod
    {
        FastDeposit = 110101,
        SDAPay = 120203,
        NextPay = 120204,
        Bill99 = 120206,
        IPS = 120207,
        WingMoney = 110308
    }

    public enum WithdrawalMethod 
    {
        BankTransfer = 210602,
        WingMoney = 210709
    }

    public enum TransactionSource 
    { 
        Mobile
    }

    public enum PaymentTransactionType 
    {
        Deposit = 1,
        Withdrawal = 2
    }

    public static string ChinaDomain
    {
        get
        {
            return ConfigurationManager.AppSettings.Get("CN_domain");
        }
    }
}