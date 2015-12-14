using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Slots_ClubMassimo : BasePage
{

    protected System.Xml.Linq.XElement xeErrors = null;
    private System.Xml.Linq.XElement xeResources = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        commonCulture.appData.getRootResource("/Slots/ClubMassimo.aspx", out xeResources);

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

                    sbGames.AppendFormat("<li rel='{0}.jpg' class='bkg-game'><div class='div-links'>", commonCulture.ElementValues.getResourceString("ImageName", xeGame));

                    //{RealUrl}/cashapillar/en?casinoID=5053&loginType=VanguardSessionToken&isRGI=true&bankingURL={cashier}&authToken={token}&lobbyURL={lobby}
                    //{FunUrl}/cashapillar/en?casinoID=5002&loginType=VanguardSessionToken&isRGI=true&authToken=&isPracticePlay=true&bankingURL=&lobbyURL={lobby}

                    if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) { sbGames.AppendFormat("<a class='btn-primary' target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubMassimo") + "' data-rel='dialog' data-transition='slidedown'>"); }
                    else { sbGames.AppendFormat("<a href='{0}' target='_blank'>", commonCulture.ElementValues.getResourceString("PlayForRealURL", xeGame).Replace("{RealUrl}", commonClubMassimo.getRealUrl).Replace("{token}", commonVariables.CurrentMemberSessionId)).Replace("{lobby}", "m.w88988.com/ClubMassimo").Replace("{cashier}", "m.w88988.com/fundtransfer"); }

                    sbGames.Append("<i class='icon-play_arrow'></i></a>");
                    sbGames.AppendFormat("<a class='btn-secondary' target='_blank' href='{0}' data-ajax='false'><i class='icon-fullscreen'></i></a></div>", commonCulture.ElementValues.getResourceString("PlayForFunURL", xeGame).Replace("{FunUrl}", commonClubMassimo.getFunUrl).Replace("{token}", commonVariables.CurrentMemberSessionId)).Replace("{lobby}", "m.w88.com/ClubMassimo");
                    sbGames.Append("</li>");
                }

                sbGames.Append("</ul></div></div></div>");
                collapsed = true;
            }

            divContainer.InnerHtml = Convert.ToString(sbGames);
        }
    }
}
