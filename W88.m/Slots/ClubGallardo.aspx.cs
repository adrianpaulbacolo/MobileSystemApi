using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml.Linq;

public partial class Slots_ClubGallardo : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    private System.Xml.Linq.XElement xeResources = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strGameId = string.Empty;
        string strLanguageCode = string.Empty;

        commonCulture.appData.getRootResource("/Slots/ClubGallardo.aspx", out xeResources);

        if (!Page.IsPostBack)
        {
            System.Text.StringBuilder sbGames = new System.Text.StringBuilder();

            System.Xml.Linq.XElement xeCategories = xeResources.Element("Category");

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
                sbGames.AppendFormat("<div data-role='collapsible' data-collapsed='false' data-theme='b' data-content-theme='a' data-mini='true'><h4>{0}</h4>", xeCategory.Attribute("Label").Value);

                sbGames.AppendFormat("<div id='div{0}' class='div-product'><div><ul>", xeCategory.Name);

                List<XElement> topgames = xeCategory.Elements().Where(m => m.Attribute("Top") != null).OrderBy(f => f.Attribute("Top").Value).ToList();

                IEnumerable<XElement> sortedGame = xeCategory.Elements().Where(m => m.Attribute("Top") == null).OrderBy(game => game.Name.ToString());

                topgames.AddRange(sortedGame);

                foreach (XElement xeGame in topgames)
                {
                    bool isPnG = false;

                    if (xeGame.Attribute("ProductId") == null)
                    {
                        strGameId = xeGame.Name.ToString().ToLower();
                        isPnG = true;
                    }
                    else
                    {
                        strGameId = xeGame.Attribute("ProductId").Value;
                    }

                    sbGames.AppendFormat("<li rel='{0}.jpg' class='bkg-game'><div class='div-links'>", commonCulture.ElementValues.getResourceString("ImageName", xeGame));

                    if (isPnG)
                    {
                        string newstrLanguageCode = (strLanguageCode == "chs") ? "zh_CN" : "en_GB";

                        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                            sbGames.AppendFormat("<a class='btn-primary' target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubGallardo") + "' data-rel='dialog' data-transition='slidedown'>");
                        else
                            sbGames.AppendFormat("<a href='{0}' target='_blank'>", commonClubBravado.getThirdPartyRealUrl.Replace("{GAME}", Convert.ToString(strGameId)).Replace("{LANG}", newstrLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));

                        sbGames.Append("<i class='icon-play_arrow'></i></a>");
                        sbGames.AppendFormat("<a class='btn-secondary' href='{0}' target='_blank'><i class='icon-fullscreen'></i></a></div>", commonClubBravado.getThirdPartyFunUrl.Replace("{GAME}", commonCulture.ElementValues.getResourceString("ImageName", xeGame)).Replace("{LANG}", newstrLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                            sbGames.AppendFormat("<a class='btn-primary' target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubGallardo") + "' data-rel='dialog' data-transition='slidedown'>");
                        else
                            sbGames.AppendFormat("<a href='{0}' target='_blank'>", commonClubGallardo.getRealUrl.Replace("{GAME}", Convert.ToString(strGameId)).Replace("{LANG}", strLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId).Replace("{LOBBYURL}", lobbyUrl));

                        sbGames.Append("<i class='icon-play_arrow'></i></a>");

                        sbGames.AppendFormat("<a class='btn-secondary' target='_blank' href='{0}'><i class='icon-fullscreen'></i></a></div>", commonClubGallardo.getFunUrl.Replace("{GAME}", Convert.ToString(strGameId)).Replace("{LANG}", strLanguageCode).Replace("{CURCODE}", currCode).Replace("{LOBBYURL}", lobbyUrl));
                    }

                    sbGames.Append("</li>");
                }

                sbGames.Append("</ul></div></div></div>");
            }

            divContainer.InnerHtml = Convert.ToString(sbGames);
        }
    }
}
