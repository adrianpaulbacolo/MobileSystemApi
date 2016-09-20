using System.Web;
using W88.Rewards.BusinessLogic.Rewards.Models;
using W88.Utilities;
using W88.Utilities.Geo;

namespace W88.Rewards.BusinessLogic.Shared.Helpers
{
    public class CookieHelpers
    {
        static readonly IpHelper Ip = new IpHelper();

        public static ProductDetails ProductCookie
        {
            get
            {
                var cookie = HttpContext.Current.Request.Cookies.Get("product");
                return cookie == null ? null : Common.DeserializeObject<ProductDetails>(cookie.Value);
            }
            set
            {
                var cookie = HttpContext.Current.Request.Cookies.Get("product");
                if (cookie == null)
                {
                    cookie = new HttpCookie("product");
                    cookie.Value = Common.SerializeObject(value);
                    if (!string.IsNullOrEmpty(Ip.DomainName)) { cookie.Domain = Ip.DomainName; }
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
                else
                {
                    cookie.Value = Common.SerializeObject(value);
                    if (!string.IsNullOrEmpty(Ip.DomainName)) { cookie.Domain = Ip.DomainName; }
                    HttpContext.Current.Response.Cookies.Set(cookie);
                }
            }
        }
    }
}