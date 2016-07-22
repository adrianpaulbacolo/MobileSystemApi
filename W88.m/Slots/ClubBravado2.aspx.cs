using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class Slots_ClubBravado : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    private System.Xml.Linq.XElement xeResources = null;

    private List<string> _allLangSupport = new List<string>();
    private string _LanguageCode = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string strGameId = string.Empty;
        commonCulture.appData.getRootResource("/Slots/ClubBravado.aspx", out xeResources);

        if (Page.IsPostBack) return;

        SetTitle(commonCulture.ElementValues.getResourceXPathString("/Products/ClubBravado/Label", commonVariables.ProductsXML));
        StringBuilder sbGames = new StringBuilder();
        XElement xeCategories = xeResources.Element("Category");

        _allLangSupport.Add("sevenwonders");
        _allLangSupport.Add("fortunekoi");
        _allLangSupport.Add("ladyluck");
        _allLangSupport.Add("godofgamblers");
        _allLangSupport.Add("sevenbrothers");
        _allLangSupport.Add("deepblue");
        _allLangSupport.Add("zeus");

        var selectedLang = commonVariables.SelectedLanguage;

        foreach (System.Xml.Linq.XElement xeCategory in xeCategories.Elements())
        {
            sbGames.AppendFormat("<div data-role='collapsible' data-collapsed='false' data-theme='b' data-content-theme='a' data-mini='true'><h4>{0}</h4>", xeCategory.Attribute("Label").Value);

            sbGames.AppendFormat("<div id='div{0}' class='div-product'><div><ul>", xeCategory.Name);

            List<XElement> topgames = xeCategory.Elements().Where(m => m.Attribute("Top") != null).OrderBy(f => f.Attribute("Top").Value).ToList();

            IEnumerable<XElement> sortedGame = xeCategory.Elements().Where(m => m.Attribute("Top") == null).OrderBy(game => game.Name.ToString());

            topgames.AddRange(sortedGame);

            foreach (XElement xeGame in topgames)
            {
                strGameId = (xeGame.Attribute("ProductId") == null ? "" : xeGame.Attribute("ProductId").Value);
                sbGames.AppendFormat("<li class='bkg-game'><div rel='{0}.jpg'><div class='div-links'>", commonCulture.ElementValues.getResourceString("ImageName", xeGame));

                var gameSupport = _allLangSupport.Exists(x => x.Contains(xeGame.Name.ToString()));
                if (gameSupport)
                {
                    _LanguageCode = commonVariables.SelectedLanguageShort;
                }
                else
                {
                    SetDefaultLang(selectedLang);
                }

                bool isInternal = false;
                string slotType = string.Empty;
                if (commonCulture.ElementValues.getResourceString("IsInternal", xeGame) != "")
                    isInternal = bool.Parse(commonCulture.ElementValues.getResourceString("IsInternal", xeGame));

                if (commonCulture.ElementValues.getResourceString("SlotType", xeGame) != "")
                    slotType = Convert.ToString(commonCulture.ElementValues.getResourceString("SlotType", xeGame));

                //in-house
                if (isInternal)
                {
                    if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                        sbGames.AppendFormat("<a target='_blank' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubBravado") + "' data-rel='dialog' data-transition='slidedown'>");
                    else
                        if (slotType == "RSLOT")
                        {
                            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");
                            sbGames.AppendFormat("<a href='{0}' target='_blank'>", commonClubBravado.getRealUrl_mrslot.Replace("{GAME}", Convert.ToString(xeGame.Name)).Replace("{LANG}", _LanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));
                        }
                        else
                        {
                            sbGames.AppendFormat("<a href='{0}' target='_blank'>", commonClubBravado.getRealUrl.Replace("{GAME}", Convert.ToString(xeGame.Name)).Replace("{LANG}", _LanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));
                        }

                    sbGames.AppendFormat("{0}</a>", commonCulture.ElementValues.getResourceXPathString("/Products/Play", commonVariables.ProductsXML));

                    if (slotType == "RSLOT")
                    {
                        sbGames.AppendFormat("<a target='_blank' href='{1}' data-ajax='false'>{0}</a></div>", commonCulture.ElementValues.getResourceXPathString("/Products/Try", commonVariables.ProductsXML), commonClubBravado.getFunUrl_mrslot.Replace("{GAME}", Convert.ToString(xeGame.Name)).Replace("{LANG}", _LanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));
                    }
                    else
                    {
                        sbGames.AppendFormat("<a target='_blank' href='{1}' data-ajax='false'>{0}</a></div>", commonCulture.ElementValues.getResourceXPathString("/Products/Try", commonVariables.ProductsXML), commonClubBravado.getFunUrl.Replace("{GAME}", Convert.ToString(xeGame.Name)).Replace("{LANG}", _LanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));
                    }
                }
                else
                {
                    string newstrLanguageCode = (_LanguageCode == "zh") ? "zh_CN" : "en_GB";


                    if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                        sbGames.AppendFormat("<a class='btn-primary' href='/_Secure/Login.aspx?redirect=" + Server.UrlEncode("/ClubBravado") + "' data-rel='dialog' data-transition='slidedown'>");
                    else
                        sbGames.AppendFormat("<a href='{0}'>", commonClubBravado.getThirdPartyRealUrl.Replace("{GAME}", commonCulture.ElementValues.getResourceString("ImageName", xeGame)).Replace("{LANG}", newstrLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));

                    sbGames.AppendFormat("{0}</a>", commonCulture.ElementValues.getResourceXPathString("/Products/Play", commonVariables.ProductsXML));
                    sbGames.AppendFormat("<a href='{1}' target='_blank' data-ajax='false'>{0}</i></a></div>", commonCulture.ElementValues.getResourceXPathString("/Products/Try", commonVariables.ProductsXML), commonClubBravado.getThirdPartyFunUrl.Replace("{GAME}", commonCulture.ElementValues.getResourceString("ImageName", xeGame)).Replace("{LANG}", newstrLanguageCode).Replace("{TOKEN}", commonVariables.CurrentMemberSessionId));
                }

                sbGames.Append("</div></li>");
            }

            sbGames.Append("</ul></div></div></div>");
            //collapsed = true;
        }

        divContainer.InnerHtml = Convert.ToString(sbGames);
    }

    private void SetDefaultLang(string selectedLang)
    {
        switch (selectedLang)
        {
            case "zh-cn":
                _LanguageCode = "zh";
                break;

            default:
                _LanguageCode = "en";
                break;
        }
    }
}