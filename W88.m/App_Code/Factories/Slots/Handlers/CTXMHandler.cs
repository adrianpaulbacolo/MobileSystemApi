using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Models;

namespace Factories.Slots.Handlers
{
    /// <summary>
    /// This is the handler for Crescendo Bet8 (CTXM)
    /// </summary>
    public class CTXMHandler : GameLoaderBase
    {

        public CTXMHandler(string token) : base(GameProvider.CTXM)
        {
            GameProvider = GameProvider.CTXM;
            GameLink = new GameLinkInfo
            {
                Fun = GameSettings.GetGameUrl(GameProvider, GameLinkSetting.Fun),
                Real = GameSettings.GetGameUrl(GameProvider, GameLinkSetting.Real),
                MemberSessionId = token
            };

        }

        protected override string CreateFunUrl(XElement element)
        {
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            string url = "";
            string funUrl = IsElementExists("Fun", element, out url) ? url : GameLink.Fun;

            string lang = SetSpecialUrlLanguageCode();
            return funUrl.Replace("{GAME}", gameName).Replace("{DOMAIN}", commonIp.DomainName).Replace("{LANG}", lang);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            string url = "";
            string realUrl = IsElementExists("Real", element, out url) ? url : GameLink.Real;

            string lang = SetSpecialUrlLanguageCode();
            return realUrl.Replace("{GAME}", gameName).Replace("{DOMAIN}", commonIp.DomainName).Replace("{LANG}", lang).Replace("{TOKEN}", GameLink.MemberSessionId);
        }

        private string SetSpecialUrlLanguageCode()
        {
            switch (commonVariables.SelectedLanguage)
            {
                case "th-th":
                    return "th";

                case "zh-cn":
                    return "zh";

                default:
                    return "en";
            }
        }
    }
}