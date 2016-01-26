using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Static_Palazzo_slots : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getRootResource("/TexasMahjongiOSDownload", out xeResources);

        spanMsg.InnerHtml = commonCulture.ElementValues.getResourceString("message", xeResources);
        sDownload.InnerText = commonCulture.ElementValues.getResourceString("downloadnow", xeResources);
        sDownload.HRef = Session["tmiosLink"].ToString();
    }
}