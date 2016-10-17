using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Games.Handlers;
using W88.BusinessLogic.Games.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.Utilities.Geo;

namespace W88.BusinessLogic.Games.Factories.Handlers
{
    /// <summary>
    /// This is the handler for Crescendo Bet8 (CTXM)
    /// </summary>
    public class CTXMHandler : GameLoaderBase
    {
        public CTXMHandler(UserSessionInfo user) : base(GameProvider.CTXM, user.LanguageCode)
        {
            GameLink = new GameLinkInfo
            {
                Fun = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Fun),
                Real = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Real),
                MemberSessionId = user.Token,
            };
        }

        protected override string CreateFunUrl(XElement element)
        {
            return BuildUrl(element, GameLinkSetting.Fun);
        }

        protected override string CreateRealUrl(XElement element)
        {
            return BuildUrl(element, GameLinkSetting.Real);
        }

        private string BuildUrl(XElement element, GameLinkSetting setting)
        {

            var gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);
            var lang = SetSpecialUrlLanguageCode();
            string url;
            string gameUrl;

            if (setting == GameLinkSetting.Real)
            {
                var realUrl = IsElementExists(GameLinkSetting.Real.ToString(), element, out url) ? url : GameLink.Real;
                gameUrl = realUrl.Replace("{TOKEN}", GameLink.MemberSessionId);
            }
            else
            {
                gameUrl = IsElementExists(GameLinkSetting.Fun.ToString(), element, out url) ? url : GameLink.Fun;
            }

            return gameUrl.Replace("{GAME}", gameName).Replace("{LANG}", lang).Replace("{DOMAIN}", new IpHelper().DomainName);
        }

        private string SetSpecialUrlLanguageCode()
        {
            return LanguageCode.Equals("zh-cn", StringComparison.OrdinalIgnoreCase) ? "zh" : "en";
        }


    }
}