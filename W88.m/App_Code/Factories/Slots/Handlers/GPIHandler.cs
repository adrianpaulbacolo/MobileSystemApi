﻿using Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
﻿using Helpers.GameProviders;
﻿using Models;

namespace Factories.Slots.Handlers
{
    /// <summary>
    /// This is the handler for Gameplay Interactive (GPI)
    /// </summary>
    public class GPIHandler : GameLoaderBase
    {
        Gpi _gpi;

        public GPIHandler(string token) : base(GameProvider.GPI)
        {
            GameProvider = GameProvider.GPI;
            GameLink = new GameLinkInfo
            {
                Fun = GameSettings.GetGameUrl(GameProvider, GameLinkSetting.Fun),
                Real = GameSettings.GetGameUrl(GameProvider, GameLinkSetting.Real),
                MemberSessionId = token
            };

            _gpi = new Gpi(GameLink);
        }

        protected override string SetLanguageCode()
        {
            return new Gpi(GameLink).GetLanguageCode();
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