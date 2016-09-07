using System;
using System.Web;

namespace W88.Utilities.Geo
{
    public class IpHelper
    {
        public string User
        {
            get
            {
                string strIpList = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                string strIpAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                try
                {
                    if (!string.IsNullOrEmpty(strIpList)) { strIpAddress = strIpList.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0]; }
                }
                catch (Exception) { }
                return strIpAddress;
            }
        }

        public string DomainName
        {
            get
            {
                string strServerName = HttpContext.Current.Request.ServerVariables.Get("SERVER_NAME");
                int intFirstDot = strServerName.IndexOf('.') + 1;
                return strServerName.Substring(intFirstDot, strServerName.Length - intFirstDot);
            }
        }

        public string Remote
        {
            get
            {
                string strIp = HttpContext.Current.Request.ServerVariables.Get("REMOTE_ADDR");
                return string.IsNullOrEmpty(strIp) ? "" : strIp;
            }
        }

        public string Forwarded
        {
            get
            {
                string strIp = HttpContext.Current.Request.ServerVariables.Get("HTTP_X_FORWARDED_FOR");
                return string.IsNullOrEmpty(strIp) ? "" : strIp;
            }
        }

        public object Requester
        {
            get
            {
                string strIp = HttpContext.Current.Request.UserHostAddress;
                return string.IsNullOrEmpty(strIp) ? "" : strIp;
            }
        }
    }
}
