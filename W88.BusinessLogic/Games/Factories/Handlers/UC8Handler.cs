using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using W88.BusinessLogic.Games.Handlers;
using W88.BusinessLogic.Shared.Helpers;

namespace W88.BusinessLogic.Games.Factories.Handlers
{
    /// <summary>
    /// This is the handler for UC8 Slots game provider (UC8)
    /// </summary>
    public class UC8Handler : GameLoaderBase
    {
        private string fun;
        private string real;
        private string lobbyPage;
        private string cashierPage;
        private string memberSessionId;

        public UC8Handler(string token, string lobby, string cashier) 
            : base(GameProvider.UC8)
        {
            fun = GameSettings.GetGameUrl(GameProvider.UC8, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.UC8, GameLinkSetting.Real);

            memberSessionId = token;
            cashierPage = cashier;
            lobbyPage = lobby;
        }

        protected override string SetLanguageCode()
        {
            switch (LanguageHelpers.SelectedLanguage)
            {
                case "id-id":
                    return "id_ID";
                case "ja-jp":
                    return "ja_JP";
                case "ko-kr":
                    return "ko_KR";
                case "th-th":
                    return "th_TH";
                case "zh-cn":
                    return "zh_CN";
                default:
                    return "en_US";
            }
        }

        protected override string CreateFunUrl(XElement element)
        {
            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            return fun.Replace("{GAME}", gameName).Replace("{LANG}", langCode).Replace("{LOBBY}", lobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            return real.Replace("{GAME}", gameName).Replace("{TOKEN}", memberSessionId).Replace("{LANG}", base.langCode).Replace("{LOBBY}", lobbyPage).Replace("{CASHIER}", cashierPage);
        }
    }
}