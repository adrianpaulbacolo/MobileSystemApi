using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using customConfig;
using Factories.Slots.Handlers;
using Models;

public partial class Slots_ClubNuovo : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        //var handler = new GNSHandler(commonVariables.CurrentMemberSessionId, "ClubNuovo", "FundTransfer");
        //var gnsCategory = handler.Process();

        var plsHandler = new PLSHandler(commonVariables.CurrentMemberSessionId, "ClubNuovo", "FundTransfer");
        var plsCategory = plsHandler.Process();

        var mrsHandler = new MRSHandler(commonVariables.CurrentMemberSessionId, "ClubNuovo", "FundTransfer");
        var mrsCategory = mrsHandler.Process();

        var opSettings = new OperatorSettings(System.Configuration.ConfigurationManager.AppSettings.Get("Operator"));
        var addGpi = Convert.ToBoolean(opSettings.Values.Get("GPIAddOtheClubs"));

        IEnumerable<IGrouping<string, GameCategoryInfo>> games;
        if (addGpi)
        {
            var gpiHandler = new GPIHandler(commonVariables.CurrentMemberSessionId);
            var gpiCategory = gpiHandler.Process(true);
            //gnsCategory[0].Current = gpiHandler.InsertInjectedGames(gpiCategory, gnsCategory[0].Current);
            //games = gnsCategory.Union(plsCategory).Union(gpiCategory).GroupBy(x => x.Title);

            plsCategory[0].Current = gpiHandler.InsertInjectedGames(gpiCategory, plsCategory[0].Current);
            games = plsCategory.Union(mrsCategory).Union(gpiCategory).GroupBy(x => x.Title);
        }
        else
        {
            games = plsCategory.Union(mrsCategory).GroupBy(x => x.Title);
        }

        var sbGames = new StringBuilder();
        foreach (var category in games)
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
            var providerClass = string.Empty;
            if (!string.IsNullOrEmpty(game.Provider.ToString())) providerClass = "slot-" + game.Provider;

            sbGames.AppendFormat("<li class='bkg-game {1}'><div rel='{0}.jpg'><div class='div-links'>", game.Image, providerClass);

            if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                sbGames.AppendFormat("<a target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubNuovo") + "' data-rel='dialog' data-transition='slidedown' data-ajax='false'>");
            else
                sbGames.AppendFormat("<a class=\"track-play-now\" href='{0}' target='_blank' data-ajax='false'>", game.RealUrl);

            sbGames.AppendFormat("{0}</a>", commonCulture.ElementValues.getResourceXPathString("/Products/Play", commonVariables.ProductsXML));
            sbGames.AppendFormat("<a class=\"track-try-now\" data-ajax='false' target='_blank' href='{1}'>{0}</a></div>", commonCulture.ElementValues.getResourceXPathString("/Products/Try", commonVariables.ProductsXML), game.FunUrl);

            sbGames.Append("</div></li>");
        }
    }
}