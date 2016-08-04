using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Factories.Slots.Handlers
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

        public BSHandler(string token, string lobby, string cashier, GameDevice gameDevice)
            : base(GameProvider.BS)
        {
            fun = GameSettings.BSFun;
            real = GameSettings.BSReal;

            memberSessionId = token;
            lobbyPage = lobby;
            cashierPage = cashier;
            device = gameDevice;
        }

        protected override string SetLanguageCode()
        {
            return commonVariables.SelectedLanguage.Equals("zh-cn", StringComparison.OrdinalIgnoreCase) ? "zh" : "en";
        }

        protected override string CreateFunUrl(XElement element)
        {
            string gameName = "";
            if (GameDevice.IOS == device)
                gameName = element.Attribute("IOSId") != null ? element.Attribute("IOSId").Value : "";

            if (GameDevice.ANDROID == device)
                gameName = element.Attribute("AndroidId") != null ? element.Attribute("AndroidId").Value : "";

            if (GameDevice.WP == device)
                gameName = element.Attribute("WPId") != null ? element.Attribute("WPId").Value : "";

            return fun.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{LOBBY}", lobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string gameName = "";
            if (GameDevice.IOS == device)
                gameName = element.Attribute("IOSId") != null ? element.Attribute("IOSId").Value : "";

            if (GameDevice.ANDROID == device)
                gameName = element.Attribute("AndroidId") != null ? element.Attribute("AndroidId").Value : "";

            if (GameDevice.WP == device)
                gameName = element.Attribute("WPId") != null ? element.Attribute("WPId").Value : "";

            return real.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{TOKEN}", memberSessionId).Replace("{CASHIER}", cashierPage).Replace("{LOBBY}", lobbyPage);
        }
    }
}