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
    /// This is the handler for ISoftBet Simply Play (ISB)
    /// Lobby = Club Landing Page
    /// Currency = Currency Code
    /// </summary>
    public class ISBHandler : GameLoaderBase
    {
        private string currencyCode;

        public ISBHandler(UserSessionInfo user, string lobby, string currency) : base(GameProvider.ISB, user.LanguageCode)
        {
            currencyCode = currency;
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
            string languageCode;
            switch (base.LanguageCode)
            {
                case "zh-cn":
                    languageCode = "chs";
                    break;

                case "th-th":
                    languageCode = "th";
                    break;

                case "ko-kr":
                    languageCode = "kr";
                    break;

                case "vi-vn":
                    languageCode = "vi";
                    break;

                case "ja-jp":
                    languageCode = "ja";
                    break;

                default:
                    languageCode = "en";
                    break;
            }

            return languageCode;
        }

        protected override string CreateFunUrl(XElement element)
        {
            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            string currency = string.IsNullOrWhiteSpace(this.currencyCode) || this.currencyCode.Equals("rmb", StringComparison.OrdinalIgnoreCase) ? "CNY" : this.currencyCode;

            return GameLink.Fun.Replace("{GAME}", gameName).Replace("{LANG}", base.LanguageCode).Replace("{CURRENCY}", currency).Replace("{LOBBY}", GameLink.LobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            return GameLink.Real.Replace("{GAME}", gameName).Replace("{LANG}", base.LanguageCode).Replace("{TOKEN}", GameLink.MemberSessionId).Replace("{LOBBY}", GameLink.LobbyPage);
        }
    }
}