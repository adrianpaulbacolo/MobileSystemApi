using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;

public partial class _Overview : System.Web.UI.Page
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected System.Xml.Linq.XElement xeResources = null;
    protected System.Xml.Linq.XElement xeResourcesMonth = null;
    protected System.Xml.Linq.XElement xeResourcesProduct = null;

    public string lastLogin = "";
    public string affiliateLink = "";
    public string affiliateUrl = "";
    public string totalMember = "";
    public string NoOfClick = "";
    public string newSignup = "0";
    public string newSignupWithDeposit = "0";
    public string activeMembers = "0";
    public string commissionCount = "";
    public string commission = "";
    public string negativeBalance = "";
    public string negativeBalCount = "";
    public string overallSummary = "";
    public string overallComm = "";
    public string overallNegBal = "";
    public string overallSummary2 = "";
    public string transactionData = "";
    public string totalSubAffCompanyWinLose = "0.00";
    public string totalSubAffActiveMember = "0";
    public string SubAffiliate = "";
    public string SubAffiliateProduct = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) { Response.Redirect("/Index.aspx", true); }

        else
        {
            xeErrors = commonVariables.ErrorsXML;
            commonCulture.appData.getRootResource("/Overview.aspx", out xeResources);
            commonCulture.appData.getRootResource("/Month.aspx", out xeResourcesMonth);
            commonCulture.appData.getRootResource("/ReportProduct.aspx", out xeResourcesProduct);

            //Link
            //testing
            //System.Web.HttpContext.Current.Session["AffiliateId"] = "20264";

            //affiliateLink = "/Track.aspx?affiliateid=" + Convert.ToString(string.IsNullOrEmpty((string)System.Web.HttpContext.Current.Session["AffiliateId"]) ? "" : (string)System.Web.HttpContext.Current.Session["AffiliateId"]);
            //affiliateUrl = System.Web.HttpContext.Current.Request.Url.Host.ToString() + "/Track.aspx?affiliateid=" + System.Web.HttpContext.Current.Session["AffiliateId"].ToString();

            affiliateLink = "http://affiliate.w88aff.com/Track.aspx?affiliateid=" + Convert.ToString(string.IsNullOrEmpty((string)System.Web.HttpContext.Current.Session["AffiliateId"]) ? "" : (string)System.Web.HttpContext.Current.Session["AffiliateId"]);
            affiliateUrl = "http://affiliate.w88aff.com/Track.aspx?affiliateid=" + System.Web.HttpContext.Current.Session["AffiliateId"].ToString();

            #region overall summary
            DataSet dsSummary = new DataSet("Summary");

            DataTable summary1 = dsSummary.Tables.Add("OverallSummary");

            dsSummary.Tables[0].Columns.Add(new DataColumn("S1"));
            dsSummary.Tables[0].Columns.Add(new DataColumn("S2"));

            //add header to dataset
            DataTable dt = (DataTable)dsSummary.Tables[0];
            DataRow drHeader = dt.NewRow();

            drHeader["S1"] = commonCulture.ElementValues.getResourceString("lblSummary", xeResources);

            dt.Rows.Add(drHeader);
            #endregion

            #region overall Commission
            DataSet dsComm = new DataSet("Comm");

            DataTable comm = dsComm.Tables.Add("OverallCommission");

            dsComm.Tables[0].Columns.Add(new DataColumn("C1"));
            dsComm.Tables[0].Columns.Add(new DataColumn("C2"));

            //add header to dataset
            DataTable dtComm = (DataTable)dsComm.Tables[0];
            DataRow drHeader2 = dtComm.NewRow();

            drHeader2["C1"] = commonCulture.ElementValues.getResourceString("lblCommission", xeResources);

            dtComm.Rows.Add(drHeader2);
            #endregion

            #region overall NegativeBal
            DataSet dsNegBal = new DataSet("NegativeBal");

            DataTable negBal = dsNegBal.Tables.Add("OverallNegativeBal");

            dsNegBal.Tables[0].Columns.Add(new DataColumn("N1"));
            dsNegBal.Tables[0].Columns.Add(new DataColumn("N2"));

            //add header to dataset
            DataTable dtNegBal = (DataTable)dsNegBal.Tables[0];
            DataRow drHeader3 = dtNegBal.NewRow();

            drHeader3["N1"] = commonCulture.ElementValues.getResourceString("lblNegBal", xeResources);

            dtNegBal.Rows.Add(drHeader3);
            #endregion

            #region overall summary part2
            DataSet ds2 = new DataSet("Overall2");

            DataTable summary2 = ds2.Tables.Add("OverallSummary2");

            ds2.Tables[0].Columns.Add(new DataColumn("QS1"));
            ds2.Tables[0].Columns.Add(new DataColumn("QS2"));

            //add header to dataset
            DataTable dt2 = (DataTable)ds2.Tables[0];
            DataRow drHeader4 = dt2.NewRow();

            drHeader4["QS1"] = commonCulture.ElementValues.getResourceString("lblQuickSummary", xeResources);

            dt2.Rows.Add(drHeader4);
            #endregion

            #region transaction data
            DataSet ds3 = new DataSet("TransData");

            DataTable transData = ds3.Tables.Add("TransData");

            ds3.Tables[0].Columns.Add(new DataColumn("P1A"));
            ds3.Tables[0].Columns.Add(new DataColumn("P1B"));

            #endregion

            #region sub affiliate
            DataSet dsSubAff = new DataSet("SubAffiliate");

            DataTable subAff = dsSubAff.Tables.Add("SubAffiliate");

            dsSubAff.Tables[0].Columns.Add(new DataColumn("S1"));
            dsSubAff.Tables[0].Columns.Add(new DataColumn("S2"));

            //add header to dataset
            DataTable dtSubAff = (DataTable)dsSubAff.Tables[0];
            DataRow drHeader6 = dtSubAff.NewRow();

            drHeader6["S1"] = commonCulture.ElementValues.getResourceString("lblSubAffTitle", xeResources);

            dtSubAff.Rows.Add(drHeader6);
            #endregion

            #region sub affiliate Product
            DataSet dsSubAffProduct = new DataSet("SubAffiliateProduct");

            DataTable subAffProduct = dsSubAffProduct.Tables.Add("SubAffiliateProduct");

            dsSubAffProduct.Tables[0].Columns.Add(new DataColumn("S1"));
            dsSubAffProduct.Tables[0].Columns.Add(new DataColumn("S2"));
            dsSubAffProduct.Tables[0].Columns.Add(new DataColumn("S3"));

            //add header to dataset
            DataTable dtSubAffProduct = (DataTable)dsSubAffProduct.Tables[0];
            DataRow drHeader7 = dtSubAffProduct.NewRow();

            drHeader7["S1"] = commonCulture.ElementValues.getResourceString("lblProduct", xeResources);
            drHeader7["S2"] = commonCulture.ElementValues.getResourceString("lblTurnOver", xeResources);
            drHeader7["S3"] = commonCulture.ElementValues.getResourceString("lblWinLoss", xeResources);

            dtSubAffProduct.Rows.Add(drHeader7);
            #endregion

            string first = commonCulture.ElementValues.getResourceString("lbl1sthalf", xeResourcesMonth);
            string second = commonCulture.ElementValues.getResourceString("lbl2ndhalf", xeResourcesMonth);

            //Overview
            using (wsAffiliateMS1.affiliateWSSoapClient svcAffiliateMS1 = new wsAffiliateMS1.affiliateWSSoapClient())
            {

                DataSet dsAffOverview = svcAffiliateMS1.AffiliateOverview(long.Parse((string)System.Web.HttpContext.Current.Session["AffiliateId"]));
                DataSet dsProdOverview = svcAffiliateMS1.ProductOverview(long.Parse((string)System.Web.HttpContext.Current.Session["AffiliateId"]));
                DataTable dtAffOverviewProduct = dsProdOverview.Tables[0].DefaultView.ToTable(true, "productName");

                #region aff Info & summary
                if (dsAffOverview.Tables.Count > 0)
                {
                    if (dsAffOverview.Tables[0].Rows.Count > 0)
                    {

                        #region Summary
                        DataTable dtData = (DataTable)dsSummary.Tables[0];
                        DataRow drData = dtData.NewRow();

                        DataTable dtDataQuick = (DataTable)ds2.Tables[0];


                        //if (Session["language"] != null && (string)Session["language"].ToString() == "zh-cn")

                        if (commonVariables.SelectedLanguage != null && commonVariables.SelectedLanguage == "zh-cn")
                        {
                            lastLogin = dsAffOverview.Tables[0].Rows[0]["lastLogiin"].ToString();

                            if (dsAffOverview.Tables[0].Rows[0]["lastLogiin"] != null && dsAffOverview.Tables[0].Rows[0]["lastLogiin"].ToString() != "")
                            {
                                int month_int = DateTime.ParseExact(lastLogin, "dd MMM yyyy HH:mm:ss", CultureInfo.CurrentCulture).Month;
                                string month_str = DateTime.ParseExact(lastLogin, "dd MMM yyyy HH:mm:ss", CultureInfo.CurrentCulture).ToString("MMM");
                                lastLogin = lastLogin.Replace(month_str, month_int.ToString() + "月");
                            }
                            else
                            {
                                lastLogin = dsAffOverview.Tables[0].Rows[0]["lastLogiin"].ToString();
                            }

                        }
                        else
                        {
                            lastLogin = dsAffOverview.Tables[0].Rows[0]["lastLogiin"].ToString();
                        }

                        totalMember = dsAffOverview.Tables[0].Rows[0]["TotalMember"].ToString();

                        drData["S1"] = commonCulture.ElementValues.getResourceString("lblTotalMember", xeResources);
                        drData["S2"] = totalMember;
                        dtData.Rows.Add(drData);

                        #endregion

                        NoOfClick = dsAffOverview.Tables[0].Rows[0]["NoOfClick"].ToString();

                        DataRow drDataQuick = dtDataQuick.NewRow();
                        drDataQuick["QS1"] = commonCulture.ElementValues.getResourceString("lblNoOfClicks", xeResources);
                        drDataQuick["QS2"] = NoOfClick;
                        dt2.Rows.Add(drDataQuick);

                        using (mwsAffiliateMain.affiliateWSSoapClient svcAffiliateMain = new mwsAffiliateMain.affiliateWSSoapClient())
                        {
                            DataSet dsNewSignupMemberCount = svcAffiliateMain.NewSignupMemberCount(long.Parse(Session["AffiliateId"].ToString()), DateTime.Now.Month, DateTime.Now.Year);

                            if (dsNewSignupMemberCount.Tables.Count > 0)
                            {
                                newSignup = dsNewSignupMemberCount.Tables[0].Rows[0][0].ToString();
                                newSignupWithDeposit = dsNewSignupMemberCount.Tables[1].Rows[0][0].ToString();
                            }
                            else
                            {
                                newSignup = "0";
                                newSignupWithDeposit = "0";
                            }

                            DataRow drDataQuick2 = dtDataQuick.NewRow();
                            drDataQuick2["QS1"] = commonCulture.ElementValues.getResourceString("lblNewSignUp", xeResources);
                            drDataQuick2["QS2"] = newSignup;
                            dt2.Rows.Add(drDataQuick2);

                            DataRow drDataQuick3 = dtDataQuick.NewRow();
                            drDataQuick3["QS1"] = commonCulture.ElementValues.getResourceString("lblNewSignUpWDeposit", xeResources);
                            drDataQuick3["QS2"] = newSignupWithDeposit;
                            dt2.Rows.Add(drDataQuick3);

                            #region commission
                            //Commission

                            DataTable dtDataComm = (DataTable)dsComm.Tables[0];
                            DataRow drDataComm = dtDataComm.NewRow();

                            if (dsAffOverview.Tables[1].Rows.Count > 0)
                            {
                                commissionCount = dsAffOverview.Tables[1].Rows.Count.ToString();

                                for (int i = 0; i < 1; i++)
                                {

                                    string monthx = dsAffOverview.Tables[1].Rows[i]["settlementdate"].ToString().Replace("1st half ", "").Replace("2nd half ", "");
                                    int month_int = DateTime.ParseExact(monthx, "MMM yyyy", CultureInfo.CurrentCulture).Month;
                                    string month_str = DateTime.ParseExact(monthx, "MMM yyyy", CultureInfo.CurrentCulture).ToString("MMM");
                                    string monthTranslate = commonCulture.ElementValues.getResourceString("lbl" + month_int.ToString(), xeResourcesMonth);
                                    string dateFinal = dsAffOverview.Tables[1].Rows[i]["settlementdate"].ToString().Replace("1st half", first).Replace("2nd half", second).Replace(month_str, monthTranslate);

                                    drDataComm["C1"] = dateFinal;
                                    drDataComm["C2"] = "USD " + commonFunctions.Tuncate2DecimalToString(double.Parse(dsAffOverview.Tables[1].Rows[i]["grandtotal"].ToString()));
                                }
                            }
                            else
                            {
                                drDataComm["C1"] = "No Data";
                                commissionCount = "0";
                            }

                            dtDataComm.Rows.Add(drDataComm);

                            #endregion

                            #region Negative Balance
                            //------------- Negatie balance Rollover

                            DataTable dtDataNegBal = (DataTable)dsNegBal.Tables[0];
                            DataRow drDataNegBal = dtDataNegBal.NewRow();

                            if (dsAffOverview.Tables[2].Rows.Count > 0)
                            {
                                negativeBalCount = dsAffOverview.Tables[2].Rows.Count.ToString();

                                for (int i = 0; i < 1; i++)
                                {
                                    string monthx = dsAffOverview.Tables[2].Rows[i]["settlementdate"].ToString().Replace("1st half ", "").Replace("2nd half ", "");
                                    int month_int = DateTime.ParseExact(monthx, "MMM yyyy", CultureInfo.CurrentCulture).Month;
                                    string month_str = DateTime.ParseExact(monthx, "MMM yyyy", CultureInfo.CurrentCulture).ToString("MMM");
                                    string monthTranslate = commonCulture.ElementValues.getResourceString("lbl" + month_int.ToString(), xeResourcesMonth);
                                    string dateFinal = dsAffOverview.Tables[2].Rows[i]["settlementdate"].ToString().Replace("1st half", first).Replace("2nd half", second).Replace(month_str, monthTranslate);

                                    drDataNegBal["N1"] = dateFinal;
                                    drDataNegBal["N2"] = "USD " + commonFunctions.Tuncate2DecimalToString(double.Parse(dsAffOverview.Tables[2].Rows[i]["NegativeBalanceRollover"].ToString()));

                                }
                            }
                            else
                            {
                                drDataNegBal["N1"] = "No Data";
                                negativeBalCount = "0";
                            }

                            dtDataNegBal.Rows.Add(drDataNegBal);

                            #endregion

                            //add remaining row of record
                            if (int.Parse(commissionCount) > 1 || int.Parse(negativeBalCount) > 1)
                            {
                                int comCount = commissionCount == "" ? 0 : int.Parse(commissionCount);
                                int negCount = negativeBalCount == "" ? 0 : int.Parse(negativeBalCount);
                                int count = comCount >= negCount ? comCount : negCount;

                                for (int i = 1; i < count; i++)
                                {

                                    if (i < comCount)
                                    {
                                        drDataComm = dtDataComm.NewRow();

                                        string monthx = dsAffOverview.Tables[1].Rows[i]["settlementdate"].ToString().Replace("1st half ", "").Replace("2nd half ", "");
                                        int month_int = DateTime.ParseExact(monthx, "MMM yyyy", CultureInfo.CurrentCulture).Month;
                                        string month_str = DateTime.ParseExact(monthx, "MMM yyyy", CultureInfo.CurrentCulture).ToString("MMM");
                                        string monthTranslate = commonCulture.ElementValues.getResourceString("lbl" + month_int.ToString(), xeResourcesMonth);
                                        string dateFinal = dsAffOverview.Tables[1].Rows[i]["settlementdate"].ToString().Replace("1st half", first).Replace("2nd half", second).Replace(month_str, monthTranslate);

                                        drDataComm["C1"] = dateFinal;
                                        drDataComm["C2"] = "USD " + commonFunctions.Tuncate2DecimalToString(double.Parse(dsAffOverview.Tables[1].Rows[i]["grandtotal"].ToString()));

                                        dtDataComm.Rows.Add(drDataComm);
                                    }

                                    if (i < negCount)
                                    {
                                        drDataNegBal = dtDataNegBal.NewRow();

                                        string monthx = dsAffOverview.Tables[2].Rows[i]["settlementdate"].ToString().Replace("1st half ", "").Replace("2nd half ", "");
                                        int month_int = DateTime.ParseExact(monthx, "MMM yyyy", CultureInfo.CurrentCulture).Month;
                                        string month_str = DateTime.ParseExact(monthx, "MMM yyyy", CultureInfo.CurrentCulture).ToString("MMM");
                                        string monthTranslate = commonCulture.ElementValues.getResourceString("lbl" + month_int.ToString(), xeResourcesMonth);
                                        string dateFinal = dsAffOverview.Tables[2].Rows[i]["settlementdate"].ToString().Replace("1st half", first).Replace("2nd half", second).Replace(month_str, monthTranslate);

                                        drDataNegBal["N1"] = dateFinal;
                                        drDataNegBal["N2"] = "USD " + commonFunctions.Tuncate2DecimalToString(double.Parse(dsAffOverview.Tables[2].Rows[i]["NegativeBalanceRollover"].ToString()));

                                        dtDataNegBal.Rows.Add(drDataNegBal);

                                    }


                                }
                            }

                            if (dsProdOverview.Tables.Count > 0)
                            {
                                if (dsProdOverview.Tables[1].Rows.Count > 0)
                                {
                                    activeMembers = dsProdOverview.Tables[1].Rows[0]["ActiveMember"].ToString();
                                }
                                else
                                {
                                    activeMembers = "0";
                                }

                                DataRow drDataQuick4 = dtDataQuick.NewRow();
                                drDataQuick4["QS1"] = commonCulture.ElementValues.getResourceString("lblActiveMembers", xeResources);
                                drDataQuick4["QS2"] = activeMembers;
                                dt2.Rows.Add(drDataQuick4);
                            }
                        }
                    }
                }
                #endregion

                #region Transaction Data
                if (dsProdOverview.Tables.Count > 0)
                {
                    if (dsProdOverview.Tables[0].Rows.Count > 0)
                    {
                        DataTable dt3 = (DataTable)ds3.Tables[0];
                        DataRow drHeader5 = dt3.NewRow();

                        DataTable dtWL = (DataTable)ds3.Tables[0];
                        DataRow drWL = dtWL.NewRow();

                        DataTable dtAM = (DataTable)ds3.Tables[0];
                        DataRow drAM = dtAM.NewRow();

                        DataTable dtNB = (DataTable)ds3.Tables[0];
                        DataRow drNB = dtNB.NewRow();

                        DataTable dtTO = (DataTable)ds3.Tables[0];
                        DataRow drTO = dtTO.NewRow();

                        DataTable dtEmptyLine = (DataTable)ds3.Tables[0];
                        DataRow drEmptyLine = dtEmptyLine.NewRow();

                        //foreach (DataRow drProdOverview in dsProdOverview.Tables[0].Rows)
                        for (int i = 0; i < dtAffOverviewProduct.Rows.Count; i++)
                        {
                            DataRow[] result = dsProdOverview.Tables[0].Select("productName = '" + dtAffOverviewProduct.Rows[i]["productName"].ToString() + "'");
                            foreach (DataRow row in result)
                            {
                              
                                drHeader5["P1A"] = commonCulture.ElementValues.getResourceString("lblTitle", xeResources) + commonCulture.ElementValues.getResourceString("lbl" + dtAffOverviewProduct.Rows[i]["productName"].ToString().Replace(" ", string.Empty).ToLower(), xeResourcesProduct);
                                dt3.Rows.Add(drHeader5);
                                drHeader5 = dt3.NewRow();

                                drWL["P1A"] = commonCulture.ElementValues.getResourceString("lblWinLoss", xeResources);
                                drWL["P1B"] = "USD " + decimal.Parse(dsProdOverview.Tables[0].Rows[i]["base_winlost_amount"].ToString()).ToString("n2");
                                dtWL.Rows.Add(drWL);
                                drWL = dtWL.NewRow();

                                drAM["P1A"] = commonCulture.ElementValues.getResourceString("lblActiveMember", xeResources);
                                drAM["P1B"] = dsProdOverview.Tables[0].Rows[i]["totalactivemember"].ToString();
                                dtAM.Rows.Add(drAM);
                                drAM = dtAM.NewRow();

                                drNB["P1A"] = commonCulture.ElementValues.getResourceString("lblBets", xeResources);
                                drNB["P1B"] = dsProdOverview.Tables[0].Rows[i]["total_hand"].ToString();
                                dtNB.Rows.Add(drNB);
                                drNB = dtNB.NewRow();

                                drTO["P1A"] = commonCulture.ElementValues.getResourceString("lblTurnOver", xeResources);
                                drTO["P1B"] = "USD " + decimal.Parse(dsProdOverview.Tables[0].Rows[i]["base_stake_amount"].ToString()).ToString("n2");
                                dtTO.Rows.Add(drTO);
                                drTO = dtTO.NewRow();

                            }
                        }

                    }
                }
                #endregion

                #region sub affiliate summary & product
                DataSet dsProdSubAff = svcAffiliateMS1.ProductSubAffOverview(long.Parse((string)System.Web.HttpContext.Current.Session["AffiliateId"]));
                DataTable dtProdSubAff = dsProdSubAff.Tables[0].DefaultView.ToTable(true, "productName");

                DataTable dtSA = (DataTable)dsSubAff.Tables[0];
                DataRow drDataSubAff = dtSA.NewRow();

                if (dsProdSubAff.Tables.Count > 0)
                {
                    if (dsProdSubAff.Tables[1].Rows.Count > 0)
                    {

                        drDataSubAff["S1"] = commonCulture.ElementValues.getResourceString("lblWinLoss", xeResources);
                        drDataSubAff["S2"] = "USD " + decimal.Parse(dsProdSubAff.Tables[1].Rows[0]["base_winlost_amount"].ToString()).ToString("n2");
                        dtSA.Rows.Add(drDataSubAff);

                        drDataSubAff = dtSA.NewRow();
                        drDataSubAff["S1"] = commonCulture.ElementValues.getResourceString("lblActiveMember", xeResources);
                        drDataSubAff["S2"] = dsProdSubAff.Tables[1].Rows[0]["ActiveMember"].ToString();
                        dtSA.Rows.Add(drDataSubAff);


                        //totalSubAffActiveMember = dsProdSubAff.Tables[1].Rows[0]["ActiveMember"].ToString();
                        //totalSubAffCompanyWinLose = commonFunction.Tuncate2DecimalToString(double.Parse(dsProdSubAff.Tables[1].Rows[0]["base_winlost_amount"].ToString()));
                    }
                    else
                    {
                        drDataSubAff["S1"] = commonCulture.ElementValues.getResourceString("lblWinLoss", xeResources);
                        drDataSubAff["S2"] = totalSubAffCompanyWinLose;
                        dtSA.Rows.Add(drDataSubAff);

                        drDataSubAff = dtSA.NewRow();
                        drDataSubAff["S1"] = commonCulture.ElementValues.getResourceString("lblActiveMember", xeResources);
                        drDataSubAff["S2"] = totalSubAffActiveMember;
                        dtSA.Rows.Add(drDataSubAff);
                    }


                    //lblSubProduct.Text = "";
                    //for (int i = 0; i < dtProdSubAff.Rows.Count; i++)
                    //{
                    //    DataRow[] result = dsProdSubAff.Tables[0].Select("productName = '" + dtProdSubAff.Rows[i]["productName"].ToString() + "'");
                    //    foreach (DataRow row in result)
                    //    {
                    //        lblSubProduct.Text += "<tr>";
                    //        lblSubProduct.Text += setCurrentMonthSubAffProduct(HttpContext.GetLocalResourceObject(localResx_product, dtProdSubAff.Rows[i]["productName"].ToString()).ToString(), commonFunction.Tuncate2DecimalToString(double.Parse(row["base_winlost_amount"].ToString())), commonFunction.Tuncate2DecimalToString(double.Parse(row["base_stake_amount"].ToString())));
                    //        lblSubProduct.Text += "</tr>";
                    //    }
                    //}

                    DataTable dtSAProduct = (DataTable)dsSubAffProduct.Tables[0];
                    DataRow drDataSubAffProduct = dtSAProduct.NewRow();

                    for (int i = 0; i < dtProdSubAff.Rows.Count; i++)
                    {
                        DataRow[] result = dsProdSubAff.Tables[0].Select("productName = '" + dtProdSubAff.Rows[i]["productName"].ToString() + "'");
                        foreach (DataRow row in result)
                        {
                            //drDataSubAffProduct["S1"] = dtProdSubAff.Rows[i]["productName"].ToString();
                            drDataSubAffProduct["S1"] = commonCulture.ElementValues.getResourceString("lbl" + dtProdSubAff.Rows[i]["productName"].ToString().Replace(" ", string.Empty).ToLower(), xeResourcesProduct);
                            drDataSubAffProduct["S2"] = "USD " + decimal.Parse(row["base_winlost_amount"].ToString()).ToString("n2");
                            drDataSubAffProduct["S3"] = "USD " + decimal.Parse(row["base_stake_amount"].ToString()).ToString("n2");
                            dtSAProduct.Rows.Add(drDataSubAffProduct);
                            drDataSubAffProduct = dtSAProduct.NewRow();
                        }
                    }

                }
                #endregion
            }

            //Add in data table
            DataTable dtOverallSummary = (DataTable)dsSummary.Tables[0];
            overallSummary = ConvertDataTableToHTML(dtOverallSummary);

            DataTable dtOverallComm = (DataTable)dsComm.Tables[0];
            overallComm = ConvertDataTableToHTML(dtOverallComm);

            DataTable dtOverallNegBal = (DataTable)dsNegBal.Tables[0];
            overallNegBal = ConvertDataTableToHTML(dtOverallNegBal);

            DataTable dtOverallSummary2 = (DataTable)ds2.Tables[0];
            overallSummary2 = ConvertDataTableToHTML(dtOverallSummary2);

            DataTable dtTransData = (DataTable)ds3.Tables[0];
            transactionData = ConvertDataTableToHTML2(dtTransData);

            DataTable dtSubAff1 = (DataTable)dsSubAff.Tables[0];
            SubAffiliate = ConvertDataTableToHTML(dtSubAff1);

            DataTable dtSubAff2 = (DataTable)dsSubAffProduct.Tables[0];
            SubAffiliateProduct = ConvertDataTableToHTML3(dtSubAff2);
        }
    }

    public static string ConvertDataTableToHTML(DataTable dt)
    {
        string html = "<table width=100% cellspacing=0 cellpadding=0>";

        //add header row
        //html += "<tr>";
        //for (int i = 0; i < dt.Columns.Count; i++)
        //    html += "<td>" + dt.Columns[i].ColumnName + "</td>";
        //html += "</tr>";

        //add rows
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            html += "<tr>";
            for (int j = 0; j < dt.Columns.Count; j++)
            {

                if (i == 0)
                {
                    //if (j == 1 || j == 3 || j == 5)
                    if (j % 2 != 0)
                    {
                        html += "";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                        {
                            html += "<td style=background-color:#1f1f1f;></td>";
                        }
                        else if (j != 4)
                        {
                            html += "<td align=center colspan=2 style=border:0px;border-bottom:2px;border-style:solid;border-color:#1f1f1f;>" + dt.Rows[i][j].ToString() + "</td>";
                        }
                        else
                        {
                            html += "<td align=center colspan=2>" + dt.Rows[i][j].ToString() + "</td>";
                        }
                    }


                }
                else if (string.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                {
                    html += "";
                }
                else if (j == 1)
                {
                    html += "<td align=right style=border:0px;border-bottom:2px;border-style:solid;border-color:#1f1f1f;>" + dt.Rows[i][j].ToString() + "</td>";
                }
                else
                {
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                }
            }
            html += "</tr>";
        }
        html += "</table>";
        return html;
    }

    public static string ConvertDataTableToHTML2(DataTable dt)
    {
        string html = "<table width=100% cellspacing=0 cellpadding=0>";

        //add header row
        //html += "<tr>";
        //for (int i = 0; i < dt.Columns.Count; i++)
        //    html += "<td>" + dt.Columns[i].ColumnName + "</td>";
        //html += "</tr>";

        //add rows
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            html += "<tr>";
            for (int j = 0; j < dt.Columns.Count; j++)
            {

                if (i == 0 || i % 5== 0)
                {
                    if (j % 2 != 0)
                    {
                        html += "";
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                        {
                            html += "<td style=background-color:#1f1f1f;></td>";
                        }
                        else if (j != 4)
                        {
                            html += "<td align=center colspan=2 style=border:0px;border-bottom:2px;border-style:solid;border-color:#1f1f1f;background-color:#333333;>" + dt.Rows[i][j].ToString() + "</td>";
                        }
                        else
                        {
                            html += "<td align=center colspan=2>" + dt.Rows[i][j].ToString() + "</td>";
                        }
                    }


                }
                else if (string.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                {
                    html += "";
                }
                else if (j == 1)
                {
                    html += "<td align=right style=border:0px;border-bottom:2px;border-style:solid;border-color:#1f1f1f;>" + dt.Rows[i][j].ToString() + "</td>";
                }
                else
                {
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                }
            }
            html += "</tr>";
        }
        html += "</table>";
        return html;
    }

    public static string ConvertDataTableToHTML3(DataTable dt)
    {
        string html = "<table width=100% cellspacing=0 cellpadding=0>";

        //add header row
        //html += "<tr>";
        //for (int i = 0; i < dt.Columns.Count; i++)
        //    html += "<td>" + dt.Columns[i].ColumnName + "</td>";
        //html += "</tr>";

        //add rows
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            html += "<tr>";
            for (int j = 0; j < dt.Columns.Count; j++)
            {

                if (i == 0)
                {
                    //if (j == 1 || j == 3 || j == 5)
                    //if (j % 2 != 0)
                    //{
                    //    html += "";
                    //}
                    //else
                    //{
                        if (string.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                        {
                            html += "<td style=background-color:#1f1f1f;></td>";
                        }
                        else if (j != 4)
                        {
                            html += "<td align=center style=border:0px;border-bottom:2px;border-style:solid;border-color:#1f1f1f;>" + dt.Rows[i][j].ToString() + "</td>";
                        }
                        else
                        {
                            html += "<td align=center>" + dt.Rows[i][j].ToString() + "</td>";
                        }
                    //}


                }
                else if (string.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                {
                    html += "";
                }
                else if (j != 0)
                {
                    html += "<td align=right style=border:0px;border-bottom:2px;border-style:solid;border-color:#1f1f1f;>" + dt.Rows[i][j].ToString() + "</td>";
                }
                else
                {
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                }
            }
            html += "</tr>";
        }
        html += "</table>";
        return html;
    }

}