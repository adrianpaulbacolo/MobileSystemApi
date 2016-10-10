using System;
using System.Web;
using System.Web.UI;
using W88.Utilities;

public partial class LiveChat_Default : Page
{
    protected string LiveChatLink = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        var currentUrl = HttpContext.Current.Request.Url.ToString();
        var uri = new Uri(currentUrl);
        var host = uri.Host.Split('.');
        LiveChatLink = string.Format(Common.GetAppSetting<string>("WebHandler"), host[1], currentUrl);
    }
}