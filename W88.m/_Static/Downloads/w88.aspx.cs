using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Static_Downloads_w88 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getRootResource("/Downloads", out xeResources);

        spanMsg.InnerHtml = commonCulture.ElementValues.getResourceString("w88iOSMessage", xeResources);
        sDownload.InnerText = commonCulture.ElementValues.getResourceString("downloadnow", xeResources);
        sDownload.HRef = ConfigurationManager.AppSettings["w88IOS_URL"];
    }
}