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
        private const string mrSlot = "mrslots";
        private const string mSlot = "mslots";

        private string memberSessionId;

        public GPIHandler(string token)
            : base(GameProvider.GPI)
        {
            fun = GameSettings.GPIFun;
            real = GameSettings.GPIReal;

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

        protected override string CreateFunUrl(XElement element)
        {
            bool isRSlot = element.Attribute("Type") != null && element.Attribute("Type").Value.Equals("rslot", StringComparison.OrdinalIgnoreCase) ? true : false;

            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            string slotType = isRSlot ? mrSlot : mSlot;

            return fun.Replace("{TYPE}", slotType).Replace("{GAME}", gameName).Replace("{LANG}", langCode);
        }

        protected override string CreateRealUrl(XElement element)
        {
            bool isRSlot = element.Attribute("Type") != null && element.Attribute("Type").Value.Equals("rslot", StringComparison.OrdinalIgnoreCase) ? true : false;

            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            string slotType = isRSlot ? mrSlot : mSlot;

            return real.Replace("{TYPE}", slotType).Replace("{GAME}", gameName).Replace("{LANG}", langCode).Replace("{TOKEN}", memberSessionId);
        }
    }
}