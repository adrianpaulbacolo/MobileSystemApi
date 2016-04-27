using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class Slots_ClubDivino : BasePage
{
    protected XElement xeErrors = null;
    private XElement xeResources = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strGameId = string.Empty;
        string strLanguageCode = string.Empty;
        Uri myUri = new Uri(System.Web.HttpContext.Current.Request.Url.ToString());

        commonCulture.appData.getRootResource("/Slots/ClubDivino.aspx", out xeResources);

        if (!Page.IsPostBack)
        {
            XElement xeCategories = xeResources.Element("Category");

            switch (commonVariables.SelectedLanguage)
            {
                case "zh-cn":
                    strLanguageCode = "zh";
                    break;

                default:
                    strLanguageCode = "en";
                    break;
            }

            StringBuilder sbiOS = new StringBuilder();
            StringBuilder sbAndroid = new StringBuilder();
            StringBuilder sbWP = new StringBuilder();

            foreach (XElement xeCategory in xeCategories.Elements())
            {
                string divHeader = "<div data-role='collapsible' data-collapsed='false' data-theme='b' data-content-theme='a' data-mini='true' ";

                sbiOS.Append(divHeader);
                sbiOS.AppendFormat("type='{1}'><h4>{2}</h4><div id='div{0}_{1}' class='div-product'><div><ul>", xeCategory.Name, xeCategory.Attribute("IOSName").Value, xeCategory.Attribute("Label").Value);

                sbAndroid.Append(divHeader);
                sbAndroid.AppendFormat("type='{1}'><h4>{2}</h4><div id='div{0}_{1}' class='div-product'><div><ul>", xeCategory.Name, xeCategory.Attribute("AndroidName").Value, xeCategory.Attribute("Label").Value);

                sbWP.Append(divHeader);
                sbWP.AppendFormat("type='{1}'><h4>{2}</h4><div id='div{0}_{1}' class='div-product'><div><ul>", xeCategory.Name, xeCategory.Attribute("WPName").Value, xeCategory.Attribute("Label").Value);

                List<XElement> combinedGames = new List<XElement>();

                XElement Betsoft = xeCategory.Element("Betsoft");
                if (Betsoft != null && Betsoft.HasElements)
                {
                    List<XElement> topBetSoft = Betsoft.Elements().Where(m => m.Attribute("Top") != null).OrderBy(f => f.Attribute("Top").Value).ToList();
                    IEnumerable<XElement> sortedBetsoft = Betsoft.Elements().Where(m => m.Attribute("Top") == null).OrderBy(game => game.Element("Label").Value.ToString());
                    topBetSoft.AddRange(sortedBetsoft);
                    combinedGames.AddRange(topBetSoft);
                }

                XElement ctxm = xeCategory.Element("CTXM");
                if (ctxm != null && ctxm.HasElements)
                {
                    List<XElement> topCtxm = ctxm.Elements().Where(m => m.Attribute("Top") != null).OrderBy(f => f.Attribute("Top").Value).ToList();
                    IEnumerable<XElement> sortedCtxm = ctxm.Elements().Where(m => m.Attribute("Top") == null).OrderBy(game => game.Element("Label").Value.ToString());

                    topCtxm.AddRange(sortedCtxm);
                    combinedGames.AddRange(topCtxm);
                }

                foreach (XElement xeGame in combinedGames)
                {
                    bool isCrescendo = false;

                    if (xeGame.Attribute("IOSId") == null && xeGame.Attribute("AndroidId") == null && xeGame.Attribute("WPId") == null)
                    {
                        strGameId = xeGame.Name.ToString();
                        isCrescendo = true;
                    }

                    string iOSID = (xeGame.Attribute("IOSId") == null ? "" : xeGame.Attribute("IOSId").Value);
                    string andrID = (xeGame.Attribute("AndroidId") == null ? "" : xeGame.Attribute("AndroidId").Value);
                    string wpID = (xeGame.Attribute("WPId") == null ? "" : xeGame.Attribute("WPId").Value);

                    if (isCrescendo)
                    {
                        sbiOS.Append(CreateCrescendoGames(xeGame, strGameId));

                        sbAndroid.Append(CreateCrescendoGames(xeGame, strGameId));

                        sbWP.Append(CreateCrescendoGames(xeGame, strGameId));
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(iOSID))
                            sbiOS.Append(CreateDivinoGames(xeGame, iOSID, strLanguageCode, myUri));

                        if (!string.IsNullOrWhiteSpace(andrID))
                            sbAndroid.Append(CreateDivinoGames(xeGame, andrID, strLanguageCode, myUri));

                        if (!string.IsNullOrWhiteSpace(wpID))
                            sbWP.Append(CreateDivinoGames(xeGame, wpID, strLanguageCode, myUri));
                    }

                }

                sbiOS.Append("</ul></div></div></div>");

                sbAndroid.Append("</ul></div></div></div>");

                sbWP.Append("</ul></div></div></div>");
            }


            divContainer.InnerHtml = Convert.ToString(sbiOS) + Convert.ToString(sbAndroid) + Convert.ToString(sbWP);
        }
    }

    private StringBuilder CreateDivinoGames(XElement xeGame, string strGameId, string strLanguageCode, Uri myUri)
    {
        StringBuilder sbGames = new StringBuilder();

        sbGames.AppendFormat("<li class='bkg-game'><div rel='{0}.jpg'><div class='div-links'>", commonCulture.ElementValues.getResourceString("ImageName", xeGame));

        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
        {
            sbGames.AppendFormat("<a class='btn-primary' target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubDivino") + "' data-rel='dialog' data-transition='slidedown'>");
        }
        else
        {
            sbGames.AppendFormat("<a href='{0}' target='_blank'>", commonClubDivino.getRealUrl.Replace("{GAMEID}", strGameId).Replace("{LANG}", strLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId).Replace("{HOMEURL}", myUri.Host).Replace("{CASHIERURL}", myUri.Host));
        }

        sbGames.Append("<i class='icon-play_arrow'></i></a>");
        sbGames.AppendFormat("<a class='btn-secondary' target='_blank' href='{0}'><i class='icon-fullscreen'></i></a></div>", commonClubDivino.getFunUrl.Replace("{GAMEID}", strGameId).Replace("{LANG}", strLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId).Replace("{HOMEURL}", myUri.Host).Replace("{CASHIERURL}", myUri.Host));
        sbGames.Append("</div></li>");

        return sbGames;
    }

    private StringBuilder CreateCrescendoGames(XElement xeGame, string strGameId)
    {
        StringBuilder sbGames = new StringBuilder();

        sbGames.AppendFormat("<li class='bkg-game'><div rel='{0}.jpg'><div class='div-links'>", strGameId);

        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
        {
            sbGames.AppendFormat("<a class='btn-primary' target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubDivino") + "' data-rel='dialog' data-transition='slidedown'>");
        }
        else
        {
            sbGames.AppendFormat("<a href='{0}' target='_blank'>", commonCulture.ElementValues.getResourceString("PlayForRealURL", xeGame).Replace("{SlotsUrl}", commonClubCrescendo.getSlotsUrl).Replace("{token}", commonVariables.CurrentMemberSessionId));
        }

        sbGames.Append("<i class='icon-play_arrow'></i></a>");
        sbGames.AppendFormat("<a class='btn-secondary' target='_blank' href='{0}' data-ajax='false'><i class='icon-fullscreen'></i></a></div>", commonCulture.ElementValues.getResourceString("PlayForFunURL", xeGame).Replace("{SlotsUrl}", commonClubCrescendo.getSlotsUrl).Replace("{token}", commonVariables.CurrentMemberSessionId));
        sbGames.Append("</div></li>");
        return sbGames;
    }
}
