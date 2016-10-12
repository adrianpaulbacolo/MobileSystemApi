using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Xml.Linq;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Games.Handlers;
using W88.BusinessLogic.Games.Models;
using W88.BusinessLogic.Shared.Helpers;

namespace W88.BusinessLogic.Games.Factories.Handlers
{
    /// <summary>
    /// This is the handler for UC8 Slots game provider (UC8)
    /// </summary>
    public class UC8Handler : GameLoaderBase
    {
        public UC8Handler(UserSessionInfo user, string lobby, string cashier) : base(GameProvider.UC8, user.LanguageCode)
        {
            GameLink = new GameLinkInfo
            {
                Fun = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Fun),
                Real = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Real),
                MemberSessionId = user.Token,
                LobbyPage = lobby,
                CashierPage = cashier
            };
        }

        protected override string SetLanguageCode()
        {
            switch (LanguageCode)
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

            return GameLink.Fun.Replace("{GAME}", gameName).Replace("{LANG}", base.LanguageCode).Replace("{LOBBY}", GameLink.LobbyPage);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            return GameLink.Real.Replace("{GAME}", gameName).Replace("{TOKEN}", GameLink.MemberSessionId).Replace("{LANG}", base.LanguageCode).Replace("{LOBBY}", GameLink.LobbyPage).Replace("{CASHIER}", GameLink.CashierPage);
        }
    }
}