using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using W88.BusinessLogic.Games.Handlers;
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

        private string fun;
        private string real;
        private string prefix;

        private string lobbyPage;
        private string supportPage;
        private string logoutPage;

        public PTHandler(string username, string lobby, string support, string logout)
            : base(GameProvider.PT)
        {
            fun = GameSettings.GetGameUrl(GameProvider.PT, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.PT, GameLinkSetting.Real);
            prefix = GameSettings.PTAcctPrefix;

            user = prefix + username.ToUpper();
            lobbyPage = lobby;
            supportPage = support;
            logoutPage = logout;
        }

        protected override string SetLanguageCode()
        {
            switch (LanguageHelpers.SelectedLanguage)
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

            return fun.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode);
        }

        protected override string CreateRealUrl(XElement element)
        {
            bool isNGM = CultureHelpers.ElementValues.GetResourceXPathAttribute("Type", element).Equals("ngm", StringComparison.OrdinalIgnoreCase) ? true : false;

            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            return isNGM ? real.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{USER}", user)
                .Replace("{LOBBY}", lobbyPage).Replace("{SUPPORT}", supportPage).Replace("{LOGOUT}", logoutPage) : "";
        }
    }
}