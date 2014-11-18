using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Data;
using System.Diagnostics;
using System.Text;

public partial class _Index : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;

    protected void Page_Init(object sender, EventArgs e)
    {
        System.Text.RegularExpressions.Regex rxDomains_CN = new System.Text.RegularExpressions.Regex(@"(.w88uat|.w88cn)");

        if (string.IsNullOrEmpty(commonVariables.SelectedLanguage))
        {
            if (rxDomains_CN.IsMatch(Request.ServerVariables["SERVER_NAME"]))
                commonVariables.SelectedLanguage = "zh-cn";
        }

        #region Catalogue

        using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
        {

            DataSet ds = sClient.getCatalogueSearch(commonVariables.OperatorId, commonVariables.SelectedLanguage,
                string.IsNullOrEmpty((string)Session["CountryCode"]) ? "0" : (string)Session["CountryCode"],
                string.IsNullOrEmpty((string)Session["CurrencyCode"]) ? "0" : (string)Session["CurrencyCode"],
                string.IsNullOrEmpty((string)Session["RiskId"]) ? "0" : (string)Session["RiskId"]);

            if (ds.Tables.Count > 0)
            {
                if (!ds.Tables[0].Columns.Contains("redemptionValidity"))
                    ds.Tables[0].Columns.Add("redemptionValidity");

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string imgNameOn = dr["imageNameOn"].ToString().Split('.')[0];
                    string imgPathOn = imgNameOn + ".png";
                    string imgNameOff = dr["imageNameOff"].ToString().Split('.')[0];
                    string imgPathOff = imgNameOn + ".png";

                    dr["imagePathOn"] =
                        Convert.ToString(
                            System.Configuration.ConfigurationManager.AppSettings.Get("ImagesDirectoryPath") +
                            "Category/" + imgPathOn);
                    dr["imagePathOff"] =
                        Convert.ToString(
                            System.Configuration.ConfigurationManager.AppSettings.Get("ImagesDirectoryPath") +
                            "Category/" + imgPathOff);

                    if (!string.IsNullOrEmpty((string)Session["user_riskID"]))
                    {
                        dr["redemptionValidity"] += ",";
                        if (dr["redemptionValidity"].ToString().ToUpper() != "ALL,")
                        {
                            if (
                                ((string)dr["redemptionValidity"]).IndexOf(
                                    ((string)Session["user_riskID"]).ToUpper() + ",") < 0)
                                dr["redemptionValidity"] = "0";
                            else
                                dr["redemptionValidity"] = "1";
                        }
                        else
                            dr["redemptionValidity"] = "1";
                    }
                    else
                        dr["redemptionValidity"] += "0";
                }
            }

            CategoryListView.DataSource = ds.Tables[0];
            DataTable dt = (DataTable)CategoryListView.DataSource;

            DataRow drAll = dt.NewRow();
            drAll["categoryId"] = "0";
            drAll["categoryName"] = "All";
            drAll["imagePathOn"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("ImagesDirectoryPath") + "Category/" + "clt_all_on.png");
            drAll["imagePathOff"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("ImagesDirectoryPath") + "Category/" + "clt_all_off.png");

            dt.Rows.InsertAt(drAll, 0);
            CategoryListView.DataBind();



        #endregion

            #region getProduct

            //HttpContext.Current.Session.Add("MemberSessionId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberSessionId"]));
            //HttpContext.Current.Session.Add("MemberId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberId"]));
            //HttpContext.Current.Session.Add("MemberCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberCode"]));
            //HttpContext.Current.Session.Add("CountryCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["countryCode"]));
            //HttpContext.Current.Session.Add("CurrencyCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["currencyCode"]));
            //HttpContext.Current.Session.Add("LanguageCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["languageCode"]));
            //HttpContext.Current.Session.Add("RiskId", Convert.ToString(dsSignin.Tables[0].Rows[0]["riskId"]));
            //HttpContext.Current.Session.Add("PartialSignup", Convert.ToString(dsSignin.Tables[0].Rows[0]["partialSignup"]));
            //HttpContext.Current.Session.Add("ResetPassword", Convert.ToString(dsSignin.Tables[0].Rows[0]["resetPassword"]));


            string categoryId = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("categoryId")) ? "0" : HttpContext.Current.Request.QueryString.Get("categoryId");
            string sortBy = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("sortBy")) ? "" : HttpContext.Current.Request.QueryString.Get("sortBy");
            int min = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("min")) ? 0 : int.Parse(HttpContext.Current.Request.QueryString.Get("min"));
            int max = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("max")) ? 2000000 : int.Parse(HttpContext.Current.Request.QueryString.Get("max"));
            string search = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("search")) ? "" : HttpContext.Current.Request.QueryString.Get("search");

            string userMemberId = string.IsNullOrEmpty((string)Session["MemberId"]) ? "" : (string)Session["MemberId"];
            string countryCode = string.IsNullOrEmpty((string)Session["CountryCode"]) ? "0" : (string)Session["CountryCode"];
            string currencyCode = string.IsNullOrEmpty((string)Session["CurrencyCode"]) ? "0" : (string)Session["CurrencyCode"];
            string riskId = string.IsNullOrEmpty((string)Session["RiskId"]) ? "0" : (string)Session["RiskId"];

            string pageSize = "15";
            string pageNum = "0";
            string pointLevel = "";
            int pointLevelDiscount = 0;
            int totalCount = 0;


            DataSet dsPr = sClient.getProductSearch(commonVariables.OperatorId, categoryId, commonVariables.SelectedLanguage, min, max, search,
                countryCode, currencyCode, riskId, System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), sortBy, pageSize, pageNum);

            ////continue shopping session
            //System.Web.HttpContext.Current.Session["categoryIdReload"] = categoryId;
            //System.Web.HttpContext.Current.Session["sortByReload"] = ddlSorting.SelectedValue;
            //System.Web.HttpContext.Current.Session["minReload"] =
            //    string.IsNullOrEmpty((string)Request.QueryString["min"])
            //        ? 0
            //        : int.Parse((string)Request.QueryString["min"]);
            //System.Web.HttpContext.Current.Session["maxReload"] =
            //    string.IsNullOrEmpty((string)Request.QueryString["max"])
            //        ? 2000000
            //        : int.Parse((string)Request.QueryString["max"]);
            //System.Web.HttpContext.Current.Session["searchReload"] =
            //    string.IsNullOrEmpty((string)Request.QueryString["search"])
            //        ? ""
            //        : (string)Request.QueryString["search"];
            //System.Web.HttpContext.Current.Session["sizeReload"] = string.IsNullOrEmpty(Request.QueryString["size"])
            //    ? "15"
            //    : Request.QueryString["size"];
            //System.Web.HttpContext.Current.Session["indexReload"] = string.IsNullOrEmpty(Request.QueryString["index"])
            //    ? 0
            //    : int.Parse(Request.QueryString["index"]);


            if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
            {
                pointLevel = sClient.getMemberPointLevelFE(userMemberId);
                pointLevelDiscount = sClient.getMemberPointLevelDiscount(commonVariables.OperatorId, currencyCode, pointLevel);
            }

            if (dsPr.Tables.Count > 0)
            {
                if (!dsPr.Tables[0].Columns.Contains("redemptionValidity"))
                    dsPr.Tables[0].Columns.Add("redemptionValidity");

                if (dsPr.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in dsPr.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
                        {
                            if (dr["discountPoints"] == DBNull.Value && pointLevelDiscount != 0 &&
                                dr["productType"].ToString() != "1")
                            {
                                //reverse to new points maintain in bo Discount
                                //dr["pointsRequired"] = int.Parse(dr["pointsRequired"].ToString()) - int.Parse(dr["discountPoints"].ToString());
                                double percentage = Convert.ToDouble(pointLevelDiscount) / 100;
                                int normalPoint = int.Parse(dr["pointsRequired"].ToString());

                                double points = Math.Floor(normalPoint * (1 - percentage));
                                int pointAfterLevelDiscount = Convert.ToInt32(points);
                                dr["pointsRequired"] = pointAfterLevelDiscount;
                            }
                        }

                        dr["imagePath"] =
                            Convert.ToString(
                                System.Configuration.ConfigurationManager.AppSettings.Get("ImagesDirectoryPath") +
                                "Product/" + dr["imageName"]);

                        categoryId = dr["categoryId"].ToString(); //???? 

                        //remove - cause overwrite during redeem
                        //System.Web.HttpContext.Current.Session["categoryId"] = categoryId;


                        if (!string.IsNullOrEmpty((string)Session["user_riskID"]))
                        {
                            dr["redemptionValidity"] += ",";
                            if (dr["redemptionValidity"].ToString().ToUpper() != "ALL,")
                            {
                                if (((string)dr["redemptionValidity"]).IndexOf(
                                        ((string)Session["user_riskID"]).ToUpper() + ",") < 0)
                                    dr["redemptionValidity"] = "0";
                                else
                                    dr["redemptionValidity"] = "1";
                            }
                            else
                                dr["redemptionValidity"] = "1";
                        }
                        else
                            dr["redemptionValidity"] += "0";
                    }


                    if (sortBy == "2")
                        dsPr.Tables[0].DefaultView.Sort = "pointsRequired";

                    //john paging
                    if (dsPr.Tables.Count > 1)
                    {
                        if (dsPr.Tables[1].Rows.Count > 0)
                            totalCount = int.Parse(dsPr.Tables[1].Rows[0][0].ToString());
                    }
                    ListviewProduct.DataSource = dsPr.Tables[0];
                    ListviewProduct.DataBind();
                    lblnodata.Visible = false;
                }
                else
                {
                    lblnodata.Visible = true;
                }

            }
           


        }


        //}

            #endregion


    }

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Web.UI.WebControls.Literal litScript = (System.Web.UI.WebControls.Literal)Page.FindControl("litScript");

        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("lang"))) { commonVariables.SelectedLanguage = HttpContext.Current.Request.QueryString.Get("lang"); }

        xeErrors = commonVariables.ErrorsXML;

        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getRootResource("/Index.aspx", out xeResources);

        if (!Page.IsPostBack)
        {
            if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("Error")) && !string.IsNullOrEmpty(commonVariables.GetSessionVariable("Error")))
            {
                Session.Remove("Error");
                if (litScript != null) { litScript.Text += string.Format("<script type='text/javascript'>alert('{0}');</script>", HttpContext.Current.Request.QueryString.Get("Error")); }
            }
            lblLogin.InnerHtml = commonCulture.ElementValues.getResourceString("lblPlaceBet", xeResources);//redemption xml not done



            if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
            {
                divLoginMessage.Visible = false;
                lblPoint.InnerText = "Points Bal: " + getCurrentPoints().ToString();
                divLevel.Visible = true;
            }

            //if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("categoryId")) &&
            // !string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("sortBy")))
            //{


            //}

        }





    }


    [WebMethod()]
    [System.Web.Script.Services.ScriptMethod(
    ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static int getCurrentPoints()
    {
        int total = 0;
        int claim = 0;
        int current = 0;
        int cart = 0;

        try
        {

            if (!string.IsNullOrEmpty((string)HttpContext.Current.Session["MemberId"]))
            {
                using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
                {
                    string strMemberCode = string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["MemberCode"] as string) ? string.Empty : Convert.ToString(System.Web.HttpContext.Current.Session["MemberCode"]);

                    System.Data.DataSet ds = sClient.getRedemptionDetail(commonVariables.OperatorId, strMemberCode);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            total = int.Parse(ds.Tables[0].Rows[0][0].ToString());

                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                claim = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                            }

                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                cart = int.Parse(ds.Tables[2].Rows[0][0].ToString());
                            }
                            claim = claim + cart;
                        }

                    }
                    current = total - claim;

                    HttpContext.Current.Session["pointsBalance"] = current;

                    return current;

                }
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            //throw;
            return 0;
        }
    }


    [WebMethod()]
    [System.Web.Script.Services.ScriptMethod(
    ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static int getPointLevel()
    {
        string pointLevel = "";

        try
        {

            if (!string.IsNullOrEmpty((string)HttpContext.Current.Session["MemberId"]))
            {
                //PMessageServices.PMessageClient pmClient = new PMessageServices.PMessageClient();
                //return pmClient.GetUnreadMessageCount(long.Parse((string)HttpContext.Current.Session["user_MemberID"])).ToString();
                using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
                {
                    pointLevel = sClient.getMemberPointLevelFE((string)HttpContext.Current.Session["MemberId"]);

                    return int.Parse(pointLevel);
                    //System.Web.HttpContext.Current.Session["pointsBalance"] = current;
                }
            }
            else
            {
                return 0;
            }
        }
        catch (Exception)
        {
            //throw;
            return 0;
        }
    }


    [WebMethod()]
    [System.Web.Script.Services.ScriptMethod(
    ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static string getPointLevelBar()
    {

        long pointsEarn = 0;
        //get current month earn
        var monthStart = DateTime.Today.AddDays(1 - DateTime.Today.Day);
        var monthEnd = monthStart.AddMonths(1).AddSeconds(-1);
        string strMemberCode = string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["user_MemberCode"] as string) ? string.Empty : (string)(System.Web.HttpContext.Current.Session["user_MemberCode"]);
        string pointLevel = "";
        string level = "";
        int nextLevel = 0;

        long nextPoints = 0;
        long currentPoints = 0;

        //decimal percentage = 0;
        int percentage = 0;
        int percentageColor = 0;
        //int pointPercentage = 0;

        try
        {

            if (!string.IsNullOrEmpty((string)HttpContext.Current.Session["user_MemberID"]))
            {
                //PMessageServices.PMessageClient pmClient = new PMessageServices.PMessageClient();
                //return pmClient.GetUnreadMessageCount(long.Parse((string)HttpContext.Current.Session["user_MemberID"])).ToString();

                using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
                {
                    //pointsEarn = 87;
                    pointsEarn = sClient.getMemberEarnFE(AppSettings.operator_id.ToString(), strMemberCode, monthStart.ToString("yyyy-MM-dd HH:mm:ss"), monthEnd.ToString("yyyy-MM-dd HH:mm:ss"));

                    pointLevel = sClient.getMemberPointLevelFE((string)HttpContext.Current.Session["user_MemberID"]);

                    currentPoints = sClient.getMemberPointLevelRequired(AppSettings.operator_id.ToString(), (string)HttpContext.Current.Session["user_Currency"], pointLevel);

                    //System.Data.DataSet ds = new DataSet();
                    System.Data.DataSet ds = sClient.getMemberPointLevelRange(AppSettings.operator_id.ToString(), (string)HttpContext.Current.Session["user_Currency"], pointsEarn);

                    if (ds.Tables.Count > 0)
                    {
                        ds.Tables[0].Columns.Add("pointsEarn");
                        ds.Tables[0].Columns.Add("currentLevel");
                        ds.Tables[0].Columns.Add("currentPoint");
                        ds.Tables[0].Columns.Add("percentage");
                        ds.Tables[0].Columns.Add("percentageColor");
                        ds.Tables[0].Columns.Add("remainPoint");
                        ds.Tables[0].Columns.Add("language");

                        foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                        {
                            level = dr["pointlevel"].ToString();

                            dr["pointsEarn"] = pointsEarn;
                            dr["currentLevel"] = pointLevel;
                            dr["currentPoint"] = currentPoints;
                            HttpContext.Current.Session["levelFrom"] = pointLevel;
                            dr["language"] = (string)HttpContext.Current.Session["language"];

                            if (pointLevel == "8")
                            {
                                nextLevel = int.Parse(pointLevel);
                                dr["pointlevel"] = nextLevel.ToString();
                                nextPoints = 0;
                            }
                            else if (int.Parse(level) <= int.Parse(pointLevel))
                            {
                                nextLevel = int.Parse(pointLevel) + 1;

                                if (nextLevel > 8)
                                {
                                    nextLevel = 8;
                                }
                                dr["pointlevel"] = nextLevel.ToString();
                                nextPoints = sClient.getMemberPointLevelRequired(AppSettings.operator_id.ToString(), (string)HttpContext.Current.Session["user_Currency"], nextLevel.ToString());
                                dr["pointsRequired"] = nextPoints;
                            }
                            else
                            {
                                nextLevel = int.Parse(level);
                                nextPoints = long.Parse(dr["pointsRequired"].ToString());
                            }

                            HttpContext.Current.Session["levelTo"] = nextLevel.ToString();

                            if (pointsEarn > currentPoints)
                            {
                                //percentage = ((pointsEarn - currentPoints) / (nextPoints - currentPoints)) * 100;

                                //grap 1 integer for level bar display
                                //percentage = Convert.ToInt32(Decimal.Divide((pointsEarn - currentPoints), (nextPoints - currentPoints)) * 100);
                                //percentageColor = Convert.ToInt32(Decimal.Divide((pointsEarn - currentPoints), (nextPoints - currentPoints)) * 10);

                                //get percentage from point 1
                                percentage = Convert.ToInt32(Decimal.Divide(pointsEarn, nextPoints) * 100);
                                percentageColor = Convert.ToInt32(Decimal.Divide(pointsEarn, nextPoints) * 10);

                                if (nextLevel >= 6)
                                {
                                    percentage = 50;
                                    percentageColor = 5;
                                }
                            }

                            long remainPoint = nextPoints - pointsEarn;

                            //HttpContext.Current.Session["remainPoint"] = remainPoint.ToString();
                            dr["remainPoint"] = remainPoint.ToString();
                            //HttpContext.Current.Session["percentage"] = percentage + "%";
                            //dr["percentage"] = percentage + "%";                
                            dr["percentage"] = percentage;
                            dr["percentageColor"] = percentageColor;
                        }


                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0], Newtonsoft.Json.Formatting.Indented);
                    //System.Web.HttpContext.Current.Session["pointsBalance"] = current;
                }
            }
            else
            {
                return "0";
            }
        }
        catch (Exception)
        {

            throw;
            //return "0";
        }
    }

}






