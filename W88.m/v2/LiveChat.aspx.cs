using System;
using System.Configuration;
using System.Web;
using System.Web.UI;

public partial class v2_LiveChat : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            string CurrentUrl = HttpContext.Current.Request.Url.ToString();
            var liveChatUrl = ConfigurationManager.AppSettings["LiveChat"].Replace("{DOMAIN}", commonIp.DomainName) + CurrentUrl;
            Response.Redirect(liveChatUrl, true);
        }
    }
}