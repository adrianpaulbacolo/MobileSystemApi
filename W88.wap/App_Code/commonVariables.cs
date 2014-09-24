﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class commonVariables
{
    public static System.Xml.Linq.XElement LeftMenuXML { get { if (System.Web.HttpContext.Current.Cache.Get("leftMenuXML_" + commonVariables.SelectedLanguage) != null) { return System.Web.HttpContext.Current.Cache.Get("leftMenuXML_" + commonVariables.SelectedLanguage) as System.Xml.Linq.XElement; } else { System.Xml.Linq.XElement xcMenu = commonCulture.appData.getRootResource("/leftMenu"); System.Web.HttpContext.Current.Cache.Add("leftMenuXML_" + commonVariables.SelectedLanguage, xcMenu, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 15, 0), System.Web.Caching.CacheItemPriority.AboveNormal, null); return xcMenu; } } }
    public static System.Xml.Linq.XElement ProductsXML { get { if (System.Web.HttpContext.Current.Cache.Get("ProductsXML_" + commonVariables.SelectedLanguage) != null) { return System.Web.HttpContext.Current.Cache.Get("ProductsXML_" + commonVariables.SelectedLanguage) as System.Xml.Linq.XElement; } else { System.Xml.Linq.XElement xcMenu = commonCulture.appData.getRootResource("/Products"); System.Web.HttpContext.Current.Cache.Add("ProductsXML_" + commonVariables.SelectedLanguage, xcMenu, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 15, 0), System.Web.Caching.CacheItemPriority.AboveNormal, null); return xcMenu; } } }

    //public static System.Xml.Linq.XElement LeftMenuXML { get { return commonCulture.appData.getRootResource("/leftMenu"); } }
    //public static System.Xml.Linq.XElement ProductsXML { get { return commonCulture.appData.getRootResource("/Products"); } }

    public static System.Xml.Linq.XElement ErrorsXML { get { if (System.Web.HttpContext.Current.Cache.Get("errorsXML_" + commonVariables.SelectedLanguage) != null) { return System.Web.HttpContext.Current.Cache.Get("errorsXML_" + commonVariables.SelectedLanguage) as System.Xml.Linq.XElement; } else { System.Xml.Linq.XElement xcErrors = commonCulture.appData.getRootResource("/Errors"); System.Web.HttpContext.Current.Cache.Add("errorsXML_" + commonVariables.SelectedLanguage, xcErrors, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 15, 0), System.Web.Caching.CacheItemPriority.AboveNormal, null); return xcErrors; } } }
    public static string SiteUrl { get { return System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"]; } }

    public static string DateTimeFormat { get { return System.Configuration.ConfigurationManager.AppSettings.Get("DateTimeFormat"); } }
    public static string DecimalFormat { get { return System.Configuration.ConfigurationManager.AppSettings.Get("DecimalFormat"); } }
    public static string SelectedLanguage { get { return string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["SelectedLanguage"] as string) ? (!string.IsNullOrEmpty(commonCookie.CookieLanguage) ? commonCookie.CookieLanguage : "en-us") : Convert.ToString(System.Web.HttpContext.Current.Session["SelectedLanguage"]); } set { commonCookie.CookieLanguage = value; commonVariables.SetSessionVariable("SelectedLanguage", value); } }

    public static string CurrentMemberSessionId { get { return string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["MemberSessionId"] as string) ? (!string.IsNullOrEmpty(commonCookie.CookieS) ? commonCookie.CookieS : "") : Convert.ToString(System.Web.HttpContext.Current.Session["MemberSessionId"]); } }

    public static string OperatorId
    {
        get
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings(System.Configuration.ConfigurationManager.AppSettings.Get("Operator"));
            return opSettings.Values.Get("OperatorId");
        }
    }
    public static string OperatorCode
    {
        get
        {
            return System.Configuration.ConfigurationManager.AppSettings.Get("Operator");
        }
    }

    public static string GetSessionVariable(string key) { return string.IsNullOrEmpty(HttpContext.Current.Session[key] as string) ? "" : Convert.ToString(HttpContext.Current.Session[key]); }
    public static void SetSessionVariable(string key, string value) { HttpContext.Current.Session.Add(key, value); }

    public static void ClearSessionVariables() 
    {
        string strLanguage = string.Empty;
        string strVCode = string.Empty;

        strLanguage = commonVariables.SelectedLanguage;
        strVCode = commonVariables.GetSessionVariable("vCode");

        System.Web.HttpContext.Current.Session.Clear();
        System.Web.HttpContext.Current.Session.Abandon();

        commonVariables.SelectedLanguage = strLanguage;
        commonVariables.SetSessionVariable("vCode", strVCode);
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
        rhymepoker = 6
    }

    internal enum operatorCode
    {
        W88 = 1
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
        Mobile,
        Wap
    }

    public enum PaymentTransactionType 
    {
        Deposit = 1,
        Withdrawal = 2
    }
}