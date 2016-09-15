using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Factories.Slots.Handlers
{
    /// <summary>
    /// This is the handler for Play'n Go (PNG)
    /// Lobby = Club Landing Page
    /// </summary>
    public class PNGHandler : GameLoaderBase
    {
        private string fun;
        private string real;
        private string lobbyPage;

        private string memberSessionId;

        public PNGHandler(string token, string lobby)
            : base(GameProvider.PNG)
        {
            fun = GameSettings.GetGameUrl(GameProvider.PNG, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.PNG, GameLinkSetting.Real);

            GameProvider = GameProvider.PNG;
            memberSessionId = token;
            lobbyPage = lobby;
        }

        protected override string SetLanguageCode()
        {
            return commonVariables.SelectedLanguage.Equals("zh-cn", StringComparison.OrdinalIgnoreCase) ? commonVariables.SelectedLanguage : "en-gb";
        }

        protected override string CreateFunUrl(XElement element)
        {
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            return fun.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{LOBBY}", lobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            return real.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{TOKEN}", memberSessionId).Replace("{LOBBY}", lobbyPage);
        }
    }
}