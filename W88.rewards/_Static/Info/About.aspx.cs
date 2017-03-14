using System;

public partial class _Info_About : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }
        var path =  string.Format("{0}\\_Static\\Info\\About\\aboutus.{1}.htm", Server.MapPath("~").ToLower(), Language);
        htmltext.Text = System.IO.File.ReadAllText(path);       
    }
}