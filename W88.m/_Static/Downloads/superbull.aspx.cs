using System;
using System.Configuration;

public partial class _Static_Downloads_superbull : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getRootResource("/Downloads", out xeResources);

        spanMsg.InnerHtml = commonCulture.ElementValues.getResourceString("superbulliOSMessage", xeResources);
        sDownload.InnerText = commonCulture.ElementValues.getResourceString("downloadnow", xeResources);

        sDownload.HRef = commonCookie.CookieLanguage.ToLower() == "zh-cn"
            ? ConfigurationManager.AppSettings["SuperBull_IOS_URL"]
            : ConfigurationManager.AppSettings["SuperBull_IOS_URL_EN"];

    }
}