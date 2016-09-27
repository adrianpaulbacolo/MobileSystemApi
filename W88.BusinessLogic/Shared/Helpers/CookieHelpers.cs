using System.Web;
using W88.Utilities.Geo;

namespace W88.BusinessLogic.Shared.Helpers
{
    class CookieHelpers
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

        public static string CookieProduct
        {
            get
            {
                var cookie = HttpContext.Current.Request.Cookies.Get("product");
                return cookie == null ? "" : cookie.Value;
            }
            set
            {
                var cookie = HttpContext.Current.Request.Cookies.Get("product");
                if (cookie == null)
                {
                    cookie = new HttpCookie("product");
                    cookie.Value = value;
                    if (!string.IsNullOrEmpty(Ip.DomainName)) { cookie.Domain = Ip.DomainName; }
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                else
                {
                    cookie.Value = value;
                    if (!string.IsNullOrEmpty(Ip.DomainName)) { cookie.Domain = Ip.DomainName; }
                    HttpContext.Current.Response.Cookies.Set(cookie);
                }
            }
        }
    }
}
