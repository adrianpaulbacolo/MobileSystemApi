using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_Palazzo : BasePage
{
    System.Xml.Linq.XElement _xeLoginResources;

    protected void Page_Load(object sender, EventArgs e)
    {
        commonCulture.appData.getRootResource("/_Secure/Login.aspx", out _xeLoginResources);

        lblRegister.Text = commonCulture.ElementValues.getResourceString("lblRegister2", _xeLoginResources);
        lblRegNote.Text = commonCulture.ElementValues.getResourceString("lblMsgNote", _xeLoginResources);
        lblUsername.Text = commonCulture.ElementValues.getResourceString("lblUsername", _xeLoginResources);
        lblPassword.Text = commonCulture.ElementValues.getResourceString("lblPassword", _xeLoginResources);
    }
}