using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace common
{
    public class variables
    {
        public static System.Xml.Linq.XElement LeftMenuXML { get { if (System.Web.HttpContext.Current.Cache.Get("leftMenuXML_" + common.variables.SelectedLanguage) != null) { return System.Web.HttpContext.Current.Cache.Get("leftMenuXML_" + common.variables.SelectedLanguage) as System.Xml.Linq.XElement; } else { System.Xml.Linq.XElement xcMenu = common.cultures.appData.getRootResource("/leftMenu"); System.Web.HttpContext.Current.Cache.Add("leftMenuXML_" + common.variables.SelectedLanguage, xcMenu, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 15, 0), System.Web.Caching.CacheItemPriority.AboveNormal, null); return xcMenu; } } }
        public static System.Xml.Linq.XElement ProductsXML { get { if (System.Web.HttpContext.Current.Cache.Get("ProductsXML_" + common.variables.SelectedLanguage) != null) { return System.Web.HttpContext.Current.Cache.Get("ProductsXML_" + common.variables.SelectedLanguage) as System.Xml.Linq.XElement; } else { System.Xml.Linq.XElement xcMenu = common.cultures.appData.getRootResource("/Products"); System.Web.HttpContext.Current.Cache.Add("ProductsXML_" + common.variables.SelectedLanguage, xcMenu, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 15, 0), System.Web.Caching.CacheItemPriority.AboveNormal, null); return xcMenu; } } }
        public static System.Xml.Linq.XElement ContactUsXML { get { if (System.Web.HttpContext.Current.Cache.Get("ContactUsXML_" + common.variables.SelectedLanguage) != null) { return System.Web.HttpContext.Current.Cache.Get("ContactUsXML_" + common.variables.SelectedLanguage) as System.Xml.Linq.XElement; } else { System.Xml.Linq.XElement xcMenu = common.cultures.appData.getRootResource("/ContactUs.aspx"); System.Web.HttpContext.Current.Cache.Add("ContactUsXML_" + common.variables.SelectedLanguage, xcMenu, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 15, 0), System.Web.Caching.CacheItemPriority.AboveNormal, null); return xcMenu; } } }

        public static System.Xml.Linq.XElement ErrorsXML { get { if (System.Web.HttpContext.Current.Cache.Get("errorsXML_" + common.variables.SelectedLanguage) != null) { return System.Web.HttpContext.Current.Cache.Get("errorsXML_" + common.variables.SelectedLanguage) as System.Xml.Linq.XElement; } else { System.Xml.Linq.XElement xcErrors = common.cultures.appData.getRootResource("/Errors"); System.Web.HttpContext.Current.Cache.Add("errorsXML_" + common.variables.SelectedLanguage, xcErrors, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 15, 0), System.Web.Caching.CacheItemPriority.AboveNormal, null); return xcErrors; } } }
        public static string SiteUrl { get { return System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"]; } }

        public static string DisplayDateFormat { get { return System.Configuration.ConfigurationManager.AppSettings.Get("DisplayDateFormat"); } }
        public static string DisplayDateTimeFormat { get { return System.Configuration.ConfigurationManager.AppSettings.Get("DisplayDateTimeFormat"); } }
        public static string DateTimeFormat { get { return System.Configuration.ConfigurationManager.AppSettings.Get("DateTimeFormat"); } }
        public static string DecimalFormat { get { return System.Configuration.ConfigurationManager.AppSettings.Get("DecimalFormat"); } }
        public static string SelectedLanguage { get { return string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["SelectedLanguage"] as string) ? (!string.IsNullOrEmpty(common.cookies.cookie_lang) ? common.cookies.cookie_lang : "") : Convert.ToString(System.Web.HttpContext.Current.Session["SelectedLanguage"]); } set { common.cookies.cookie_lang = value; common.variables.SetSessionVariable("SelectedLanguage", value); } }
        public static string SelectedLanguageShort
        {
            get
            {
                switch (common.variables.SelectedLanguage.ToLower())
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

        public static string CurrentMemberSessionId { get { return string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["MemberSessionId"] as string) ? (!string.IsNullOrEmpty(common.cookies.cookie_s) ? common.cookies.cookie_s : "") : Convert.ToString(System.Web.HttpContext.Current.Session["MemberSessionId"]); } }

        public static string OperatorId
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("OperatorId");                
            }
        }
        public static string OperatorCode
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("OperatorCode");
            }
        }

        public static string GetSessionVariable(string key) { return string.IsNullOrEmpty(HttpContext.Current.Session[key] as string) ? "" : Convert.ToString(HttpContext.Current.Session[key]); }
        public static void SetSessionVariable(string key, string value) { HttpContext.Current.Session.Add(key, value); }
        public static void RemoveSessionVariable(string key) { HttpContext.Current.Session.Remove(key); }


        public static void ClearSessionVariables()
        {
            string strLanguage = string.Empty;
            string strVCode = string.Empty;

            strLanguage = common.variables.SelectedLanguage;
            strVCode = common.variables.GetSessionVariable("vCode");

            System.Web.HttpContext.Current.Session.Clear();
            System.Web.HttpContext.Current.Session.Abandon();

            common.variables.SelectedLanguage = strLanguage;
            common.variables.SetSessionVariable("vCode", strVCode);
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
            WingMoney = 110308,
            SDPay = 120223
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
    }
}