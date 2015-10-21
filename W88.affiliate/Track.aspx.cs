using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Web;

public partial class Track : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string[] domain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToLower().ToString().Split('.');
        string host = Request.Url.Host.ToLower();

        if (Request.QueryString["affiliateid"] != null)
        {
            string landing = string.Empty;
            DataSet ds = new DataSet();
            long aid = 0;
            long cid = 0;
            long tid = 0;
            string language = "";
            aid = long.Parse(Request.QueryString["affiliateid"].ToString(CultureInfo.InvariantCulture));
            var wsaffiliate = new wsAffiliateMS1.affiliateWSSoapClient();

            if (aid != 0)
            {
                if (Request.QueryString["tid"] != null && Request.QueryString["cid"] != null)
                {
                    tid = long.Parse(Request.QueryString["tid"].ToString());
                    cid = long.Parse(Request.QueryString["cid"].ToString());
                }

                if (Request.QueryString["language"] != null)
                {
                    language = "&language=" + Request.QueryString["language"].ToString(CultureInfo.InvariantCulture);
                }

                ds = wsaffiliate.Tracking(aid, tid, cid, Request.UserHostAddress);
            }

            //string linkAddress = ConfigurationManager.AppSettings["redirect_w88live"]; ;
            string linkAddress = ConfigurationManager.AppSettings.Get("redirect_w88live");
            //string mobile_desktop = "http://www.w88uat.com/";
            //Return domain , example www.website.com
            string redirectUrl = host;
            if (IsCorrectLink(redirectUrl)) //Check link is not empty
            {
                //in case link is w88aff.com or w88.com then use webconfig configuration else go to current domain site
                linkAddress = IsW88Url(redirectUrl) ? linkAddress : GetLink(redirectUrl, "mobile");
            }
            
            //commonFunction.audit_trail(Request.QueryString["affiliateid"].ToString(), "Track.aspx", "TrackCode", "TrackCode", "00", "", "-", "-", "http://www." + domain[1] + "." + domain[2] + "?affiliateid=" + aid.ToString(), "0",
            //            linkAddress + domain[1] + "." + domain[2] + "?affiliateid=" + aid.ToString() + language, true);

            Response.Status = "301 Moved Permanently";

            if (ds != null)
            {
                if (ds.Tables[0].Rows[0]["landingpage_id"].ToString() == "1")
                {
                    linkAddress = linkAddress + "_secure/register.aspx";
                }
                else if (ds.Tables[0].Rows[0]["landingpage_id"].ToString() == "2")
                {
                    linkAddress = linkAddress + "Promotions.aspx";
                }
            }
            Response.AddHeader("Location", linkAddress + "?affiliateid=" + aid.ToString(CultureInfo.InvariantCulture) + language);
            Response.End();
        }
        else
        {
            Response.Status = "301 Moved Permanently";
            Response.AddHeader("Location", "http://m.w88.com/");
            Response.End();
        }
    }

    #region Utility

    //Check link is not empty
    private static bool IsCorrectLink(string redirectUrl)
    {
        return redirectUrl != "" && redirectUrl != " " && redirectUrl != null;
    }
    //Check link is from w88
    private static bool IsW88Url(string redirectUrl)
    {
        return (redirectUrl == "maff.w88uat.com") || (redirectUrl == "maffiliate.w88live.com");
    }

    //Depend on mobile or site then go to diff link
    private static string GetLink(string redirectUrl, string linkType)
    {
        string[] substrings = redirectUrl.Split(':');
        string httpx = "http";
        if (substrings.Length > 1)
        {
            httpx = substrings[0].ToString(CultureInfo.InvariantCulture);
        }
        if (linkType.Equals("mobile"))
        {
            return httpx + "://" + redirectUrl.Replace(".com/", ".com").Replace("affiliate", "m") + "/";
        }
        return httpx + "://" + redirectUrl.Replace(".com/", ".com").Replace("affiliate", "www") + "/";
    }

    #endregion
}