using System;
using System.Web;
using W88.Utilities.Geo;

namespace W88.BusinessLogic.Shared.Helpers
{
    public class CookieHelpers
    {
        static readonly IpHelper Ip = new IpHelper();

        public static string CookieLanguage
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
                if (!string.IsNullOrEmpty(Ip.DomainName)) { cookie.Domain = Ip.DomainName; }
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        public static void ClearCookies()
        {
            var keys = HttpContext.Current.Request.Cookies.AllKeys;
            foreach (var key in keys)
            {
                var cookie = HttpContext.Current.Request.Cookies[key];
                if (cookie != null)
                {
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    cookie.Value = string.Empty;
                    HttpContext.Current.Response.Cookies.Set(cookie);
                }
            }
        }
    }
}
