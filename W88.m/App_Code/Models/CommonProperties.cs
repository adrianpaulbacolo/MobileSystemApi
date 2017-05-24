using System.Web;
using System.Xml.Linq;

namespace Models
{
    /// <summary>
    /// Summary description for CommonProperties
    /// </summary>
    public abstract class CommonProperties
    {
        public string ioBlackBox { get; set; }

        public string SiteUrl = commonVariables.SiteUrl;

        public string DeviceId = HttpContext.Current.Request.UserAgent;

        public long OperatorId = long.Parse(commonVariables.OperatorId);

        public XElement XeErrors = commonVariables.ErrorsXML;
    }

    public class W88Constant
    {
        public struct PageNames
        {
            public static string Dashboard = "/v2/Dashboard.aspx";
            public static string Funds = "/v2/Funds.aspx";
            public static string Slots = "/v2/Slots";
            public static string Lottery = "/v2/Lottery.aspx";
            public static string Login = "/_Secure/Login.aspx";
            public static string Downloads = "/v2/Downloads.aspx";
            public static string Sports = "/v2/Sports.aspx";
            public static string VSports = "/v2/V-Sports.aspx";
            public static string ContactUs = "/v2/ContactUs.aspx";
            public static string Account = "/v2/Account/Default.aspx";
            public static string ChangePassword = "/v2/Account/ChangePassword.aspx";
            public static string Upload = "/v2/Account/Upload.aspx";
            public static string LiveChat = "/LiveChat/Default.aspx";
            public static string Rebates = "/v2/Account/Rebates.aspx";
            public static string BankDetails = "/v2/Account/BankDetails.aspx";
        }
    }
}