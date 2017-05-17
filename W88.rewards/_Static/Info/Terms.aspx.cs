using System;

public partial class _Info_Terms : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }
        var path = string.Format("{0}_Static\\Info\\Terms\\TnC.{1}.htm", Server.MapPath("~").ToLower(), ContentLanguage);          
        htmltext.Text = System.IO.File.ReadAllText(path);          
    }
}