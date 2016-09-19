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

    public string getPromoBanner()
    {
        var slider = string.Empty;
        try
        {
            System.Xml.Linq.XElement promoResource;
            commonCulture.appData.getRootResource("leftMenu", out promoResource);
            IEnumerable<System.Xml.Linq.XElement> promoNode = promoResource.Element("PromoBanner").Elements();
            foreach (System.Xml.Linq.XElement promo in promoNode)
            {
                var imageSrc = promo.Element("imageSrc").Value;
                var url = promo.Element("url").Value;
                var mainText = promo.Element("title").Value;
                var descText = promo.Element("description").Value;
                var linkClass = promo.Element("class").Value;
                var content = "";
                var description = "";

                var hasCurrency = (promo.HasAttributes && promo.Attribute("currency") != null);
                var isPublic = (promo.HasAttributes && promo.Attribute("public") != null);

                if (hasCurrency && !string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                {
                    var currencies = promo.Attribute("currency").Value;
                    if (!string.IsNullOrEmpty(currencies))
                    {
                        if (string.IsNullOrEmpty(commonCookie.CookieCurrency)) continue;
                        var currenciesArr = currencies.ToString().Split(',');
                        var test = Array.Find(currenciesArr, element => element.StartsWith(commonCookie.CookieCurrency, StringComparison.Ordinal));
                        if (string.IsNullOrEmpty(test)) continue;
                    }
                }
                if (isPublic && string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                {
                    var publicAttr = promo.Attribute("public").Value;
                    if(!string.IsNullOrEmpty(publicAttr)){
                        if(publicAttr != "1"){
                            continue;
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(descText)) description = "<p>" + descText + "</p>";
                if (!string.IsNullOrWhiteSpace(mainText)) content = "<div class=\"slide-title\"><h2>" + mainText + "</h2>" + description + "</div>";

                var bannerText = "";
                if (!string.IsNullOrWhiteSpace(content) || !string.IsNullOrWhiteSpace(content))
                {
                    bannerText = "<div class=\"slide_content\"><div class=\"textarea\">" + content + description + "</div></div>";
                }

                slider += "<div class=\"slide\">" +
                            "<a href=\"" + url + "\" data-ajax=\"false\" class=\"" + linkClass + "\">" +
                            content +
                                "<img src=\"/_Static/Images/promo-banner/" + imageSrc + "\" alt=\"banner\" class=\"img-responsive\"> " +
                            "</a>" +
                        "</div>";
            }
        }
        catch (Exception)
        {
        }
        return slider;
    }
}