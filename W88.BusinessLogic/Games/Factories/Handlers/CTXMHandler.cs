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
    /// This is the handler for Crescendo Bet8 (CTXM)
    /// </summary>
    public class CTXMHandler : GameLoaderBase
    {
        private string fun;
        private string real;

        private string memberSessionId;

        public CTXMHandler(UserSessionInfo user) : base(GameProvider.CTXM, user.LanguageCode)
        {
            fun = GameSettings.GetGameUrl(GameProvider.CTXM, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.CTXM, GameLinkSetting.Real);

            memberSessionId = user.Token;
        }

        protected override string CreateFunUrl(XElement element)
        {
            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            return fun.Replace("{GAME}", gameName);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string gameName = CultureHelpers.ElementValues.GetResourceXPathAttribute("Id", element);

            return real.Replace("{GAME}", gameName).Replace("{TOKEN}", memberSessionId);
        }
    }
}