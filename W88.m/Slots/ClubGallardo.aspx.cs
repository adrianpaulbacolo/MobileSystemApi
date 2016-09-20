﻿using Factories.Slots;
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

        var isbHandler = new ISBHandler(commonVariables.CurrentMemberSessionId, "ClubGallardo", commonVariables.GetSessionVariable("CurrencyCode"));
        var isbCategory = isbHandler.Process();

        var pngHandler = new PNGHandler(commonVariables.CurrentMemberSessionId, "ClubGallardo");
        var pngCategory = pngHandler.Process();

        var gpiHandler = new GPIHandler(commonVariables.CurrentMemberSessionId);
        var gpiCategory = gpiHandler.Process(true);
        isbCategory[0].Current = gpiHandler.InsertInjectedGames(gpiCategory, isbCategory[0].Current);

        var gallardo = isbCategory.Union(pngCategory).Union(gpiCategory).GroupBy(x => x.Title);

        StringBuilder sbGames = new StringBuilder();
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
            var providerClass = string.Empty;
            if (!string.IsNullOrEmpty(game.Provider.ToString())) providerClass = "slot-" + game.Provider; 

            sbGames.AppendFormat("<li class='bkg-game {1}'><div rel='{0}.jpg'><div class='div-links'>", game.Image, providerClass);

            if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                sbGames.AppendFormat("<a target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubGallardo") + "' data-rel='dialog' data-transition='slidedown' data-ajax='false'>");
            else
                sbGames.AppendFormat("<a class=\"track-play-now\" href='{0}' target='_blank' data-ajax='false'>", game.RealUrl);

            sbGames.AppendFormat("{0}</a>", commonCulture.ElementValues.getResourceXPathString("/Products/Play", commonVariables.ProductsXML));
            sbGames.AppendFormat("<a class=\"track-try-now\" target='_blank' href='{0}' data-ajax='false'>{1}</a></div>", game.FunUrl, commonCulture.ElementValues.getResourceXPathString("/Products/Try", commonVariables.ProductsXML));

            sbGames.Append("</div></li>");
        }
    }
}