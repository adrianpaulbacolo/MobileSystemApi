using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace common
{
    public static class cookies
    {
        /// <summary>Session ID</summary>
        public static string cookie_s
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("s");
                return cookie == null ? "" : cookie.Value;
            }
            set
            {
                HttpCookie cookie = new HttpCookie("s");
                cookie.Value = value;
                if (!string.IsNullOrEmpty(common.ips.domainname)) { cookie.Domain = common.ips.domainname; }
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>Sportsbook Session ID</summary>
        public static string cookie_g
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("g");

                return cookie == null ? "" : cookie.Value;
            }
            set
            {
                HttpCookie cookie = new HttpCookie("g");
                cookie.Value = value;
                if (!string.IsNullOrEmpty(common.ips.domainname)) { cookie.Domain = common.ips.domainname; }
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static string cookie_lang
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("language");
                return cookie == null ? "" : cookie.Value;
            }
            set
            {
                HttpCookie cookie = new HttpCookie("language");
                cookie.Value = value;
                if (!string.IsNullOrEmpty(common.ips.domainname)) { cookie.Domain = common.ips.domainname; }
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static string cookie_io
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("mio");
                return cookie == null ? "" : cookie.Value;
            }
            set
            {
                HttpCookie cookie = new HttpCookie("mio");
                cookie.Value = value;
                if (!string.IsNullOrEmpty(common.ips.domainname)) { cookie.Domain = common.ips.domainname; }
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }



        public static void clearAll()
        {
            common.variables.ClearSessionVariables();

            System.Web.HttpContext.Current.Response.Cookies["s"].Domain = common.ips.domainname;
            System.Web.HttpContext.Current.Response.Cookies["s"].Value = "";
            System.Web.HttpContext.Current.Response.Cookies["s"].Expires = DateTime.Now.AddYears(-1);
            System.Web.HttpContext.Current.Response.Cookies["g"].Domain = common.ips.domainname;
            System.Web.HttpContext.Current.Response.Cookies["g"].Value = "";
            System.Web.HttpContext.Current.Response.Cookies["g"].Expires = DateTime.Now.AddYears(-1);
            /*
            foreach (string strCookieName in HttpContext.Current.Request.Cookies.AllKeys) 
            {
                //respCookie.Domain = commonIp.domainName;
                //respCookie.Value = string.Empty;
                //respCookie.Expires = System.DateTime.Now.AddYears(-1);
                //System.web
                if (System.Web.HttpContext.Current.Request.Cookies[strCookieName] != null)
                {
                    HttpCookie myCookie = new HttpCookie(strCookieName);
                    myCookie.Expires = DateTime.Now.AddDays(-1d);
                    System.Web.HttpContext.Current.Response.Cookies.Add(myCookie);
                }
            }
            */
        }
    }
}