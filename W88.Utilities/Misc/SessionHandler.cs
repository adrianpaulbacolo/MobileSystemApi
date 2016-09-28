using System;
using System.Web;

namespace W88.Utilities
{
    internal static class SessionHandler 
    {
        internal static string GetSessionVariable(string key)
        {
            if (HttpContext.Current.Session == null) return "";

            return string.IsNullOrEmpty(HttpContext.Current.Session[key] as string)
                ? ""
                : Convert.ToString(HttpContext.Current.Session[key]);
        }

        internal static void SetSessionVariable(string key, string value)
        {
            HttpContext.Current.Session.Add(key, value);
        }
    }
}
