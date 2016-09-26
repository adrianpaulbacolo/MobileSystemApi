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
    /// This is the handler for Play'n Go (PNG)
    /// Lobby = Club Landing Page
    /// </summary>
    public class PNGHandler : GameLoaderBase
    {
        private string fun;
        private string real;
        private string lobbyPage;

        private string memberSessionId;

        public PNGHandler(UserSessionInfo user, string lobby)
            : base(GameProvider.PNG)
        {
            fun = GameSettings.GetGameUrl(GameProvider.PNG, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.PNG, GameLinkSetting.Real);

            memberSessionId = user.Token;
            lobbyPage = lobby;
        }

        protected override string SetLanguageCode()
        {
            return LanguageHelpers.SelectedLanguage.Equals("zh-cn", StringComparison.OrdinalIgnoreCase) ? LanguageHelpers.SelectedLanguage : "en-gb";
        }

        protected override string CreateFunUrl(XElement element)
        {
            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            return fun.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{LOBBY}", lobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            return real.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{TOKEN}", memberSessionId).Replace("{LOBBY}", lobbyPage);
        }
    }
}