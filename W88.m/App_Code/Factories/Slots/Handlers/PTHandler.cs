using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

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
            prefix = GameSettings.PtAcctPrefix;
            fun = GameSettings.GetGameUrl(GameProvider.PT, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.PT, GameLinkSetting.Real);


            user = prefix + username.ToUpper();
            lobbyPage = lobby;
            supportPage = support;
            logoutPage = logout;
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
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            return fun.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode);
        }

        protected override string CreateRealUrl(XElement element)
        {
            bool isNGM = element.Attribute("Type") != null && element.Attribute("Type").Value.Equals("ngm", StringComparison.OrdinalIgnoreCase) ? true : false;
           
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            return isNGM ? real.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{USER}", user)
                .Replace("{LOBBY}", lobbyPage).Replace("{SUPPORT}", supportPage).Replace("{LOGOUT}", logoutPage) : "";
        }
    }
}