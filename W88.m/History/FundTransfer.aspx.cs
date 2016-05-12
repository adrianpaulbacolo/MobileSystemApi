using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class History_FundTransfer: BasePage
{
    ICollection<KeyValuePair<int, string>> _wallet;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.IsPostBack) return;

        _wallet = commonPaymentMethodFunc.GetWallets();
        Initialize();
    }
    private void Initialize()
        {
        lblDateFrom.Text = commonCulture.ElementValues.getResourceString("lblDateFrom", commonVariables.HistoryXML);
        lblDateTo.Text = commonCulture.ElementValues.getResourceString("lblDateTo", commonVariables.HistoryXML);
        lblStatus.Text = commonCulture.ElementValues.getResourceString("lblStatus", commonVariables.HistoryXML);
        lblType.Text = commonCulture.ElementValues.getResourceString("lblType", commonVariables.HistoryXML);
        btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", commonVariables.HistoryXML);

        // This will reuse the "ALL" translation with the same wallet id in History XML
        ddlType.Items.Add(new ListItem(commonVariables.HistoryXML.Elements("ftStatus").FirstOrDefault().Value, commonVariables.HistoryXML.Elements("ftStatus").FirstOrDefault().Attribute("id").Value));
        ddlType.Items.AddRange(_wallet.Select(type => new ListItem(type.Value, Convert.ToString(type.Key))).ToArray());
        ddlStatus.Items.AddRange(commonVariables.HistoryXML.Elements("ftStatus").Select(type => new ListItem(type.Value, type.Attribute("id").Value)).ToArray());
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        var dateFrom = txtDateFrom.Text + " 00:00";
        var dateTo = txtDateTo.Text + " 23:59";

        var url = string.Format("FundTransferResults.aspx?dateFrom={0}&dateTo={1}&status={2}&type={3}", dateFrom, dateTo, ddlStatus.SelectedValue, ddlType.SelectedValue);
        Response.Redirect(url);
    }

}