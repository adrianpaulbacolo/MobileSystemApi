using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class Slots_ClubGallardo : BasePage
{
    protected XElement xeErrors = null;
    private XElement xeResources = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strGameId = string.Empty;
        string strLanguageCode = string.Empty;

        commonCulture.appData.getRootResource("/Slots/ClubGallardo.aspx", out xeResources);

        if (Page.IsPostBack) return;

        SetTitle(commonCulture.ElementValues.getResourceXPathString("/Products/ClubGallardo/Label", commonVariables.ProductsXML));

        StringBuilder sbGames = new StringBuilder();

        XElement xeCategories = xeResources.Element("Category");

        switch (commonVariables.SelectedLanguage)
        {
            case "zh-cn":
                strLanguageCode = "chs";
                break;

            case "th-th":
                strLanguageCode = "th";
                break;

            case "ko-kr":
                strLanguageCode = "kr";
                break;

            case "vi-vn":
                strLanguageCode = "vi";
                break;

            case "ja-jp":
                strLanguageCode = "ja";
                break;

            default:
                strLanguageCode = "en";
                break;
        }

        customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");

        string currencyCode = commonVariables.GetSessionVariable("CurrencyCode");
        string currCode = string.IsNullOrWhiteSpace(currencyCode) || currencyCode.Equals("rmb", StringComparison.OrdinalIgnoreCase) ? "CNY" : currencyCode;
        string lobbyUrl = System.Web.HttpContext.Current.Request.Url.ToString();

        foreach (XElement xeCategory in xeCategories.Elements())
        {
            List<XElement> combinedGames = new List<XElement>();

            XElement iSoftBet = xeCategory.Element("iSoftBet");
            bool isISoftBetNotSupported = false;
            if (iSoftBet != null && iSoftBet.HasElements)
            {
                List<XElement> topiSoftBet = iSoftBet.Elements().Where(m => m.Attribute("Top") != null).OrderBy(f => f.Attribute("Top").Value).ToList();
                IEnumerable<XElement> sortediSoftBet = iSoftBet.Elements().Where(m => m.Attribute("Top") == null).OrderBy(game => game.Name.ToString());

                topiSoftBet.AddRange(sortediSoftBet);
                combinedGames.AddRange(topiSoftBet);

                string iSoftBetNotSuppCurr = iSoftBet != null && iSoftBet.HasAttributes ? iSoftBet.Attribute("NotSupportedCurrency").Value : "";

                string[] iSoftBetNotSupp = iSoftBetNotSuppCurr.Split(',');
                isISoftBetNotSupported = iSoftBetNotSupp.Contains(currencyCode);
            }

            XElement PNG = xeCategory.Element("PNG");
            if (PNG != null && PNG.HasElements)
            {
                List<XElement> topPNG = PNG.Elements().Where(m => m.Attribute("Top") != null).OrderBy(f => f.Attribute("Top").Value).ToList();
                IEnumerable<XElement> sortedPNG = PNG.Elements().Where(m => m.Attribute("Top") == null).OrderBy(game => game.Name.ToString());
                topPNG.AddRange(sortedPNG);
                combinedGames.AddRange(topPNG);
            }

            sbGames.AppendFormat("<div data-role='collapsible' data-collapsed='false' data-theme='b' data-content-theme='a' data-mini='true'><h4>{0}</h4>", xeCategory.Attribute("Label").Value);

            sbGames.AppendFormat("<div id='div{0}' class='div-product'><div><ul>", xeCategory.Name);

            foreach (XElement xeGame in combinedGames)
            {
                string notSupCurr = (xeGame.Attribute("NotSupportedCurrency") != null) ? xeGame.Attribute("NotSupportedCurrency").Value : "";

                string[] currNotSupp = notSupCurr.Split(',');

                bool isGameNotSupported = currNotSupp.Contains(currencyCode);

                if (!isGameNotSupported || string.IsNullOrWhiteSpace(currencyCode))
                {
                    if (xeGame.Attribute("ProductId") == null)
                    {
                        strGameId = xeGame.Name.ToString().ToLower();

                        sbGames.AppendFormat("<li class='bkg-game'><div rel='{0}.jpg'><div class='div-links'>", commonCulture.ElementValues.getResourceString("ImageName", xeGame));

                        string newstrLanguageCode = (strLanguageCode == "chs") ? "zh_CN" : "en_GB";

                        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                            sbGames.AppendFormat("<a target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubGallardo") + "' data-rel='dialog' data-transition='slidedown'>");
                        else
                            sbGames.AppendFormat("<a href='{0}' target='_blank'>", commonClubBravado.getThirdPartyRealUrl.Replace("{GAME}", Convert.ToString(strGameId)).Replace("{LANG}", newstrLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));

                        sbGames.AppendFormat("{0}</a>", commonCulture.ElementValues.getResourceXPathString("/Products/Play", commonVariables.ProductsXML));
                        sbGames.AppendFormat("<a href='{1}' target='_blank' data-ajax='false'>{0}</a></div>", commonCulture.ElementValues.getResourceXPathString("/Products/Try", commonVariables.ProductsXML), 
                            commonClubBravado.getThirdPartyFunUrl.Replace(
                            "{GAME}", 
                            Convert.ToString(strGameId)).Replace("{LANG}", 
                            newstrLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId)
                            );

                        sbGames.Append("</div></li>");

                    }
                    else if (!isISoftBetNotSupported)
                    {
                        strGameId = xeGame.Attribute("ProductId").Value;

                        sbGames.AppendFormat("<li class='bkg-game'><div rel='{0}.jpg'><div class='div-links'>", commonCulture.ElementValues.getResourceString("ImageName", xeGame));

                        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                            sbGames.AppendFormat("<a class='btn-primary' target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubGallardo") + "' data-rel='dialog' data-transition='slidedown'>");
                        else
                            sbGames.AppendFormat("<a href='{0}' target='_blank'>", commonClubGallardo.getRealUrl.Replace("{GAME}", Convert.ToString(strGameId)).Replace("{LANG}", strLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId).Replace("{LOBBYURL}", lobbyUrl));

                        sbGames.AppendFormat("{0}</a>", commonCulture.ElementValues.getResourceXPathString("/Products/Play", commonVariables.ProductsXML));

                        sbGames.AppendFormat("<a target='_blank' href='{1}' data-ajax='false'>{0}</a></div>", commonCulture.ElementValues.getResourceXPathString("/Products/Try", commonVariables.ProductsXML), commonClubGallardo.getFunUrl.Replace("{GAME}", Convert.ToString(strGameId)).Replace("{LANG}", strLanguageCode).Replace("{CURCODE}", currCode).Replace("{LOBBYURL}", lobbyUrl));

                        sbGames.Append("</div></li>");
                    }
                }

                sbGames.Append("</li>");
            }

            sbGames.Append("</ul></div></div></div>");
        }

        divContainer.InnerHtml = Convert.ToString(sbGames);
    }
}