using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _CommissionPlans : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //commonFunction.init();
        //phMeta.Controls.Add(Page.LoadControl("~/common/meta.ascx"));
        //phHeader.Controls.Add(Page.LoadControl("~/common/header.ascx"));
        //phFooter.Controls.Add(Page.LoadControl("~/common/footer.ascx"));

        string path1 = Server.MapPath("~").ToLower() + string.Format("_Static\\commissionplans\\content_main\\{0}.htm", commonVariables.SelectedLanguage);
        mainContent.Text = System.IO.File.ReadAllText(path1);
    }
}