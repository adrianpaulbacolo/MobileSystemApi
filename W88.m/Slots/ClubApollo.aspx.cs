using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using customConfig;
using Factories.Slots.Handlers;
using Helpers;
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

        if (Request.QueryString["provider"] != null && Request.QueryString["gameId"] != null)
        {
            var provider = Request.QueryString.Get("provider");
            var gameId = Request.QueryString.Get("gameId");
            var gameName = Request.QueryString.Get("gameName");
            var gameType = Request.QueryString.Get("gameType");
            var lang = Request.QueryString.Get("gameLang");
            var lobby = Request.QueryString.Get("lobby");

            if (provider == Convert.ToString(GameProvider.TTG))
            {
                var url = GameSettings.GetGameUrl(GameProvider.TTG, GameLinkSetting.Real);
                url = url.Replace("{GAME}", gameId)
                    .Replace("{LANG}", lang)
                    .Replace("{LOBBY}", lobby)
                    .Replace("{GAMETYPE}", gameType)
                    .Replace("{GAMENAME}", gameName).Replace("{TOKEN}", userInfo.CurrentSessionId);

                if (XDocument.Load(url).Root.Element("error_code").Value == "0")
                {
                    var gamelink = XDocument.Load(url).Root.Element("redirect_url").Value;
                    Response.Redirect(gamelink, true);
                }
                else
                {
                    var sc = "<script>$(document).ready( function () { window.w88Mobile.Growl.shout('" + XDocument.Load(url).Root.Element("error_msg").Value + "', function () {window.close();} ); });</script>";
                    ClientScript.RegisterClientScriptBlock(this.GetType(), Guid.NewGuid().ToString(), sc, false);
                }
            }
        }

        if (Page.IsPostBack) return;

        CheckSupportedCurrency();

        SetTitle(commonCulture.ElementValues.getResourceXPathString("/Products/ClubApollo/Label", commonVariables.ProductsXML));

        var opSettings = new OperatorSettings(System.Configuration.ConfigurationManager.AppSettings.Get("Operator"));
        var addGpi = Convert.ToBoolean(opSettings.Values.Get("GPIAddOtheClubs"));

        var handler = new QTHandler(commonVariables.CurrentMemberSessionId, "ClubApollo");
        var qtCategory = handler.Process();

        var ppHandler = new PPHandler(commonVariables.CurrentMemberSessionId, "ClubApollo", "FundTransfer");
        var ppCategory = ppHandler.Process(true);

        var ttgHandler = new TTGHandler(commonVariables.CurrentMemberSessionId, "ClubApollo", "FundTransfer");
        var ttgCategory = ttgHandler.Process(true);

        IEnumerable<IGrouping<string, GameCategoryInfo>> games;
        if (addGpi)
        {
        var gpiHandler = new GPIHandler(commonVariables.CurrentMemberSessionId);
        var gpiCategory = gpiHandler.Process(true);

        qtCategory[0].Current = handler.InsertInjectedGames(gpiCategory, qtCategory[0].Current);

            games = qtCategory.Union(ppCategory).Union(ttgCategory).Union(gpiCategory).GroupBy(x => x.Title);
        }
        else
        {
            games = qtCategory.Union(ppCategory).Union(ttgCategory).GroupBy(x => x.Title);
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
            {
                sbGames.AppendFormat("<a class='btn-primary' target='_blank' href='/_Secure/Login.aspx?redirect=" +
                                     Server.UrlEncode("/ClubApollo") +
                                     "' data-rel='dialog' data-transition='slidedown' data-ajax='false'>");
            }
            else
            {
                if (game.Provider == GameProvider.TTG)
                {
                    var realUrl = new Uri(game.RealUrl);
                    var gameId = HttpUtility.ParseQueryString(realUrl.Query).Get("gameId");
                    var gameName = HttpUtility.ParseQueryString(realUrl.Query).Get("gameName");
                    var gameType = HttpUtility.ParseQueryString(realUrl.Query).Get("gameType");
                    var lang = HttpUtility.ParseQueryString(realUrl.Query).Get("lang");
                    var lobby = HttpUtility.ParseQueryString(realUrl.Query).Get("lobbyURL");

                    sbGames.AppendFormat(
                        "<a class=\"track-play-now\" href='#' onclick='javascript:w88Mobile.Slots.launchTTG(\"{0}\", \"{1}\", \"{2}\", \"{3}\", \"{4}\")' target='_blank' data-ajax='false'>",
                        gameId, gameName, gameType, lang, "ClubApollo");
                }
            else
                {
                    sbGames.AppendFormat("<a class=\"track-play-now\" href='{0}' target='_blank' data-ajax='false'>",
                        game.RealUrl);
                }

            }

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