using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Slots_Default : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    private System.Xml.Linq.XElement xeResources = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        commonCulture.appData.getRootResource("/Slots/Default.aspx", out xeResources);

        if (!Page.IsPostBack)
        {
            System.Text.StringBuilder sbGames = new System.Text.StringBuilder();

            System.Xml.Linq.XElement xeCategories = xeResources.Element("Category");

            bool collapsed = false;

            foreach (System.Xml.Linq.XElement xeCategory in xeCategories.Elements())
            {
                sbGames.AppendFormat("<div data-role='collapsible' data-collapsed='false' data-theme='b' data-content-theme='a' data-mini='true'><h4>{0}</h4>", xeCategory.Attribute("Label").Value);

                sbGames.AppendFormat("<div id='div{0}' class='div-product'><div><ul>", xeCategory.Name);

                foreach (System.Xml.Linq.XElement xeGame in xeCategory.Elements())
                {
                    sbGames.AppendFormat("<li rel='{0}-{1}.jpg' class='bkg-game'><div class='div-links'>", xeCategory.Name, xeGame.Name);
              
                    if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) { sbGames.AppendFormat("<a class='btn-primary' target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubCrescendo") + "' data-rel='dialog' data-transition='slidedown'>"); }
                    else { sbGames.AppendFormat("<a href='{0}' target='_blank'>", commonCulture.ElementValues.getResourceString("PlayForRealURL", xeGame).Replace("{SlotsUrl}", commonClubCrescendo.getSlotsUrl).Replace("{token}", commonVariables.CurrentMemberSessionId)); }

                    sbGames.Append("<i class='icon-play_arrow'></i></a>");
                    sbGames.AppendFormat("<a class='btn-secondary' target='_blank' href='{0}' data-ajax='false'><i class='icon-fullscreen'></i></a></div>", commonCulture.ElementValues.getResourceString("PlayForFunURL", xeGame).Replace("{SlotsUrl}", commonClubCrescendo.getSlotsUrl).Replace("{token}", commonVariables.CurrentMemberSessionId));
                    sbGames.Append("</li>");
                }

                sbGames.Append("</ul></div></div></div>");
                collapsed = true;
            }

            divContainer.InnerHtml = Convert.ToString(sbGames);
        }
    }
}
