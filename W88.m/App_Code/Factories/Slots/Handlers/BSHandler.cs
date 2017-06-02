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
    /// This is the handler for Betsoft Gaming (BS)
    /// Lobby = Club Landing Page
    /// Cashier = Fund Transfer Page
    /// </summary>
    public class BSHandler : GameLoaderBase
    {
        private GameDevice device;

        public BSHandler(string token, string lobby, string cashier, GameDevice gameDevice) : base(GameProvider.BS)
        {
            GameProvider = GameProvider.BS;
            device = gameDevice;

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
            switch (commonVariables.SelectedLanguage)
            {
                case "ja-jp":
                    return "jp";
                case "ko-kr":
                    return "ko";
                case "th-th":
                    return "th";
                case "vi-vn":
                    return "vi";
                case "zh-cn":
                    return "zh-cn";
                default:
                    return "en";
            }
        }

        protected override string CreateFunUrl(XElement element)
        {
            var gpi = new Gpi(GameLink).CheckRSlot(GameLinkSetting.Fun, element);
            if (!string.IsNullOrWhiteSpace(gpi))
            {
                return gpi;
            }

            string gameName = GetGameId(element);
            return GameLink.Fun.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{LOBBY}", GameLink.LobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            var gpi = new Gpi(GameLink).CheckRSlot(GameLinkSetting.Real, element);
            if (!string.IsNullOrWhiteSpace(gpi))
            {
                return gpi;
            }
           
            string gameName = GetGameId(element);
            return GameLink.Real.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{TOKEN}", GameLink.MemberSessionId).Replace("{CASHIER}", GameLink.CashierPage).Replace("{LOBBY}", GameLink.LobbyPage);
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