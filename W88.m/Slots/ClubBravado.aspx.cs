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

        var handler = new GPIHandler(commonVariables.CurrentMemberSessionId);

        var gpiCategory = handler.Process();

        StringBuilder sbGames = new StringBuilder();

        foreach (var category in gpiCategory)
        {
            sbGames.AppendFormat("<div id='div{0}' class='box'><div class='game-card-box'>", category.Title);

            AddGames(sbGames, category.New);

            AddGames(sbGames, category.Current);

            sbGames.Append("</div></div>");
        }

        divContainer.InnerHtml = Convert.ToString(sbGames);
    }

    private void AddGames(StringBuilder sbGames, List<GameInfo> games)
    {
        foreach (var game in games)
        {
            var providerClass = string.Empty;
            if (!string.IsNullOrEmpty(game.Provider.ToString())) providerClass = "slot-" + game.Provider; 

            sbGames.AppendFormat("<a href='#' class='game-card {0}' onclick='javascript:w88Mobile.Slots.showGameModal(\"{3}.jpg\", \"{1}\", \"{2}\")'><div rel='{3}.jpg'></div></a>", providerClass, game.RealUrl, game.FunUrl, game.Image);
        }
    }
}