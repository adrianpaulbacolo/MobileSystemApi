using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class Slots_ClubPalazzo: BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    private System.Xml.Linq.XElement xeResources = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strGameId = string.Empty;
        string strLanguageCode = string.Empty;

        commonCulture.appData.getRootResource("/Slots/ClubPalazzo.aspx", out xeResources);

        if (!Page.IsPostBack)
        {
            System.Text.StringBuilder sbGames = new System.Text.StringBuilder();

            System.Xml.Linq.XElement xeCategories = xeResources.Element("Category");

            switch (commonVariables.SelectedLanguage)
            {
                case "ko-kr":
                    strLanguageCode = "ko";
                    break;
                case "th-th":
                    strLanguageCode = "th";
                    break;
                case "zh-cn":
                    strLanguageCode = "zh-cn";
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
                    sbGames.AppendFormat("<li class='bkg-game'><div rel='{0}.png'><div class='div-links'>", commonCulture.ElementValues.getResourceString("ImageName", xeGame));

                    //Real URL
                    if (commonCulture.ElementValues.getResourceString("PlayForReal", xeGame) == "true")
                    { 
                        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                            sbGames.AppendFormat("<a class='btn-primary' target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubPalazzo") + "' data-rel='dialog' data-transition='slidedown'>");
                        else
                            sbGames.AppendFormat(
                                "<a href=\"/Slots/ClubPalazzoLauncher.aspx?type={0}&name={1}&mode={2}\" target='_blank'>", 
                                commonCulture.ElementValues.getResourceString("Type", xeGame), 
                                commonCulture.ElementValues.getResourceString("ImageName", xeGame), 
                                "real");
                        sbGames.Append("<i class='icon-play_arrow'></i></a>");
                    }

                    sbGames.Append("</div></li>");
                }

                sbGames.Append("</ul></div></div></div>");
            }

            divContainer.InnerHtml = Convert.ToString(sbGames);
        }
    }
}
