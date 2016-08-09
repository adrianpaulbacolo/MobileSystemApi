using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class History_FundTransferResults : BasePage
{

    ICollection<KeyValuePair<int, string>> _wallet;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!string.IsNullOrEmpty(Request["dateFrom"]) && !string.IsNullOrEmpty(Request["dateTo"]) &&
            !string.IsNullOrEmpty(Request["status"]) && !string.IsNullOrEmpty(Request["type"]) &&
            !string.IsNullOrEmpty(commonVariables.OperatorId) &&
            !string.IsNullOrEmpty(base.userInfo.MemberCode))
        {

            _wallet = commonPaymentMethodFunc.GetWallets();

            //Request Params
            var dateFrom = DateTime.Parse(Request["dateFrom"].ToString());
            var dateTo = DateTime.Parse(Request["dateTo"].ToString());
            var status = Request["status"];
            var type = int.Parse(Request["type"]);

            try
            {
                using (var svcInstance = new svcPayMember.MemberClient())
                {
                    //Other Params
                    var strOperatorId = int.Parse(commonVariables.OperatorId);
                    var strMemberCode = base.userInfo.MemberCode;
                    string statusCode;
                    var history = svcInstance.getFundTransferHistory(strOperatorId, strMemberCode, type, status, dateFrom, dateTo, out statusCode);

                    GridView1.DataSource = history;
                    GridView1.PagerSettings.Mode = PagerButtons.NextPrevious;
                    GridView1.EmptyDataText = commonCulture.ElementValues.getResourceXPathString("norecords", commonVariables.HistoryXML);
                    GridView1.Columns[1].HeaderText = commonCulture.ElementValues.getResourceXPathString("dateTime", commonVariables.HistoryXML);
                    GridView1.Columns[2].HeaderText = commonCulture.ElementValues.getResourceXPathString("transId", commonVariables.HistoryXML);
                    GridView1.Columns[3].HeaderText = commonCulture.ElementValues.getResourceXPathString("from", commonVariables.HistoryXML);
                    GridView1.Columns[4].HeaderText = commonCulture.ElementValues.getResourceXPathString("to", commonVariables.HistoryXML);
                    GridView1.Columns[5].HeaderText = commonCulture.ElementValues.getResourceXPathString("source", commonVariables.HistoryXML);
                    GridView1.Columns[6].HeaderText = commonCulture.ElementValues.getResourceXPathString("amount", commonVariables.HistoryXML);
                    GridView1.Columns[7].HeaderText = commonCulture.ElementValues.getResourceXPathString("lblStatus", commonVariables.HistoryXML);
                    GridView1.DataBind();
                }
            }
            catch (Exception ex)
            {
                Console.Out.Write("ex: " + ex);
            }
        }
        else
        {
            Response.Redirect((string)HttpContext.Current.Session["domain_Account"] + "/History");
        }
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
    }

    protected void GridView1_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;
        
        var from = DataBinder.Eval(e.Row.DataItem, "transferFromWalletId");
        foreach (var pair in _wallet.Where(pair => pair.Key.Equals(Convert.ToInt32(@from))))
        {
            e.Row.Cells[3].Text = pair.Value;
        }

        var to = DataBinder.Eval(e.Row.DataItem, "transferToWalletId");
        foreach (var pair in _wallet.Where(pair => pair.Key.Equals(Convert.ToInt32(to))))
        {
            e.Row.Cells[4].Text = pair.Value;
        }

        var statusValue = DataBinder.Eval(e.Row.DataItem, "transferstatus").ToString();
        e.Row.Cells[7].Text = commonCulture.ElementValues.GetResourceXPathAttribute("ftStatus", "id", statusValue, commonVariables.HistoryXML);
    }
    
}