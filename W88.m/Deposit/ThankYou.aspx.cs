using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class Deposit_ThankYou : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        XElement xeResources = null;
        commonCulture.appData.getRootResource("Deposit/Default.aspx", out xeResources);

        lblMessage.Text = commonCulture.ElementValues.getResourceString("lblThankYou", xeResources);
    }
}