using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Games.Handlers;
using W88.BusinessLogic.Shared.Helpers;

namespace W88.BusinessLogic.Games.Factories.Handlers
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

        public GPIHandler(UserSessionInfo user): base(GameProvider.GPI, user.LanguageCode)
        {
            fun = GameSettings.GetGameUrl(GameProvider.GPI, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.GPI, GameLinkSetting.Real);

            memberSessionId = user.Token;
        }

        protected override string SetLanguageCode()
        {
            switch (LanguageCode)
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

            bool isRSlot = CultureHelpers.ElementValues.GetResourceXPathAttribute("Type", element).Equals("rslot", StringComparison.OrdinalIgnoreCase) ? true : false;

            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            string slotType = isRSlot ? mrSlot : mSlot;

            return fun.Replace("{TYPE}", slotType).Replace("{GAME}", gameName).Replace("{LANG}", lang);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string lang = GetGameLanguage(element);

            bool isRSlot = CultureHelpers.ElementValues.GetResourceXPathAttribute("Type", element).Equals("rslot", StringComparison.OrdinalIgnoreCase) ? true : false;

            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            string slotType = isRSlot ? mrSlot : mSlot;

            return real.Replace("{TYPE}", slotType).Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{TOKEN}", memberSessionId);
        }

        private string GetGameLanguage(XElement element)
        {
            if (!string.IsNullOrWhiteSpace(CultureHelpers.ElementValues.GetResourceXPathAttribute("LanguageCode", element)))
            {
                string[] languagesCodes = CultureHelpers.ElementValues.GetResourceXPathAttribute("LanguageCode", element).Split(',');

                bool isLangSupp = languagesCodes.Contains(LanguageCode, StringComparer.OrdinalIgnoreCase);

                return isLangSupp ? LanguageCode : "en";
            }
            else
            {
                return LanguageCode.Equals("cn", StringComparison.OrdinalIgnoreCase) ? "zn" : "en";
            }
        }
    }
}