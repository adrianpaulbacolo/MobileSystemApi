using System;
using System.Web.UI;
using W88.BusinessLogic.Shared.Helpers;

public partial class _Info_Levels : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }
        var path = Server.MapPath("~").ToLower() + string.Format("_Static\\Info\\Levels\\levels.{0}.htm", LanguageHelpers.SelectedLanguage);
        htmltext.Text = System.IO.File.ReadAllText(path);                   
    }
}