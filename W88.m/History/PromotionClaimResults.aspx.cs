using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class History_PromotionClaimResults : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
            if (!string.IsNullOrEmpty(Request["dateFrom"]) && !string.IsNullOrEmpty(Request["dateTo"]) &&
                !string.IsNullOrEmpty(commonVariables.OperatorId) &&
                !string.IsNullOrEmpty(commonVariables.GetSessionVariable("MemberId")))
            {
                //Request Params
                var dateFrom = DateTime.Parse(Request["dateFrom"].ToString());
                var dateTo = DateTime.Parse(Request["dateTo"].ToString());

                //Other Params
                var strOperatorId = int.Parse(commonVariables.OperatorId);
                var strMemberId = long.Parse(commonVariables.GetSessionVariable("MemberId"));

                try
                {
                    using (var svcInstance = new wsMemberMS1.memberWSSoapClient())
                    {
                        string statusCode;
                        DataSet history = svcInstance.MemberPromotionRegistrationHistory(strOperatorId,strMemberId,dateFrom,dateTo);

                        GridView1.DataSource = history;
                        GridView1.PagerSettings.Mode = PagerButtons.NextPrevious;
                        GridView1.EmptyDataText = commonCulture.ElementValues.getResourceXPathString("norecords", commonVariables.HistoryXML);
                        GridView1.Columns[1].HeaderText = commonCulture.ElementValues.getResourceXPathString("dateTime", commonVariables.HistoryXML);
                        GridView1.Columns[2].HeaderText = commonCulture.ElementValues.getResourceXPathString("subjectCode", commonVariables.HistoryXML);

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
                Response.Redirect((string) HttpContext.Current.Session["domain_Account"] + "/History");
            }
        

    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
    }
}