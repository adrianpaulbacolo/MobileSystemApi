using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Games.Handlers;
using W88.BusinessLogic.Games.Models;
using W88.BusinessLogic.Shared.Helpers;

namespace W88.BusinessLogic.Games.Factories.Handlers
{
    /// <summary>
    /// This is the handler for GPI-QTech (QT)
    /// Lobby = Club Landing Page
    /// </summary>
    public class QTHandler : GameLoaderBase
    {
        private UserSessionInfo _userInfo;

        public QTHandler(UserSessionInfo user, string lobby) : base(GameProvider.QT, user.LanguageCode)
        {
            _userInfo = user;
            GameLink = new GameLinkInfo
            {
                Fun = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Fun),
                Real = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Real),
                MemberSessionId = user.Token,
                LobbyPage = lobby
            };
        }

        protected override string CreateFunUrl(XElement element)
        {
            return BuildUrl(element, GameLinkSetting.Fun);
        }

        protected override string CreateRealUrl(XElement element)
        {
            return BuildUrl(element, GameLinkSetting.Real);
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

            if (!string.IsNullOrEmpty(_userInfo.LanguageCode) && string.IsNullOrEmpty(_userInfo.CurrencyCode))
            {
                if (_userInfo.LanguageCode == "en-us" && ((string)_userInfo.CurrencyCode == "MY"))
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

        private string BuildUrl(XElement element, GameLinkSetting setting)
        {
            string gameUrl;
            var lang = GetGameLanguage(element);
            var gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            if (setting == GameLinkSetting.Real)
            {
                gameUrl = GameLink.Real.Replace("{TOKEN}", GameLink.MemberSessionId);
            }
            else
            {
                gameUrl = GameLink.Fun.Replace("{CURRENCY}", GetCurrencyByLanguage());

            }

            return gameUrl.Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{LOBBY}", GameLink.LobbyPage);
        }
    }
}