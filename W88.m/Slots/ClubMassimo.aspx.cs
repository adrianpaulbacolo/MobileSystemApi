using customConfig;
using Factories.Slots;
using Factories.Slots.Handlers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Xml.Linq;

public partial class Slots_ClubMassimo : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        SetTitle(commonCulture.ElementValues.getResourceXPathString("/Products/ClubMassimoSlots/Label", commonVariables.ProductsXML));

        var opSettings = new OperatorSettings(System.Configuration.ConfigurationManager.AppSettings.Get("Operator"));
        var addGpi = Convert.ToBoolean(opSettings.Values.Get("GPIAddOtheClubs"));

        var handler = new MGSHandler(commonVariables.CurrentMemberSessionId, "ClubMassimo", "FundTransfer");
        var mgsCategory = handler.Process();

        IEnumerable<IGrouping<string, GameCategoryInfo>> games;
        if (addGpi)
        {
            var gpiHandler = new GPIHandler(commonVariables.CurrentMemberSessionId);
            var gpiCategory = gpiHandler.Process(true);
            mgsCategory[0].Current = gpiHandler.InsertInjectedGames(gpiCategory, mgsCategory[0].Current);

            games = mgsCategory.Union(gpiCategory).GroupBy(x => x.Title);
        }
        else
        {
            games = mgsCategory.GroupBy(x => x.Title);
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
                sbGames.AppendFormat("<a target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubMassimo") + "' data-rel='dialog' data-transition='slidedown' data-ajax='false'>");
            else
                sbGames.AppendFormat("<a class=\"track-play-now\" href='{0}' target='_blank' data-ajax='false'>", game.RealUrl);

            sbGames.AppendFormat("{0}</a>", commonCulture.ElementValues.getResourceXPathString("/Products/Play", commonVariables.ProductsXML));
            sbGames.AppendFormat("<a class=\"track-try-now\" data-ajax='false' target='_blank' href='{1}'>{0}</a></div>", commonCulture.ElementValues.getResourceXPathString("/Products/Try", commonVariables.ProductsXML), game.FunUrl);

            sbGames.Append("</div></li>");
        }
    }
}