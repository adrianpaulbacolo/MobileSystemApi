using System;
using System.Xml.Linq;
using W88.BusinessLogic.Shared.Helpers;

public partial class _Lang : System.Web.UI.Page
{
    protected XElement LeftMenu = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        LeftMenu = CultureHelpers.AppData.GetRootResource("leftMenu");
    }
}