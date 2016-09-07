using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace W88.BusinessLogic.Shared.Helpers
{
    public class LanguageHelpers
    {
        public IDictionary<string, string> Language = new Dictionary<string, string>();

        public LanguageHelpers()
        {
            Language.Add("en-us", "en");
            Language.Add("id-id", "id");
            Language.Add("km-kh", "kh");
            Language.Add("ko-kr", "kr");
            Language.Add("th-th", "th");
            Language.Add("vi-vn", "vn");
            Language.Add("ja-jp", "jp");
            Language.Add("zh-cn", "cn");
        }

        public static string SelectedLanguage
        {
            get
            {
                if (!string.IsNullOrEmpty(CookieHelpers.CookieLanguage))
                {
                    return CookieHelpers.CookieLanguage;
                }

                if (!string.IsNullOrWhiteSpace(Utilities.Common.GetSessionVariable("SelectedLanguage")))
                {
                    return Convert.ToString(Utilities.Common.GetSessionVariable("SelectedLanguage"));
                }

                var rxDomainsCn = new Regex(Utilities.Common.GetAppSetting<string>("CN_domain"));
                return rxDomainsCn.IsMatch(HttpContext.Current.Request.ServerVariables["SERVER_NAME"])
                    ? "zh-cn"
                    : "en-us";
            }
            set
            {
                CookieHelpers.CookieLanguage = value;
                Utilities.Common.SetSessionVariable("SelectedLanguage", value);
            }
        }

        public static string SelectedLanguageShort
        {
            get
            {
                IDictionary<string, string> language = new Dictionary<string, string>();

                language.Add("en-us", "en");
                language.Add("id-id", "id");
                language.Add("km-kh", "kh");
                language.Add("ko-kr", "kr");
                language.Add("th-th", "th");
                language.Add("vi-vn", "vn");
                language.Add("ja-jp", "jp");
                language.Add("zh-cn", "cn");

                foreach (var pair in language.Where(pair => pair.Key.Equals(SelectedLanguage.ToLower())))
                {
                    return pair.Value;
                }

                return language["en"];
            }
        }

        public static string GetLanguageByDomain(string domain)
        {
            string language;

            if (Utilities.Common.GetAppSetting<string>(Utilities.Constant.HeaderKeys.COUNTRY_DOMAIN_CN).Contains(domain))
            {
                language = "zh-cn";
            }
            else if (Utilities.Common.GetAppSetting<string>(Utilities.Constant.HeaderKeys.COUNTRY_DOMAIN_VN).Contains(domain))
            {
                language = "vi-vn";
            }
            else if (Utilities.Common.GetAppSetting<string>(Utilities.Constant.HeaderKeys.COUNTRY_DOMAIN_TH).Contains(domain))
            {
                language = "th-th";
            }
            else if (Utilities.Common.GetAppSetting<string>(Utilities.Constant.HeaderKeys.COUNTRY_DOMAIN_ID).Contains(domain))
            {
                language = "id-id";
            }
            else if (Utilities.Common.GetAppSetting<string>(Utilities.Constant.HeaderKeys.COUNTRY_DOMAIN_MY).Contains(domain))
            {
                language = "en-us";
            }
            else if (Utilities.Common.GetAppSetting<string>(Utilities.Constant.HeaderKeys.COUNTRY_DOMAIN_KR).Contains(domain))
            {
                language = "ko-kr";
            }
            else if (Utilities.Common.GetAppSetting<string>(Utilities.Constant.HeaderKeys.COUNTRY_DOMAIN_JP).Contains(domain))
            {
                language = "ja-jp";
            }
            else if (Utilities.Common.GetAppSetting<string>(Utilities.Constant.HeaderKeys.COUNTRY_DOMAIN_KH).Contains(domain))
            {
                language = "km-kh";
            }
            else if (!string.IsNullOrWhiteSpace(SelectedLanguage))
            {
                language = SelectedLanguage;
            }
            else
            {
                language = "en-us";
            }

            return language;
        }
    }
}
