using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Factories.Slots.Handlers
{
    /// <summary>
    /// This is the handler for Pragmatic Play Slots game provider
    /// </summary>
    public class PPHandler : GameLoaderBase
    {
        private string fun;
        private string real;
        private string lobbyPage;
        private string cashierPage;
        private string memberSessionId;

        public PPHandler(string token, string lobby, string cashier) : base(GameProvider.PP)
        {
            fun = GameSettings.GetGameUrl(GameProvider.PP, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.PP, GameLinkSetting.Real);

            GameProvider = GameProvider.PP;
            memberSessionId = token;
            cashierPage = cashier;
            lobbyPage = lobby;
        }

        protected override string CreateFunUrl(XElement element)
        {
            string lang = GetGameLanguage(element);
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            return fun.Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{LOBBY}", lobbyPage).Replace("{CASHIER}", cashierPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string lang = GetGameLanguage(element);
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            return real.Replace("{GAME}", gameName).Replace("{TOKEN}", memberSessionId).Replace("{LANG}", lang).Replace("{LOBBY}", lobbyPage).Replace("{CASHIER}", cashierPage);
        }

        private string GetGameLanguage(XElement element)
        {
            if (element.Attribute("LanguageCode") == null) return "en";

            var lang = langCode.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            string[] languagesCodes = element.Attribute("LanguageCode").Value.Split(',');
            bool isLangSupp = languagesCodes.Contains(langCode, StringComparer.OrdinalIgnoreCase);
            return isLangSupp ? lang[0] : "en";
        }
    }
}