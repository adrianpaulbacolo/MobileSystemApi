using System;

public partial class _Info_Faq : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }
        var path = string.Format("{0}_Static\\Info\\FAQ\\FAQ_rewards_{1}.shtml", Server.MapPath("~").ToLower(), Language);
        htmltext.Text = System.IO.File.ReadAllText(path);                   
    }
}