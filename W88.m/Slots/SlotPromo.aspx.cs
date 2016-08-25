using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Web.Script.Services;

public partial class Slots_SlotPromo : BasePage
{
    protected XElement xeErrors = null;

    protected override void OnPreInit(EventArgs e)
    {
        this.isPublic = false;
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (commonCookie.CookieCurrency != "RMB")
        {
            Response.Redirect("/Index");
        }

        if (Page.IsPostBack) return;

        SetTitle(commonCulture.ElementValues.getResourceString("Title", commonVariables.PromotionsXML));


        // set literals
        mainLabel.Text = commonCulture.ElementValues.getResourceString("MainLabel", commonVariables.PromotionsXML);
        description.Text = commonCulture.ElementValues.getResourceString("Description", commonVariables.PromotionsXML);


    }

    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static string claimPromo(string id, string club)
    {
        var slotPromo = new SlotPromo();
        var promoId = Convert.ToInt64(id);
        var claimResponse = slotPromo.createClaim(promoId, club);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        var responseData = serializer.Serialize(claimResponse);
        return responseData;
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public static string claimDetails()
    {
        var yesterday = (DateTime.Now.Hour >= 2) ? DateTime.Now.AddDays(-1).Date : DateTime.Now.AddDays(-2).Date;

        var slotPromo = new SlotPromo();
        var promoList = slotPromo.getPromo(yesterday, yesterday);
        var claimPromo = new SlotPromo.SlotPromoItem();
        if (promoList.Count > 0)
        {
            var filterPromo = promoList.Find(x => (x.game != null));
            if (filterPromo != null)
            {
                claimPromo = promoList.Find(x => x.game.name != null);
                claimPromo.info = slotPromo.getClaimInfo(claimPromo.id);
            }

        }
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        var responseData = serializer.Serialize(claimPromo);
        return responseData;

    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public static string getWeeklyPromo(string week)
    {
        var diffDays = 7 * Convert.ToDouble(week);
        // set filter
        DateTime dataDate = DateTime.Now.AddDays(diffDays);
        int delta = DayOfWeek.Monday - dataDate.DayOfWeek;
        DateTime monday = dataDate.AddDays(delta);
        var sunday = monday.AddDays(6);

        var slotPromo = new SlotPromo();
        var promoList = slotPromo.getPromo(monday, sunday);
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        var responseData = serializer.Serialize(promoList.ToList());
        return responseData;
    }

}