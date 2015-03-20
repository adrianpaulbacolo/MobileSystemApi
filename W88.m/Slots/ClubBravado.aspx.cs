using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            System.Text.StringBuilder sbGames = new System.Text.StringBuilder();

            System.Xml.Linq.XElement xeCategories = xeResources.Element("Category");

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

                foreach (System.Xml.Linq.XElement xeGame in xeCategory.Elements())
                {
                    strGameId = (xeGame.Attribute("ProductId") == null ? "" : xeGame.Attribute("ProductId").Value);
                    sbGames.AppendFormat("<li rel='{0}.jpg' class='bkg-game'><div class='div-links'>", commonCulture.ElementValues.getResourceString("ImageName", xeGame));

                    //in-house
                    if (xeGame.Name == "Soccer" || xeGame.Name == "ThreeKingdoms" || xeGame.Name == "FreedomFighter" || xeGame.Name == "LittleMonsters" || xeGame.Name == "Fruitilicious" || xeGame.Name == "Ninetailedninja")
                    {
                        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                            sbGames.AppendFormat("<a href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubBravado") + "' data-rel='dialog' data-transition='slidedown'>");
                        else
                            sbGames.AppendFormat("<a href='{0}'>", commonClubBravado.getRealUrl.Replace("{GAME}", Convert.ToString(xeGame.Name)).Replace("{LANG}", strLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));

                        sbGames.Append("<img src='/_Static/Images/btn_play.jpg' /></a>");
                        sbGames.AppendFormat("<a href='{0}'><img src='/_Static/Images/btn_try.jpg' /></a></div>", commonClubBravado.getFunUrl.Replace("{GAME}", Convert.ToString(xeGame.Name)).Replace("{LANG}", strLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));
                    }
                    else
                    {
                     string newstrLanguageCode = (strLanguageCode == "zh") ? "zh_CN" : "en_GB"; 


                        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                            sbGames.AppendFormat("<a href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubBravado") + "' data-rel='dialog' data-transition='slidedown'>");
                        else
                            sbGames.AppendFormat("<a href='{0}'>", commonClubBravado.getThirdPartyRealUrl.Replace("{GAME}", commonCulture.ElementValues.getResourceString("ImageName", xeGame)).Replace("{LANG}", newstrLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));

                        sbGames.Append("<img src='/_Static/Images/btn_play.jpg' /></a>");
                        sbGames.AppendFormat("<a href='{0}'><img src='/_Static/Images/btn_try.jpg' /></a></div>", commonClubBravado.getThirdPartyFunUrl.Replace("{GAME}", commonCulture.ElementValues.getResourceString("ImageName", xeGame)).Replace("{LANG}", newstrLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));
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