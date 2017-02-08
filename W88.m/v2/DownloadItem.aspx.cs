using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class v2_DownloadItem : BasePage
{
    public string Item;
    public string ItemBanner;
    protected XElement xeResources;

    protected override void OnLoad(EventArgs e)
    {
        Page.Title = "Download Details";
        Page.Items.Add("Parent", "/v2/Downloads");
        base.OnLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Item = Request.RequestContext.RouteData.Values["item"].ToString();
            switch(Item){
                case "palazzo-casino":
                    commonCulture.appData.getRootResource("/ClubPalazzoDownload", out xeResources);
                    instructions.InnerHtml = commonCulture.ElementValues.getResourceString("content", xeResources);
                    instructionHeader.InnerHtml = commonCulture.ElementValues.getResourceString("header", xeResources);
                    downloadlink.InnerText = commonCulture.ElementValues.getResourceString("downloadnow", xeResources);
                    downloadlink.HRef = "http://mlive.w88palazzo.com";

                    ItemBanner = "/_static/v2/assets/images/downloads/PT-LiveCasino-DownloadPage.jpg";
                    break;
                case "palazzo-slots":
                    commonCulture.appData.getRootResource("/ClubPalazzoDownload", out xeResources);
                    instructions.InnerHtml = commonCulture.ElementValues.getResourceString("content", xeResources);
                    instructionHeader.InnerHtml = commonCulture.ElementValues.getResourceString("header", xeResources);
                    downloadlink.InnerText = commonCulture.ElementValues.getResourceString("downloadnow", xeResources);
                    downloadlink.HRef = "http://mgames.w88palazzo.com";

                    ItemBanner = "/_static/v2/assets/images/downloads/PT-Slots-DownloadPage.jpg";
                    break;
                case "texas-mahjong-ios":
                    commonCulture.appData.getRootResource("/TexasMahjongiOSDownload", out xeResources);
                    instructions.InnerHtml = commonCulture.ElementValues.getResourceString("content", xeResources);
                    instructionHeader.InnerHtml = commonCulture.ElementValues.getResourceString("header", xeResources);
                    downloadlink.InnerText = commonCulture.ElementValues.getResourceString("downloadnow", xeResources);
                    downloadlink.HRef = ConfigurationManager.AppSettings["TexasMahjongIOS_URL"];

                    ItemBanner = "/_static/v2/assets/images/downloads/TM-DownloadPage.jpg";
                    break;
                default:
                    break;
            }
        }
        catch (Exception ex)
        {
            Item = string.Empty;
        }

    }
}