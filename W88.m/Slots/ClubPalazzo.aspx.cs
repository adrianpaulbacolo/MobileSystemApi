using customConfig;
using Factories.Slots;
using Factories.Slots.Handlers;
using Helpers;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Slots_ClubPalazzo : BasePage
{
    protected string javascriptToken;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        SetTitle(commonCulture.ElementValues.getResourceXPathString("/Products/ClubPalazzoSlots/Label", commonVariables.ProductsXML));

        var info = new Members().MemberData();
        var handler = new PTHandler(info.MemberCode, "ClubPalazzo", "LiveChat/Default.aspx", "Logout");
        var ptCategory = handler.Process();

        var opSettings = new OperatorSettings(System.Configuration.ConfigurationManager.AppSettings.Get("Operator"));
        var addGpi = Convert.ToBoolean(opSettings.Values.Get("GPIAddOtheClubs"));

        IEnumerable<IGrouping<string, GameCategoryInfo>> games;
        if (addGpi)
        {
            var gpiHandler = new GPIHandler(commonVariables.CurrentMemberSessionId);
            var gpiCategory = gpiHandler.Process(true);
            ptCategory[0].Current = gpiHandler.InsertInjectedGames(gpiCategory, ptCategory[0].Current);

            games = ptCategory.Union(gpiCategory).GroupBy(x => x.Title);
        }
        else
        {
            games = ptCategory.GroupBy(x => x.Title);
        }

        var sbGames = new StringBuilder();
        foreach (var category in games)
        {
            sbGames.AppendFormat("<div data-role='collapsible' data-collapsed='false' data-theme='b' data-content-theme='a' data-mini='true'><h4>{0}</h4>", category.Key);

            sbGames.AppendFormat("<div id='div{0}' class='div-product'><div><ul>", category.Key);
            
            foreach (var item in category)
            {
                    AddGames(sbGames, item.New, handler.user, handler.languageCode);
                    AddGames(sbGames, item.Current, handler.user, handler.languageCode);    
                }
                
            sbGames.Append("</ul></div></div></div>");
        }

        divContainer.InnerHtml = Convert.ToString(sbGames);
    }

    private void AddGames(StringBuilder sbGames, List<GameInfo> games, string userName, string langCode)
    {
        foreach (var game in games)
        {
            if (game.Provider == GameProvider.GPI)
            {
                var providerClass = string.Empty;
                if (!string.IsNullOrEmpty(game.Provider.ToString())) providerClass = "slot-" + game.Provider; 

                sbGames.AppendFormat("<li class='bkg-game {1}'><div rel='{0}.jpg'><div class='div-links'>", game.Image, providerClass);

                if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                    sbGames.AppendFormat("<a target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubBravado") + "' data-rel='dialog' data-transition='slidedown' data-ajax='false'>");
                else
                    sbGames.AppendFormat("<a class=\"track-play-now\" href='{0}' target='_blank' data-ajax='false'>", game.RealUrl);

                sbGames.AppendFormat("{0}</a>", commonCulture.ElementValues.getResourceXPathString("/Products/Play", commonVariables.ProductsXML));
                sbGames.AppendFormat("<a class=\"track-try-now\" target='_blank' href='{1}'>{0}</a></div>", commonCulture.ElementValues.getResourceXPathString("/Products/Try", commonVariables.ProductsXML), game.FunUrl);
            }
            else
            {
                sbGames.AppendFormat("<li class='bkg-game'><div rel='{0}.png'><div class='div-links'>", game.Image);

                if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                    sbGames.AppendFormat("<a class='btn-primary' target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubPalazzo") + "' data-rel='dialog' data-transition='slidedown' data-ajax='false'>");
                else
                    sbGames.AppendFormat("<a href='#' onclick='javascript:w88Mobile.Slots.launchPalazzo(1, \"{0}\", \"{1}\", \"{2}\", \"{3}\")' target='_blank' data-ajax='false'>",userName, commonEncryption.Decrypt(commonCookie.CookiePalazzo), langCode, game.RealUrl);

                sbGames.AppendFormat("{0}</a>",commonCulture.ElementValues.getResourceXPathString("/Products/Play", commonVariables.ProductsXML));
            }

            sbGames.Append("</div></li>");

        }
    }
}