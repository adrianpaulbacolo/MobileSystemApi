using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Index : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getLocalResource(out xeResources);
        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("lang"))) { commonVariables.SelectedLanguage = HttpContext.Current.Request.QueryString.Get("lang"); }

        if (!Page.IsPostBack) 
        {
            aHrefASports.InnerText = commonCulture.ElementValues.getResourceXPathString("Products/ASports/Label", commonVariables.ProductsXML);
            aHrefASports.HRef = commonASports.getSportsbookUrl;
            aHrefFundTransfer.InnerText = commonCulture.ElementValues.getResourceString("transfer", commonVariables.LeftMenuXML);
            aHrefLogout.InnerText = commonCulture.ElementValues.getResourceString("logout", commonVariables.LeftMenuXML);
        }
    }
}