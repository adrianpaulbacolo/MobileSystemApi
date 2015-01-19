using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class OurProducts_OurProductsDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string strTitle = Request.QueryString.Get("OurProducts");

        string path1 = Server.MapPath("~").ToLower() + string.Format("_Static\\ourproducts\\content_main\\{0}.{1}.htm", strTitle, commonVariables.SelectedLanguage);
        mainContent.Text = System.IO.File.ReadAllText(path1);
    }
}