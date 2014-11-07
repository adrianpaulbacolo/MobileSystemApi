using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Diagnostics;
using System.Text;
using PaymentServices;
using System.Globalization;

public partial class Points : BasePage
{
    protected string type = string.Empty;
    protected string title = string.Empty;
    protected string html = string.Empty;
    protected string strAlertCode = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!Page.IsPostBack)
        {
            if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
            {
                Response.Redirect("~/Index");
            }
            else
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("type")))
                {
                    type = HttpContext.Current.Request.QueryString.Get("type");

                    switch (type)
                    {
                        case "stake":
                            break;
                        case "redeemed":
                            title = "Redemption Listing";
                            break;
                        case "expired":
                            title = "Points Expired";
                            break;
                        case "adjusted":
                            title = "Net Points Adjusted";
                            break;
                        case "cart":
                         //   title = "Cart Listing";
                            break;
                        default:
                            break;
                    }
                }

            }
        }


    }



    public string GetRedemptionList(string userMemberCode, DateTime dateFrom, DateTime dateTo)
    {

        string dtFrom = String.Format("{0:yyyy/M/d HH:mm:ss}", dateFrom);//2014-10-31 12:57:12.337
        string dtTo = String.Format("{0:yyyy/M/d HH:mm:ss}", dateTo);
        string html = "";

        using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
        {
            System.Data.DataSet ds = sClient.getRedemptionFE(commonVariables.OperatorId, userMemberCode, dtFrom, dtTo);


            if (ds.Tables.Count > 0)
            {
                ds.Tables[0].Columns.Add("rType");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    html = "<table>";
                    string th = "";
                    string tr = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        if (dt.Rows[i]["rType"].ToString() != "0")
                            dt.Rows[i]["rType"] = "Spin Wheel Prize";
                        else
                            dt.Rows[i]["rType"] = "Redemption";


                        string columnname = "";
                        tr += "<tr>";

                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (i == 0)
                            {
                                switch (dt.Columns[j].ColumnName)
                                {
                                    case "createdDateTime":
                                        columnname = "Date";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    case "redemptionId":
                                        columnname = "ID";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    case "categoryCode":
                                        columnname = "Category (Points)";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    case "statusName":
                                        columnname = "Status";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    default:
                                        //th += "<td><div class='pointDetailMainHeaderHor'><span>" + dt.Columns[j].ColumnName + "</span></div></td>";
                                        break;
                                }
                            }


                            if (j == 0) //create date
                                tr += "<td><div class='points'><span>" + String.Format("{0:d/M/yyyy}", Convert.ToDateTime(dt.Rows[i].ItemArray[0].ToString())) + "</span></div></td>";
                            else if (j == 3) //category=3, point=4
                                tr += "<td><div class='points'><span>" + dt.Rows[i].ItemArray[j] + " (" +
                                        String.Format("{0:#,###,##0.##}", dt.Rows[i].ItemArray[4]) + ")" + "</span></div></td>";
                            else if (j == 1 || j == 5)//1=id, 5=status
                                tr += "<td><div class='points'><span>" + dt.Rows[i].ItemArray[j] +
                                        "</span></div></td>";
                            else
                                tr += "";

                        }
                        tr += "</tr>";
                    }
                    html = "<table width='100%'>" + "<tr>" + th + "</tr>" + tr + "</table>";
                }

            }
        }
        return html;

    }

    public string GetAdjustmentList(string userMemberCode, DateTime dateFrom, DateTime dateTo)
    {

        string dtFrom = String.Format("{0:yyyy/M/d HH:mm:ss}", dateFrom);//2014-10-31 12:57:12.337
        string dtTo = String.Format("{0:yyyy/M/d HH:mm:ss}", dateTo);
        string html = "";

        using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
        {
            System.Data.DataSet ds = sClient.getAdjustmentFE(commonVariables.OperatorId, userMemberCode, dtFrom, dtTo);


            if (ds.Tables.Count > 0)
            {

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    html = "<table>";
                    string th = "";
                    string tr = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty(dt.Rows[i]["remarks"].ToString()))
                            dt.Rows[i]["remarks"] = "-";

                        string columnname = "";
                        tr += "<tr>";

                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (i == 0)
                            {
                                switch (dt.Columns[j].ColumnName)
                                {
                                    case "createdDateTime":
                                        columnname = "Date";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    case "actionName":
                                        columnname = "Adjustment";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    case "pointsAdjusted":
                                        columnname = "Points";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    case "remarks":
                                        columnname = "Remarks";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    default:
                                        //th += "<td><div class='pointDetailMainHeaderHor'><span>" + dt.Columns[j].ColumnName + "</span></div></td>";
                                        break;
                                }
                            }

                            if (j == 0) //create date
                                tr += "<td><div class='points'><span>" + String.Format("{0:d/M/yyyy}", Convert.ToDateTime(dt.Rows[i].ItemArray[0].ToString())) + "</span></div></td>";
                            else
                                tr += "<td><div class='points'><span>" + dt.Rows[i].ItemArray[j] +
                                       "</span></div></td>";
                        }
                        tr += "</tr>";
                    }
                    html = "<table width='100%'>" + "<tr>" + th + "</tr>" + tr + "</table>";
                }
            }
        }
        return html;

    }

    public string GetExpiredList(string userMemberCode, DateTime dateFrom, DateTime dateTo)
    {
        string dtFrom = String.Format("{0:yyyy/M/d HH:mm:ss}", dateFrom);//2014-10-31 12:57:12.337
        string dtTo = String.Format("{0:yyyy/M/d HH:mm:ss}", dateTo);
       
        string html = "";

        using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
        {
            System.Data.DataSet ds = sClient.getExpiredFE(commonVariables.OperatorId, userMemberCode, dtFrom, dtTo);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    html = "<table>";
                    string th = "";
                    string tr = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (string.IsNullOrEmpty(dt.Rows[i]["remarks"].ToString()))
                          dt.Rows[i]["remarks"] = "-";
                        
                        string columnname = "";
                        tr += "<tr>";

                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (i == 0)
                            {
                                switch (dt.Columns[j].ColumnName)
                                {
                                    case "createdDateTime":
                                        columnname = "Date";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    case "pointsExpired":
                                        columnname = "Points Expired";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    case "remarks":
                                        columnname = "Remarks";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    default:
                                        //th += "<td><div class='pointDetailMainHeaderHor'><span>" + dt.Columns[j].ColumnName + "</span></div></td>";
                                        break;
                                }
                            }
                            if (j == 8) //create date
                                tr += "<td><div class='points'><span>" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dt.Rows[i].ItemArray[j].ToString())) + "</span></div></td>";
                            else if (j == 5 || j == 6)
                                tr += "<td><div class='points'><span>" + dt.Rows[i].ItemArray[j] +
                                       "</span></div></td>";
                            else
                                tr += "";
                        }
                        tr += "</tr>";
                    }
                    html = "<table width='100%'>" + "<tr>" + th + "</tr>" + tr + "</table>";
                }
            }
        }
        return html;

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string text = hidDateFrom.Value;
        strAlertCode = hidError.Value;
        string userMemberCode = string.IsNullOrEmpty((string)Session["MemberCode"]) ? "" : (string)Session["MemberCode"];

        if (strAlertCode == "valid")
        {
            string from = hidDateFrom.Value;
            string to = hidDateTo.Value;
            string[] fromarray = from.Split('/');
            string[] toarray = to.Split('/');
            DateTime datefrom = new DateTime(int.Parse(fromarray[2]), int.Parse(fromarray[1]), int.Parse(fromarray[0]));
            DateTime dateto = new DateTime(int.Parse(toarray[2]), int.Parse(toarray[1]), int.Parse(toarray[0])).AddHours(23).AddMinutes(59).AddSeconds(59);

            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("type")))
            {
                type = HttpContext.Current.Request.QueryString.Get("type");
                switch (type)
                {
                    case "redeemed":
                        title = "Redemption Listing";
                        html = GetRedemptionList(userMemberCode, datefrom, dateto);
                        if (!string.IsNullOrEmpty(html))
                            resultpanel.InnerHtml = html;
                        else
                        {
                            strAlertCode = "nodata";
                            resultpanel.InnerHtml = "";
                        }
                        break;
                    case "expired":
                        title = "Points Expired";
                     //   html = GetExpiredList("mooretest1", datefrom, dateto);
                        html = GetExpiredList(userMemberCode, datefrom, dateto);
                        if (!string.IsNullOrEmpty(html))
                            resultpanel.InnerHtml = html;
                        else
                        {
                            strAlertCode = "nodata";
                            resultpanel.InnerHtml = "";
                        }
                        break;
                    case "adjusted":
                        title = "Net Points Adjusted";
                     //   html = GetAdjustmentList("testRMB01", datefrom, dateto);
                        html = GetAdjustmentList(userMemberCode, datefrom, dateto);
                        if (!string.IsNullOrEmpty(html))
                            resultpanel.InnerHtml = html;
                        else
                        {
                            strAlertCode = "nodata";
                            resultpanel.InnerHtml = "";
                        }
                        break;
                    case "cart":
                        //title = "Cart Listing";
                        break;
                    default:
                        break;
                }
            }
        }




    }





}