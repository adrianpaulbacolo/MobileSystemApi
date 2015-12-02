using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _ContactUs : BaseBeforeLogin
{
    protected System.Xml.Linq.XElement xeResources = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Web.UI.WebControls.Literal litScript = (System.Web.UI.WebControls.Literal)Page.FindControl("litScript");
        //commonCulture.appData.getLocalResource(out xeResources);
       // commonCulture.appData.getRootResource("ContactUs.aspx", out xeResources);

        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("Error")) && !string.IsNullOrEmpty(commonVariables.GetSessionVariable("Error")))
            {
                Session.Remove("Error");
                if (litScript != null)
                {
                    litScript.Text += string.Format("<script type='text/javascript'>alert('{0}');</script>", HttpContext.Current.Request.QueryString.Get("Error"));
                }
            }

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("lang"))) { commonVariables.SelectedLanguage = HttpContext.Current.Request.QueryString.Get("lang"); }

            //aSkype.HRef = commonCulture.ElementValues.getResourceString("lnkSkype", xeResources);
            //aEmail.HRef = commonCulture.ElementValues.getResourceString("lnkEmail", xeResources);
            //aBanking.HRef = commonCulture.ElementValues.getResourceString("lnkBanking", xeResources);
            //aPhone.HRef = commonCulture.ElementValues.getResourceString("lnkPhone", xeResources);

            //if (string.IsNullOrEmpty(commonCulture.ElementValues.getResourceString("lnkPhone", xeResources))) 
            //{
            //    liPhone.Visible = false;
            //}
        }
    }
}