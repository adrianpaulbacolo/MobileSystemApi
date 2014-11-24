using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Info_Terms : BasePage
{
    

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!Page.IsPostBack)
        {

            string path = Server.MapPath("~").ToLower() + string.Format("_Static\\Info\\Terms\\TnC.{0}.htm", commonVariables.SelectedLanguage);
            htmltext.Text = System.IO.File.ReadAllText(path);
          
        }
    }
}