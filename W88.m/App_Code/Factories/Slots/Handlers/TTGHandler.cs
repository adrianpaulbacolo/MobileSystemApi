using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Models;

namespace Factories.Slots.Handlers
{
    /// <summary>
    /// This is the handler for UC8 Slots game provider (UC8)
    /// </summary>
    public class TTGHandler : GameLoaderBase
    {
        public TTGHandler(string token, string lobby, string cashier)
            : base(GameProvider.TTG)
        {
            GameProvider = GameProvider.TTG;
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
                    return "ja";

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
            var id = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";
            var gameType = element.Attribute("type") != null ? element.Attribute("type").Value : "";
            var gameName = element.Attribute("name") != null ? element.Attribute("name").Value : "";

            return
                GameLink.Fun.Replace("{GAME}", id)
                    .Replace("{LANG}", langCode)
                    .Replace("{LOBBY}", GameLink.LobbyPage)
                    .Replace("{GAMETYPE}", gameType)
                    .Replace("{GAMENAME}", gameName);
        }

        protected override string CreateRealUrl(XElement element)
        {
            var id = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";
            var gameType = element.Attribute("type") != null ? element.Attribute("type").Value : "";
            var gameName = element.Attribute("name") != null ? element.Attribute("name").Value : "";

            return
                GameLink.Real.Replace("{GAME}", id)
                    .Replace("{TOKEN}", GameLink.MemberSessionId)
                    .Replace("{LANG}", base.langCode)
                    .Replace("{LOBBY}", GameLink.LobbyPage)
                    .Replace("{CASHIER}", GameLink.CashierPage)
                    .Replace("{GAMETYPE}", gameType)
                    .Replace("{GAMENAME}", gameName);
        }
    }
}