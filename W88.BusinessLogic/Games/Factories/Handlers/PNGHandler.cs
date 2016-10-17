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
    /// This is the handler for Play'n Go (PNG)
    /// Lobby = Club Landing Page
    /// </summary>
    public class PNGHandler : GameLoaderBase
    {
        public PNGHandler(UserSessionInfo user, string lobby) : base(GameProvider.PNG, user.LanguageCode)
        {
            GameLink = new GameLinkInfo
            {
                Fun = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Fun),
                Real = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Real),
                MemberSessionId = user.Token,
                LobbyPage = lobby
            };
        }

        protected override string SetLanguageCode()
        {
            return LanguageCode.Equals("zh-cn", StringComparison.OrdinalIgnoreCase) ? "zh_CN" : "en_GB";
        }

        protected override string CreateFunUrl(XElement element)
        {
            return BuildUrl(element, GameLinkSetting.Fun);
        }

        protected override string CreateRealUrl(XElement element)
        {
            return BuildUrl(element, GameLinkSetting.Real);
        }

        private string BuildUrl(XElement element, GameLinkSetting setting)
        {
            string url;
            var gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            if (setting == GameLinkSetting.Real)
            {
                url = GameLink.Real.Replace("{TOKEN}", GameLink.MemberSessionId);
            }
            else
            {
                url = GameLink.Fun;    
            }

            return url.Replace("{GAME}", gameName).Replace("{LANG}", base.LanguageCode).Replace("{LOBBY}", GameLink.LobbyPage);
        }
    }
}