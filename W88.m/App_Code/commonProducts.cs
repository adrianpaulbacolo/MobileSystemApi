using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.XPath;

public class commonASports
{
    public static string getSportsbookUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ASportsUrl");
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

    public static System.Collections.Specialized.NameValueCollection Values { get { return System.Configuration.ConfigurationManager.GetSection("ProductGroupSettings/" + commonVariables.OperatorCode  + "/oneworks") as System.Collections.Specialized.NameValueCollection; } }
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
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
        }
    }
    public static string getRealUrl
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strUrl = opSettings.Values.Get("ClubBravadoRealUrl");
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
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
            return string.IsNullOrEmpty(strUrl) ? "" : strUrl.Replace("{DOMAIN}", commonIp.DomainName);
        }
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

   
}