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
        private GameDevice device;

        public BSHandler(string token, string lobby, string cashier, GameDevice gameDevice)
            : base(GameProvider.BS)
        {
            Fun = GameSettings.GetGameUrl(GameProvider.BS, GameLinkSetting.Fun);
            Real = GameSettings.GetGameUrl(GameProvider.BS, GameLinkSetting.Real);

            GameProvider = GameProvider.BS;
            MemberSessionId = token;
            LobbyPage = lobby;
            CashierPage = cashier;
            device = gameDevice;
        }

        protected override string SetLanguageCode()
        {
            return commonVariables.SelectedLanguage.Equals("zh-cn", StringComparison.OrdinalIgnoreCase) ? "zh" : "en";
        }

        protected override string CreateFunUrl(XElement element)
        {
            var gpi = CheckRSlot(GameLinkSetting.Fun, element);
            if (!string.IsNullOrWhiteSpace(gpi))
            {
                return gpi;
            }

            string gameName = GetGameId(element);
            return Fun.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{LOBBY}", LobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            var gpi = CheckRSlot(GameLinkSetting.Real, element);
            if (!string.IsNullOrWhiteSpace(gpi))
            {
                return gpi;
            }
           
            string gameName = GetGameId(element);
            return Real.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{TOKEN}", MemberSessionId).Replace("{CASHIER}", CashierPage).Replace("{LOBBY}", LobbyPage);
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