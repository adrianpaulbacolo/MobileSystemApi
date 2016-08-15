using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Factories.Slots.Handlers
{
    /// <summary>
    /// This is the handler for Crescendo Bet8 (CTXM)
    /// </summary>
    public class CTXMHandler : GameLoaderBase
    {
        private string fun;
        private string real;

        private string memberSessionId;

        public CTXMHandler(string token)
            : base(GameProvider.CTXM)
        {
            fun = GameSettings.GetGameUrl(GameProvider.CTXM, GameLinkSetting.Fun);
            real = GameSettings.GetGameUrl(GameProvider.CTXM, GameLinkSetting.Real);

            memberSessionId = token;
        }

        protected override string CreateFunUrl(XElement element)
        {
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            return fun.Replace("{GAME}", gameName);
        }

        protected override string CreateRealUrl(XElement element)
        {
            string gameName = element.Attribute("Id") != null ? element.Attribute("Id").Value : "";

            return real.Replace("{GAME}", gameName).Replace("{TOKEN}", memberSessionId);
        }
    }
}