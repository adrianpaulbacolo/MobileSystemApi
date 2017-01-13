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
        var languages = LanguageDictionary;
        var divBuilder = new StringBuilder();
        var keys = languages.Keys;
        var languageNames = LanguageNames;
        foreach (var key in keys)
        {
            divBuilder.Append(@"<div class='col-xs-6'>")
                .Append(@"<a id='" + key + "'>")
                .Append(@"<img src='/_Static/Css/images/flags/")
                .Append(languages[key])
                .Append(@".svg' alt=''/>")
                .Append(@"<span>" + GetLanguageNameTranslation(languageNames, languages[key]) + "</span>")
                .Append(@"</a></div>");
        }
        divLanguageContainer.InnerHtml = divBuilder.ToString();
    }

    private NameValueCollection OperatorSettings
    {
        get
        {
            return ConfigurationManager.GetSection("OperatorGroupSettings/W88") as NameValueCollection;
        }
    }

    private Dictionary<string, string> LanguageDictionary
    {
        get
        {
            try
            {
                var languages = new Dictionary<string, string>();
                var languageList = OperatorSettings.Get("LanguageSelection").Split('|');
                foreach (var language in languageList)
                {
                    var trimmed = language.Trim();
                    switch (trimmed)
                    {
                        case "en-us":
                            languages.Add(trimmed, "en");
                            break;
                        case "zh-cn":
                            languages.Add(trimmed, "cn");
                            break;
                        case "id-id":
                            languages.Add(trimmed, "id");
                            break;
                        case "ja-jp":
                            languages.Add(trimmed, "jp");
                            break;
                        case "km-kh":
                            languages.Add(trimmed, "kh");
                            break;
                        case "ko-kr":
                            languages.Add(trimmed, "kr");
                            break;
                        case "th-th":
                            languages.Add(trimmed, "th");
                            break;
                        case "vi-vn":
                            languages.Add(trimmed, "vn");
                            break;
                    }
                }
                return languages;
            }
            catch (Exception)
            {
                return new Dictionary<string, string>();
            }
        }
    }

    private string[] LanguageNames
    {
        get
        {
            try
            {
                return OperatorSettings.Get("list_language_translation").Split(',');
            }
            catch (Exception)
            {
                return new string[0];
            }
        }
    }

    private string GetLanguageNameTranslation(IEnumerable<string> languageNames, string shortLangCode)
    {
        var languageName = string.Empty;
        foreach(var name in languageNames) {
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