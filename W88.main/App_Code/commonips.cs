using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace common
{
    public class ips
    {
        public static string remoteip
        {
            get
            {
                string strIp = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
                return string.IsNullOrEmpty(strIp) ? "" : strIp;
            }
        }

        public static string forwardedip
        {
            get
            {
                string strIp = HttpContext.Current.Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR");
                return string.IsNullOrEmpty(strIp) ? "" : strIp;
            }
        }

        public static string requesterip
        {
            get
            {
                string strIp = HttpContext.Current.Request.UserHostAddress;
                return string.IsNullOrEmpty(strIp) ? "" : strIp;
            }
        }

        public static string userip
        {
            get
            {
                string strIPList = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                string strIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                try
                {
                    if (!string.IsNullOrEmpty(strIPList)) { strIPAddress = strIPList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0]; }
                }
                catch (Exception) { }
                return strIPAddress;
            }
        }

        internal static string httphost
        {
            get { return HttpContext.Current.Request.ServerVariables["HTTP_HOST"]; }
        }

        public static string domainname
        {
            get
            {
                string strServerName = HttpContext.Current.Request.ServerVariables.Get("SERVER_NAME");
                int intFirstDot = strServerName.IndexOf('.') + 1;
                return strServerName.Substring(intFirstDot, strServerName.Length - intFirstDot);
            }
        }

        internal static string servername
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables.Get("SERVER_NAME");
            }
        }

        /*
        static void checkCountry() 
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
            string strBlockedCountries = opSettings.Values.Get("BlockedCountries");
            string strCountryCode = string.Empty;
            string strType = string.Empty;

            using (wsIP2Loc.ServiceSoapClient wsInstance = new wsIP2Loc.ServiceSoapClient()) 
            {
                wsInstance.location(commonIp.remoteIP, ref strCountryCode, ref strType);
            }

            if (strBlockedCountries.IndexOf(strCountryCode) > -1) 
            {
                //System.Web.HttpContext.Current.Response.Redirect("/forbidden.html");
            }
        }
        */
    }
}