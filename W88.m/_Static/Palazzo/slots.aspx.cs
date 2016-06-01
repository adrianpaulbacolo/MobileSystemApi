using System;

namespace _Static.Palazzo
{
    public partial class StaticPalazzoSlots : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            System.Xml.Linq.XElement xeResources;
            commonCulture.appData.getRootResource("/ClubPalazzoDownload", out xeResources);

            spanMsg.InnerHtml = commonCulture.ElementValues.getResourceString("message", xeResources);
            sDownload.InnerText = commonCulture.ElementValues.getResourceString("downloadnow", xeResources);

            EnableLogoOnPage(false, false, true);
        }
    }
}