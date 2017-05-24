using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Factories.Slots;

namespace Helpers
{
    public class GameSettings
    {
        private static readonly customConfig.OperatorSettings Settings =
            new customConfig.OperatorSettings(commonVariables.OperatorCode);

        public static string GetGameUrl(GameProvider provider, GameLinkSetting linkType)
        {

            var url = string.Empty;

            switch (provider)
            {
                case GameProvider.BS:
                    if (linkType == GameLinkSetting.Fun)
                        url = Settings.Values.Get("BSFunUrl");
                    else if (linkType == GameLinkSetting.Real)
                        url = Settings.Values.Get("BSRealUrl");
                    break;

                case GameProvider.CTXM:
                    if (linkType == GameLinkSetting.Fun)
                        url = Settings.Values.Get("CTXMFunUrl");
                    else if (linkType == GameLinkSetting.Real)
                        url = Settings.Values.Get("CTXMRealUrl");
                    break;

                case GameProvider.GPI:
                    if (linkType == GameLinkSetting.Fun)
                        url = Settings.Values.Get("GPIFunUrl");
                    else if (linkType == GameLinkSetting.Real)
                        url = Settings.Values.Get("GPIRealUrl");
                    break;

                case GameProvider.ISB:
                    if (linkType == GameLinkSetting.Fun)
                        url = Settings.Values.Get("ISBFunUrl");
                    else if (linkType == GameLinkSetting.Real)
                        url = Settings.Values.Get("ISBRealUrl");
                    break;

                case GameProvider.MGS:
                    if (linkType == GameLinkSetting.Fun)
                        url = Settings.Values.Get("MGSFunUrl");
                    else if (linkType == GameLinkSetting.Real)
                        url = Settings.Values.Get("MGSRealUrl");
                    break;

                case GameProvider.PNG:
                    if (linkType == GameLinkSetting.Fun)
                        url = Settings.Values.Get("PNGFunUrl");
                    else if (linkType == GameLinkSetting.Real)
                        url = Settings.Values.Get("PNGRealUrl");
                    break;

                case GameProvider.PT:
                    if (linkType == GameLinkSetting.Fun)
                        url = Settings.Values.Get("PTFunUrl");
                    else if (linkType == GameLinkSetting.Real)
                        url = Settings.Values.Get("PTRealUrl");
                    break;

                case GameProvider.QT:
                    if (linkType == GameLinkSetting.Fun)
                        url = Settings.Values.Get("QTFunUrl");
                    else if (linkType == GameLinkSetting.Real)
                        url = Settings.Values.Get("QTRealUrl");
                    break;

                case GameProvider.UC8:
                    if (linkType == GameLinkSetting.Fun)
                        url = Settings.Values.Get("UC8FunUrl");
                    else if (linkType == GameLinkSetting.Real)
                        url = Settings.Values.Get("UC8RealUrl");
                    break;

                case GameProvider.PP:
                    if (linkType == GameLinkSetting.Fun)
                        url = Settings.Values.Get("PPFunUrl");
                    else if (linkType == GameLinkSetting.Real)
                        url = Settings.Values.Get("PPRealUrl");
                    break;

                case GameProvider.GNS:
                    if (linkType == GameLinkSetting.Fun)
                        url = Settings.Values.Get("GNSFunUrl");
                    else if (linkType == GameLinkSetting.Real)
                        url = Settings.Values.Get("GNSRealUrl");
                    break;

                case GameProvider.PLS:
                    if (linkType == GameLinkSetting.Fun)
                        url = Settings.Values.Get("PLSFunUrl");
                    else if (linkType == GameLinkSetting.Real)
                        url = Settings.Values.Get("PLSRealUrl");
                    break;

                case GameProvider.TTG:
                    if (linkType == GameLinkSetting.Fun)
                        url = Settings.Values.Get("TTGFunUrl");
                    else if (linkType == GameLinkSetting.Real)
                        url = Settings.Values.Get("TTGRealUrl");
                    break;

                case GameProvider.MRS:
                    if (linkType == GameLinkSetting.Fun)
                        url = Settings.Values.Get("MRSFunUrl");
                    else if (linkType == GameLinkSetting.Real)
                        url = Settings.Values.Get("MRSRealUrl");
                    break;

                default:
                    url = "";
                    break;
            }

            return string.IsNullOrEmpty(url) ? "" : url.Replace("{DOMAIN}", commonIp.DomainName);

        }

        public static string PtAcctPrefix
        {
            get
            {
                var settings = new customConfig.OperatorSettings("W88");
                string prefix = settings.Values.Get("PTAccountPrefix");
                return string.IsNullOrEmpty(prefix) ? "" : prefix;
            }
        }
    }
}