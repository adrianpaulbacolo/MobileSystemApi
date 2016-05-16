using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class History_ReferralBonusResults : BasePage
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
                
                //if((dateTo-dateFrom).TotalDays > 90)
                //{
                //    dateTo = dateFrom.AddDays(90);
                //}


                try
                {
                    using (var svcInstance = new wsMemberMS1.memberWSSoapClient())
                    {
                        DataSet history = svcInstance.MemberReferralHistory(strMemberId, dateFrom, dateTo);

                        lblInvitees.Text = history.Tables[0].Rows[0]["totInvitees"].ToString();
                        lblRegistered.Text = history.Tables[0].Rows[0]["totRegistered"].ToString();
                        lblSuccessfulReferrals.Text = history.Tables[0].Rows[0]["totSuccessful"].ToString();
                        lblTotalReferralBonus.Text = history.Tables[0].Rows[0]["totBonus"].ToString();

                        GridView1.DataSource = history;
                        GridView1.PagerSettings.Mode = PagerButtons.NextPrevious;
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