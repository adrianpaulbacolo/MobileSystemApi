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
            string lang = GetGameLanguage(element);

            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            string funUrl = "";

            if (element.Element("Fun") != null)
            {
                funUrl = element.Element("Fun").Value;

                lang = SetSpecialUrlLanguageCode();
            }
            else
            {
                funUrl = GameLink.Fun;
            }

            return funUrl.Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{LOBBY}", GameLink.LobbyPage).Replace("{CASHIER}", GameLink.CashierPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string lang = GetGameLanguage(element);

            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            string realUrl = "";

            if (element.Element("Real") != null)
            {
                realUrl = element.Element("Real").Value;

                lang = SetSpecialUrlLanguageCode();
            }
            else
            {
                realUrl = GameLink.Real;
            }

            return realUrl.Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{TOKEN}", GameLink.MemberSessionId).Replace("{CASHIER}", GameLink.CashierPage).Replace("{LOBBY}", GameLink.LobbyPage);
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
    }
}