using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Factories.Slots.Handlers
{
    /// <summary>
    /// This is the handler for Gameplay Interactive (GPI)
    /// </summary>
    public class GPIHandler : GameLoaderBase
    {
        private string fun;
        private string real;
        private string rSlotFun;
        private string rSlotReal;

        private string memberSessionId;

        public GPIHandler(string token)
            : base(GameProvider.GPI)
        {
            fun = GameSettings.GPIFun;
            real = GameSettings.GPIReal;
            rSlotFun = GameSettings.GPIRSlotFun;
            rSlotReal = GameSettings.GPIRSlotReal;

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
                url = rSlotReal.Replace("{GAME}", gameName).Replace("{LANG}", langCode).Replace("{TOKEN}", memberSessionId);
            }
            else
            {
                url = real.Replace("{GAME}", gameName).Replace("{LANG}", langCode).Replace("{TOKEN}", memberSessionId);
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
                url = rSlotFun.Replace("{GAME}", gameName).Replace("{LANG}", langCode).Replace("{TOKEN}", memberSessionId);
            }
            else
            {
                url = fun.Replace("{GAME}", gameName).Replace("{LANG}", langCode).Replace("{TOKEN}", memberSessionId);
            }

            return url;
        }
    }
}