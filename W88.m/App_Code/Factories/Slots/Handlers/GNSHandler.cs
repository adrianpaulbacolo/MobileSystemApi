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
    /// This is the handler for Genesis (GNS)
    /// Lobby = Club Landing Page
    /// Cashier = Fund Transfer Page
    /// </summary>
    public class GNSHandler : GameLoaderBase
    {
        public GNSHandler(string token, string lobby, string cashier) : base(GameProvider.GNS)
        {
            GameProvider = GameProvider.GNS;

            GameLink = new GameLinkInfo
            {
                Fun = GameSettings.GetGameUrl(GameProvider, GameLinkSetting.Fun),
                Real = GameSettings.GetGameUrl(GameProvider, GameLinkSetting.Real),
                MemberSessionId = token,
                LobbyPage = lobby,
                CashierPage = cashier
            };
        }

        protected override string SetLanguageCode()
        {
            return commonVariables.SelectedLanguage.Equals("zh-cn", StringComparison.OrdinalIgnoreCase) ? "zh-hans" : "en-us";
        }

        protected override string CreateFunUrl(XElement element)
        {
            var gpi = new Gpi(GameLink).CheckRSlot(GameLinkSetting.Fun, element);
            if (!string.IsNullOrWhiteSpace(gpi))
            {
                return gpi;
            }

            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            string url = "";
            string funUrl = IsElementExists("Fun", element, out url) ? url : GameLink.Fun;

            return funUrl.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{LOBBY}", GameLink.LobbyPage).Replace("{CASHIER}", GameLink.CashierPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            var gpi = new Gpi(GameLink).CheckRSlot(GameLinkSetting.Real, element);
            if (!string.IsNullOrWhiteSpace(gpi))
            {
                return gpi;
            }

            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            string url = "";
            string realUrl = IsElementExists("Real", element, out url) ? url : GameLink.Real;

            return realUrl.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{TOKEN}", GameLink.MemberSessionId).Replace("{CASHIER}", GameLink.CashierPage).Replace("{LOBBY}", GameLink.LobbyPage);
        }
    }
}