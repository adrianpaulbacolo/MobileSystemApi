using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class _Index : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        string priorityVIP = commonVariables.GetSessionVariable("PriorityVIP");

        checkCDN();
        string CDN_Value = getCDNValue();
        string key = getCDNKey();

        if (!string.IsNullOrWhiteSpace(commonVariables.SelectedLanguage)) return;

        if (!string.IsNullOrEmpty(CDN_Value) && !string.IsNullOrEmpty(key))
        {
            commonVariables.SelectedLanguage = commonCountry.GetLanguageByCountry(GetCountryCode(CDN_Value, key));
        }
        else
        {
            Uri myUri = new Uri(System.Web.HttpContext.Current.Request.Url.ToString());
            string[] host = myUri.Host.Split('.');

            if (host.Count() > 1)
            {
                commonVariables.SelectedLanguage = GetLanguageByDomain("." + host[1] + "." + host[2]);
            }
            else
            {
                commonVariables.SelectedLanguage = GetLanguageByDomain("default");
            }

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Web.UI.WebControls.Literal litScript = (System.Web.UI.WebControls.Literal)Page.FindControl("litScript");

        xeErrors = commonVariables.ErrorsXML;

        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getRootResource("/Index.aspx", out xeResources);

        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("Error")) && !string.IsNullOrEmpty(commonVariables.GetSessionVariable("Error")))
            {
                Session.Remove("Error");
                if (litScript != null) { litScript.Text += string.Format("<script type='text/javascript'>alert('{0}');</script>", HttpContext.Current.Request.QueryString.Get("Error")); }
            }
        }

        string affiliateId = HttpContext.Current.Request.QueryString.Get("AffiliateId");

        if (!string.IsNullOrEmpty(affiliateId))
        {
            commonVariables.SetSessionVariable("AffiliateId", affiliateId);

            commonCookie.CookieAffiliateId = affiliateId;
        }

        DetectDownloadLinks(DetectMobileDevice());
    }

    public int DetectMobileDevice()
    {
        string strUserAgent = Request.UserAgent.ToString().ToLower();

        int responseCode = 0;

        if (strUserAgent != null)
        {
            if (strUserAgent.Contains("mac"))
            {
                pokerAndroid_link.Visible = false;
                pokerIOS_link.Visible = true;
                responseCode = 1;
                Session["responseCode"] = "1";
            }
            else if (strUserAgent.Contains("android"))
            {
                pokerAndroid_link.Visible = true;
                pokerIOS_link.Visible = false;
                responseCode = 2;
                Session["responseCode"] = "2";
            }
            else
            {
                pokerAndroid_link.Visible = true;
                pokerIOS_link.Visible = true;
                responseCode = 3;
                Session["responseCode"] = "3";
            }
        }
        return responseCode;
    }

    public void DetectDownloadLinks(int responseCode)
    {
        if (commonVariables.SelectedLanguage == "en-us")
        {
            if (responseCode == 1)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);
                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
            }
            else if (responseCode == 2)
            {
                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
            else if (responseCode == 3)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);

                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);

                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
        }
        if (commonVariables.SelectedLanguage == "km-kh")
        {
            if (responseCode == 1)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);
                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
            }
            else if (responseCode == 2)
            {
                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
            else if (responseCode == 3)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);

                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);

                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
        }
        if (commonVariables.SelectedLanguage == "ja-jp")
        {
            if (responseCode == 1)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_JA"]);
                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_JA"];
            }
            else if (responseCode == 2)
            {
                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_JA"]);
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_JA"];
            }
            else if (responseCode == 3)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_JA"]);

                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_JA"]);

                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_JA"];
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_JA"];
            }
        }
        if (commonVariables.SelectedLanguage == "ko-kr")
        {
            if (responseCode == 1)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);
                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
            }
            else if (responseCode == 2)
            {
                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
            else if (responseCode == 3)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);

                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);

                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
        }
        if (commonVariables.SelectedLanguage == "id-id")
        {
            if (responseCode == 1)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_ID"]);
                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_ID"];
            }
            else if (responseCode == 2)
            {
                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_ID"]);
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_ID"];
            }
            else if (responseCode == 3)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_ID"]);

                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_ID"]);

                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_ID"];
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_ID"];
            }
        }
        if (commonVariables.SelectedLanguage == "th-th")
        {
            if (responseCode == 1)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);
                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
            }
            else if (responseCode == 2)
            {
                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
            else if (responseCode == 3)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);

                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);

                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
        }
        if (commonVariables.SelectedLanguage == "vi-vn")
        {
            if (responseCode == 1)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);
                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
            }
            else if (responseCode == 2)
            {
                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
            else if (responseCode == 3)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_EN"]);

                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_EN"]);

                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_EN"];
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_EN"];
            }
        }
        if (commonVariables.SelectedLanguage == "zh-cn")
        {
            if (responseCode == 1)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_CN"]);
                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_CN"];
            }
            else if (responseCode == 2)
            {
                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_CN"]);
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_CN"];
            }
            else if (responseCode == 3)
            {
                pokerIOS.Attributes.Remove("href");
                pokerIOS.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_IOSURL_CN"]);

                pokerAndroid.Attributes.Remove("href");
                pokerAndroid.Attributes.Add("href", ConfigurationManager.AppSettings["Poker_AndroidURL_CN"]);

                Session["iosLink"] = ConfigurationManager.AppSettings["Poker_IOSURL_CN"];
                Session["androidLink"] = ConfigurationManager.AppSettings["Poker_AndroidURL_CN"];

            }
        }

        TexasMahjongDownloadLinks(responseCode);
    }

    private void TexasMahjongDownloadLinks(int responseCode)
    {
        Session["tmandroidLink"] = ConfigurationManager.AppSettings["TexasMahjongAndroid_URL"];
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("Casino.aspx");
    }
    protected void ASport_Btn_Click1(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
        {
            string CurrentUrl = System.Web.HttpContext.Current.Request.Url.ToString();
            Response.Redirect("/_Secure/Login.aspx?redirect=" + CurrentUrl);
        }
        else
        {
            Response.Redirect(commonASports.getSportsbookUrl);
        }
    }
}