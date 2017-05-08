using System;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Factories.Slots;
using Models;

namespace Helpers.GameProviders
{
    /// <summary>
    /// Summary description for GPI
    /// </summary>
    public sealed class Gpi
    {
        public string mrSlot = "mrslots";
        public string mSlot = "mslots";
        public string LanguageCode;
        private GameLinkInfo _gameLink;

        public Gpi(GameLinkInfo gameLink)
        {
            LanguageCode = GetLanguageCode();
            _gameLink = gameLink;
        }

        public string GetLanguageCode()
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

        private string GetGameLanguage(XElement element)
        {
            if (element.Attribute("LanguageCode") != null)
            {
                string[] languagesCodes = element.Attribute("LanguageCode").Value.Split(',');

                bool isLangSupp = languagesCodes.Contains(LanguageCode, StringComparer.OrdinalIgnoreCase);

                return isLangSupp ? LanguageCode : "en";
            }
            else
            {
                return LanguageCode.Equals("cn", StringComparison.OrdinalIgnoreCase) ? "zn" : "en";
            }
        }

        private bool IsRslot(XElement element)
        {
            return element.Attribute("Type") != null && element.Attribute("Type").Value.Equals("rslot", StringComparison.OrdinalIgnoreCase);
        }

        public string CheckRSlot(GameLinkSetting setting, XElement element)
        {
            if (IsRslot(element))
            {
                string url = "";
                if (setting == GameLinkSetting.Fun)
                {
                    var funUrl = GameSettings.GetGameUrl(GameProvider.GPI, GameLinkSetting.Fun);
                    return BuildUrl(funUrl, element, GameLinkSetting.Fun);
                }

                var realUrl = GameSettings.GetGameUrl(GameProvider.GPI, GameLinkSetting.Real);
                return BuildUrl(realUrl, element, GameLinkSetting.Real);
            }

            return string.Empty;
        }

        public string BuildUrl(string url, XElement element, GameLinkSetting setting)
        {
            var lang = GetGameLanguage(element);
            var gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";
            var slotType = IsRslot(element) ? mrSlot : mSlot;
            var domainLauncher = LanguageCode.ToLower() == "cn"
                ? ConfigurationManager.AppSettings.Get("GPIGameLauncherCN")
                : ConfigurationManager.AppSettings.Get("GPIGameLauncherDefault");

            if (setting == GameLinkSetting.Real)
                url = url.Replace("{TOKEN}", _gameLink.MemberSessionId);

            return url.Replace("{TYPE}", slotType).Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{LAUNCHER}", domainLauncher);
        }
    }
}