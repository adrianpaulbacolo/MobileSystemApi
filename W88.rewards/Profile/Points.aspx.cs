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
    protected int selecteddayfrom = 1;
    protected int selectedmonthfrom = DateTime.Now.Month;
    protected int selectedyearfrom = DateTime.Now.Year;
    protected int selecteddayto = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
    protected int selectedmonthto = DateTime.Now.Month;
    protected int selectedyearto = DateTime.Now.Year;
    public string localResx = "~/default.{0}.aspx";


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            localResx = string.Format("~/default.{0}.aspx", commonVariables.SelectedLanguage);
            lbdatefrom.Text = HttpContext.GetLocalResourceObject(localResx, "lbl_date_from").ToString() + ":";
            lbdateto.Text = HttpContext.GetLocalResourceObject(localResx, "lbl_date_to").ToString() + ":";
            btnSubmit.Text = HttpContext.GetLocalResourceObject(localResx, "lbl_search").ToString();

            if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                Response.Redirect("~/Index");
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
                            title = HttpContext.GetLocalResourceObject(localResx, "lbl_redemption_listing").ToString();
                            break;
                        case "expired":
                            title = HttpContext.GetLocalResourceObject(localResx, "lbl_points_expired").ToString();
                            break;
                        case "adjusted":
                            title = HttpContext.GetLocalResourceObject(localResx, "lbl_points_adjusted").ToString();
                            break;
                        case "cart":
                            //   title = "Cart Listing";
                            break;
                        default:
                            break;
                    }
                }
                #region populatedropdownlist
                for (int d = 1; d <= 31; d++)
                {
                    ListItem day = new ListItem();
                    day.Text = d.ToString();
                    day.Value = d.ToString();
                    if (d == selecteddayfrom)
                        day.Selected = day.Value.Contains(selecteddayfrom.ToString());
                    selectdayfrom.Items.Add(day);
                }
                for (int m = 1; m <= 12; m++)
                {
                    ListItem mth = new ListItem();
                    mth.Text = m.ToString();
                    mth.Value = m.ToString();
                    if (m == selectedmonthfrom)
                        mth.Selected = mth.Value.Contains(selectedmonthfrom.ToString());
                    selectmonthfrom.Items.Add(mth);
                }
                for (int y = DateTime.Now.Year; y >= 2013; y--)
                {
                    ListItem yr = new ListItem();
                    yr.Text = y.ToString();
                    yr.Value = y.ToString();
                    if (y == selectedyearfrom)
                        yr.Selected = yr.Value.Contains(selectedyearfrom.ToString());
                    selectyearfrom.Items.Add(yr);
                }

                for (int d2 = 1; d2 <= 31; d2++)
                {
                    ListItem day = new ListItem();
                    day.Text = d2.ToString();
                    day.Value = d2.ToString();
                    if (d2 == selecteddayto)
                        day.Selected = day.Value.Contains(selecteddayto.ToString());
                    selectdayto.Items.Add(day);
                }
                for (int m2 = 1; m2 <= 12; m2++)
                {
                    ListItem mth = new ListItem();
                    mth.Text = m2.ToString();
                    mth.Value = m2.ToString();
                    if (m2 == selectedmonthto)
                        mth.Selected = mth.Value.Contains(selectedmonthto.ToString());
                    selectmonthto.Items.Add(mth);
                }
                for (int y2 = DateTime.Now.Year; y2 >= 2013; y2--)
                {
                    ListItem yr = new ListItem();
                    yr.Text = y2.ToString();
                    yr.Value = y2.ToString();
                    if (y2 == selectedyearto)
                        yr.Selected = yr.Value.Contains(selectedyearto.ToString());
                    selectyearto.Items.Add(yr);
                }

                #endregion populatedropdownlist
            }
        }
    }

    public string GetRedemptionList(string userMemberCode, DateTime dateFrom, DateTime dateTo)
    {
        string dtFrom = String.Format("{0:yyyy/M/d HH:mm:ss}", dateFrom);//2014-10-31 12:57:12.337
        string dtTo = String.Format("{0:yyyy/M/d HH:mm:ss}", dateTo);
        string html = "";

        try
        {
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
                                            columnname = HttpContext.GetLocalResourceObject(localResx, "lbl_date").ToString();
                                            th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                            break;
                                        case "redemptionId":
                                            columnname = "ID";
                                            th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                            break;
                                        case "categoryCode":
                                            columnname = HttpContext.GetLocalResourceObject(localResx, "lbl_category").ToString() + " (" + HttpContext.GetLocalResourceObject(localResx, "lbl_points").ToString() + ")";
                                            th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                            break;
                                        case "statusName":
                                            columnname = HttpContext.GetLocalResourceObject(localResx, "lbl_status").ToString();
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

        }
        catch (Exception)
        {
            throw;
        }
        return html;
    }

    public string GetAdjustmentList(string userMemberCode, DateTime dateFrom, DateTime dateTo)
    {
        string dtFrom = String.Format("{0:yyyy/M/d HH:mm:ss}", dateFrom);//2014-10-31 12:57:12.337
        string dtTo = String.Format("{0:yyyy/M/d HH:mm:ss}", dateTo);
        string html = "";

        try
        {
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
                                            columnname = HttpContext.GetLocalResourceObject(localResx, "lbl_date").ToString();
                                            th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                            break;
                                        case "actionName":
                                            columnname = HttpContext.GetLocalResourceObject(localResx, "lbl_adjustment_type").ToString();
                                            th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                            break;
                                        case "pointsAdjusted":
                                            columnname = HttpContext.GetLocalResourceObject(localResx, "lbl_points").ToString();
                                            th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                            break;
                                        case "remarks":
                                            columnname = HttpContext.GetLocalResourceObject(localResx, "lbl_remarks").ToString();
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
        }
        catch (Exception)
        {

            throw;
        }
        return html;
    }

    public string GetExpiredList(string userMemberCode, DateTime dateFrom, DateTime dateTo)
    {
        string dtFrom = String.Format("{0:yyyy/M/d HH:mm:ss}", dateFrom);//2014-10-31 12:57:12.337
        string dtTo = String.Format("{0:yyyy/M/d HH:mm:ss}", dateTo);
        string html = "";

        try
        {
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
                                            columnname = HttpContext.GetLocalResourceObject(localResx, "lbl_date").ToString();
                                            th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                            break;
                                        case "pointsExpired":
                                            columnname = HttpContext.GetLocalResourceObject(localResx, "lbl_points_expired").ToString();
                                            th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                            break;
                                        case "remarks":
                                            columnname = HttpContext.GetLocalResourceObject(localResx, "lbl_remarks").ToString();
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
        }
        catch (Exception)
        {

            throw;
        }

        return html;

    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        localResx = string.Format("~/default.{0}.aspx", commonVariables.SelectedLanguage);
        strAlertCode = "";
        bool isvalid = false;
        string userMemberCode = string.IsNullOrEmpty((string)Session["MemberCode"]) ? "" : (string)Session["MemberCode"];
        DateTime fromDate;
        DateTime toDate;
        string fromdatestring = selectdayfrom.Value.PadLeft(2, '0') + "/" + selectmonthfrom.Value.PadLeft(2, '0') + "/" + selectyearfrom.Value;
        string todatestring = selectdayto.Value.PadLeft(2, '0') + "/" + selectmonthto.Value.PadLeft(2, '0') + "/" + selectyearto.Value;

        var formats = new[] { "dd/MM/yyyy" };
        if (DateTime.TryParseExact(fromdatestring, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDate))
        {
            if (DateTime.TryParseExact(todatestring, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out toDate))
            {
                if (fromDate > toDate)
                {
                    strAlertCode = "wrongdaterange";
                    return;
                }
                else
                {
                    toDate = toDate.AddHours(23).AddMinutes(59).AddSeconds(59);
                    isvalid = true;
                }
            }
            else
            {
                strAlertCode = "wrongtodate";
                return;
            }
        }
        else
        {
            strAlertCode = "wrongfromdate";
            return;
        }

        if (isvalid)
        {
            selecteddayfrom = int.Parse(selectdayfrom.Value);
            selectedmonthfrom = int.Parse(selectmonthfrom.Value);
            selectedyearfrom = int.Parse(selectyearfrom.Value);
            selecteddayto = int.Parse(selectdayto.Value);
            selectedmonthto = int.Parse(selectmonthto.Value);
            selectedyearto = int.Parse(selectyearto.Value);

            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("type")))
                {
                    type = HttpContext.Current.Request.QueryString.Get("type");
                    switch (type)
                    {
                        case "redeemed":
                            title = HttpContext.GetLocalResourceObject(localResx, "lbl_redemption_listing").ToString();
                            html = GetRedemptionList(userMemberCode, fromDate, toDate);
                            if (!string.IsNullOrEmpty(html))
                                resultpanel.InnerHtml = html;
                            else
                            {
                                strAlertCode = "nodata";
                                resultpanel.InnerHtml = "";
                            }
                            break;
                        case "expired":
                            title = HttpContext.GetLocalResourceObject(localResx, "lbl_points_expired").ToString();
                            //   html = GetExpiredList("mooretest1", fromDate, toDate);
                            html = GetExpiredList(userMemberCode, fromDate, toDate);
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
                            title = HttpContext.GetLocalResourceObject(localResx, "lbl_points_adjusted").ToString();
                            html = GetAdjustmentList(userMemberCode, fromDate, toDate);
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
            catch (Exception ex)
            {
                strAlertCode = "FAIL";

                Guid newerrorid = new Guid();
                commonAuditTrail.appendLog("system", "Points.aspx", type, "Points.aspx", "", Title, "", ex.Message + " stacktrace: " + ex.StackTrace, "" + " Member COde: " + userMemberCode, "", newerrorid.ToString(), false);
            }
        }
        else
        {
            resultpanel.InnerHtml = ""; 
        }


    }





}