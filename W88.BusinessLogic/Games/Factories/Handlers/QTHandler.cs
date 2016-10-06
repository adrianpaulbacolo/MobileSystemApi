using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Games.Handlers;
using W88.BusinessLogic.Shared.Helpers;

namespace W88.BusinessLogic.Games.Factories.Handlers
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
        private UserSessionInfo userInfo;

        public QTHandler(UserSessionInfo user, string lobby) : base(GameProvider.QT, user.LanguageCode)
        {
            fun = GameSettings.GetGameUrl(GameProvider.QT, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.QT, GameLinkSetting.Real);

            memberSessionId = user.Token;
            lobbyPage = lobby;
            userInfo = user;

        }

        protected override string CreateFunUrl(XElement element)
        {
            string lang = GetGameLanguage(element);
            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            return fun.Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{CURRENCY}", GetCurrencyByLanguage()).Replace("{LOBBY}", lobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string lang = GetGameLanguage(element);
            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            return real.Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{TOKEN}", memberSessionId).Replace("{LOBBY}", lobbyPage);
        }

        private string GetCurrencyByLanguage()
        {
            string currency;
            switch (LanguageCode)
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

            if (!string.IsNullOrEmpty(userInfo.LanguageCode) && string.IsNullOrEmpty(userInfo.CurrencyCode))
            {
                if (userInfo.LanguageCode == "en-us" && ((string)userInfo.CurrencyCode == "MY"))
                {
                    currency = "MYR";
                }
            }

            return currency;
        }

        private string GetGameLanguage(XElement element)
        {
            if (string.IsNullOrWhiteSpace(CultureHelpers.ElementValues.GetResourceXPathAttribute("LanguageCode", element))) return "en_US";

            var lang = LanguageCode.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                
            string[] languagesCodes = CultureHelpers.ElementValues.GetResourceXPathAttribute("LanguageCode", element).Split(',');

            bool isLangSupp = languagesCodes.Contains(LanguageCode, StringComparer.OrdinalIgnoreCase);

            return isLangSupp ? string.Format("{0}_{1}", lang[0], lang[1].ToUpper()) : "en_US";
        }
    }
}