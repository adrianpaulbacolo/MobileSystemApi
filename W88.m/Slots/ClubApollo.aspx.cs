using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class Slots_ClubApollo : BasePage
{
    protected XElement xeErrors = null;
    private XElement xeResources = null;
    private string _selectedLanguage;

    protected void Page_Load(object sender, EventArgs e)
    {
        commonCulture.appData.GetRootResourceNonLanguage("/Slots/ClubApollo.aspx", out xeResources);

        CheckSupportedCurrency();

        if (Page.IsPostBack) return;

       _selectedLanguage = SetLanguageSupport();

        SetTitle(commonCulture.ElementValues.getResourceXPathString("/Products/ClubApollo/Label", commonVariables.ProductsXML));
        var sbGames = new StringBuilder();

        XElement xeCategories = xeResources.Element("Category");

        foreach (XElement xeCategory in xeCategories.Elements())
        {
            var header = GetHeadTranslation(xeCategory);

            sbGames.AppendFormat("<div data-role='collapsible' data-collapsed='false' data-theme='b' data-content-theme='a' data-mini='true'><h4>{0}</h4>", header);

            sbGames.AppendFormat("<div id='div{0}' class='div-product'><div><ul>", xeCategory.Name);

            List<XElement> topgames = xeCategory.Elements().Where(m => m.Attribute("Top") != null).OrderBy(f => f.Value).ToList();
            
            IEnumerable<XElement> sortedGame = xeCategory.Elements().Where(m => m.Attribute("Top") == null).OrderBy(game => game.Name.ToString());

            topgames.AddRange(sortedGame);

            foreach (XElement xeGame in topgames)
            {
                sbGames.AppendFormat("<li class='bkg-game'><div rel='{0}.jpg'><div class='div-links'>", commonCulture.ElementValues.getResourceString("ImageName", xeGame));

                if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) 
                { 
                    sbGames.AppendFormat("<a href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubApollo") + "' data-rel='dialog' data-transition='slidedown'>"); 
                }
                else 
                {
                    sbGames.AppendFormat("<a href='{0}' target='_blank'>", CommonClubApollo.GetRealUrl.Replace("{GAME}", Convert.ToString(xeGame.Name)).Replace("{LANG}", _selectedLanguage).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId)).Replace("{LOBBYURL}", HttpContext.Current.Request.Url.AbsoluteUri).Replace("{cashier}", HttpContext.Current.Request.Url.Authority + "/fundtransfer"); 
                }

                sbGames.AppendFormat("{0}</a>", commonCulture.ElementValues.getResourceXPathString("/Products/Play", commonVariables.ProductsXML));
                sbGames.AppendFormat("<a target='_blank' href='{1}' data-ajax='false'>{0}</a></div>", commonCulture.ElementValues.getResourceXPathString("/Products/Try", commonVariables.ProductsXML), CommonClubApollo.GetFunUrl.Replace("{GAME}", Convert.ToString(xeGame.Name)).Replace("{LANG}", _selectedLanguage).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId)).Replace("{CURCODE}", GetCurrencyByLanguage()).Replace("{LOBBYURL}", HttpContext.Current.Request.Url.AbsoluteUri);
                sbGames.Append("</div></li>");
            }

            sbGames.Append("</ul></div></div></div>");
        }

        divContainer.InnerHtml = Convert.ToString(sbGames);
    }

    private string GetCurrencyByLanguage()
    {
        string currency;
        switch (commonVariables.SelectedLanguage)
        {
            case "zh-cn":
                currency = "CNY";
                break;
            //case "ko-kr": //temporary commented these lines as it is not yet supported.
            //    currency = "KRW";
            //    break;
            case "ja-jp":
                currency = "JPY";
                break;
            default:
                currency = "USD";
                break;
        }

        if (HttpContext.Current.Session["LanguageCode"] != null && HttpContext.Current.Session["CurrencyCode"] != null)
        {
            if ((string)HttpContext.Current.Session["LanguageCode"] == "en-us" && ((string)HttpContext.Current.Session["CurrencyCode"] == "MY"))
            {
                currency = "MYR";
            }
        }

        return currency;
    }

    private string SetLanguageSupport()
    {
        var lang = commonVariables.SelectedLanguage.Split(new char[] {'-'}, StringSplitOptions.RemoveEmptyEntries);
        var splitLang = string.Format("{0}_{1}", lang[0], lang[1].ToUpper());

        string supportedLang = null;
        switch (splitLang)
        {
            case "ko_KR":
                supportedLang = "en_US";
                break;
        }

        return supportedLang ?? splitLang;
    }

    private string GetHeadTranslation(XElement element)
    {
        string headerText;
        var lang = commonVariables.SelectedLanguage;

        if (string.IsNullOrEmpty(lang))
        {
            headerText = element.Attribute("Label").Value;
        }
        else
        {
            if (element.Attribute(lang) != null && element.Attribute(lang).Value.Length > 0)
            {
                headerText = element.Attribute(lang).Value;    
            }
            else
            {
                headerText = element.Attribute("Label").Value;
            }
        }

        return headerText;
    }

    private void CheckSupportedCurrency()
    {
        if (commonVariables.GetSessionVariable("clubapollo") == "0")
        {
            Response.Redirect("~/Slots.aspx", true);
        }
    }
}