using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Helpers
{
    public class GameSettings
    {
        public static string GPIFun
        {
            get
            {
                var settings = new customConfig.OperatorSettings("W88");
                if (settings == null) return "";

                string url = settings.Values.Get("ClubBravadoFunUrl");

                return string.IsNullOrEmpty(url) ? "" : url.Replace("{ORIGIN}", commonIp.DomainName);
            }
        }

        public static string GPIReal
        {
            get
            {
                var settings = new customConfig.OperatorSettings("W88");
                if (settings == null) return "";

                string url = settings.Values.Get("ClubBravadoRealUrl");

                var paramString = "&s=" + commonVariables.CurrentMemberSessionId;

                return string.IsNullOrEmpty(url) ? "" : url.Replace("{DOMAIN}", commonIp.DomainName) + paramString;
            }
        }

        public static string GPIRSlotFun
        {
            get
            {
                var settings = new customConfig.OperatorSettings("W88");
                if (settings == null) return "";

                string url = settings.Values.Get("ClubBravadoFunUrl_MR");

                return string.IsNullOrEmpty(url) ? "" : url.Replace("{ORIGIN}", commonIp.DomainName);
            }
        }

        public static string GPIRSlotReal
        {
            get
            {
                var settings = new customConfig.OperatorSettings("W88");
                if (settings == null) return "";

                string url = settings.Values.Get("ClubBravadoRealUrl_MR");

                var paramString = "&s=" + commonVariables.CurrentMemberSessionId;

                return string.IsNullOrEmpty(url) ? "" : url.Replace("{DOMAIN}", commonIp.DomainName) + paramString;

            }
        }
    }
}