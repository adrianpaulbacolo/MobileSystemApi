using W88.BusinessLogic.Games.Factories;
using W88.BusinessLogic.Shared.Helpers;

namespace W88.BusinessLogic.Games.Handlers
{
    public class GameSettings
    {
        private static readonly OperatorSettings Settings = new OperatorSettings(W88.Utilities.Constant.Settings.OperatorName);

        public static string GetGameUrl(GameProvider provider, GameLinkSetting linkType)
        {

            var url = string.Empty;
            var ipHelper = new W88.Utilities.Geo.IpHelper();

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

                default:
                    url = "";
                    break;
            }

            return string.IsNullOrEmpty(url) ? "" : url.Replace("{DOMAIN}", ipHelper.DomainName);
        }

        public static string GamePath
        {
            get
            {
                string prefix = Settings.Values.Get("GamePath");
                return string.IsNullOrEmpty(prefix) ? "" : prefix;
            }
        }

        public static string PTAcctPrefix
        {
            get
            {
                string prefix = Settings.Values.Get("PTAccountPrefix");
                return string.IsNullOrEmpty(prefix) ? "" : prefix;
            }
        }
    
    }
}