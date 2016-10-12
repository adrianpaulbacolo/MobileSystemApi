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
    /// This is the handler for Playtech (PT)
    /// Encrypted Token = Encrypted User Password
    /// Username = Username
    /// Lobby = Club Landing Page
    /// Support = Live Chat Page
    /// Logout = Logout Page
    /// </summary>
    public class PTHandler : GameLoaderBase
    {
        public string languageCode;
        public string user;
        private string prefix;
        private string supportPage;
        private string logoutPage;

        public PTHandler(UserSessionInfo userInfo, string lobby, string support, string logout) : base(GameProvider.PT, userInfo.LanguageCode)
        {
            prefix = GameSettings.PTAcctPrefix;

            user = prefix + userInfo.MemberCode.ToUpper();
            supportPage = support;
            logoutPage = logout;

            GameLink = new GameLinkInfo
            {
                Fun = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Fun),
                Real = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Real),
                MemberSessionId = userInfo.Token,
                LobbyPage = lobby
            };
        }

        protected override string SetLanguageCode()
        {
            switch (LanguageCode)
            {
                case "ko-kr":
                    languageCode = "ko";
                    break;
                case "th-th":
                    languageCode = "th";
                    break;
                case "zh-cn":
                    languageCode = "zh-cn";
                    break;
                default:
                    languageCode = "en";
                    break;
            }

            return languageCode;
        }

        protected override string CreateFunUrl(XElement element)
        {
            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            return GameLink.Fun.Replace("{GAME}", gameName).Replace("{LANG}", base.LanguageCode);
        }

        protected override string CreateRealUrl(XElement element)
        {
            bool isNGM = CultureHelpers.ElementValues.GetResourceXPathAttribute("Type", element).Equals("ngm", StringComparison.OrdinalIgnoreCase) ? true : false;

            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            return isNGM ? GameLink.Real.Replace("{GAME}", gameName).Replace("{LANG}", base.LanguageCode).Replace("{USER}", user)
                .Replace("{LOBBY}", GameLink.LobbyPage).Replace("{SUPPORT}", supportPage).Replace("{LOGOUT}", logoutPage) : "";
        }
    }
}