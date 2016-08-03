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

        public static string MGSFun
        {
            get
            {
                var settings = new customConfig.OperatorSettings("W88");
                if (settings == null) return "";

                string url = settings.Values.Get("MGSFunUrl");

                return string.IsNullOrEmpty(url) ? "" : url.Replace("{DOMAIN}", commonIp.DomainName);
            }
        }

        public static string MGSReal
        {
            get
            {
                var settings = new customConfig.OperatorSettings("W88");
                if (settings == null) return "";

                string url = settings.Values.Get("MGSRealUrl");

                return string.IsNullOrEmpty(url) ? "" : url.Replace("{DOMAIN}", commonIp.DomainName);
            }
        }

        public static string BSFun
        {
            get
            {
                var settings = new customConfig.OperatorSettings("W88");
                if (settings == null) return "";

                string url = settings.Values.Get("BSFunUrl");

                return string.IsNullOrEmpty(url) ? "" : url.Replace("{DOMAIN}", commonIp.DomainName);
            }
        }

        public static string BSReal
        {
            get
            {
                var settings = new customConfig.OperatorSettings("W88");
                if (settings == null) return "";

                string url = settings.Values.Get("BSRealUrl");

                return string.IsNullOrEmpty(url) ? "" : url.Replace("{DOMAIN}", commonIp.DomainName);
            }
        }

        public static string CTXMFun
        {
            get
            {
                var settings = new customConfig.OperatorSettings("W88");
                if (settings == null) return "";

                string url = settings.Values.Get("CTXMFunUrl");

                return string.IsNullOrEmpty(url) ? "" : url.Replace("{DOMAIN}", commonIp.DomainName);
            }
        }

        public static string CTXMReal
        {
            get
            {
                var settings = new customConfig.OperatorSettings("W88");
                if (settings == null) return "";

                string url = settings.Values.Get("CTXMRealUrl");

                return string.IsNullOrEmpty(url) ? "" : url.Replace("{DOMAIN}", commonIp.DomainName);
            }
        }

    }
}