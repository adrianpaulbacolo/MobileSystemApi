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
using System.Configuration;
using System.Net;

public partial class _Index : BasePage
{
    protected System.Xml.Linq.XElement xeErrors = null;
    public string localResx = "~/default.{0}.aspx";

    protected void Page_Init(object sender, EventArgs e)
    {
        string CDN_Value = string.Empty;
        string key = string.Empty;

        if (!string.IsNullOrEmpty(this.GetValue<string>(Request.ServerVariables[Keys.HTTP_X_AKAMAI_EDGESCAPE])))
        {
            CDN_Value = Request.ServerVariables[Keys.HTTP_X_AKAMAI_EDGESCAPE].ToString();
            key = Keys.HTTP_X_AKAMAI_EDGESCAPE;
        }
        if (!string.IsNullOrEmpty(this.GetValue<string>(Request.ServerVariables[Keys.HTTP_CF_IPCOUNTRY])))
        {
            CDN_Value = Request.ServerVariables[Keys.HTTP_CF_IPCOUNTRY].ToString();
            key = Keys.HTTP_CF_IPCOUNTRY;
        }

        if (!string.IsNullOrEmpty(this.GetValue<string>(Request.ServerVariables[Keys.HTTP_GEO_COUNTRY])))
        {
            CDN_Value = Request.ServerVariables[Keys.HTTP_GEO_COUNTRY].ToString();
            key = Keys.HTTP_GEO_COUNTRY;
        }


        if (!string.IsNullOrEmpty(CDN_Value) && !string.IsNullOrEmpty(key))
        {
            commonVariables.SelectedLanguage = GetLanguageByCountry(GetCountryCode(CDN_Value, key));
        }
        else
        {
            try
            {
                Uri myUri = new Uri(System.Web.HttpContext.Current.Request.Url.ToString());
                string[] host = myUri.Host.Split('.');

                commonVariables.SelectedLanguage = GetLanguageByDomain("." + host[1] + "." + host[2]);
            }
            catch (Exception)
            {
                commonVariables.SelectedLanguage = GetLanguageByDomain("default");
            }

        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Web.UI.WebControls.Literal litScript = (System.Web.UI.WebControls.Literal)Page.FindControl("litScript");

        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("lang"))) { commonVariables.SelectedLanguage = HttpContext.Current.Request.QueryString.Get("lang"); }
        localResx = string.Format("~/default.{0}.aspx", commonVariables.SelectedLanguage);

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
        }

        if (!string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId))
        {
            divLoginMessage.Visible = false;
            lblPoint.InnerText = HttpContext.GetLocalResourceObject(localResx, "lbl_points").ToString() + ": " + getCurrentPoints().ToString();
            divLevel.Visible = true;


        }
        #region Catalogue
        using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
        {

            //HttpContext.Current.Session.Add("MemberSessionId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberSessionId"]));
            //HttpContext.Current.Session.Add("MemberId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberId"]));
            //HttpContext.Current.Session.Add("MemberCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberCode"]));
            //HttpContext.Current.Session.Add("CountryCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["countryCode"]));
            //HttpContext.Current.Session.Add("CurrencyCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["currencyCode"]));
            //HttpContext.Current.Session.Add("LanguageCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["languageCode"]));
            //HttpContext.Current.Session.Add("RiskId", Convert.ToString(dsSignin.Tables[0].Rows[0]["riskId"]));
            //HttpContext.Current.Session.Add("PartialSignup", Convert.ToString(dsSignin.Tables[0].Rows[0]["partialSignup"]));
            //HttpContext.Current.Session.Add("ResetPassword", Convert.ToString(dsSignin.Tables[0].Rows[0]["resetPassword"]));
           
        

            DataSet ds = sClient.getCatalogueSearch(commonVariables.OperatorId, commonVariables.SelectedLanguage,
                string.IsNullOrEmpty((string)Session["CountryCode"]) ? "0" : (string)Session["CountryCode"],
            string.IsNullOrEmpty((string)Session["CurrencyCode"]) ? "0" : (string)Session["CurrencyCode"],
            string.IsNullOrEmpty((string)Session["RiskId"]) ? "0" : (string)Session["RiskId"]);

            if (ds.Tables.Count > 0)
            {
                if (!ds.Tables[0].Columns.Contains("redemptionValidity"))
                {
                    ds.Tables[0].Columns.Add("redemptionValidity");
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string imgNameOn = dr["imageNameOn"].ToString().Split('.')[0];
                    string imgPathOn = imgNameOn + ".png";

                    string imgNameOff = dr["imageNameOff"].ToString().Split('.')[0];
                    string imgPathOff = imgNameOn + ".png";


                    string catname = dr["categoryName"].ToString();

                    dr["imagePathOn"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("ImagesDirectoryPath") + "Category/" + imgPathOn);
                    dr["imagePathOff"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("ImagesDirectoryPath") + "Category/" + imgPathOff);

                    if (!string.IsNullOrEmpty((string)Session["user_riskID"]))
                    {
                        dr["redemptionValidity"] += ",";
                        if (dr["redemptionValidity"].ToString().ToUpper() != "ALL,")
                        {
                            if (((string)dr["redemptionValidity"]).IndexOf(((string)Session["user_riskID"]).ToUpper() + ",") < 0)
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

            Listview1.DataSource = ds;
            Listview1.DataBind();
            
        }
        #endregion

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



    public T GetValue<T>(object obj)
    {
        if (obj == DBNull.Value || obj == null)
        {
            return default(T);
        }

        return (T)Convert.ChangeType(obj, typeof(T));
    }


    private class Keys
    {
        public const string HTTP_X_AKAMAI_EDGESCAPE = "HTTP_X_AKAMAI_EDGESCAPE";
        public const string HTTP_CF_IPCOUNTRY = "HTTP_CF_IPCOUNTRY";
        public const string HTTP_GEO_COUNTRY = "HTTP_GEO_COUNTRY";
        public const string COUNTRY_DOMAIN_CN = "country_domain_cn";
        public const string COUNTRY_DOMAIN_VN = "country_domain_vn";
        public const string COUNTRY_DOMAIN_TH = "country_domain_th";
        public const string COUNTRY_DOMAIN_ID = "country_domain_id";
        public const string COUNTRY_DOMAIN_MY = "country_domain_my";
        public const string COUNTRY_DOMAIN_KR = "country_domain_kr";
        public const string COUNTRY_DOMAIN_JP = "country_domain_jp";
        public const string COUNTRY_DOMAIN_KH = "country_domain_kh";
    }


    public string GetCountryCode(string CDN_Value, string key)
    {
        string CountryCode = string.Empty;

        if (key == Keys.HTTP_X_AKAMAI_EDGESCAPE)
        {
            string[] Values = new string[100];
            Values = CDN_Value.Split(',');
            CountryCode = Values[1].Split('=')[1];
        }
        if (key == Keys.HTTP_CF_IPCOUNTRY)
        {
            CountryCode = CDN_Value;
        }
        if (key == Keys.HTTP_GEO_COUNTRY)
        {
            CountryCode = CDN_Value;
        }

        return CountryCode;
    }


    public string GetLanguageByCountry(string CountryCode)
    {
        switch (CountryCode.ToLower())
        {
            case "us":
                return "en-us";
            case "id":
                return "id-id";
            case "km-kh":
                return "km-kh";
            case "kr":
                return "ko-kr";
            case "th":
                return "th-th";
            case "vn":
                return "vi-vn";
            case "cn":
                return "zh-cn";
            case "jp":
                return "ja-jp";
            default:
                return "en-us";
        }
    }


    public string GetLanguageByDomain(string Domain)
    {
        string Language = string.Empty;

        if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_CN].Contains(Domain))
        {
            Language = "zh-cn";
        }
        else if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_VN].Contains(Domain))
        {
            Language = "vi-vn";
        }
        else if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_TH].Contains(Domain))
        {
            Language = "th-th";
        }
        else if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_ID].Contains(Domain))
        {
            Language = "id-id";
        }
        else if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_MY].Contains(Domain))
        {
            Language = "en-us";
        }
        else if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_KR].Contains(Domain))
        {
            Language = "ko-kr";
        }
        else if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_JP].Contains(Domain))
        {
            Language = "ja-jp";
        }
        else if (ConfigurationManager.AppSettings[Keys.COUNTRY_DOMAIN_KH].Contains(Domain))
        {
            Language = "km-kh";
        }
        else
        {
            Language = "en-us";
        }

        return Language;
    }

}
