using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Index : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;

    protected void Page_Init(object sender, EventArgs e) 
    {
        System.Text.RegularExpressions.Regex rxDomains_CN = new System.Text.RegularExpressions.Regex(@"(.w88uat|.w88cn)");

        if (string.IsNullOrEmpty(commonVariables.SelectedLanguage))
        {
            if (rxDomains_CN.IsMatch(Request.ServerVariables["SERVER_NAME"]))
            {
                commonVariables.SelectedLanguage = "zh-cn";
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Web.UI.WebControls.Literal litScript = (System.Web.UI.WebControls.Literal)Page.FindControl("litScript");
        
        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("lang"))) { commonVariables.SelectedLanguage = HttpContext.Current.Request.QueryString.Get("lang"); }

        xeErrors = commonVariables.ErrorsXML;

        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getRootResource("/Index.aspx", out xeResources);

        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("Error")) && !string.IsNullOrEmpty(commonVariables.GetSessionVariable("Error")))
            {
                Session.Remove("Error");
                if (litScript != null) { litScript.Text += string.Format("<script type='text/javascript'>alert('{0}');</script>", HttpContext.Current.Request.QueryString.Get("Error")); }
            }

            lblLogin.InnerHtml = commonCulture.ElementValues.getResourceString("lblPlaceBet", xeResources);
        }

        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("AffiliateId"))) { commonVariables.SetSessionVariable("AffiliateId", HttpContext.Current.Request.QueryString.Get("AffiliateId")); }

        if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) { divLoginMessage.Visible = false; }
    }
}