using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml.Linq;
using System.Text;

public partial class Slots_ClubBravado : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    private System.Xml.Linq.XElement xeResources = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strGameId = string.Empty;
        string strLanguageCode = string.Empty;

        commonCulture.appData.getRootResource("/Slots/ClubBravado.aspx", out xeResources);

        if (!Page.IsPostBack)
        {
            StringBuilder sbGames = new StringBuilder();

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

            foreach (System.Xml.Linq.XElement xeCategory in xeCategories.Elements())
            {
                sbGames.AppendFormat("<div data-role='collapsible' data-collapsed='false' data-theme='b' data-content-theme='a' data-mini='true'><h4>{0}</h4>", xeCategory.Attribute("Label").Value);

                sbGames.AppendFormat("<div id='div{0}' class='div-product'><div><ul>", xeCategory.Name);

                List<XElement> topgames = xeCategory.Elements().Where(m => m.Attribute("Top") != null).OrderBy(f => f.Attribute("Top").Value).ToList();

                IEnumerable<XElement> sortedGame = xeCategory.Elements().Where(m => m.Attribute("Top") == null).OrderBy(game => game.Name.ToString());

                topgames.AddRange(sortedGame);

                foreach (XElement xeGame in topgames)
                {
                    strGameId = (xeGame.Attribute("ProductId") == null ? "" : xeGame.Attribute("ProductId").Value);
                    sbGames.AppendFormat("<li rel='{0}.jpg' class='bkg-game'><div class='div-links'>", commonCulture.ElementValues.getResourceString("ImageName", xeGame));

                    bool isInternal = false;
                    string slotType = string.Empty;
                    if (commonCulture.ElementValues.getResourceString("IsInternal", xeGame) != "")
                        isInternal = bool.Parse(commonCulture.ElementValues.getResourceString("IsInternal", xeGame));

                    if (commonCulture.ElementValues.getResourceString("SlotType", xeGame) != "")
                        slotType = Convert.ToString(commonCulture.ElementValues.getResourceString("SlotType", xeGame));

                    //in-house
                    if (isInternal)
                    {
                        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                            sbGames.AppendFormat("<a class='btn-primary' target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubBravado") + "' data-rel='dialog' data-transition='slidedown'>");
                        else
                            if (slotType == "RSLOT")
                            {
                                customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
                                sbGames.AppendFormat("<a href='{0}' target='_blank'>", commonClubBravado.getRealUrl_mrslot.Replace("{GAME}", Convert.ToString(xeGame.Name)).Replace("{LANG}", strLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));
                            }
                            else
                            {
                                sbGames.AppendFormat("<a href='{0}' target='_blank'>", commonClubBravado.getRealUrl.Replace("{GAME}", Convert.ToString(xeGame.Name)).Replace("{LANG}", strLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));
                            }

                        sbGames.Append("<i class='icon-play_arrow'></i></a>");

                        if (slotType == "RSLOT")
                        {
                            sbGames.AppendFormat("<a class='btn-secondary' target='_blank' href='{0}'><i class='icon-fullscreen'></i></a></div>", commonClubBravado.getFunUrl_mrslot.Replace("{GAME}", Convert.ToString(xeGame.Name)).Replace("{LANG}", strLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));
                        }
                        else
                        {
                            sbGames.AppendFormat("<a class='btn-secondary' target='_blank' href='{0}'><i class='icon-fullscreen'></i></a></div>", commonClubBravado.getFunUrl.Replace("{GAME}", Convert.ToString(xeGame.Name)).Replace("{LANG}", strLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));
                        }
                    }
                    else
                    {
                        string newstrLanguageCode = (strLanguageCode == "zh") ? "zh_CN" : "en_GB";


                        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                            sbGames.AppendFormat("<a class='btn-primary' target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubBravado") + "' data-rel='dialog' data-transition='slidedown'>");
                        else
                            sbGames.AppendFormat("<a href='{0}'>", commonClubBravado.getThirdPartyRealUrl.Replace("{GAME}", commonCulture.ElementValues.getResourceString("ImageName", xeGame)).Replace("{LANG}", newstrLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));

                        sbGames.Append("<i class='icon-play_arrow'></i></a>");
                        sbGames.AppendFormat("<a class='btn-secondary' href='{0}' target='_blank'><i class='icon-fullscreen'></i></a></div>", commonClubBravado.getThirdPartyFunUrl.Replace("{GAME}", commonCulture.ElementValues.getResourceString("ImageName", xeGame)).Replace("{LANG}", newstrLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));
                    }

                    sbGames.Append("</li>");
                }

                sbGames.Append("</ul></div></div></div>");
                //collapsed = true;
            }

            divContainer.InnerHtml = Convert.ToString(sbGames);
        }
    }
}
