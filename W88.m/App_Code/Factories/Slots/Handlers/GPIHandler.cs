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
            switch (commonVariables.SelectedLanguage)
            {
                case "id-id":
                    return "id";
                case "ja-jp":
                    return "jp";
                case "km-kh":
                    return "kh";
                case "ko-kr":
                    return "kr";
                case "th-th":
                    return "th";
                case "vi-vn":
                    return "vn";
                case "zh-cn":
                    return "cn";
                default:
                    return "en";
            }
        }

        protected override string CreateFunUrl(XElement element)
        {
            string lang = GetGameLanguage(element);

            bool isRSlot = element.Attribute("Type") != null && element.Attribute("Type").Value.Equals("rslot", StringComparison.OrdinalIgnoreCase) ? true : false;

            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            string slotType = isRSlot ? mrSlot : mSlot;

            return fun.Replace("{TYPE}", slotType).Replace("{GAME}", gameName).Replace("{LANG}", lang);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string lang = GetGameLanguage(element);

            bool isRSlot = element.Attribute("Type") != null && element.Attribute("Type").Value.Equals("rslot", StringComparison.OrdinalIgnoreCase) ? true : false;

            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            string slotType = isRSlot ? mrSlot : mSlot;

            return real.Replace("{TYPE}", slotType).Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{TOKEN}", memberSessionId);
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
                return langCode.Equals("cn", StringComparison.OrdinalIgnoreCase) ? "zn" : "en";
            }
        }
    }
}