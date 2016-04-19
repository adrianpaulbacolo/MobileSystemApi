using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class History_DepositWithdrawal : System.Web.UI.Page
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
        lblStatus.Text = commonCulture.ElementValues.getResourceString("lblStatus", commonVariables.HistoryXML);
        lblType.Text = commonCulture.ElementValues.getResourceString("lblType", commonVariables.HistoryXML);

        btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", commonVariables.HistoryXML);

        ddlType.Items.AddRange(commonVariables.HistoryXML.Elements("type").Select(type => new ListItem(type.Value, type.Attribute("id").Value)).ToArray());

        ddlStatus.Items.AddRange(commonVariables.HistoryXML.Elements("status").Where(type => type.Value != "").Select(type => new ListItem(type.Value, type.Attribute("id").Value)).ToArray());
        ddlStatus.Items.Remove(ddlStatus.Items.FindByValue("REJECTED"));
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var dateFrom = txtDateFrom.Text + " 00:00";
        var dateTo = txtDateTo.Text + " 23:59";

        string url = "DepositWithdrawalResults.aspx?dateFrom=" + dateFrom + "&dateTo=" + dateTo + "&status=" + ddlStatus.SelectedValue + "&type=" + ddlType.SelectedValue;
        Response.Redirect(url);
    }

}