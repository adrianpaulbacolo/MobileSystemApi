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
            fun = GameSettings.GetGameUrl(GameProvider.BS, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.BS, GameLinkSetting.Real);

            GameProvider = GameProvider.BS;
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
            gameName = GetGameId(element);

            return fun.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{LOBBY}", lobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string gameName = "";
            gameName = GetGameId(element);

            return real.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{TOKEN}", memberSessionId).Replace("{CASHIER}", cashierPage).Replace("{LOBBY}", lobbyPage);
        }
        protected override string GetGameId(XElement xeGame)
        {
            switch (device)
            {
                case GameDevice.ANDROID:
                    return xeGame.Attribute("AndroidId") != null ? xeGame.Attribute("AndroidId").Value : "";
                case GameDevice.IOS:
                    return xeGame.Attribute("IOSId") != null ? xeGame.Attribute("IOSId").Value : "";
                default:
                    return xeGame.Attribute("WPId") != null ? xeGame.Attribute("WPId").Value : "";
            }
        }
    }
}