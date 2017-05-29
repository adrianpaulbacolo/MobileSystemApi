using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Helpers;

public partial class v2_DownloadItem : BasePage
{
    public string Item;
    public string ItemBanner;
    protected XElement xeResources;

    protected override void OnLoad(EventArgs e)
    {
        Page.Title = commonCulture.ElementValues.getResourceString("download", commonVariables.LeftMenuXML);
        Page.Items.Add("Parent", Pages.Downloads);
        base.OnLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Item = Request.RequestContext.RouteData.Values["item"].ToString().ToLower();
        }
        catch (Exception ex)
        {
            Item = string.Empty;
        }

    }
}