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

                string url = settings.Values.Get("GPIFunUrl");

                return string.IsNullOrEmpty(url) ? "" : url.Replace("{DOMAIN}", commonIp.DomainName);
            }
        }

        public static string GPIReal
        {
            get
            {
                var settings = new customConfig.OperatorSettings("W88");
                if (settings == null) return "";

                string url = settings.Values.Get("GPIRealUrl");

                return string.IsNullOrEmpty(url) ? "" : url.Replace("{DOMAIN}", commonIp.DomainName);
            }
        }

    }
}