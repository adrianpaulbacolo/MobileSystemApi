using System;
using System.Web;
using W88.Utilities;

public partial class LiveChat_Default : BasePage
{
    protected string LiveChatLink = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        var currentUrl = HttpContext.Current.Request.Url.ToString();
        var uri = new Uri(currentUrl);
        var host = uri.Host.Split('.');
        LiveChatLink = string.Format(Common.GetAppSetting<string>("WebHandler"), host[1], HttpUtility.UrlEncode(currentUrl));
        if (ContentLanguage.Equals("zh-my"))
            LiveChatLink = string.Format("{0}{1}", LiveChatLink, "&ul=en-us");
        Response.Redirect(LiveChatLink, false);
    }
}