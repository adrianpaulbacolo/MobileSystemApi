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
    /// This is the handler for Betsoft Gaming (BS)
    /// Lobby = Club Landing Page
    /// Cashier = Fund Transfer Page
    /// </summary>
    public class BSHandler : GameLoaderBase
    {
        private string fun;
        private string real;
        private string lobbyPage;
        private string cashierPage;
        private GameDevice device;

        private string memberSessionId;

        public BSHandler(UserSessionInfo user, string lobby, string cashier, GameDevice gameDevice)
            : base(GameProvider.BS, user.LanguageCode)
        {
            fun = GameSettings.GetGameUrl(GameProvider.BS, GameLinkSetting.Fun); 
            real = GameSettings.GetGameUrl(GameProvider.BS, GameLinkSetting.Real);

            memberSessionId = user.Token;
            lobbyPage = lobby;
            cashierPage = cashier;
            device = gameDevice;
        }

        protected override string SetLanguageCode()
        {
            return LanguageCode.Equals("zh-cn", StringComparison.OrdinalIgnoreCase) ? "zh" : "en";
        }

        protected override string CreateFunUrl(XElement element)
        {
            string gameName = "";
            if (GameDevice.IOS == device)
                gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("IOSId", element);

            if (GameDevice.ANDROID == device)
                gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("AndroidId", element);

            if (GameDevice.WP == device)
                gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("WPId", element);

            return fun.Replace("{GAME}", gameName).Replace("{LANG}", base.LanguageCode).Replace("{LOBBY}", lobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string gameName = "";
            if (GameDevice.IOS == device)
                gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("IOSId", element);

            if (GameDevice.ANDROID == device)
                gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("AndroidId", element);

            if (GameDevice.WP == device)
                gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("WPId", element);

            return real.Replace("{GAME}", gameName).Replace("{LANG}", base.LanguageCode).Replace("{TOKEN}", memberSessionId).Replace("{CASHIER}", cashierPage).Replace("{LOBBY}", lobbyPage);
        }
    }
}