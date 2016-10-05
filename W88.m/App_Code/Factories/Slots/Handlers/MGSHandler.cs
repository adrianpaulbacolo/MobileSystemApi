﻿using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
﻿using Helpers.GameProviders;
﻿using Models;

namespace Factories.Slots.Handlers
{
    /// <summary>
    /// This is the handler for Microgaming (MGS)
    /// Lobby = Club Landing Page
    /// Cashier = Fund Transfer Page
    /// </summary>
    public class MGSHandler : GameLoaderBase
    {
        public MGSHandler(string token, string lobby, string cashier) : base(GameProvider.MGS)
        {
            GameProvider = GameProvider.MGS;
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
            string languageCode;
            switch (commonVariables.SelectedLanguage)
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
            var gpi = new Gpi(GameLink).CheckRSlot(GameLinkSetting.Fun, element);
            if (!string.IsNullOrWhiteSpace(gpi))
            {
                return gpi;
            }

            string lang = GetGameLanguage(element);

            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

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
            var gpi = new Gpi(GameLink).CheckRSlot(GameLinkSetting.Real, element);
            if (!string.IsNullOrWhiteSpace(gpi))
            {
                return gpi;
            }

            string lang = GetGameLanguage(element);

            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

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

                bool isLangSupp = languagesCodes.Contains(langCode, StringComparer.OrdinalIgnoreCase);

                return isLangSupp ? langCode : "en";
            }
            else
            {
                return "en";
            }
        }

        private string SetSpecialUrlLanguageCode()
        {
            return commonVariables.SelectedLanguage.Equals("zh-cn", StringComparison.OrdinalIgnoreCase) ? "zh" : "en";
        }
    }
}