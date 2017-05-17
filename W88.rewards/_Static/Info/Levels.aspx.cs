using System;

public partial class _Info_Levels : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }
        var path = string.Format("{0}_Static\\Info\\Levels\\levels.{1}.htm", Server.MapPath("~").ToLower(), ContentLanguage);
        htmltext.Text = System.IO.File.ReadAllText(path);                   
    }
}