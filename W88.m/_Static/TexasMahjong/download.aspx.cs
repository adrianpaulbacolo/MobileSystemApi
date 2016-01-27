using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Static_TexasMahjong_download : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getRootResource("/TexasMahjongiOSDownload", out xeResources);

        spanMsg.InnerHtml = commonCulture.ElementValues.getResourceString("message", xeResources);
        sDownload.InnerText = commonCulture.ElementValues.getResourceString("downloadnow", xeResources);
        sDownload.HRef = ConfigurationManager.AppSettings["TexasMahjongIOS_URL"];
    }
}