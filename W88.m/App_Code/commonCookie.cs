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
            if (!string.IsNullOrEmpty(commonIp.DomainName)) { cookie.Domain = commonIp.DomainName; }
            HttpContext.Current.Response.Cookies.Add(cookie);
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
            if (!string.IsNullOrEmpty(commonIp.DomainName)) { cookie.Domain = commonIp.DomainName; }
            HttpContext.Current.Response.Cookies.Add(cookie);
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


    public static void ClearCookies()
    {
        commonVariables.ClearSessionVariables();

        System.Web.HttpContext.Current.Response.Cookies["s"].Domain = commonIp.DomainName;
        System.Web.HttpContext.Current.Response.Cookies["s"].Value = "";
        System.Web.HttpContext.Current.Response.Cookies["s"].Expires = DateTime.Now.AddYears(-1);
        System.Web.HttpContext.Current.Response.Cookies["g"].Domain = commonIp.DomainName;
        System.Web.HttpContext.Current.Response.Cookies["g"].Value = "";
        System.Web.HttpContext.Current.Response.Cookies["g"].Expires = DateTime.Now.AddYears(-1);

    }
}