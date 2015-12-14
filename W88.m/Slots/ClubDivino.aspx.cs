using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Slots_ClubDivino : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    private System.Xml.Linq.XElement xeResources = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strGameId = string.Empty;
        string strLanguageCode = string.Empty;
        Uri myUri = new Uri(System.Web.HttpContext.Current.Request.Url.ToString());

        commonCulture.appData.getRootResource("/Slots/ClubDivino.aspx", out xeResources);

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
                sbGames.AppendFormat("<div data-role='collapsible' data-collapsed='false' data-theme='b' data-content-theme='a' data-mini='true' type='{1}'><h4>{0}</h4>", xeCategory.Attribute("Label").Value, xeCategory.Attribute("Version").Value);

                sbGames.AppendFormat("<div id='div{0}_{1}' class='div-product'><div><ul>", xeCategory.Name, xeCategory.Attribute("Version").Value);

                foreach (System.Xml.Linq.XElement xeGame in xeCategory.Elements())
                {
                    strGameId = (xeGame.Attribute("ProductId") == null ? "" : xeGame.Attribute("ProductId").Value);

                    sbGames.AppendFormat("<li rel='{0}.jpg' class='bkg-game'><div class='div-links'>", commonCulture.ElementValues.getResourceString("ImageName", xeGame));

                    if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) { sbGames.AppendFormat("<a class='btn-primary' target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubDivino") + "' data-rel='dialog' data-transition='slidedown'>"); }
                    else { sbGames.AppendFormat("<a href='{0}' target='_blank'>", commonClubDivino.getRealUrl.Replace("{GAMEID}", strGameId).Replace("{LANG}", strLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId).Replace("{HOMEURL}", myUri.Host).Replace("{CASHIERURL}", myUri.Host)); }

                    sbGames.Append("<i class='icon-play_arrow'></i></a>");
                    sbGames.AppendFormat("<a class='btn-secondary' target='_blank' href='{0}'><i class='icon-fullscreen'></i></a></div>", commonClubDivino.getFunUrl.Replace("{GAMEID}", strGameId).Replace("{LANG}", strLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId).Replace("{HOMEURL}", myUri.Host).Replace("{CASHIERURL}", myUri.Host));
                    sbGames.Append("</li>");
                }

                sbGames.Append("</ul></div></div></div>");
                //collapsed = true;
            }

            divContainer.InnerHtml = Convert.ToString(sbGames);
        }
    }
}
