using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class BasePage : System.Web.UI.Page
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
        string strLanguage = HttpContext.Current.Request.QueryString.Get("lang");

        strMemberSessionId = commonVariables.CurrentMemberSessionId;
        if (!string.IsNullOrEmpty(strLanguage)) { commonVariables.SelectedLanguage = strLanguage; }

        if (string.IsNullOrEmpty(commonVariables.GetSessionVariable("LoginStatus")) && !string.IsNullOrEmpty(strMemberSessionId))
        {
            Response.Redirect("/_Secure/ProcessLoginBySessionId.html" + (!string.IsNullOrEmpty(strLanguage) ? "?lang=" + strLanguage : ""), true);
        }
        else if (string.IsNullOrEmpty(strMemberSessionId) && string.Compare(commonVariables.GetSessionVariable("LoginStatus"), "success", true) == 0)
        {
            //Response.Redirect("/Expire", true);
            Response.Write("<script type='text/javascript'>window.location.replace('/Expire');</script>");
        }
        else if (string.IsNullOrEmpty(strMemberSessionId))
        {
            //Response.Redirect("/Default.aspx", true);
            //Response.Write("<script type='text/javascript'>window.location.replace('/Expire');</script>");
        }
        else
        {
            #region InitialiseLinks

            #region leftMenu
            //System.Web.UI.HtmlControls.HtmlAnchor aMnuSports = base.FindControl("aMnuSports") as System.Web.UI.HtmlControls.HtmlAnchor;
            //System.Web.UI.HtmlControls.HtmlAnchor aMnuLottery = base.FindControl("aMnuLottery") as System.Web.UI.HtmlControls.HtmlAnchor;
            //System.Web.UI.HtmlControls.HtmlAnchor aMnuSlots = base.FindControl("aMnuSlots") as System.Web.UI.HtmlControls.HtmlAnchor;
            //System.Web.UI.HtmlControls.HtmlAnchor aMnuLiveCasino = base.FindControl("aMnuLiveCasino") as System.Web.UI.HtmlControls.HtmlAnchor;

            //System.Web.UI.HtmlControls.HtmlAnchor aMnuDeposit = base.FindControl("aMnuDeposit") as System.Web.UI.HtmlControls.HtmlAnchor;
            //System.Web.UI.HtmlControls.HtmlAnchor aMnuTransfer = base.FindControl("aMnuTransfer") as System.Web.UI.HtmlControls.HtmlAnchor;
            //System.Web.UI.HtmlControls.HtmlAnchor aMnuWithdrawal = base.FindControl("aMnuWithdrawal") as System.Web.UI.HtmlControls.HtmlAnchor;
            #endregion

            #region indexPage
            //System.Web.UI.HtmlControls.HtmlAnchor aSports = base.FindControl("aSports") as System.Web.UI.HtmlControls.HtmlAnchor;
            //System.Web.UI.HtmlControls.HtmlAnchor aLottery = base.FindControl("aLottery") as System.Web.UI.HtmlControls.HtmlAnchor;
            //System.Web.UI.HtmlControls.HtmlAnchor aSlots = base.FindControl("aSlots") as System.Web.UI.HtmlControls.HtmlAnchor;
            //System.Web.UI.HtmlControls.HtmlAnchor aLiveCasino = base.FindControl("aLiveCasino") as System.Web.UI.HtmlControls.HtmlAnchor;

            //System.Web.UI.HtmlControls.HtmlAnchor aDeposit = base.FindControl("aDeposit") as System.Web.UI.HtmlControls.HtmlAnchor;
            //System.Web.UI.HtmlControls.HtmlAnchor aTransfer = base.FindControl("aTransfer") as System.Web.UI.HtmlControls.HtmlAnchor;
            //System.Web.UI.HtmlControls.HtmlAnchor aWithdrawal = base.FindControl("aWithdrawal") as System.Web.UI.HtmlControls.HtmlAnchor;
            #endregion

            #region productLinks
            if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
            {
                //if (aMnuSports != null) { aMnuSports.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Sports/Url", commonVariables.ProductsXML).Replace("{SportsbookUrl}", commonSports.getSportsbookUrl).Replace("{language}", commonSports.getSportsLanguageId(commonVariables.SelectedLanguage)).Replace("{token}", commonVariables.GetSessionVariable("MemberSessionId")); aMnuSports.Attributes.Remove("data-rel"); }
                //if (aMnuLottery != null) { aMnuLottery.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Lottery/Url", commonVariables.ProductsXML).Replace("{KenoUrl}", commonLottery.getKenoUrl).Replace("{language}", commonVariables.SelectedLanguage); aMnuLottery.Attributes.Remove("data-rel"); }
                //if (aMnuSlots != null) { aMnuSlots.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Slots/Url", commonVariables.ProductsXML); }
                //if (aMnuLiveCasino != null) { aMnuLiveCasino.HRef = commonCulture.ElementValues.getResourceXPathString("Products/LiveCasino/Url", commonVariables.ProductsXML); }

                //if (aMnuDeposit != null) { aMnuDeposit.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Deposit/Url", commonVariables.ProductsXML); aMnuDeposit.Attributes.Remove("data-rel"); aMnuDeposit.Attributes.Add("data-ajax", "false"); }
                //if (aMnuTransfer != null) { aMnuTransfer.HRef = commonCulture.ElementValues.getResourceXPathString("Products/FundTransfer/Url", commonVariables.ProductsXML); aMnuTransfer.Attributes.Remove("data-rel"); aMnuTransfer.Attributes.Add("data-ajax", "false"); }
                //if (aMnuWithdrawal != null) { aMnuWithdrawal.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Withdrawal/Url", commonVariables.ProductsXML); aMnuWithdrawal.Attributes.Remove("data-rel"); aMnuWithdrawal.Attributes.Add("data-ajax", "false"); }

                //if (aSports != null) { aSports.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Sports/Url", commonVariables.ProductsXML).Replace("{SportsbookUrl}", commonSports.getSportsbookUrl).Replace("{language}", commonSports.getSportsLanguageId(commonVariables.SelectedLanguage)).Replace("{token}", commonVariables.GetSessionVariable("MemberSessionId")); aSports.Attributes.Remove("data-rel"); }
                //if (aLottery != null) { aLottery.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Lottery/Url", commonVariables.ProductsXML).Replace("{KenoUrl}", commonLottery.getKenoUrl).Replace("{language}", commonVariables.SelectedLanguage); aLottery.Attributes.Remove("data-rel"); }
                //if (aSlots != null) { aSlots.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Slots/Url", commonVariables.ProductsXML); }
                //if (aLiveCasino != null) { aLiveCasino.HRef = commonCulture.ElementValues.getResourceXPathString("Products/LiveCasino/Url", commonVariables.ProductsXML); }

                //if (aDeposit != null) { aDeposit.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Deposit/Url", commonVariables.ProductsXML); aDeposit.Attributes.Remove("data-rel"); aDeposit.Attributes.Add("data-ajax", "false"); }
                //if (aTransfer != null) { aTransfer.HRef = commonCulture.ElementValues.getResourceXPathString("Products/FundTransfer/Url", commonVariables.ProductsXML); aTransfer.Attributes.Remove("data-rel"); aTransfer.Attributes.Add("data-ajax", "false"); }
                //if (aWithdrawal != null) { aWithdrawal.HRef = commonCulture.ElementValues.getResourceXPathString("Products/Withdrawal/Url", commonVariables.ProductsXML); aWithdrawal.Attributes.Remove("data-rel"); aWithdrawal.Attributes.Add("data-ajax", "false"); }
            }

            //if (aMnuSports != null) { aMnuSports.InnerText = commonCulture.ElementValues.getResourceXPathString("Products/Sports/Label", commonVariables.LeftMenuXML); }
            //if (aMnuLottery != null) { aMnuLottery.InnerText = commonCulture.ElementValues.getResourceXPathString("Products/Lottery/Label", commonVariables.LeftMenuXML); }
            //if (aMnuSlots != null) { aMnuSlots.InnerText = commonCulture.ElementValues.getResourceXPathString("Products/Slots/Label", commonVariables.LeftMenuXML); }
            //if (aMnuLiveCasino != null) { aMnuLiveCasino.InnerText = commonCulture.ElementValues.getResourceXPathString("Products/LiveCasino/Label", commonVariables.LeftMenuXML); }        
            #endregion

            if (!Page.IsPostBack)
            {
                //aSkype.HRef = commonCulture.ElementValues.getResourceString("lnkSkype", xeResources);
                //aEmail.HRef = commonCulture.ElementValues.getResourceString("lnkEmail", xeResources);
                //aBanking.HRef = commonCulture.ElementValues.getResourceString("lnkBanking", xeResources);
                //aPhone.HRef = commonCulture.ElementValues.getResourceString("lnkPhone", xeResources);

                //if (string.IsNullOrEmpty(commonCulture.ElementValues.getResourceString("lnkPhone", xeResources))) 
                //{
                //    liPhone.Visible = false;
                //}
            }

            #endregion
        }

        System.Web.UI.WebControls.Literal litScript = (System.Web.UI.WebControls.Literal)Page.FindControl("litScript");
        if (litScript != null) { }
        base.OnLoad(e);
    }

    protected bool CheckLogin()
    {
        string strMemberSessionId = string.Empty;

        strMemberSessionId = commonVariables.CurrentMemberSessionId;

        if (string.IsNullOrEmpty(strMemberSessionId))
        {
            base.Context.Response.Redirect("/Invalid");
        }

        return string.IsNullOrEmpty(strMemberSessionId);
    }
}