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
    /// This is the handler for Slotty Slots game provider (MRS)
    /// </summary>
    public class MRSHandler : GameLoaderBase
    {
        public MRSHandler(string token, string lobby, string cashier)
            : base(GameProvider.MRS)
        {
            GameProvider = GameProvider.MRS;
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
            return commonVariables.SelectedLanguage.Equals("zh-cn", StringComparison.OrdinalIgnoreCase) ? "zh" : "en";
        }
    
        protected override string CreateFunUrl(XElement element)
        {
            var gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";
            return GameLink.Fun.Replace("{GAME}", gameName)
                .Replace("{LANG}", langCode)
                .Replace("{LOBBY}", GameLink.LobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            var gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            return
                GameLink.Real.Replace("{GAME}", gameName)
                    .Replace("{TOKEN}", GameLink.MemberSessionId)
                    .Replace("{LANG}", base.langCode)
                    .Replace("{LOBBY}", GameLink.LobbyPage)
                    .Replace("{CASHIER}", GameLink.CashierPage)
                    .Replace("{CURR}", GetCurrency());
        }
        
        private string GetCurrency()
        {
            string currency;
            switch (commonCookie.CookieCurrency)
            {
                case "RMB":
                    currency = "CNY";
                    break;
                default:
                    currency = commonCookie.CookieCurrency;
                    break;
            }
            return currency;
        }

    }
}