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

public partial class Account : BasePage
{
    protected string type = string.Empty;
    protected string title = string.Empty;
    protected string html = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //*
        string userMemberSessionId = string.IsNullOrEmpty((string)Session["MemberSessionId"]) ? "" : (string)Session["MemberSessionId"];
        string userMemberId = string.IsNullOrEmpty((string)Session["MemberId"]) ? "" : (string)Session["MemberId"];
        string userMemberCode = string.IsNullOrEmpty((string)Session["MemberCode"]) ? "" : (string)Session["MemberCode"];
        string countryCode = string.IsNullOrEmpty((string)Session["CountryCode"]) ? "0" : (string)Session["CountryCode"];
        string currencyCode = string.IsNullOrEmpty((string)Session["CurrencyCode"]) ? "0" : (string)Session["CurrencyCode"];
        string riskId = string.IsNullOrEmpty((string)Session["RiskId"]) ? "0" : (string)Session["RiskId"];
        string LanguageCode = string.IsNullOrEmpty((string)Session["LanguageCode"]) ? "0" : (string)Session["LanguageCode"];
        //*

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
                    string walletid = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("walletid")) ? "" : HttpContext.Current.Request.QueryString.Get("walletid");
                    string yearmonth = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("yearmonth")) ? "" : HttpContext.Current.Request.QueryString.Get("yearmonth");

                    switch (type)
                    {
                        case "stake":
                            if (walletid == "") //all stakes
                            {
                                title = "Earning Listing by Product";
                                html = TotalStake(userMemberCode);
                            }
                            else if (!string.IsNullOrEmpty(walletid) && yearmonth == "") //stakes by walletid
                            {
                                title = "Earning Listing by Month";
                                html = TotalStakeMonth(userMemberCode, walletid);
                            }
                            else //stakes detail
                            {
                                title = "Earning Listing Detail";
                                html = TotalStakeDetail(userMemberCode, walletid, yearmonth);
                            }

                            if (!string.IsNullOrEmpty(html))
                                resultpanel.InnerHtml = html;
                            else
                                lblNoRecord.Visible = true;
                            ListviewHistory.Visible = false;

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
                            title = "Cart Listing";
                            break;
                        default:
                            title = "Account Summary";
                            System.Data.DataSet dsDisplay = AccountSummary(userMemberCode);
                            if (dsDisplay.Tables[0].Rows.Count > 0)
                            {  
                                lblNoRecord.Visible = false;
                                ListviewHistory.DataSource = dsDisplay.Tables[0];
                                ListviewHistory.DataBind();
                            }
                            else
                                lblNoRecord.Visible = true;
                            break;
                    }
                }
                else
                {
                    type = "summary"; title = "Account Summary";
                    System.Data.DataSet dsDisplay = AccountSummary(userMemberCode);
                    if (dsDisplay.Tables[0].Rows.Count > 0)
                    {
                        lblNoRecord.Visible = false;
                        ListviewHistory.DataSource = dsDisplay.Tables[0];
                        ListviewHistory.DataBind();
                    }
                    else
                        lblNoRecord.Visible = true;

                }
            }


        }
    }


    private static DataSet AccountSummary(string userMemberCode)
    {

        System.Data.DataSet dsDisplay = new System.Data.DataSet();
        System.Data.DataTable dt = new System.Data.DataTable("History");
        dt.Columns.Add(new System.Data.DataColumn("stake", typeof(decimal)));
        dt.Columns.Add(new System.Data.DataColumn("earning", typeof(int)));
        dt.Columns.Add(new System.Data.DataColumn("redemption", typeof(int)));
        dt.Columns.Add(new System.Data.DataColumn("expired", typeof(int)));
        dt.Columns.Add(new System.Data.DataColumn("adjusted", typeof(int)));
        dt.Columns.Add(new System.Data.DataColumn("balance", typeof(int)));
        dt.Columns.Add(new System.Data.DataColumn("cart", typeof(int)));

        using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
        {
            DataSet ds = sClient.getMemberAccount(commonVariables.OperatorId, userMemberCode);

            if (ds.Tables[0].Rows.Count > 0)
            {

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    System.Web.HttpContext.Current.Session["totalStake"] = dr["totalStake"];
                }

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    System.Web.HttpContext.Current.Session["pointsAwarded"] = dr["pointsAwarded"];
                }

                foreach (DataRow dr in ds.Tables[2].Rows)
                {
                    System.Web.HttpContext.Current.Session["pointsRequired"] = dr["pointsRequired"];

                }
                foreach (DataRow dr in ds.Tables[4].Rows)
                {
                    System.Web.HttpContext.Current.Session["pointsExpired"] = dr["pointsExpired"];

                }
                foreach (DataRow dr in ds.Tables[3].Rows)
                {
                    System.Web.HttpContext.Current.Session["pointsAdjusted"] = dr["pointsAdjusted"];

                }
                foreach (DataRow dr in ds.Tables[5].Rows)
                {
                    System.Web.HttpContext.Current.Session["pointsBalance"] = dr["pointsBalance"];
                }
                foreach (DataRow dr in ds.Tables[6].Rows)
                {
                    System.Web.HttpContext.Current.Session["pointsCart"] = dr["pointsCart"];
                }

                //Point balance deduct cart
                System.Web.HttpContext.Current.Session["pointsBalance"] =
                    (int)System.Web.HttpContext.Current.Session["pointsBalance"] -
                    (int)System.Web.HttpContext.Current.Session["pointsCart"];

                System.Data.DataRow drPoints = dt.NewRow();
                drPoints["stake"] = System.Math.Round((decimal)System.Web.HttpContext.Current.Session["totalStake"], 2);
                drPoints["earning"] = (int)System.Web.HttpContext.Current.Session["pointsAwarded"];
                drPoints["redemption"] = (int)System.Web.HttpContext.Current.Session["pointsRequired"];
                drPoints["expired"] = (int)System.Web.HttpContext.Current.Session["pointsExpired"];
                drPoints["adjusted"] = (int)System.Web.HttpContext.Current.Session["pointsAdjusted"];
                drPoints["balance"] = (int)System.Web.HttpContext.Current.Session["pointsBalance"];
                drPoints["cart"] = (int)System.Web.HttpContext.Current.Session["pointsCart"];
                dt.Rows.Add(drPoints);
                dsDisplay.Tables.Add(dt);
            }
            return dsDisplay;

        }

    }


    public string TotalStake1(string userMemberCode)
    {

        using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
        {
            string html = "";
            System.Data.DataSet ds = sClient.getEarnProductFE(commonVariables.OperatorId, userMemberCode);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    html = "<table>";

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string columnname = "";

                        switch (dt.Columns[j].ColumnName)
                        {
                            case "walletName":
                                columnname = "Product Wallet";
                                break;
                            case "totalStake":
                                columnname = "Total Stake";
                                break;
                            case "pointsAwarded":
                                columnname = "Points Earn";
                                break;
                            default:
                                break;
                        }
                        html += "<tr><td><div class='pointDetailMainHeader'><span>" + columnname + "</span></div></td>";

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (j == 0)
                                html += "";
                            else if (j == 2) //stake
                                html += "<td><div class='points'><span>" + String.Format("{0:#,###,##0.##}", dt.Rows[i].ItemArray[j]) + "</span></div></td>";
                            else if (j == 3) //points earn
                                html += "<td><a href='/Account?type=stake&walletid=" + dt.Rows[i].ItemArray[0] + "'><div class='points'><span>" +
                                        String.Format("{0:#,###,##0.##}", dt.Rows[i].ItemArray[j]) + "</span></div></a></td>";
                            else
                                html += "<td><div class='points'><span>" + dt.Rows[i].ItemArray[j] +
                                        "</span></div></td>";

                        }
                        html += "</tr>";
                    }
                    html += "</table>";

                }
            }

            return html;

        }
    }

    public string TotalStake(string userMemberCode)
    {

        using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
        {
            string html = "";
            System.Data.DataSet ds = sClient.getEarnProductFE(commonVariables.OperatorId, userMemberCode);

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
                        string columnname = "";
                        tr += "<tr>";

                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (i == 0)
                            {
                                switch (dt.Columns[j].ColumnName)
                                {
                                    case "walletName":
                                        columnname = "Product Wallet";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    case "totalStake":
                                        columnname = "Total Stake";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    case "pointsAwarded":
                                        columnname = "Points Earn";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (j == 0)
                                tr += "";
                            else if (j == 2) //stake
                                tr += "<td><div class='points'><span>" + String.Format("{0:#,###,##0.##}", dt.Rows[i].ItemArray[j]) + "</span></div></td>";
                            else if (j == 3) //points earn
                                tr += "<td><a href='/Account?type=stake&walletid=" + dt.Rows[i].ItemArray[0] + "'><div class='points'><span>" +
                                        String.Format("{0:#,###,##0.##}", dt.Rows[i].ItemArray[j]) + "</span></div></a></td>";
                            else
                                tr += "<td><div class='points'><span>" + dt.Rows[i].ItemArray[j] +
                                        "</span></div></td>";

                        }
                        tr += "</tr>";
                    }
                    html = "<table width='100%'>" + "<tr>" + th + "</tr>" + tr + "</table>";

                }
            }

            return html;

        }
    }

    public string TotalStakeMonth(string userMemberCode, string walletid)//by wallet id
    {
        using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
        {
            System.Data.DataSet ds = sClient.getEarnMonthFE(commonVariables.OperatorId, userMemberCode, walletid);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    html = "";
                    string th = "";
                    string tr = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string columnname = "";
                        tr += "<tr>";

                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            if (i == 0)
                            {
                                switch (dt.Columns[j].ColumnName)
                                {
                                    case "pointsYear":
                                        columnname = "Year";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    case "pointsMonth":
                                        columnname = "Month";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    case "totalStake":
                                        columnname = "Total Stake";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    case "pointsAwarded":
                                        columnname = "Points Earn";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    default:
                                        break;
                                }
                            }
                            //j=0 year j=1 month
                            if (j == 2) //stake
                                tr += "<td><div class='points'><span>" + String.Format("{0:#,###,##0.##}", dt.Rows[i].ItemArray[j]) + "</span></div></td>";
                            else if (j == 3) //points earn
                                tr += "<td><a href='/Account?type=stake&walletid=" + walletid + "&yearmonth=" + dt.Rows[i].ItemArray[0] + dt.Rows[i].ItemArray[1] + "'><div class='points'><span>" +
                                        String.Format("{0:#,###,##0.##}", dt.Rows[i].ItemArray[j]) + "</span></div></a></td>";
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

    public string TotalStakeDetail(string userMemberCode, string walletid, string yearmonth)//by wallet id, year, month
    {
        int year = int.Parse(yearmonth.Substring(0, 4));
        int month = (yearmonth.Length > 5) ? int.Parse(yearmonth.Substring(4, 2)) : int.Parse(yearmonth.Substring(4, 1));
        
        using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
        {
          System.Data.DataSet ds = sClient.getEarnDetailFE(commonVariables.OperatorId, userMemberCode, walletid, month, year);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                     DataTable dt = ds.Tables[0];
                    html = "";
                    string th = "";
                    string tr = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
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
                                    case "transactionDateTime":
                                        columnname = "Bet Date";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    case "totalStake":
                                        columnname = "Total Stake";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    case "pointsAwarded":
                                        columnname = "Points Earn";
                                        th += "<td><div class='pointDetailMainHeaderHor'><span>" + columnname + "</span></div></td>";
                                        break;
                                    default:
                                       // th += "<td><div class='pointDetailMainHeaderHor'><span>" + dt.Columns[j].ColumnName + "</span></div></td>";
                                        break;
                                }
                            }
                           
                            if (j == 2) //stake
                                tr += "<td><div class='points'><span>" + String.Format("{0:#,###,##0.##}", dt.Rows[i].ItemArray[j]) + "</span></div></td>";
                            else if (j == 3) //points earn
                                tr += "<td><div class='points'><span>" +
                                      String.Format("{0:#,###,##0.##}", dt.Rows[i].ItemArray[j]) + "</span></div></td>";
                            else
                                tr += "<td><div class='points'><span>" +
                                      Convert.ToString(
                                          DateTime.Parse(dt.Rows[i].ItemArray[j].ToString()).ToString("yyyy/MM/dd")) +
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


}