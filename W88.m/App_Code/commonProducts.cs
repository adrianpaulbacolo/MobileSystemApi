using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;
using Helpers;
using Models;
using svcPayMember;

public class commonASports
{
    public static string getSportsbookUrl
    {
        get
        {
            //customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            //string strUrl = opSettings.Values.Get("ASportsUrl"); 
            string strUrl = commonCountry.getISportURL();
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName).Replace("{LANG}", commonASports.getSportsLanguageId(commonVariables.SelectedLanguage)).Replace("{TOKEN}", commonVariables.GetSessionVariable("MemberSessionId"));
        }
    }
    public static string getSportsLanguageId(string msLanguageCode)
    {
        string strLanguage = string.Empty;

        if (!string.IsNullOrEmpty(msLanguageCode))
        {
            System.Xml.Linq.XElement xeLanguage = commonCulture.appData.getRootResource("SportsLanguage");
            strLanguage = commonCulture.ElementValues.getResourceXPathString(commonVariables.OperatorCode + "/Sports/Language/" + msLanguageCode, xeLanguage);
        }
        return string.IsNullOrEmpty(strLanguage) ? "en" : strLanguage;
    }
    public static string getMSLanguageCode(string SportsLanguageId)
    {
        string strLanguage = string.Empty;

        if (!string.IsNullOrEmpty(SportsLanguageId))
        {
            System.Xml.Linq.XElement xeLanguage = commonCulture.appData.getRootResource("SportsLanguage");
            strLanguage = commonCulture.ElementValues.getResourceXPathName(commonVariables.OperatorCode + "/Sports/Language", SportsLanguageId, xeLanguage);
        }
        return string.IsNullOrEmpty(strLanguage) ? "en-us" : strLanguage;
    }
    internal static string getSportsCurrencyId(string msCurrencyCode)
    {
        string strCurrencyId = string.Empty;

        if (!string.IsNullOrEmpty(msCurrencyCode))
        {
            System.Xml.Linq.XElement xeLanguage = commonCulture.appData.getRootResource("SportsLanguage");
            strCurrencyId = commonCulture.ElementValues.getResourceXPathString("/Currency/" + msCurrencyCode.ToUpper(), xeLanguage);
        }
        return strCurrencyId;
    }
    internal static string getMS1CurrencyCode(string SportsCurrencyId)
    {
        string strCurrency = string.Empty;

        if (!string.IsNullOrEmpty(SportsCurrencyId))
        {
            System.Xml.Linq.XElement xeLanguage = commonCulture.appData.getRootResource("SportsLanguage");
            strCurrency = commonCulture.ElementValues.getResourceXPathName("Currency", SportsCurrencyId, xeLanguage);
        }
        return strCurrency.ToUpper();
    }
    internal static string getSportsOperatorId(string msOperatorId)
    {
        string strOperatorId = string.Empty;

        if (!string.IsNullOrEmpty(msOperatorId))
        {
            System.Xml.Linq.XElement xeLanguage = commonCulture.appData.getRootResource("SportsLanguage");
            strOperatorId = commonCulture.ElementValues.getResourceXPathName("Operator", msOperatorId, xeLanguage);
        }
        return strOperatorId.ToUpper();
    }
    internal static string getMS1OperatorId(string SportsOperatorId)
    {
        string strOperatorCode = string.Empty;

        if (!string.IsNullOrEmpty(SportsOperatorId))
        {
            System.Xml.Linq.XElement xeLanguage = commonCulture.appData.getRootResource("SportsLanguage");
            strOperatorCode = commonCulture.ElementValues.getResourceXPathString("/Operator/" + SportsOperatorId.ToUpper(), xeLanguage);
        }
        return strOperatorCode;
    }

    public static System.Collections.Specialized.NameValueCollection Values { get { return System.Configuration.ConfigurationManager.GetSection("ProductGroupSettings/" + commonVariables.OperatorCode + "/oneworks") as System.Collections.Specialized.NameValueCollection; } }
}

public class commonESports
{
    public static string getSportsbookUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ESportsUrl");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName).Replace("{LANG}", commonESports.getSportsLanguageId(commonVariables.SelectedLanguage)).Replace("{TOKEN}", commonVariables.GetSessionVariable("MemberSessionId"));
        }
    }
    public static string getSportsLanguageId(string msLanguageCode)
    {
        string strLanguage = string.Empty;

        if (!string.IsNullOrEmpty(msLanguageCode))
        {
            System.Xml.Linq.XElement xeLanguage = commonCulture.appData.getRootResource("SportsLanguage");
            strLanguage = commonCulture.ElementValues.getResourceXPathString(commonVariables.OperatorCode + "/ESports/Language/" + msLanguageCode, xeLanguage);
        }
        return string.IsNullOrEmpty(strLanguage) ? "en" : strLanguage;
    }
    public static string getMSLanguageCode(string SportsLanguageId)
    {
        string strLanguage = string.Empty;

        if (!string.IsNullOrEmpty(SportsLanguageId))
        {
            System.Xml.Linq.XElement xeLanguage = commonCulture.appData.getRootResource("SportsLanguage");
            strLanguage = commonCulture.ElementValues.getResourceXPathName(commonVariables.OperatorCode + "/ESports/Language", SportsLanguageId, xeLanguage);
        }
        return string.IsNullOrEmpty(strLanguage) ? "en-us" : strLanguage;
    }
    /*
    internal static string getSportsCurrencyId(string msCurrencyCode)
    {
        string strCurrencyId = string.Empty;
        if (!string.IsNullOrEmpty(msCurrencyCode))
        {
            System.Xml.Linq.XElement xeLanguage = commonCulture.appData.getRootResource("SportsLanguage");
            strCurrencyId = commonCulture.ElementValues.getResourceXPathString("/Currency/" + msCurrencyCode.ToUpper(), xeLanguage);
        }
        return strCurrencyId;
    }
    internal static string getMS1CurrencyCode(string SportsCurrencyId)
    {
        string strCurrency = string.Empty;
        if (!string.IsNullOrEmpty(SportsCurrencyId))
        {
            System.Xml.Linq.XElement xeLanguage = commonCulture.appData.getRootResource("SportsLanguage");
            strCurrency = commonCulture.ElementValues.getResourceXPathName("Currency", SportsCurrencyId, xeLanguage);
        }
        return strCurrency.ToUpper();
    }
    internal static string getSportsOperatorId(string msOperatorId)
    {
        string strOperatorId = string.Empty;
        if (!string.IsNullOrEmpty(msOperatorId))
        {
            System.Xml.Linq.XElement xeLanguage = commonCulture.appData.getRootResource("SportsLanguage");
            strOperatorId = commonCulture.ElementValues.getResourceXPathName("Operator", msOperatorId, xeLanguage);
        }
        return strOperatorId.ToUpper();
    }
    internal static string getMS1OperatorId(string SportsOperatorId)
    {
        string strOperatorCode = string.Empty;
        if (!string.IsNullOrEmpty(SportsOperatorId))
        {
            System.Xml.Linq.XElement xeLanguage = commonCulture.appData.getRootResource("SportsLanguage");
            strOperatorCode = commonCulture.ElementValues.getResourceXPathString("/Operator/" + SportsOperatorId.ToUpper(), xeLanguage);
        }
        return strOperatorCode;
    }
    */
    public static System.Collections.Specialized.NameValueCollection Values { get { return System.Configuration.ConfigurationManager.GetSection("ProductGroupSettings/" + commonVariables.OperatorCode + "/oneworks") as System.Collections.Specialized.NameValueCollection; } }
}

public class commonVSports
{
    public static string getSportsbookUrlBasketball
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("VSportsUrl-Basketball");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName).Replace("{LANG}", commonVSports.getLanguageCode(commonVariables.SelectedLanguage)).Replace("{TOKEN}", commonVariables.GetSessionVariable("MemberSessionId"));
        }
    }
    public static string getSportsbookUrlTennis
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("VSportsUrl-Tennis");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName).Replace("{LANG}", commonESports.getSportsLanguageId(commonVariables.SelectedLanguage)).Replace("{TOKEN}", commonVariables.GetSessionVariable("MemberSessionId"));
        }
    }
    public static string getSportsbookUrlHorseRacing
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("VSportsUrl-HorseRacing");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName).Replace("{LANG}", commonESports.getSportsLanguageId(commonVariables.SelectedLanguage)).Replace("{TOKEN}", commonVariables.GetSessionVariable("MemberSessionId"));
        }
    }
    public static string getSportsbookUrlFootball
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("VSportsUrl-Football");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName).Replace("{LANG}", commonVSports.getLanguageCode(commonVariables.SelectedLanguage)).Replace("{TOKEN}", commonVariables.GetSessionVariable("MemberSessionId"));
        }
    }
    public static string getSportsbookUrlDogRacing
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("VSportsUrl-DogRacing");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName).Replace("{LANG}", commonESports.getSportsLanguageId(commonVariables.SelectedLanguage)).Replace("{TOKEN}", commonVariables.GetSessionVariable("MemberSessionId"));
        }
    }
    public static string getSportsLanguageId(string msLanguageCode)
    {
        string strLanguage = string.Empty;

        if (!string.IsNullOrEmpty(msLanguageCode))
        {
            System.Xml.Linq.XElement xeLanguage = commonCulture.appData.getRootResource("SportsLanguage");
            strLanguage = commonCulture.ElementValues.getResourceXPathString(commonVariables.OperatorCode + "/ESports/Language/" + msLanguageCode, xeLanguage);
        }
        return string.IsNullOrEmpty(strLanguage) ? "en" : strLanguage;
    }
    public static string getMSLanguageCode(string SportsLanguageId)
    {
        string strLanguage = string.Empty;

        if (!string.IsNullOrEmpty(SportsLanguageId))
        {
            System.Xml.Linq.XElement xeLanguage = commonCulture.appData.getRootResource("SportsLanguage");
            strLanguage = commonCulture.ElementValues.getResourceXPathName(commonVariables.OperatorCode + "/ESports/Language", SportsLanguageId, xeLanguage);
        }
        return string.IsNullOrEmpty(strLanguage) ? "en-us" : strLanguage;
    }
    public static string getLanguageCode(string selectedLanguage)
    {
        string strLanguage = string.Empty;

        if (!string.IsNullOrEmpty(selectedLanguage))
        {
            System.Xml.Linq.XElement xeLanguage = commonCulture.appData.getRootResource("SportsLanguage");
            strLanguage = commonCulture.ElementValues.getResourceXPathString(commonVariables.OperatorCode + "/VSports/Language/" + selectedLanguage, xeLanguage);
        }
        return string.IsNullOrEmpty(strLanguage) ? "en" : strLanguage;
    }
    public static System.Collections.Specialized.NameValueCollection Values { get { return System.Configuration.ConfigurationManager.GetSection("ProductGroupSettings/" + commonVariables.OperatorCode + "/oneworks") as System.Collections.Specialized.NameValueCollection; } }
}

public class commonXSports
{
    public static string SportsBookUrl
    {
        get
        {

            var user = new Members().MemberData();
            var opSettings = new customConfig.OperatorSettings("W88");
            var url = opSettings.Values.Get("xSportsFunUrl");
            var lang = commonVariables.SelectedLanguage;

            switch (lang.ToLower())
            {

                case "id-id":
                    lang = "id-ID";
                    break;

                case "ja-jp":
                    lang = "ja-JP";
                    break;

                case "ko-kr":
                    lang = "ko-KR";
                    break;

                case "th-th":
                    lang = "th-TH";
                    break;

                case "vi-vn":
                    lang = "vi-VN";
                    break;

                case "zh-cn":
                    lang = "zh-CN";
                    break;

                default:
                    lang = "en-US";
                    break;
            }

            if (!string.IsNullOrWhiteSpace(user.CurrentSessionId))
            {
                var curr = commonCookie.CookieCurrency;
                switch (curr.ToLower())
                {
                    case "rmb":
                        curr = "CNY";
                        break;
                    case "vnd":
                        curr = "VD";
                        break;
                }

                url = opSettings.Values.Get("xSportsRealUrl");
                url = url.Replace("{TOKEN}", user.CurrentSessionId).Replace("{USER}", user.MemberId).Replace("{CURR}", curr);
            }

            return url.Replace("{DOMAIN}", commonIp.DomainName).Replace("{LANG}", lang);
        }
    }
}

public class commonClubCrescendo
{
    public static string getSlotsUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubCrescendoUrl");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
        }
    }
}

public class commonClubDivino
{
    public static string getFunUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubDivinoFunUrl");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
        }
    }
    public static string getRealUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubDivinoRealUrl");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
        }
    }
}

public class commonClubBravado
{
    public static string getFunUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubBravadoFunUrl");
            //return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{ORIGIN}", commonIp.DomainName);
        }
    }
    public static string getRealUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubBravadoRealUrl");

            var token = commonCookie.CookieS;
            var paramString = "&s=" + token;

            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName) + paramString;
        }
    }

    public static string getFunUrl_mrslot
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubBravadoFunUrl_MR");
            //return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{ORIGIN}", commonIp.DomainName);
        }
    }
    public static string getRealUrl_mrslot
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubBravadoRealUrl_MR");

            var token = commonCookie.CookieS;
            var paramString = "&s=" + token;

            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName) + paramString;

        }
    }

    public static string getThirdPartyFunUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubBravadoThirdPartyFunUrl");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
        }
    }
    public static string getThirdPartyRealUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubBravadoThirdPartyRealUrl");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
        }
    }
}


public class commonLottery
{
    public static string getKenoUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("KenoUrl");
            var domain = HttpContext.Current.Request.Url.Host;
            var domainFullSplit = domain.Split('.');
            var domainHost = domainFullSplit.Length <= 2 ? domain : domainFullSplit[domainFullSplit.Length - 2] + "." + domainFullSplit[domainFullSplit.Length - 1];

            var language = commonCookie.CookieLanguage;
            var token = commonCookie.CookieS;
            var isExternalPlatform = commonFunctions.isExternalPlatform();
            var paramString = "?vendor=W88&s=" + token + "&lang=" + language + "&game=keno";
            if (!isExternalPlatform) paramString += "&domainlink=" + domainHost + "&domain=" + domainHost;
            //Change URL to GPI - Get From Config
            //return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl + paramString;
        }
    }
    public static string getPK10Url(bool isReal = false)
    {
        if (isReal && string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) return "/_Secure/Login.aspx";

        customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
        string strUrl = opSettings.Values.Get("Pk10Url");
        var domain = HttpContext.Current.Request.Url.Host;
        var domainFullSplit = domain.Split('.');
        var domainHost = domainFullSplit.Length <= 2 ? domain : domainFullSplit[domainFullSplit.Length - 2] + "." + domainFullSplit[domainFullSplit.Length - 1];

        var language = commonCookie.CookieLanguage;
        var token = commonCookie.CookieS;
        var isExternalPlatform = commonFunctions.isExternalPlatform();
        var paramString = "?vendor=W88&s=" + token + "&lang=" + language + "&game=pk10";
        if (!isReal) paramString += "&mode=Try&view=6&theme=2&version=3";
        if (!isExternalPlatform) paramString += "&domainlink=" + domainHost + "&domain=" + domainHost;
        return string.IsNullOrEmpty(strUrl) ? "" : strUrl + paramString;
    }
}

public class commonClubW
{
    public static string getUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubWUrl");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
        }
    }
}

public class commonClubMassimo
{
    public static string getFunUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubMassimoFunUrl");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
        }
    }
    public static string getRealUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubMassimoRealUrl");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
        }
    }

    public static string getDownloadUrl
    {
        get
        {
            string _downloadUrl = ConfigurationManager.AppSettings["ClubMassimoDL"];
            return string.IsNullOrWhiteSpace(_downloadUrl) ? "" : _downloadUrl;
        }
    }

}

public class commonClubWAPK
{
    public static string getDownloadUrl
    {
        get
        {
            string _downloadUrl = ConfigurationManager.AppSettings["ClubWAPK"];
            return string.IsNullOrWhiteSpace(_downloadUrl) ? "" : _downloadUrl.Replace("{DOMAIN}", commonIp.DomainName);
        }
    }

}

public class commonClubGallardo
{
    public static string getFunUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubGallardoFunUrl");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
        }
    }
    public static string getRealUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubGallardoRealUrl");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
        }
    }
}

public class CommonClubApollo
{
    public static string GetFunUrl
    {
        get
        {
            var opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubApolloFunUrl");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
        }
    }
    public static string GetRealUrl
    {
        get
        {
            var opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubApolloRealUrl");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
        }
    }
}

public class commonProduct
{
    public static string GetWallet(string productCode)
    {
        switch (productCode)
        {
            case "playtech":
                return commonCulture.ElementValues.getResourceXPathString("Products/ClubPalazzoSlots/Label", commonVariables.ProductsXML);
            case "vanguard":
                return commonCulture.ElementValues.getResourceXPathString("Products/ClubMassimoSlots/Label", commonVariables.ProductsXML);
            case "slot":
                return commonCulture.ElementValues.getResourceXPathString("Products/ClubBravado/Label", commonVariables.ProductsXML);
            case "png":
                return commonCulture.ElementValues.getResourceXPathString("Products/ClubGallardo/Label", commonVariables.ProductsXML);
            case "ags":
                return commonCulture.ElementValues.getResourceXPathString("Products/ClubApollo/Label", commonVariables.ProductsXML);
            case "betsoft":
                return commonCulture.ElementValues.getResourceXPathString("Products/ClubDivino/Label", commonVariables.ProductsXML);
            case "netent":
                return commonCulture.ElementValues.getResourceXPathString("Products/ClubNuovo/Label", commonVariables.ProductsXML);
            default:
                return productCode;
        }
    }
}

public static class FishingWorldProduct
{
    public static string GetLink()
    {
        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) return string.Empty;

        var settings = new customConfig.OperatorSettings(commonVariables.OperatorCode);
        var user = new Members().MemberData();

        var isProd = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ProductionSettings"));

        var url = isProd
            ? settings.Values.Get("FishingWorld_PROD_Url")
            : settings.Values.Get("FishingWorld_UAT_Url");

        var op = (int)commonVariables.operatorCode.W88;
        url = url.Replace("{OP}", op.ToString())
            .Replace("{ID}", user.MemberId)
            .Replace("{CURR}", commonCookie.CookieCurrency)
            .Replace("{LANG}", commonCookie.CookieLanguage)
            .Replace("{IP}", commonIp.remoteIP);

        var link = XDocument.Load(url);

        if (!string.IsNullOrEmpty((string)link.Root.Element("loginURL")))
        {
            return (string)link.Root.Element("loginURL");
        }

        commonAuditTrail.appendLog("system", "FishingWorldProduct", "NavMenu", "", "1", link.ToString(), "", "", url, Convert.ToString(1), "1", false);

        return string.Empty;

    }
}