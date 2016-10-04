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
        public QTHandler(string token, string lobby) : base(GameProvider.QT)
        {
            Fun = GameSettings.GetGameUrl(GameProvider.QT, GameLinkSetting.Fun);
            Real = GameSettings.GetGameUrl(GameProvider.QT, GameLinkSetting.Real);

            GameProvider = GameProvider.QT;
            MemberSessionId = token;
            LobbyPage = lobby;
        }

        protected override string CreateFunUrl(XElement element)
        {
            string lang = GetGameLanguage(element);
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            var gpi = CheckRSlot(GameLinkSetting.Real, element);
            if (!string.IsNullOrWhiteSpace(gpi))
            {
                return gpi;
            }

            return Fun.Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{CURRENCY}", GetCurrencyByLanguage()).Replace("{LOBBY}", LobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string lang = GetGameLanguage(element);
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            var gpi = CheckRSlot(GameLinkSetting.Real, element);
            if (!string.IsNullOrWhiteSpace(gpi))
            {
                return gpi;
            }

            return Real.Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{TOKEN}", MemberSessionId).Replace("{LOBBY}", LobbyPage);
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