using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Games.Factories;
using W88.BusinessLogic.Games.Handlers;
using W88.BusinessLogic.Games.Models;

namespace Factories.Slots.Handlers
{
    /// <summary>
    /// This is the handler for Pragmatic Play Slots game provider
    /// </summary>
    public class PPHandler : GameLoaderBase
    {
        public PPHandler(UserSessionInfo userInfo, string lobby, string cashier) : base(GameProvider.PP, userInfo.LanguageCode)
        {
             GameLink = new GameLinkInfo
            {
                Fun = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Fun),
                Real = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Real),
                MemberSessionId = userInfo.Token,
                LobbyPage = lobby,
                CashierPage = cashier
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

        private string GetGameLanguage(XElement element)
        {
            if (element.Attribute("LanguageCode") == null) return "en";

            var lang = LanguageCode.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            string[] languagesCodes = element.Attribute("LanguageCode").Value.Split(',');
            bool isLangSupp = languagesCodes.Contains(lang[0], StringComparer.OrdinalIgnoreCase);
            return isLangSupp ? lang[0] : "en";
        }

        private string BuildUrl(XElement element, GameLinkSetting setting)
        {
            var lang = GetGameLanguage(element);
            var gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";
            var gameUrl = setting == GameLinkSetting.Real ? GameLink.Real.Replace("{TOKEN}", GameLink.MemberSessionId) : GameLink.Fun;
            return gameUrl.Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{LOBBY}", GameLink.LobbyPage).Replace("{CASHIER}", GameLink.CashierPage);
        }

    }
}