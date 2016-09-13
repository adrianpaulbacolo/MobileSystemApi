using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Factories.Slots.Handlers
{
    /// <summary>
    /// This is the handler for GPI-QTech (QT)
    /// Lobby = Club Landing Page
    /// </summary>
    public class QTHandler : GameLoaderBase
    {
        private string fun;
        private string real;
        private string lobbyPage;

        private string memberSessionId;

        public QTHandler(string token, string lobby)
            : base(GameProvider.QT)
        {
            fun = GameSettings.GetGameUrl(GameProvider.QT, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.QT, GameLinkSetting.Real);


            memberSessionId = token;
            lobbyPage = lobby;
        }

        protected override string CreateFunUrl(XElement element)
        {
            string lang = GetGameLanguage(element);
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            return fun.Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{CURRENCY}", GetCurrencyByLanguage()).Replace("{LOBBY}", lobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string lang = GetGameLanguage(element);
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            return real.Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{TOKEN}", memberSessionId).Replace("{LOBBY}", lobbyPage);
        }

        private string GetCurrencyByLanguage()
        {
            string currency;
            switch (commonVariables.SelectedLanguage)
            {
                case "zh-cn":
                    currency = "CNY";
                    break;
                case "ja-jp":
                    currency = "JPY";
                    break;
                default:
                    currency = "USD";
                    break;
            }

            if (HttpContext.Current.Session["LanguageCode"] != null && HttpContext.Current.Session["CurrencyCode"] != null)
            {
                if ((string)HttpContext.Current.Session["LanguageCode"] == "en-us" && ((string)HttpContext.Current.Session["CurrencyCode"] == "MY"))
                {
                    currency = "MYR";
                }
            }

            return currency;
        }

        private string GetGameLanguage(XElement element)
        {
            if (element.Attribute("LanguageCode") == null) return "en_US";

            var lang = langCode.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                
            string[] languagesCodes = element.Attribute("LanguageCode").Value.Split(',');

            bool isLangSupp = languagesCodes.Contains(langCode, StringComparer.OrdinalIgnoreCase);

            return isLangSupp ? string.Format("{0}_{1}", lang[0], lang[1].ToUpper()) : "en_US";
        }
    }
}