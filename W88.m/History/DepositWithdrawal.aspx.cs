using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class History_DepositWithdrawal : BasePage
{
    private System.Xml.Linq.XElement xeResources = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        commonCulture.appData.getRootResource("/History/DepositWithdrawal", out xeResources);

        if (!Page.IsPostBack)
        {
            lblDateFrom.Text = commonCulture.ElementValues.getResourceString("lblDateFrom", xeResources);
            lblDateTo.Text = commonCulture.ElementValues.getResourceString("lblDateTo", xeResources);
            lblStatus.Text = commonCulture.ElementValues.getResourceString("lblStatus", xeResources);
            lblType.Text = commonCulture.ElementValues.getResourceString("lblType", xeResources);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var dateFrom = txtDateFrom.Text + " 00:00";
        var dateTo = txtDateTo.Text + " 23:59";

        string url = "DepositWithdrawalResults.aspx?dateFrom=" + dateFrom +"&dateTo=" + dateTo + "&status="+ddlStatus.SelectedValue + "&type=" +ddlType.SelectedValue;
        Response.Redirect(url);
    }

}