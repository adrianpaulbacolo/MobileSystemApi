using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for common_cookie
/// </summary>
public static class commonCookie
{
    /// <summary>Session ID</summary>
    public static string CookieS
    {
        get
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("s");
            return cookie == null ? "" : cookie.Value;
        }
        set
        {
            HttpCookie cookie = new HttpCookie("s");

            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddDays(1);

            if (!string.IsNullOrEmpty(commonIp.DomainName))
            {
                cookie.Domain = commonIp.DomainName;
            }

            if (cookie != null)
            {
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
            else
            {
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
    }

    /// <summary>Sport
    /// ok Session ID</summary>
    public static string CookieG
    {
        get
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("g");

            return cookie == null ? "" : cookie.Value;
        }
        set
        {
            HttpCookie cookie = new HttpCookie("g");

            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddDays(1);

            if (!string.IsNullOrEmpty(commonIp.DomainName))
            {
                cookie.Domain = commonIp.DomainName;
            }

            if (cookie != null)
            {
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
            else
            {
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }
    }

    public static string CookiePalazzo
    {
        get
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("palazzo");
            return cookie == null ? "" : cookie.Value;
        }
        set
        {
            HttpCookie cookie = new HttpCookie("palazzo");
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddDays(1);
            if (!string.IsNullOrEmpty(commonIp.DomainName)) { cookie.Domain = commonIp.DomainName; }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }

    public static string CookieLanguage
    {
        get
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("language");
            return cookie == null ? "" : cookie.Value;
        }
        set
        {
            HttpCookie cookie = new HttpCookie("language");
            cookie.Value = value;
            if (!string.IsNullOrEmpty(commonIp.DomainName)) { cookie.Domain = commonIp.DomainName; }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }

    public static string CookieIovation
    {
        get
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("mio");
            return cookie == null ? "" : cookie.Value;
        }
        set
        {
            HttpCookie cookie = new HttpCookie("mio");
            cookie.Value = value;
            if (!string.IsNullOrEmpty(commonIp.DomainName)) { cookie.Domain = commonIp.DomainName; }
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }

    public static string CookieAffiliateId
    {
        get
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("AffiliateId");
            return cookie == null ? "" : cookie.Value;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                HttpCookie cookie = HttpContext.Current.Response.Cookies["AffiliateId"];

                if (cookie != null)
                {
                    cookie.Value = value;
                    cookie.Expires = DateTime.Now.AddDays(365);
                    HttpContext.Current.Response.Cookies.Set(cookie);
                }
                else
                {
                    HttpCookie affliateCookie = new HttpCookie("AffiliateId");
                    affliateCookie.Value = value;
                    affliateCookie.Expires = DateTime.Now.AddDays(365);
                    HttpContext.Current.Response.Cookies.Add(affliateCookie);
                }
            }
        }
    }

    public static string CookieReferralId
    {
        get
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("ReferralId");
            return cookie == null ? "" : cookie.Value;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("ReferralId");
                // if existing cookie is present, don't override
                if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                {
                    HttpCookie affliateCookie = new HttpCookie("ReferralId");
                    affliateCookie.Value = value;
                    HttpContext.Current.Response.Cookies.Add(affliateCookie);
                }
            }
        }
    }

    public static string CookieDeviceId
    {
        get
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("fingerprint");
            return cookie == null ? "" : cookie.Value;
        }
        set
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("fingerprint");
                // if existing cookie is present, don't override
                if (cookie == null || string.IsNullOrEmpty(cookie.Value))
                {
                    HttpCookie deviceId = new HttpCookie("fingerprint");
                    deviceId.Value = value;
                    HttpContext.Current.Response.Cookies.Add(deviceId);
                }
            }
        }
    }

    public static string CookieVip
    {
        get
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("isvp");
            return cookie == null ? "" : cookie.Value;
        }
        set
        {
            if (value != null)
            {
                HttpCookie cookie = new HttpCookie("isvp");
                cookie.Value = value;
                if (!string.IsNullOrEmpty(commonIp.DomainName)) { cookie.Domain = commonIp.DomainName; }
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
            else
            {
                var httpCookie = HttpContext.Current.Request.Cookies["isvp"];
                if (httpCookie != null)
                {
                    HttpCookie cookie = new HttpCookie("isvp");
                    cookie.Value = "";
                    cookie.Expires = DateTime.Now.AddYears(-1);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
        }
    }

    public static string CookieIsApp
    {
        get
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("IsApp");
            return cookie == null ? "" : cookie.Value;
        }
        set
        {
            if (value != null)
            {
                HttpCookie cookie = new HttpCookie("IsApp");
                cookie.Value = value;
                if (!string.IsNullOrEmpty(commonIp.DomainName)) { cookie.Domain = commonIp.DomainName; }
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
            else
            {
                var httpCookie = HttpContext.Current.Request.Cookies["IsApp"];
                if (httpCookie != null)
                {
                    httpCookie.Value = "";
                    httpCookie.Expires = DateTime.Now.AddYears(-1);
                    if (!string.IsNullOrEmpty(commonIp.DomainName)) { httpCookie.Domain = commonIp.DomainName; }
                    HttpContext.Current.Response.Cookies.Add(httpCookie);
                }
            }
        }
    }

    public static string CookieCurrency
    {
        get
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies.Get("currencyCode");
            return cookie == null ? "" : cookie.Value;
        }
        set
        {
            if (value != null)
            {
                HttpCookie cookie = new HttpCookie("currencyCode");
                cookie.Value = value;
                if (!string.IsNullOrEmpty(commonIp.DomainName)) { cookie.Domain = commonIp.DomainName; }
                HttpContext.Current.Response.Cookies.Set(cookie);
            }
            else
            {
                var httpCookie = HttpContext.Current.Request.Cookies["currencyCode"];
                if (httpCookie != null)
                {
                    HttpCookie cookie = new HttpCookie("currencyCode");
                    cookie.Value = "";
                    cookie.Expires = DateTime.Now.AddYears(-1);
                    HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
        }
    }

    public static void CookieSubPlatform(string spfId)
    {
        if (!string.IsNullOrWhiteSpace(spfId))
        {
            Set("spfid_mob", spfId, DateTime.Now.AddDays(1));
        }
        else
        {
            if (string.IsNullOrWhiteSpace(commonCookie.Get("spfid_mob")))
            {
                Set("spfid_mob", "22", DateTime.Now.AddDays(1)); // Add Default Subplatform = WAP
            }
        }
    }

    public static void Set(string key, string value, DateTime expires)
    {
        HttpCookie cookie = new HttpCookie(key, value);
        cookie.Expires = expires;

        if (!string.IsNullOrEmpty(commonIp.DomainName))
        {
            cookie.Domain = commonIp.DomainName;
        }

        if (cookie != null)
        {
            HttpContext.Current.Response.Cookies.Set(cookie);
        }
        else
        {
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }

    public static string Get(string key)
    {
        string value = "";
        if (HttpContext.Current.Request.Cookies.AllKeys.Contains(key))
        {
            value = HttpContext.Current.Request.Cookies[key].Value;
        }

        return value;
    }

    public static void ClearCookies()
    {
        commonVariables.ClearSessionVariables();

        HttpCookie isApp = HttpContext.Current.Request.Cookies["IsApp"];
        HttpContext.Current.Response.Cookies.Remove("IsApp");

        if (isApp != null)
        {
            isApp.Expires = DateTime.Now.AddYears(-1);
            isApp.Value = null;
            isApp.Domain = commonIp.DomainName;
            HttpContext.Current.Response.SetCookie(isApp);
        }

        HttpCookie currencyCode = HttpContext.Current.Request.Cookies["currencyCode"];
        HttpContext.Current.Response.Cookies.Remove("currencyCode");
        if (currencyCode != null)
        {
            currencyCode.Expires = DateTime.Now.AddYears(-1);
            currencyCode.Value = null;
            currencyCode.Domain = commonIp.DomainName;
            HttpContext.Current.Response.SetCookie(currencyCode);
        }

        HttpCookie s = HttpContext.Current.Request.Cookies["s"];
        HttpContext.Current.Response.Cookies.Remove("s");

        if (s != null)
        {
            s.Expires = DateTime.Now.AddYears(-1);
            s.Value = null;
            s.Domain = commonIp.DomainName;
            HttpContext.Current.Response.SetCookie(s);
        }

        HttpCookie g = HttpContext.Current.Request.Cookies["g"];
        HttpContext.Current.Response.Cookies.Remove("g");

        if (g != null)
        {
            g.Expires = DateTime.Now.AddYears(-1);
            g.Value = null;
            g.Domain = commonIp.DomainName;
            HttpContext.Current.Response.SetCookie(g);
        }

        HttpCookie vip = HttpContext.Current.Request.Cookies["isvp"];
        HttpContext.Current.Response.Cookies.Remove("isvp");

        if (vip != null)
        {
            vip.Expires = DateTime.Now.AddYears(-1);
            vip.Value = null;
            vip.Domain = commonIp.DomainName;
            HttpContext.Current.Response.SetCookie(vip);
        }

        HttpCookie spfid = HttpContext.Current.Request.Cookies["spfid_mob"];
        HttpContext.Current.Response.Cookies.Remove("spfid_mob");

        if (spfid != null)
        {
            spfid.Expires = DateTime.Now.AddYears(-1);
            spfid.Value = null;
            spfid.Domain = commonIp.DomainName;
            HttpContext.Current.Response.SetCookie(spfid);
        }
    }
}