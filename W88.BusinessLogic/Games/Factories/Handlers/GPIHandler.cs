using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using Helpers.GameProviders;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Games.Handlers;
using W88.BusinessLogic.Games.Models;
using W88.BusinessLogic.Shared.Helpers;

namespace W88.BusinessLogic.Games.Factories.Handlers
{
    /// <summary>
    /// This is the handler for Gameplay Interactive (GPI)
    /// </summary>
    public class GPIHandler : GameLoaderBase
    {
        Gpi _gpi;

        public GPIHandler(UserSessionInfo user): base(GameProvider.GPI, user.LanguageCode)
        {
            GameLink = new GameLinkInfo
            {
                Fun = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Fun),
                Real = GameSettings.GetGameUrl(gameProvider, GameLinkSetting.Real),
                MemberSessionId = user.Token
            };

            _gpi = new Gpi(GameLink, user.LanguageCode);
        }

        protected override string SetLanguageCode()
        {
            return new Gpi(GameLink, LanguageCode).GetLanguageCode();
        }

        protected override string CreateFunUrl(XElement element)
        {
            return _gpi.BuildUrl(GameLink.Fun, element, GameLinkSetting.Fun);
        }

        protected override string CreateRealUrl(XElement element)
        {
            return _gpi.BuildUrl(GameLink.Real, element, GameLinkSetting.Real);
        }

    }
}