using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Helpers.GameProviders;
using Models;

namespace Factories.Slots.Handlers
{
    /// <summary>
    /// This is the handler for Playtech (PT)
    /// Encrypted Token = Encrypted User Password
    /// Username = Username
    /// Lobby = Club Landing Page
    /// Support = Live Chat Page
    /// Logout = Logout Page
    /// </summary>
    public class PTNHandler : GameLoaderBase
    {
        public string languageCode;
        public string user;

        private string prefix;
        private string supportPage;
        private string logoutPage;

        public PTNHandler(string username, string lobby, string support, string logout)
            : base(GameProvider.PTN)
        {
            prefix = GameSettings.PtAcctPrefix;
            GameProvider = GameProvider.PTN;
            
            user = prefix + username.ToUpper();
            supportPage = support;
            logoutPage = logout;

            GameLink = new GameLinkInfo
            {
                Fun = GameSettings.GetGameUrl(GameProvider, GameLinkSetting.Fun),
                Real = GameSettings.GetGameUrl(GameProvider, GameLinkSetting.Real),
                LobbyPage = lobby
            };
        }

        protected override string SetLanguageCode()
        {
            switch (commonVariables.SelectedLanguage)
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
            var gpi = new Gpi(GameLink).CheckRSlot(GameLinkSetting.Fun, element);
            if (!string.IsNullOrWhiteSpace(gpi))
            {
                return gpi;
            }

            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            return GameLink.Fun.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode);
        }

        protected override string CreateRealUrl(XElement element)
        {
            var gpi = new Gpi(GameLink).CheckRSlot(GameLinkSetting.Real, element);
            if (!string.IsNullOrWhiteSpace(gpi))
            {
                return gpi;
            }

            bool isNGM = element.Attribute("Type") != null && element.Attribute("Type").Value.Equals("ngm", StringComparison.OrdinalIgnoreCase) ? true : false;
           
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            return isNGM ? GameLink.Real.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{USER}", user)
                .Replace("{LOBBY}", GameLink.LobbyPage).Replace("{SUPPORT}", supportPage).Replace("{LOGOUT}", logoutPage) : "";
        }
    }
}