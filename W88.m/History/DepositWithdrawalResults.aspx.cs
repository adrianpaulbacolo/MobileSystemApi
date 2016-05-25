using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class History_DepositWithdrawalResults : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request["dateFrom"]) && !string.IsNullOrEmpty(Request["dateTo"]) &&
            !string.IsNullOrEmpty(Request["status"]) && !string.IsNullOrEmpty(Request["type"]) &&
            !string.IsNullOrEmpty(commonVariables.OperatorId) &&
            !string.IsNullOrEmpty(commonVariables.GetSessionVariable("MemberCode")))
        {
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
                    var strMemberCode = commonVariables.GetSessionVariable("MemberCode");

                    string statusCode;
                    var history = svcInstance.getDepositWithdrawalHistory(strOperatorId, strMemberCode, type, status, dateFrom, dateTo, out statusCode);

                    var dvHistory = new DataView(history) {Sort = "requestDate DESC"};

                    GridView1.DataSource = dvHistory;
                    GridView1.PagerSettings.Mode = PagerButtons.NextPrevious;
                    GridView1.EmptyDataText = commonCulture.ElementValues.getResourceXPathString("norecords", commonVariables.HistoryXML);
                    GridView1.Columns[1].HeaderText = commonCulture.ElementValues.getResourceXPathString("dateTime", commonVariables.HistoryXML);
                    GridView1.Columns[2].HeaderText = commonCulture.ElementValues.getResourceXPathString("transId", commonVariables.HistoryXML);
                    GridView1.Columns[3].HeaderText = commonCulture.ElementValues.getResourceXPathString("paymentMethod", commonVariables.HistoryXML);
                    GridView1.Columns[4].HeaderText = commonCulture.ElementValues.getResourceXPathString("lblType", commonVariables.HistoryXML);
                    GridView1.Columns[5].HeaderText = commonCulture.ElementValues.getResourceXPathString("submittedAmt", commonVariables.HistoryXML);
                    GridView1.Columns[6].HeaderText = commonCulture.ElementValues.getResourceXPathString("receivedAmt", commonVariables.HistoryXML);
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

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        var payment = DataBinder.Eval(e.Row.DataItem, "paymenttype");
        var methodId = DataBinder.Eval(e.Row.DataItem, "methodid");
        var statusValue = DataBinder.Eval(e.Row.DataItem, "status").ToString().ToUpper();

        if (payment.ToString().ToLower() == Convert.ToString(commonVariables.PaymentTransactionType.Deposit.ToString().ToLower()))
        {
            e.Row.Cells[3].Text = commonCulture.ElementValues.getResourceXPathString("d" + Enum.GetName(typeof(commonVariables.DepositMethod), methodId), commonVariables.PaymentMethodsXML);
            e.Row.Cells[4].Text = commonCulture.ElementValues.GetResourceXPathAttribute("type", "id", "1", commonVariables.HistoryXML);
        }
        else if (payment.ToString().ToLower() == Convert.ToString(commonVariables.PaymentTransactionType.Withdrawal.ToString().ToLower()))
        {
            e.Row.Cells[3].Text = commonCulture.ElementValues.getResourceXPathString("w" + Enum.GetName(typeof(commonVariables.WithdrawalMethod), methodId), commonVariables.PaymentMethodsXML);
            e.Row.Cells[4].Text = commonCulture.ElementValues.GetResourceXPathAttribute("type", "id", "2", commonVariables.HistoryXML);
        }

        e.Row.Cells[7].Text = commonCulture.ElementValues.GetResourceXPathAttribute("status", "id", statusValue, commonVariables.HistoryXML);
    }
}