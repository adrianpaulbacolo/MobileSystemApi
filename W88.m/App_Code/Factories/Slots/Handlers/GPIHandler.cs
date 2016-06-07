using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Factories.Slots.Handlers
{
    public class GPIHandler : GameLoaderBase
    {
        private string realUrl;
        private string funUrl;
        private string realUrl_rslot;
        private string funUrl_rslot;

        private string memberSessionId;

        public GPIHandler(string token)
            : base(GameProvider.GPI)
        {
            realUrl = commonClubBravado.getRealUrl;
            funUrl = commonClubBravado.getFunUrl;
            realUrl_rslot = commonClubBravado.getRealUrl_mrslot;
            funUrl_rslot = commonClubBravado.getFunUrl_mrslot;

            memberSessionId = token;
        }

        protected override string SetLanguageCode()
        {
            string languageCode;
            switch (commonVariables.SelectedLanguage)
            {
                case "zh-cn":
                    languageCode = "zh";
                    break;

                default:
                    languageCode = "en";
                    break;
            }

            return languageCode;
        }

        protected override string CreateRealUrl(XElement element)
        {
            bool isRSlot = element.Attribute("Type") != null && element.Attribute("Type").Value.Equals("rslot", StringComparison.OrdinalIgnoreCase) ? true : false;

            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";
            string url;
            if (isRSlot)
            {
                url = realUrl_rslot.Replace("{GAME}", gameName).Replace("{LANG}", langCode).Replace("{TOKEN}", memberSessionId);
            }
            else
            {
                url = realUrl.Replace("{GAME}", gameName).Replace("{LANG}", langCode).Replace("{TOKEN}", memberSessionId);
            }

            return url;
        }

        protected override string CreateFunUrl(XElement element)
        {
            bool isRSlot = element.Attribute("Type") != null && element.Attribute("Type").Value.Equals("rslot", StringComparison.OrdinalIgnoreCase) ? true : false;

            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";
            string url;
            if (isRSlot)
            {
                url = funUrl_rslot.Replace("{GAME}", gameName).Replace("{LANG}", langCode).Replace("{TOKEN}", memberSessionId);
            }
            else
            {
                url = funUrl.Replace("{GAME}", gameName).Replace("{LANG}", langCode).Replace("{TOKEN}", memberSessionId);
            }

            return url;
        }
    }
}