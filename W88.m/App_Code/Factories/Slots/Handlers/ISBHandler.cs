using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Factories.Slots.Handlers
{
    /// <summary>
    /// This is the handler for ISoftBet Simply Play (ISB)
    /// Lobby = Club Landing Page
    /// Currency = Currency Code
    /// </summary>
    public class ISBHandler : GameLoaderBase
    {
        private string currencyCode;

        public ISBHandler(string token, string lobby, string currency)
            : base(GameProvider.ISB)
        {
            Fun = GameSettings.GetGameUrl(GameProvider.ISB, GameLinkSetting.Fun);
            Real = GameSettings.GetGameUrl(GameProvider.ISB, GameLinkSetting.Real);

            GameProvider = GameProvider.ISB;
            MemberSessionId = token;
            LobbyPage = lobby;
            currencyCode = currency;
        }

        protected override string SetLanguageCode()
        {
            string languageCode;
            switch (commonVariables.SelectedLanguage)
            {
                case "zh-cn":
                    languageCode = "chs";
                    break;

                case "id-id":
                    languageCode = "id";
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
            var gpi = CheckRSlot(GameLinkSetting.Fun, element);
            if (!string.IsNullOrWhiteSpace(gpi))
            {
                return gpi;
            }

            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";
            string currency = string.IsNullOrWhiteSpace(this.currencyCode) || this.currencyCode.Equals("rmb", StringComparison.OrdinalIgnoreCase) ? "CNY" : this.currencyCode;
            return Fun.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{CURRENCY}", currency).Replace("{LOBBY}", LobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            var gpi = CheckRSlot(GameLinkSetting.Real, element);
            if (!string.IsNullOrWhiteSpace(gpi))
            {
                return gpi;
            }

            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";
            return Real.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{TOKEN}", MemberSessionId).Replace("{LOBBY}", LobbyPage);
        }
    }
}