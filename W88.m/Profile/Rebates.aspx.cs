using System;
using System.Web.UI;

public partial class Rebates : PaymentBasePage
{
    protected System.Xml.Linq.XElement xeResources = null;
    protected string Username;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        SetTitle(commonCulture.ElementValues.getResourceString("rebates", commonVariables.LeftMenuXML));
        Username = userInfo.MemberCode;
    }
}
