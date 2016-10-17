using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Helpers.GameProviders;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Games.Handlers;
using W88.BusinessLogic.Games.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.Utilities.Geo;

namespace W88.BusinessLogic.Games.Factories.Handlers
{
    /// <summary>
    /// This is the handler for Microgaming (MGS)
    /// Lobby = Club Landing Page
    /// Cashier = Fund Transfer Page
    /// </summary>
    public class MGSHandler : GameLoaderBase
    {
        public MGSHandler(UserSessionInfo user, string lobby, string cashier) : base(GameProvider.MGS, user.LanguageCode)
        {
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
            string languageCode;
            switch (base.LanguageCode)
            {
                case "id-id":
                    languageCode = "id";
                    break;

                case "ja-jp":
                    languageCode = "ja";
                    break;

                case "ko-kr":
                    languageCode = "ko";
                    break;

                case "th-th":
                    languageCode = "th";
                    break;

                case "vi-vn":
                    languageCode = "vi";
                    break;

                case "zh-cn":
                    languageCode = "zh-cn";
                    break;

                default:
                    languageCode = "en";
                    break;
            }

            return languageCode;
        }

        protected override string CreateFunUrl(XElement element)
        {
            return BuildUrl(element, GameLinkSetting.Fun);
        }

        protected override string CreateRealUrl(XElement element)
        {
            return BuildUrl(element, GameLinkSetting.Real);
        }

        private string GetGameLanguage(XElement element)
        {
            if (element.Attribute("LanguageCode") != null)
            {
                string[] languagesCodes = element.Attribute("LanguageCode").Value.Split(',');

                bool isLangSupp = languagesCodes.Contains(LanguageCode, StringComparer.OrdinalIgnoreCase);

                return isLangSupp ? LanguageCode : "en";
            }
            else
            {
                return "en";
            }
        }

        private string SetSpecialUrlLanguageCode()
        {
            return LanguageCode.Equals("zh-cn", StringComparison.OrdinalIgnoreCase) ? "zh" : "en";
        }

        private string BuildUrl(XElement element, GameLinkSetting setting)
        {
            var gpi = new Gpi(GameLink, LanguageCode).CheckRSlot(setting, element);
            if (!string.IsNullOrWhiteSpace(gpi))
            {
                return gpi;
            }

            string url;
            string gameUrl;
            var lang = GetGameLanguage(element);
            var gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            if (setting == GameLinkSetting.Real)
            {
                if (IsElementExists(GameLinkSetting.Real.ToString(), element, out url))
                {
                    lang = SetSpecialUrlLanguageCode();
                    gameUrl = url;
                }
                else
                {
                    gameUrl =  GameLink.Real;
                }

                gameUrl = gameUrl.Replace("{TOKEN}", GameLink.MemberSessionId);
            }
            else
            {
                if (IsElementExists(GameLinkSetting.Fun.ToString(), element, out url))
                {
                    lang = SetSpecialUrlLanguageCode();
                    gameUrl = url;
                }
                else
                {
                    gameUrl = GameLink.Fun;    
                }
            }

            return gameUrl.Replace("{DOMAIN}", new IpHelper().DomainName).Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{LOBBY}", GameLink.LobbyPage).Replace("{CASHIER}", GameLink.CashierPage);
        }
    }
}