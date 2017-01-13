using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Factories.Slots.Handlers;
using Models;
using Factories.Slots;

public partial class Slots_ClubApollo : BasePage
{
    protected XElement xeErrors = null;
    private XElement xeResources = null;
    private string _selectedLanguage;
    private string _currencyCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        CheckSupportedCurrency();

        SetTitle(commonCulture.ElementValues.getResourceXPathString("/Products/ClubApollo/Label", commonVariables.ProductsXML));

        var handler = new QTHandler(commonVariables.CurrentMemberSessionId, "ClubApollo");
        var qtCategory = handler.Process();

        var ppHandler = new PPHandler(commonVariables.CurrentMemberSessionId, "ClubApollo", "FundTransfer");
        var ppCategory = ppHandler.Process(true);

        //var gpiHandler = new GPIHandler(commonVariables.CurrentMemberSessionId);
        //var gpiCategory = gpiHandler.Process(true);

        //qtCategory[0].Current = handler.InsertInjectedGames(gpiCategory, qtCategory[0].Current);

        //var games = qtCategory.Union(ppCategory).Union(gpiCategory).GroupBy(x => x.Title);
        var games = qtCategory.Union(ppCategory).GroupBy(x => x.Title);

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
                sbGames.AppendFormat("<a class='btn-primary' target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubApollo") + "' data-rel='dialog' data-transition='slidedown' data-ajax='false'>");
            else
                sbGames.AppendFormat("<a class=\"track-play-now\" href='{0}' target='_blank' data-ajax='false'>", game.RealUrl);

            sbGames.AppendFormat("{0}</a>", commonCulture.ElementValues.getResourceXPathString("/Products/Play", commonVariables.ProductsXML));
            sbGames.AppendFormat("<a class=\"track-try-now\" target='_blank' href='{1}'>{0}</a></div>", commonCulture.ElementValues.getResourceXPathString("/Products/Try", commonVariables.ProductsXML), game.FunUrl);

            sbGames.Append("</div></li>");
        }
    }

    private void CheckSupportedCurrency()
    {
        if (commonVariables.GetSessionVariable("clubapollo") == "0")
        {
            Response.Redirect("~/Index", true);
        }
    }
}