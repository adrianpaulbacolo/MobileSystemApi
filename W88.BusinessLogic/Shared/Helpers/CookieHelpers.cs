using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
