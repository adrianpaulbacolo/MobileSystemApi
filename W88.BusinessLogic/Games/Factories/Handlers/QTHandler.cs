using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using W88.BusinessLogic.Games.Handlers;
using W88.BusinessLogic.Shared.Helpers;

namespace W88.BusinessLogic.Games.Factories.Handlers
{
    /// <summary>
    /// This is the handler for GPI-QTech (QT)
    /// Lobby = Club Landing Page
    /// </summary>
    public class QTHandler : GameLoaderBase
    {
        private string fun;
        private string real;
        private string lobbyPage;

        private string memberSessionId;

        public QTHandler(string token, string lobby)
            : base(GameProvider.QT)
        {
            fun = GameSettings.GetGameUrl(GameProvider.QT, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.QT, GameLinkSetting.Real);

            memberSessionId = token;
            lobbyPage = lobby;
        }

        protected override string SetLanguageCode()
        {
            var lang = LanguageHelpers.SelectedLanguage.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            var splitLang = string.Format("{0}_{1}", lang[0], lang[1].ToUpper());

            string supportedLang = null;
            switch (splitLang)
            {
                case "ko_KR":
                    supportedLang = "en_US";
                    break;
            }

            return supportedLang ?? splitLang;
        }

        protected override string CreateFunUrl(XElement element)
        {
            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            return fun.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{CURRENCY}", GetCurrencyByLanguage()).Replace("{LOBBY}", lobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            return real.Replace("{GAME}", gameName).Replace("{LANG}", base.langCode).Replace("{TOKEN}", memberSessionId).Replace("{LOBBY}", lobbyPage);
        }

        private string GetCurrencyByLanguage()
        {
            string currency;
            switch (LanguageHelpers.SelectedLanguage)
            {
                case "zh-cn":
                    currency = "CNY";
                    break;
                //case "ko-kr": //temporary commented these lines as it is not yet supported.
                //    currency = "KRW";
                //    break;
                case "ja-jp":
                    currency = "JPY";
                    break;
                default:
                    currency = "USD";
                    break;
            }

            if (HttpContext.Current.Session["LanguageCode"] != null && HttpContext.Current.Session["CurrencyCode"] != null)
            {
                if ((string)HttpContext.Current.Session["LanguageCode"] == "en-us" && ((string)HttpContext.Current.Session["CurrencyCode"] == "MY"))
                {
                    currency = "MYR";
                }
            }

            return currency;
        }
    }
}