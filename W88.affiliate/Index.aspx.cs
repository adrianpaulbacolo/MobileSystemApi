using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Index : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;

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

            string path = Server.MapPath("~").ToLower() + string.Format("_static\\home\\content_main\\{0}.htm", commonVariables.SelectedLanguage);
            mainContent.Text = System.IO.File.ReadAllText(path);

            ourProductlink.InnerText = commonCulture.ElementValues.getResourceString("lblOurProduct", xeResources);
            commissionPlanlink.InnerText = commonCulture.ElementValues.getResourceString("lblComissionPlan", xeResources);
            myAccountLink.InnerText = commonCulture.ElementValues.getResourceString("lblMyAccount", xeResources);
            overviewLink.InnerText = commonCulture.ElementValues.getResourceString("lblOverview", xeResources);
        }

        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("AffiliateId"))) { commonVariables.SetSessionVariable("AffiliateId", HttpContext.Current.Request.QueryString.Get("AffiliateId")); }
        if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) { divDefaultContent.Visible = false; } else { divAfterLoginContent.Visible = false; }

    }
}