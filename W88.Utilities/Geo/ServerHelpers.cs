using System.Web;

namespace W88.Utilities.Geo
{
    public class ServerHelpers
    {
        public string SiteUrl { get { return HttpContext.Current.Request.ServerVariables["SERVER_NAME"]; } }
    }
}
