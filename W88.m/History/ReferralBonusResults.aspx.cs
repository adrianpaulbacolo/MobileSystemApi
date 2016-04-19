using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class History_ReferralBonusResults : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!string.IsNullOrEmpty(Request["dateFrom"]) && !string.IsNullOrEmpty(Request["dateTo"]) &&
           !string.IsNullOrEmpty(commonVariables.GetSessionVariable("MemberId")))
        {
            //Request Params
            var dateFrom = DateTime.Parse(Request["dateFrom"].ToString());
            var dateTo = DateTime.Parse(Request["dateTo"].ToString());

            //Other Params
            var strMemberId = long.Parse(commonVariables.GetSessionVariable("MemberId"));

            try
            {
                using (var svcInstance = new wsMemberMS1.memberWSSoapClient())
                {
                    DataSet history = svcInstance.MemberReferralHistory(strMemberId, dateFrom, dateTo);

                    lblInvitees.Text = string.Format(": {0}", history.Tables[0].Rows[0]["totInvitees"].ToString());
                    lblRegistered.Text = string.Format(": {0}", history.Tables[0].Rows[0]["totRegistered"].ToString());
                    lblSuccessfulReferrals.Text = string.Format(": {0}", history.Tables[0].Rows[0]["totSuccessful"].ToString());
                    lblTotalReferralBonus.Text = string.Format(": {0}", history.Tables[0].Rows[0]["totBonus"].ToString());

                    GridView1.DataSource = history.Tables[1];
                    GridView1.PagerSettings.Mode = PagerButtons.NextPrevious;
                    GridView1.EmptyDataText = commonCulture.ElementValues.getResourceXPathString("norecords", commonVariables.HistoryXML);
                    GridView1.Columns[1].HeaderText = commonCulture.ElementValues.getResourceXPathString("dateTime", commonVariables.HistoryXML);
                    GridView1.Columns[2].HeaderText = commonCulture.ElementValues.getResourceXPathString("transId", commonVariables.HistoryXML);
                    GridView1.Columns[3].HeaderText = commonCulture.ElementValues.getResourceXPathString("amount", commonVariables.HistoryXML);
                    GridView1.Columns[4].HeaderText = commonCulture.ElementValues.getResourceXPathString("lblStatus", commonVariables.HistoryXML);
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

        var statusValue = DataBinder.Eval(e.Row.DataItem, "status").ToString();
        e.Row.Cells[4].Text = commonCulture.ElementValues.GetResourceXPathAttribute("status", "id", statusValue, commonVariables.HistoryXML);
    }
}