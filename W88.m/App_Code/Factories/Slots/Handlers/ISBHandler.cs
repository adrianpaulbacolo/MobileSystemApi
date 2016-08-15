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
        private string fun;
        private string real;
        private string lobbyPage;
        private string currencyCode;

        private string memberSessionId;

        public ISBHandler(string token, string lobby, string currency)
            : base(GameProvider.ISB)
        {
            fun = GameSettings.GetGameUrl(GameProvider.ISB, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.ISB, GameLinkSetting.Real);

            memberSessionId = token;
            lobbyPage = lobby;
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
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            string currency = string.IsNullOrWhiteSpace(this.currencyCode) || this.currencyCode.Equals("rmb", StringComparison.OrdinalIgnoreCase) ? "CNY" : this.currencyCode;

            return fun.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{CURRENCY}", currency).Replace("{LOBBY}", lobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            return real.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{TOKEN}", memberSessionId).Replace("{LOBBY}", lobbyPage);
        }
    }
}