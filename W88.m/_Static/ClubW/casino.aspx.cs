using System;

namespace _Static.ClubW
{
    public partial class StaticClubWCasino : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            System.Xml.Linq.XElement xeResources;
            commonCulture.appData.getRootResource("/ClubWDownload", out xeResources);

            spanMsg.InnerHtml = commonCulture.ElementValues.getResourceString("message", xeResources);
            sDownload.InnerText = commonCulture.ElementValues.getResourceString("downloadnow", xeResources);

            sDownload.HRef = commonClubWAPK.getDownloadUrl;
            EnableLogoOnPage(false, false, true);
        }
    }
}