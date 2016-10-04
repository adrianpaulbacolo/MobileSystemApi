﻿using Helpers;
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
        private string memberSessionId;

        public GPIHandler(string token)
            : base(GameProvider.GPI)
        {
            fun = GameSettings.GetGameUrl(GameProvider.GPI, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.GPI, GameLinkSetting.Real);

            GameProvider = GameProvider.GPI;
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
            string lang = GetGpiGameLanguage(element);
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";
            string slotType = base.IsRslot(element) ? mrSlot : mSlot;
            return fun.Replace("{TYPE}", slotType).Replace("{GAME}", gameName).Replace("{LANG}", lang);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string lang = GetGpiGameLanguage(element);
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";
            string slotType = base.IsRslot(element) ? mrSlot : mSlot;
            return real.Replace("{TYPE}", slotType).Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{TOKEN}", memberSessionId);
        }


    }
}