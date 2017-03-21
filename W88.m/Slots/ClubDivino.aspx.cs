using customConfig;
using Factories.Slots;
using Factories.Slots.Handlers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

public partial class Slots_ClubDivino : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        SetTitle(commonCulture.ElementValues.getResourceXPathString("/Products/ClubDivino/Label", commonVariables.ProductsXML));

        var opSettings = new OperatorSettings(System.Configuration.ConfigurationManager.AppSettings.Get("Operator"));
        var addGpi = Convert.ToBoolean(opSettings.Values.Get("GPIAddOtheClubs"));
        var sbGames = new StringBuilder();

        var bsHandler = new BSHandler(commonVariables.CurrentMemberSessionId, "ClubDivino", "FundTransfer", GetDevice());
        var bsCategory = bsHandler.Process();

        var cxHandler = new CTXMHandler(commonVariables.CurrentMemberSessionId);
        var cxCategory = cxHandler.Process();

        var uc8Handler = new UC8Handler(commonVariables.CurrentMemberSessionId, "ClubDivino", "FundTransfer");
        var uc8Category = uc8Handler.Process();

        IEnumerable<IGrouping<string, GameCategoryInfo>> games;
        if (addGpi)
        {
            var gpiHandler = new GPIHandler(commonVariables.CurrentMemberSessionId);
            var gpiCategory = gpiHandler.Process(true);
            cxCategory[0].Current = gpiHandler.InsertInjectedGames(gpiCategory, cxCategory[0].Current);

            games = bsCategory.Union(cxCategory).Union(uc8Category).Union(gpiCategory).GroupBy(x => x.Title);
        }
        else
        {
            games = bsCategory.Union(cxCategory).Union(uc8Category).GroupBy(x => x.Title);
        }

        foreach (var group in games)
        {
            AddGamesPerDevice(sbGames, group);
        }

        divContainer.InnerHtml = Convert.ToString(sbGames);
    }

    private void AddGamesPerDevice(StringBuilder sbGames, IGrouping<string, GameCategoryInfo> category)
    {
        sbGames.AppendFormat("<div data-role='collapsible' data-collapsed='false' data-theme='b' data-content-theme='a' data-mini='true' type='{1}'><h4>{0}</h4>", category.Key, Convert.ToString(GetDevice()));

        sbGames.AppendFormat("<div id='div{0}_{1}' class='div-product'><div><ul>", category.Key, Convert.ToString(GetDevice()));

        foreach (var item in category)
        {
            AddGames(sbGames, item.New);

            AddGames(sbGames, item.Current);
        }

        sbGames.Append("</ul></div></div></div>");
    }

    private void AddGames(StringBuilder sbGames, List<GameInfo> games)
    {
        foreach (var game in games)
        {
            var providerClass = string.Empty;
            if (!string.IsNullOrEmpty(game.Provider.ToString())) providerClass = "slot-" + game.Provider; 

            sbGames.AppendFormat("<li class='bkg-game {1}'><div rel='{0}.jpg'><div class='div-links'>", game.Image, providerClass);

            if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
            {
                sbGames.AppendFormat("<a target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubDivino") + "' data-rel='dialog' data-transition='slidedown' data-ajax='false'>");
            }
            else
                sbGames.AppendFormat("<a class=\"track-play-now\" href='{0}' target='_blank' data-ajax='false'>", game.RealUrl);

            sbGames.AppendFormat("{0}</a>", commonCulture.ElementValues.getResourceXPathString("/Products/Play", commonVariables.ProductsXML));
            sbGames.AppendFormat("<a class=\"track-try-now\" target='_blank' href='{0}' data-ajax='false'>{1}</a></div>", game.FunUrl, commonCulture.ElementValues.getResourceXPathString("/Products/Try", commonVariables.ProductsXML));

            sbGames.Append("</div></li>");
        }
    }

    private GameDevice GetDevice()
    {
        if (Request.UserAgent.Contains("Android") && Request.UserAgent.Contains("Mobile"))
            return GameDevice.ANDROID;

        if (Request.UserAgent.Contains("iPad") || Request.UserAgent.Contains("iPhone") || Request.UserAgent.Contains("iPod"))
            return GameDevice.IOS;

        if (Request.UserAgent.Contains("IEMobile"))
            return GameDevice.WP;

        return GameDevice.ANDROID;
    }
}