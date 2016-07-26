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
    private string _currencyCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        commonCulture.appData.GetRootResourceNonLanguage("/Slots/ClubApollo.aspx", out xeResources);

        CheckSupportedCurrency();

        if (Page.IsPostBack) return;

        _selectedLanguage = commonVariables.SelectedLanguage;
        _currencyCode = commonVariables.GetSessionVariable("CurrencyCode");

        SetTitle(commonCulture.ElementValues.getResourceXPathString("/Products/ClubApollo/Label", commonVariables.ProductsXML));
        var sbGames = new StringBuilder();

        XElement xeCategories = xeResources.Element("Category");
        XElement xeProviders = xeResources.Element("Providers");

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

                var setLanguage = "{providerId}";
                var currency = "{providerId}";

                var pId = xeGame.Attribute("providerId");
                if (pId != null)
                {
                    if (xeProviders != null)
                    {
                        foreach (var prv in xeProviders.Elements())
                        {
                            if (prv.Attribute("id").Value != pId.Value) continue;

                            var curr = prv.Attribute("Curr").Value;
                            var lang = prv.Attribute("Lang").Value;

                            if (lang.Trim().ToLower().Contains(_selectedLanguage.ToLower()))
                            {
                                var select = _selectedLanguage.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                                setLanguage = string.Format("{0}_{1}", @select[0], @select[1].ToUpper());
                            }
                            else
                            {
                                setLanguage = "en_US";
                            }

                            if (string.IsNullOrWhiteSpace(_currencyCode))
                            {
                                currency = GetCurrencyByLanguage(setLanguage);

                                if (!curr.ToLower().Trim().Contains(currency.ToLower()))
                                {
                                    currency = "USD";
                                }
                            }
                            else
                            {
                                if (curr.ToLower().Trim().Contains(_currencyCode.ToLower()))
                                {
                                    currency = _currencyCode;
                                }
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) 
                { 
                    sbGames.AppendFormat("<a href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubApollo") + "' data-rel='dialog' data-transition='slidedown'>"); 
                }
                else 
                {
                    sbGames.AppendFormat("<a href='{0}' target='_blank'>", CommonClubApollo.GetRealUrl.Replace("{GAME}", Convert.ToString(xeGame.Name)).Replace("{LANG}", setLanguage).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId)).Replace("{LOBBYURL}", HttpContext.Current.Request.Url.AbsoluteUri).Replace("{cashier}", HttpContext.Current.Request.Url.Authority + "/fundtransfer");
                }

                sbGames.AppendFormat("{0}</a>", commonCulture.ElementValues.getResourceXPathString("/Products/Play", commonVariables.ProductsXML));
                sbGames.AppendFormat("<a target='_blank' href='{1}' data-ajax='false'>{0}</a></div>", commonCulture.ElementValues.getResourceXPathString("/Products/Try", commonVariables.ProductsXML), CommonClubApollo.GetFunUrl.Replace("{GAME}", Convert.ToString(xeGame.Name)).Replace("{LANG}", setLanguage).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId)).Replace("{CURCODE}", currency).Replace("{LOBBYURL}", HttpContext.Current.Request.Url.AbsoluteUri);
                sbGames.Append("</div></li>");
            }

            sbGames.Append("</ul></div></div></div>");
        }

        divContainer.InnerHtml = Convert.ToString(sbGames);
    }

    private string GetCurrencyByLanguage(string selectedLanguage)
    {
        string currency;
        switch (selectedLanguage.ToLower())
        {
            case "zh_cn":
                currency = "CNY";
                break;
            case "ko_kr":
                currency = "KRW";
                break;
            case "ja_jp":
                currency = "JPY";
                break;
            case "th_th":
                currency = "THB";
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
            Response.Redirect("~/Index", true);
        }
    }
}