using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Factories.Slots;
using Factories.Slots.Handlers;
using Models;

public partial class Slots_ClubBravado : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        SetTitle(commonCulture.ElementValues.getResourceXPathString("/Products/ClubBravado/Label", commonVariables.ProductsXML));

        GPIHandler handler = new GPIHandler(commonVariables.CurrentMemberSessionId);

        var gpiCategory = handler.Process();

        StringBuilder sbGames = new StringBuilder();

        foreach (var category in gpiCategory)
        {
            sbGames.AppendFormat("<div data-role='collapsible' data-collapsed='false' data-theme='b' data-content-theme='a' data-mini='true'><h4>{0}</h4>", category.Title);

            sbGames.AppendFormat("<div id='div{0}' class='div-product'><div><ul>", category.Title);

            AddGames(sbGames, category.New);

            AddGames(sbGames, category.Current);

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
                sbGames.AppendFormat("<a target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubBravado") + "' data-rel='dialog' data-transition='slidedown' data-ajax='false'>");
            else
                sbGames.AppendFormat("<a href='{0}' target='_blank' data-ajax='false'>", game.RealUrl);

            sbGames.AppendFormat("{0}</a>", commonCulture.ElementValues.getResourceXPathString("/Products/Play", commonVariables.ProductsXML));
            sbGames.AppendFormat("<a target='_blank' href='{1}'>{0}</a></div>", commonCulture.ElementValues.getResourceXPathString("/Products/Try", commonVariables.ProductsXML), game.FunUrl);

            sbGames.Append("</div></li>");
        }
    }
}