using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Factories.Slots.Handlers
{
    /// <summary>
    /// This is the handler for Crescendo Bet8 (CTXM)
    /// </summary>
    public class CTXMHandler : GameLoaderBase
    {
        private string fun;
        private string real;

        private string memberSessionId;

        public CTXMHandler(string token)
            : base(GameProvider.CTXM)
        {
            fun = GameSettings.GetGameUrl(GameProvider.CTXM, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.CTXM, GameLinkSetting.Real);

            GameProvider = GameProvider.CTXM;
            memberSessionId = token;
        }

        protected override string CreateFunUrl(XElement element)
        {
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            string url = "";
            string funUrl = IsElementExists("Fun", element, out url) ? url : fun;

            string lang = SetSpecialUrlLanguageCode();
            return funUrl.Replace("{GAME}", gameName).Replace("{DOMAIN}", commonIp.DomainName).Replace("{LANG}", lang);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            string url = "";
            string realUrl = IsElementExists("Real", element, out url) ? url : real;

            string lang = SetSpecialUrlLanguageCode();
            return realUrl.Replace("{GAME}", gameName).Replace("{DOMAIN}", commonIp.DomainName).Replace("{LANG}", lang).Replace("{TOKEN}", memberSessionId);
        }

        private string SetSpecialUrlLanguageCode()
        {
            return commonVariables.SelectedLanguage.Equals("zh-cn", StringComparison.OrdinalIgnoreCase) ? "zh" : "en";
        }
    }
}