using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Helpers
{
    /// <summary>
    /// Summary description for Pages
    /// </summary>
    public class Pages
    {
        private static List<XElement> _listing;

        private static void Initialize()
        {
            XElement xePages;
            commonCulture.appData.GetRootResourceNonLanguage("/Shared/Pages", out xePages);
            _listing = xePages.Elements("Pages").Elements().ToList();
        }

        private static string GetUrl(string id)
        {
            Initialize();
            foreach (var item in _listing.Where(item => item.Attribute("id").Value.Equals(id)))
            {
                return item.Value;
            }

            return "#";
        }

        public static string Dashboard
        {
            get { return GetUrl("dashboard"); }
        }

        public static string Funds
        {
            get { return GetUrl("funds"); }
        }

        public static string Slots
        {
            get { return GetUrl("slots"); }
        }

        public static string Lottery
        {
            get { return GetUrl("lottery"); }
        }

        public static string Login
        {
            get { return GetUrl("login"); }
        }

        public static string Downloads
        {
            get { return GetUrl("downloads"); }
        }

        public static string Sports
        {
            get { return GetUrl("sports"); }
        }

        public static string VSports
        {
            get { return GetUrl("vSports"); }
        }

        public static string ContactUs
        {
            get { return GetUrl("contactUs"); }
        }

        public static string Account
        {
            get { return GetUrl("account"); }
        }

        public static string ChangePassword
        {
            get { return GetUrl("changePassword"); }
        }

        public static string Upload
        {
            get { return GetUrl("upload"); }
        }

        public static string LiveChat
        {
            get
            {
                return GetUrl("livechat").Replace("{DOMAIN}", commonIp.DomainName) + HttpContext.Current.Request.Url.ToString();
            }
        }

        public static string Rebates
        {
            get { return GetUrl("rebates"); }
        }

        public static string BankDetails
        {
            get { return GetUrl("bankdetails"); }
        }

        public static string Register
        {
            get { return GetUrl("register"); }
        }
    }
}