using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Configuration;
using System.Data;

public partial class _Report : System.Web.UI.Page
{

    protected System.Xml.Linq.XElement xeErrors = null;
    protected System.Xml.Linq.XElement xeResources = null;

    protected void Page_Load(object sender, EventArgs e)
    {

        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) { Response.Redirect("/Index.aspx", true); }

        else
        {
            xeErrors = commonVariables.ErrorsXML;
            commonCulture.appData.getRootResource("/Report.aspx", out xeResources);


            if (!IsPostBack)
            {
                

                setDDL();
                ddlSortBy.Items.Add(new ListItem(commonCulture.ElementValues.getResourceString("lbl_dropdown_earliest_first", xeResources), "1"));
                ddlSortBy.Items.Add(new ListItem(commonCulture.ElementValues.getResourceString("lbl_dropdown_latest_first", xeResources), "2"));
                //rb1 = "checked";
            }
            else
            {
                //if (Request.Form["Period"] != "")
                //{
                //    if (Request.Form["Period"] == "1")
                //        rb1 = "checked";
                //    else if (Request.Form["Period"] == "2")
                //        rb2 = "checked";
                //    else if (Request.Form["Period"] == "3")
                //        rb3 = "checked";
                //    else if (Request.Form["Period"] == "4")
                //        rb4 = "checked";
                //}

                string value = Request.Form["Period"];

                using (wsAffiliateMS1.affiliateWSSoapClient svcAffiliateMS1 = new wsAffiliateMS1.affiliateWSSoapClient())
                {
                //ws_affiliate.affiliateWSSoapClient wsaffiliate = new ws_affiliate.affiliateWSSoapClient();
                DateTime startdate = DateTime.Now.Date;
                DateTime enddate = DateTime.Now.Date;

                int validateResult = 1;

                if (value == "4")
                {
                    try
                    {
                        startdate = commonFunctions.DateParse(txtFrom.Text);
                        enddate = commonFunctions.DateParse(txtTo.Text);
                    }
                    catch (Exception)
                    {
                        validateResult = -4;
                    }
                }

                //Current day
                if (value == "1")
                {
                    startdate = DateTime.Now.Date;
                    enddate = DateTime.Now.Date;
                }//Current Week
                else if (value == "2")
                {
                    startdate = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek)).Date;
                    enddate = DateTime.Now.Date;
                }//Current Month
                else if (value == "3")
                {
                    startdate = DateTime.Today.AddDays(-1 * ((int)(DateTime.Today.Day) + 1)).Date;
                    enddate = DateTime.Now.Date;
                }
                else if (value == "4")
                {
                    try
                    {
                        startdate = commonFunctions.DateParse(txtFrom.Text);
                        enddate = commonFunctions.DateParse(txtTo.Text);
                    }
                    catch (Exception)
                    {

                    }
                }

                string message = "";
                string javascript = "";
                if (validateResult == -4)
                {
                    message = commonCulture.ElementValues.getResourceString("msg_option_from_to_date_blank", xeResources);
                    if (!string.IsNullOrEmpty(message))
                    {
                        javascript = string.Format("window.parent.showMessage({0});", commonFunctions.EncodeJsString(message)) + javascript;
                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "aworkinglongscriptkey", javascript, true);
                }
                else
                {
                    DataSet ds = svcAffiliateMS1.GetTrackingStatistic(long.Parse((string)System.Web.HttpContext.Current.Session["AffiliateId"]), long.Parse(ddlTrackingCode.SelectedValue), startdate, enddate);

                    if (ds.Tables.Count > 0)
                    {

                        lblTable.Text = "<table id=myTable>";
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string direction = "DESC";
                            if (ddlSortBy.SelectedValue == "1")
                                direction = "ASC";
                            else if (ddlSortBy.SelectedValue == "2")
                                direction = "DESC";

                            DataTable dt;

                            ds.Tables[0].DefaultView.Sort = "dates" + " " + direction;
                            dt = ds.Tables[0].DefaultView.ToTable();


                            //------start First Header--------
                            DataTable dtTrackGroup = dt.DefaultView.ToTable(true, "trackCodeName");
                            lblTable.Text += "<tr>";
                            lblTable.Text += "<td>";
                            lblTable.Text += "</td>";
                            for (int i = 0; i < dtTrackGroup.Rows.Count; i++)
                            {
                                lblTable.Text += "<td colspan='2'>";
                                lblTable.Text += dtTrackGroup.Rows[i]["trackCodeName"].ToString();
                                lblTable.Text += "</td>";
                            }
                            lblTable.Text += "</tr>";
                            //------end First Header--------


                            //------start Second Header--------
                            lblTable.Text += "<tr>";
                            lblTable.Text += "<td>";
                            lblTable.Text += commonCulture.ElementValues.getResourceString("lbl_table_header_date", xeResources).ToString();
                            lblTable.Text += "</td>";
                            for (int i = 0; i < dtTrackGroup.Rows.Count; i++)
                            {
                                lblTable.Text += "<td>";
                                lblTable.Text += commonCulture.ElementValues.getResourceString("lbl_table_header_clicks", xeResources).ToString();
                                lblTable.Text += "</td>";
                                lblTable.Text += "<td>";
                                lblTable.Text += commonCulture.ElementValues.getResourceString("lbl_table_header_unique_clicks", xeResources).ToString();
                                lblTable.Text += "</td>";
                            }
                            lblTable.Text += "</tr>";
                            //------end Second Header--------


                            //------start Rows for Column Date--------
                            DataTable dtDateDistinct = dt.DefaultView.ToTable(true, "dates");
                            for (int i = 0; i < dtDateDistinct.Rows.Count; i++)
                            {
                                //------start Second Header--------
                                lblTable.Text += "<tr>";
                                lblTable.Text += "<td>";
                                lblTable.Text += DateTime.Parse(dtDateDistinct.Rows[i]["dates"].ToString()).ToString(commonFunctions.wcDateFormat);
                                lblTable.Text += "</td>";
                                for (int x = 0; x < dtTrackGroup.Rows.Count; x++)
                                {
                                    bool hasValue = false;

                                    DataRow[] result = dt.Select("dates = '" + dtDateDistinct.Rows[i]["dates"].ToString() + "' AND trackCodeName = '" + dtTrackGroup.Rows[x]["trackCodeName"] + "'");
                                    foreach (DataRow row in result)
                                    {
                                        lblTable.Text += "<td>";
                                        lblTable.Text += row["CountClick"];
                                        lblTable.Text += "</td>";
                                        lblTable.Text += "<td>";
                                        lblTable.Text += row["CountUniqueClick"];
                                        lblTable.Text += "</td>";
                                        hasValue = true;
                                    }
                                    if (!hasValue)
                                    {
                                        lblTable.Text += "<td>";
                                        lblTable.Text += "0";
                                        lblTable.Text += "</td>";
                                        lblTable.Text += "<td>";
                                        lblTable.Text += "0";
                                        lblTable.Text += "</td>";
                                    }
                                }
                                lblTable.Text += "</tr>";
                                //------end Second Header--------
                            }
                            //------end Rows for Column Date--------


                            //------start Footer for Column Date--------
                            //------start Second Header--------
                            lblTable.Text += "<tr>";
                            lblTable.Text += "<td>";
                            lblTable.Text += commonCulture.ElementValues.getResourceString("lbl_table_footer_total_click", xeResources).ToString();
                            lblTable.Text += "</td>";
                            for (int x = 0; x < dtTrackGroup.Rows.Count; x++)
                            {
                                lblTable.Text += "<td>";
                                lblTable.Text += dt.Compute("Sum(CountClick)", "trackCodeName = '" + dtTrackGroup.Rows[x]["trackCodeName"] + "'");
                                lblTable.Text += "</td>";
                                lblTable.Text += "<td>";
                                lblTable.Text += dt.Compute("Sum(CountUniqueClick)", "trackCodeName = '" + dtTrackGroup.Rows[x]["trackCodeName"] + "'");
                                lblTable.Text += "</td>";
                            }
                            lblTable.Text += "</tr>";
                            //------end Second Header--------
                            //------end Rows for Column Date--------
                        }
                        else
                        {
                            lblTable.Text += "<tr>";
                            lblTable.Text += "<td>";
                            lblTable.Text += commonCulture.ElementValues.getResourceString("lbl_table_no_result", xeResources).ToString();
                            lblTable.Text += "</td>";
                            lblTable.Text += "</tr>";
                        }
                        lblTable.Text += "</table>";

                    }
                    else
                    {
                        lblTable.Text = "<table>";
                        lblTable.Text += "<tr>";
                        lblTable.Text += "<td>";
                        lblTable.Text += commonCulture.ElementValues.getResourceString("lbl_table_no_result", xeResources).ToString();
                        lblTable.Text += "</td>";
                        lblTable.Text += "</tr>";
                        lblTable.Text += "</table>";
                    }
                }
            }//ws
            }
        }
    }

    private void setDDL()
    {
        using (wsAffiliateMS1.affiliateWSSoapClient svcAffiliateMS1 = new wsAffiliateMS1.affiliateWSSoapClient())
        {
            //ws_affiliate.affiliateWSSoapClient wsaffiliate = new ws_affiliate.affiliateWSSoapClient();
            DataSet ds = svcAffiliateMS1.GetCreativeDropdown(long.Parse(System.Web.HttpContext.Current.Session["affaffiliateID"].ToString()), long.Parse(commonVariables.OperatorId));

            if (ds.Tables.Count > 0)
            {
                ddlTrackingCode.DataTextField = "trackCodeName";
                ddlTrackingCode.DataValueField = "trackCodeID";
                ddlTrackingCode.DataSource = ds.Tables[2];
                ddlTrackingCode.DataBind();
                ddlTrackingCode.Items.Add(new ListItem("ALL", "-1"));
                ddlTrackingCode.SelectedValue = "-1";

            }
        }
    }
}