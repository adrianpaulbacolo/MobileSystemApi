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
    /// This is the handler for Betsoft Gaming (BS)
    /// Lobby = Club Landing Page
    /// Cashier = Fund Transfer Page
    /// </summary>
    public class BSHandler : GameLoaderBase
    {
        private GameDevice device;

        public BSHandler(UserSessionInfo user, string lobby, string cashier, GameDevice gameDevice) : base(GameProvider.BS, user.LanguageCode)
        {
            device = gameDevice;
            GameLink = new GameLinkInfo
            {
                Fun = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Fun),
                Real = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Real),
                MemberSessionId = user.Token,
                LobbyPage = lobby,
                CashierPage = cashier
            };
        }

        protected override string SetLanguageCode()
        {
            return LanguageCode.Equals("zh-cn", StringComparison.OrdinalIgnoreCase) ? "zh" : "en";
        }

        protected override string CreateFunUrl(XElement element)
        {
            return BuildUrl(GameLink.Fun, element, GameLinkSetting.Fun);
        }

        protected override string CreateRealUrl(XElement element)
        {
            return BuildUrl(GameLink.Real, element, GameLinkSetting.Real);
        }

        private string BuildUrl(string url, XElement element, GameLinkSetting setting)
        {
            string gameName = "";
            if (GameDevice.IOS == device)
                gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("IOSId", element);

            if (GameDevice.ANDROID == device)
                gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("AndroidId", element);

            if (GameDevice.WP == device)
                gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("WPId", element);

            if (setting == GameLinkSetting.Real)
                url = url.Replace("{TOKEN}", GameLink.MemberSessionId).Replace("{CASHIER}", GameLink.CashierPage);

            return url.Replace("{GAME}", gameName).Replace("{LANG}", base.LanguageCode).Replace("{LOBBY}", GameLink.LobbyPage);
       
        }
    }
}