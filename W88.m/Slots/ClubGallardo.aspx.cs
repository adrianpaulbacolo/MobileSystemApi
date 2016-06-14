using Factories.Slots.Handlers;
using Models;
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
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        SetTitle(commonCulture.ElementValues.getResourceXPathString("/Products/ClubGallardo/Label", commonVariables.ProductsXML));

        StringBuilder sbGames = new StringBuilder();

        var isbHandler = new ISBHandler(commonVariables.CurrentMemberSessionId, "ClubGallardo", commonVariables.GetSessionVariable("CurrencyCode"));
        var isbCategory = isbHandler.Process();

        var pngHandler = new PNGHandler(commonVariables.CurrentMemberSessionId, "ClubGallardo");
        var pngCategory = pngHandler.Process();

        var gallardo = isbCategory.Union(pngCategory).GroupBy(x => x.Title);

        foreach (var category in gallardo)
        {
            sbGames.AppendFormat("<div data-role='collapsible' data-collapsed='false' data-theme='b' data-content-theme='a' data-mini='true'><h4>{0}</h4>", category.Key);

            sbGames.AppendFormat("<div id='div{0}' class='div-product'><div><ul>", category.Key);

            foreach (var item in category)
            {
                AddGames(sbGames, item.New);

                AddGames(sbGames, item.Current);
            }

            sbGames.Append("</ul></div></div></div>");
        }

        divContainer.InnerHtml = Convert.ToString(sbGames);
    }

    private void AddGames(StringBuilder sbGames, List<GameInfo> games)
    {
        foreach (var game in games)
        {
            sbGames.AppendFormat("<li class='bkg-game'><div rel='{0}.jpg'><div class='div-links'>", game.Image);

            if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                sbGames.AppendFormat("<a class='btn-primary' target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubGallardo") + "' data-rel='dialog' data-transition='slidedown' data-ajax='false'>");
            else
                sbGames.AppendFormat("<a href='{0}' target='_blank' data-ajax='false'>", game.RealUrl);

            sbGames.Append("<i class='icon-play_arrow'></i></a>");
            sbGames.AppendFormat("<a class='btn-secondary' target='_blank' href='{0}' data-ajax='false'><i class='icon-fullscreen'></i></a></div>", game.FunUrl);

            sbGames.Append("</div></li>");
        }
    }
}