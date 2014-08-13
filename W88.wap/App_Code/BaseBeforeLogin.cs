using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class BaseBeforeLogin : System.Web.UI.Page
{
    protected override void OnPreInit(EventArgs e)
    {
        if (string.Compare(System.Configuration.ConfigurationManager.AppSettings.Get("ClearWebCache"), "true", true) == 0)
        {
            foreach (System.Collections.DictionaryEntry deCache in System.Web.HttpContext.Current.Cache)
            {
                HttpContext.Current.Cache.Remove(Convert.ToString(deCache.Key));
            }
        }

        base.OnPreInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        string strMemberSessionId = string.Empty;

        strMemberSessionId = commonVariables.CurrentMemberSessionId;

        #region InitialiseProductLinks
        //System.Web.UI.HtmlControls.HtmlAnchor aMnuSports = base.FindControl("aMnuSports") as System.Web.UI.HtmlControls.HtmlAnchor;
        //System.Web.UI.HtmlControls.HtmlAnchor aMnuLottery = base.FindControl("aMnuLottery") as System.Web.UI.HtmlControls.HtmlAnchor;
        //System.Web.UI.HtmlControls.HtmlAnchor aMnuSlots = base.FindControl("aMnuSlots") as System.Web.UI.HtmlControls.HtmlAnchor;
        //System.Web.UI.HtmlControls.HtmlAnchor aMnuLiveCasino = base.FindControl("aMnuLiveCasino") as System.Web.UI.HtmlControls.HtmlAnchor;

        //System.Web.UI.HtmlControls.HtmlAnchor aSports = base.FindControl("aSports") as System.Web.UI.HtmlControls.HtmlAnchor;
        //System.Web.UI.HtmlControls.HtmlAnchor aLottery = base.FindControl("aLottery") as System.Web.UI.HtmlControls.HtmlAnchor;
        //System.Web.UI.HtmlControls.HtmlAnchor aSlots = base.FindControl("aSlots") as System.Web.UI.HtmlControls.HtmlAnchor;
        //System.Web.UI.HtmlControls.HtmlAnchor aLiveCasino = base.FindControl("aLiveCasino") as System.Web.UI.HtmlControls.HtmlAnchor;

        System.Web.UI.HtmlControls.HtmlGenericControl divNavPanel = base.FindControl("divNavPanel") as System.Web.UI.HtmlControls.HtmlGenericControl;
        
        if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
        {
            //if (aMnuSports != null) { aMnuSports.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Sports/Url", commonVariables.ProductsXML).Replace("{SportsbookUrl}", commonSports.getSportsbookUrl).Replace("{language}", commonSports.getSportsLanguageId(commonVariables.SelectedLanguage)).Replace("{token}", commonVariables.GetSessionVariable("MemberSessionId")); aMnuSports.Attributes.Remove("data-rel"); }
            //if (aMnuLottery != null) { aMnuLottery.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Lottery/Url", commonVariables.ProductsXML).Replace("{KenoUrl}", commonLottery.getKenoUrl).Replace("{language}", commonVariables.SelectedLanguage); aMnuLottery.Attributes.Remove("data-rel"); }
            //if (aMnuSlots != null) { aMnuSlots.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Slots/Url", commonVariables.ProductsXML); }
            //if (aMnuLiveCasino != null) { aMnuLiveCasino.HRef = commonCulture.ElementValues.getResourceXPathString("Products/LiveCasino/Url", commonVariables.ProductsXML); }

            //if (aSports != null) { aSports.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Sports/Url", commonVariables.ProductsXML).Replace("{SportsbookUrl}", commonSports.getSportsbookUrl).Replace("{language}", commonSports.getSportsLanguageId(commonVariables.SelectedLanguage)).Replace("{token}", commonVariables.GetSessionVariable("MemberSessionId")); aSports.Attributes.Remove("data-rel"); }
            //if (aLottery != null) { aLottery.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Lottery/Url", commonVariables.ProductsXML).Replace("{KenoUrl}", commonLottery.getKenoUrl).Replace("{language}", commonVariables.SelectedLanguage); aLottery.Attributes.Remove("data-rel"); }
            //if (aSlots != null) { aSlots.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Slots/Url", commonVariables.ProductsXML); }
            //if (aLiveCasino != null) { aLiveCasino.HRef = commonCulture.ElementValues.getResourceXPathString("Products/LiveCasino/Url", commonVariables.ProductsXML); }
        }
        else 
        {
            //divNavPanel.Visible = false;
        }

        #endregion

        base.OnLoad(e);
    }
}