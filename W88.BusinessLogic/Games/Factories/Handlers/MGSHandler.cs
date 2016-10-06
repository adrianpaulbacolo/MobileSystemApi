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
    /// This is the handler for Microgaming (MGS)
    /// Lobby = Club Landing Page
    /// Cashier = Fund Transfer Page
    /// </summary>
    public class MGSHandler : GameLoaderBase
    {
        private string fun;
        private string real;
        private string lobbyPage;
        private string cashierPage;

        private string memberSessionId;

        public MGSHandler(UserSessionInfo user, string lobby, string cashier)
            : base(GameProvider.MGS, user.LanguageCode)
        {
            fun = GameSettings.GetGameUrl(GameProvider.MGS, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.MGS, GameLinkSetting.Real);

            memberSessionId = user.Token;
            lobbyPage = lobby;
            cashierPage = cashier;
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
                funUrl = fun;
            }

            return funUrl.Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{LOBBY}", lobbyPage).Replace("{CASHIER}", cashierPage);
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
                realUrl = real;
            }

            return realUrl.Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{TOKEN}", memberSessionId).Replace("{CASHIER}", cashierPage).Replace("{LOBBY}", lobbyPage);
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