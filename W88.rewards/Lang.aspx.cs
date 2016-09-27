using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.Xml.Linq;
using W88.BusinessLogic.Shared.Helpers;

public partial class _Lang : System.Web.UI.Page
{
    protected XElement LeftMenu = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        LeftMenu = CultureHelpers.AppData.GetRootResource("leftMenu");
        var languages = (new LanguageHelpers()).Language;
        var keys = languages.Keys;
        var divBuilder = new StringBuilder();

        foreach(var key in keys)
        {
            divBuilder.Append(@"<div class='col-xs-6'>")
                .Append(@"<a id='" + key + "'>")
                .Append(@"<img src='/_Static/Css/images/flags/")
                .Append(languages[key])
                .Append(@".svg' alt=''/>")
                .Append(@"<span>" + GetLanguageNameTranslation(languages[key]) + "</span>")
                .Append(@"</a></div>");
        }

        divLanguageContainer.InnerHtml = divBuilder.ToString();
    }

    private string GetLanguageNameTranslation(string shortLangCode)
    {
        var operatorSettings = ConfigurationManager.GetSection("OperatorGroupSettings/W88") as NameValueCollection;
        var languageNames = operatorSettings.Get("list_language_translation");
        var languageName = string.Empty;
        var names = languageNames.Split(',');
        
        foreach(var name in names) {
            var split = name.Split('=');
            var langCode = split[0];
            if(langCode.Equals(shortLangCode)) {
                languageName = split[1];
                break;
            }
        }
        return languageName;
    }
}