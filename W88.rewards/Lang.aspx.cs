using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Text;
using System.Web.UI;

public partial class _Lang : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var languages = new Dictionary<string, string>();
        languages.Add("en-us", "en");
        languages.Add("zh-cn", "cn");
        languages.Add("id-id", "id");
        languages.Add("ja-jp", "jp");
        languages.Add("km-kh", "kh");
        languages.Add("ko-kr", "kr");
        languages.Add("th-th", "th");
        languages.Add("vi-vn", "vn");

        var divBuilder = new StringBuilder();
        var keys = languages.Keys;
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