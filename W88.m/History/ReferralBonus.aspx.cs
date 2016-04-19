using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class History_ReferralBonus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.Initialize();
        }
    }
    private void Initialize()
    {
        lblDateFrom.Text = commonCulture.ElementValues.getResourceString("lblDateFrom", commonVariables.HistoryXML);
        lblDateTo.Text = commonCulture.ElementValues.getResourceString("lblDateTo", commonVariables.HistoryXML);

        btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", commonVariables.HistoryXML);
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var dateFrom = txtDateFrom.Text + " 00:00";
        var dateTo = txtDateTo.Text + " 23:59";

        string url = "ReferralBonusResults.aspx?dateFrom=" + dateFrom + "&dateTo=" + dateTo;
        Response.Redirect(url);
    }

}