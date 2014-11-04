using System;
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

public partial class Account : BasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        string type = "";
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
                }
            }

            //*
            string userMemberSessionId = string.IsNullOrEmpty((string)Session["MemberSessionId"])
                ? ""
                : (string)Session["MemberSessionId"];
            string userMemberId = string.IsNullOrEmpty((string)Session["MemberId"]) ? "" : (string)Session["MemberId"];
            string userMemberCode = string.IsNullOrEmpty((string)Session["MemberCode"])
                ? ""
                : (string)Session["MemberCode"];
            string countryCode = string.IsNullOrEmpty((string)Session["CountryCode"])
                ? "0"
                : (string)Session["CountryCode"];
            string currencyCode = string.IsNullOrEmpty((string)Session["CurrencyCode"])
                ? "0"
                : (string)Session["CurrencyCode"];
            string riskId = string.IsNullOrEmpty((string)Session["RiskId"]) ? "0" : (string)Session["RiskId"];
            string LanguageCode = string.IsNullOrEmpty((string)Session["LanguageCode"])
                ? "0"
                : (string)Session["LanguageCode"];
            //*

            System.Data.DataSet dsDisplay = AccountSummary(userMemberCode);

            if (dsDisplay.Tables[0].Rows.Count > 0)
            {
                lblNoRecord.Visible = false;
                ListviewHistory.DataSource = dsDisplay.Tables[0];
                ListviewHistory.DataBind();
            }
            else
            {
                lblNoRecord.Visible = true;
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
}