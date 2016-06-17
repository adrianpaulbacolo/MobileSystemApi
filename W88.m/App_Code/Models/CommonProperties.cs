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
}