using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_Dashboard : BasePage
{
    public string BannerDiv;
    public string FishingLink;

    protected override void OnLoad(EventArgs e)
    {
        Page.Title = "Dashboard";
        base.OnLoad(e);
        var deviceId = commonFunctions.getMobileDevice(Request);
        FishingLink = (deviceId == 2) ? "https://s3-ap-southeast-1.amazonaws.com/w88download/fishing/FishingMaster.apk" : "itms-services://?action=download-manifest&url=https://s3-ap-southeast-1.amazonaws.com/w88download/fishing/manifest.plist";
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        var banner = new Banner();
        BannerDiv = banner.GetBanners();
    }
}