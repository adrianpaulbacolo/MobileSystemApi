using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Rebates : PaymentBasePage
{
    protected System.Xml.Linq.XElement xeResources = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        SetTitle(commonCulture.ElementValues.getResourceString("rebates", commonVariables.LeftMenuXML));
        
        var weekPromo = DateTime.Today.AddDays(1 + -1 * (DateTime.Today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.Today.DayOfWeek));
        var textWeekPromo = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(weekPromo.DayOfYear) / 7)).ToString("d2") + weekPromo.ToString("yy");

        if (commonCookie.CookieCurrency != "RMB")
        {
            hfWeekPromo.Value =
                "RebateWeeklyClaim.aspx?code=RBASB[weekyear],RBESB[weekyear],RBISB[weekyear]&product=asports,esports,isports"
                    .Replace("[weekyear]", textWeekPromo);
        }
        else
        {
            hfWeekPromo.Value =
                "RebateWeeklyClaim.aspx?code=RBASB[weekyear],RBESB[weekyear]&product=asports,esports".Replace(
                    "[weekyear]", textWeekPromo);
        }
    }
}