using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class common_handler : System.Web.UI.Page
{

	private static string page_name = "handler.aspx.cs";
	//Private audit_serial_id As Integer = 0
	private static string audit_id;
	private static string audit_task;
	private static string audit_comp;
	private static string audit_detail;
	private static string audit_remark;

	[WebMethod]
	[System.Web.Script.Services.ScriptMethod(
	ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
	public static DateTime timer()
	{
		string[] time = { DateTime.Now.ToString("dd/MM/yyyy"), DateTime.Now.ToString("HH:mm:ss") };
		return DateTime.Now;
	}

	[WebMethod]
	[System.Web.Script.Services.ScriptMethod(
	ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
	public static string getAnnouncement()
	{
		ws_announcement.announcementWSSoapClient ws_announcement = new ws_announcement.announcementWSSoapClient();
		DataSet ds = ws_announcement.GetMemberSiteAnnouncement(AppSettings.operator_id, (string)HttpContext.Current.Session["language"], (!string.IsNullOrEmpty((string)HttpContext.Current.Session["user_MemberID"])) ? long.Parse((string)HttpContext.Current.Session["user_MemberID"]) : 0);
		string announcementHtml = "";
		if (ds.Tables.Count > 0)
		{
			if (ds.Tables[0].Rows.Count > 0)
			{
				//ds.Tables[0].Rows[0]["announcementdetails"]
				foreach (DataRow dr in ds.Tables[0].Rows)
				{
					announcementHtml += string.Format("<li class=\"announcement_{1}\">{0}</li>", dr["announcementdetails"], dr["announcementid"]);
				}

				announcementHtml = string.Format("<ul>{0}</ul>", announcementHtml);
			}
		}
		return System.Web.HttpContext.Current.Server.HtmlEncode(announcementHtml);
	}




    //cart count
    [WebMethod()]
    [System.Web.Script.Services.ScriptMethod(
    ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static string cartCount()
    {
        string cartCount="";
        try
        {
            if (!string.IsNullOrEmpty((string)HttpContext.Current.Session["user_MemberID"]))
            {
                using (RewardsServices.RewardsServicesClient sClientCart = new RewardsServices.RewardsServicesClient())
                {
					DataSet ds = sClientCart.getCart(AppSettings.operator_id.ToString(), (string)System.Web.HttpContext.Current.Session["user_MemberCode"], (string)HttpContext.Current.Session["language"], (string)System.Web.HttpContext.Current.Session["user_riskID"]);

                    if (ds.Tables.Count > 0)
                    {
                       
                        return cartCount = ds.Tables[3].Rows[0][0].ToString();
                    }
                    else
                    {
                        return "0";
                    }
                }
            }
            else
            {
                return "0";
            }
        }
        catch (Exception)
        {
            //throw;
            return "0";
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

            if (!string.IsNullOrEmpty((string)HttpContext.Current.Session["user_MemberID"]))
            {
                //PMessageServices.PMessageClient pmClient = new PMessageServices.PMessageClient();
                //return pmClient.GetUnreadMessageCount(long.Parse((string)HttpContext.Current.Session["user_MemberID"])).ToString();
                using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
                {
                    string strMemberCode = string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["user_MemberCode"] as string) ? string.Empty : Convert.ToString(System.Web.HttpContext.Current.Session["user_MemberCode"]);
                    System.Data.DataSet ds = sClient.getRedemptionDetail(AppSettings.operator_id.ToString(), strMemberCode);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            total = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                            //}
                            //if (ds.Tables[1].Rows.Count > 1)
                            //{
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                claim = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                            }
                            //}
                            //if (ds.Tables[2].Rows.Count > 1)
                            //{
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                cart = int.Parse(ds.Tables[2].Rows[0][0].ToString());
                            }
                            claim = claim + cart;
                        }

                    }
                    current = total - claim;

                    HttpContext.Current.Session["pointsBalance"] = current;
                    //foreach (DataRow dr in ds.Tables[0].Rows)
                    //{
                    //    dr["points"] = current;
                    //}
                    return current;
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
    public static int getPointLevel()
    {
        string pointLevel = "";

        try
        {

            if (!string.IsNullOrEmpty((string)HttpContext.Current.Session["user_MemberID"]))
            {
                //PMessageServices.PMessageClient pmClient = new PMessageServices.PMessageClient();
                //return pmClient.GetUnreadMessageCount(long.Parse((string)HttpContext.Current.Session["user_MemberID"])).ToString();
                using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
                {
                    pointLevel = sClient.getMemberPointLevelFE((string)HttpContext.Current.Session["user_MemberID"]);

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
                                percentage = Convert.ToInt32(Decimal.Divide(pointsEarn,nextPoints) * 100);
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

    [WebMethod()]
    [System.Web.Script.Services.ScriptMethod(
    ResponseFormat = System.Web.Script.Services.ResponseFormat.Json)]
    public static string getSpinWheel()
    {

        string strMemberCode = string.IsNullOrEmpty(System.Web.HttpContext.Current.Session["user_MemberCode"] as string) ? string.Empty : (string)(System.Web.HttpContext.Current.Session["user_MemberCode"]);
       
        int itemCount = 0;
        int totalSpin = 0;
        int play = 0;

        HttpContext.Current.Session["dobPrize"] = false;

        try
        {

            if (!string.IsNullOrEmpty((string)HttpContext.Current.Session["user_MemberID"]))
            {
                //PMessageServices.PMessageClient pmClient = new PMessageServices.PMessageClient();
                //return pmClient.GetUnreadMessageCount(long.Parse((string)HttpContext.Current.Session["user_MemberID"])).ToString();
                
                using (RewardsServices.RewardsServicesClient sClient = new RewardsServices.RewardsServicesClient())
                {
                    System.Data.DataSet ds = sClient.getMemberSpinWheelFE((string)HttpContext.Current.Session["user_MemberID"], (string)HttpContext.Current.Session["user_Currency"], (string)HttpContext.Current.Session["language"]);

                    if (ds.Tables.Count > 0)
                    {
                        foreach (System.Data.DataRow dr in ds.Tables[0].Rows)
                        {
                            System.Data.DataSet dsItem = sClient.getMemberSpinWheelFEItem((string)HttpContext.Current.Session["user_MemberID"], (string)HttpContext.Current.Session["user_Currency"], (string)HttpContext.Current.Session["language"]);

                            if (dsItem.Tables.Count >= 1)
                            {
                                ds.Tables[0].Columns.Add("emptyItem");
                                ds.Tables[0].Columns.Add("remainPlay");
                                ds.Tables[0].Columns.Add("reachLimit");
                                ds.Tables[0].Columns.Add("notEligible");
                               
                                foreach (System.Data.DataRow drItem in dsItem.Tables[0].Rows)
                                {
                                    itemCount = itemCount + 1;

                                    ds.Tables[0].Columns.Add("currencyCode" + itemCount);
                                    ds.Tables[0].Columns.Add("categoryId" + itemCount);
                                    ds.Tables[0].Columns.Add("productId" + itemCount);
                                    ds.Tables[0].Columns.Add("productType" + itemCount);
                                    ds.Tables[0].Columns.Add("amount" + itemCount);
                                    ds.Tables[0].Columns.Add("imageName" + itemCount);
                                    ds.Tables[0].Columns.Add("prizeGroupPercentage" + itemCount);
                                    ds.Tables[0].Columns.Add("prizeName" + itemCount);
                                    
                                    dr["currencyCode" + itemCount] = drItem["currencyCode"];
                                    dr["categoryId" + itemCount] = drItem["categoryId"];
                                    dr["productId" + itemCount] = drItem["productId"];
                                    dr["productType" + itemCount] = drItem["productType"];
                                    dr["amount" + itemCount] = drItem["amount"];
                                    dr["imageName" + itemCount] = drItem["imageName"];
                                    dr["prizeGroupPercentage" + itemCount] = drItem["prizeGroupPercentage"];
                                    dr["prizeName" + itemCount] = drItem["prizeName"];

                                    string group = drItem["prizeGroupPercentage"].ToString();

                                    //percentage
                                    //System.Web.HttpContext.Current.Session["prizeGroupPercentage" + itemCount] = dr["prizeGroupPercentage" + itemCount].ToString();
                                    System.Web.HttpContext.Current.Session["prizeGroupPercentage" + itemCount] = group;

                                    System.Web.HttpContext.Current.Session["pointLevel"] = dr["pointLevel"].ToString();
                                    System.Web.HttpContext.Current.Session["frequencyPlay"] = dr["frequencyPlay"].ToString();
                                    System.Web.HttpContext.Current.Session["frequencyTimeRange"] = dr["frequencyTimeRange"].ToString();
                                    
                                }

                                
                                #region calculate spin remaining
                                totalSpin = int.Parse(dr["frequencyPlay"].ToString());
                                string TimeRange = (string)HttpContext.Current.Session["frequencyTimeRange"];

                                string dateFrom = "";
                                string dateTo = "";

                                var memberDOB = DateTime.Today;
                                string DOB = "";


                                //daily
                                var dayStart = DateTime.Today;
                                var dayEnd = DateTime.Today.AddDays(1).AddSeconds(-1);

                                //weekly
                                DateTime d = DateTime.Today;
                                int offset = d.DayOfWeek - DayOfWeek.Monday;
                                DateTime lastMonday = d.AddDays(-offset);
                                DateTime nextSunday = lastMonday.AddDays(6);

                                //monthly
                                var monthStart = DateTime.Today.AddDays(1 - DateTime.Today.Day);
                                var monthEnd = monthStart.AddMonths(1).AddSeconds(-1);

                                switch (TimeRange)
                                {

                                    case "2":
                                        dateFrom = dayStart.ToString("yyyy-MM-dd HH:mm:ss");
                                        dateTo = dayEnd.ToString("yyyy-MM-dd HH:mm:ss");
                                        break;
                                    case "3":
                                        dateFrom = lastMonday.ToString("yyyy-MM-dd HH:mm:ss");
                                        dateTo = nextSunday.ToString("yyyy-MM-dd HH:mm:ss");
                                        break;
                                    case "4":
                                        dateFrom = monthStart.ToString("yyyy-MM-dd HH:mm:ss");
                                        dateTo = monthEnd.ToString("yyyy-MM-dd HH:mm:ss");
                                        break;

                                }

                                //get DOB
                                System.Data.DataSet dsDOB = sClient.getMemberDOB((string)HttpContext.Current.Session["user_MemberID"]);

                                if (dsDOB.Tables.Count > 0)
                                {
                                    if (dsDOB.Tables[0].Rows.Count > 0)
                                    {
                                        foreach (System.Data.DataRow drDOB in dsDOB.Tables[0].Rows)
                                        {
                                            memberDOB = DateTime.Parse(drDOB["dob"].ToString());

                                        }
                                    }
                                }

                                int dobMonth = memberDOB.Month;
                                int month = System.DateTime.Now.Month;
                                string riskId = (string)HttpContext.Current.Session["user_riskID"];

                                //if (dobMonth == month && (riskId == "VIPB" || riskId == "VIPG" || riskId == "VIPP" || riskId == "VIPD"))
                                if (dobMonth == month)
                                {
                                    //exclude VIPB required by kary
                                    //if (riskId == "VIPB" || riskId == "VIPG" || riskId == "VIPP" || riskId == "VIPD")

                                    //risk category to run expiration
                                    string riskCategoryId = System.Configuration.ConfigurationManager.AppSettings.Get("riskId");

                                    //if (riskId == "VIPG" || riskId == "VIPP" || riskId == "VIPD")
                                    //{
                                    string[] results;
                                    string[] splitChar = { "|" };
                                    results = riskCategoryId.Split(splitChar, StringSplitOptions.None );

                                    foreach (string r in results)
                                    {

                                        string riskList = r;

                                        if (riskList == riskId)
                                        {

                                            //valid for start of the dob month till next year
                                            //var dobStart = memberDOB.AddDays(1 - memberDOB.Day);
                                            //var dobEnd = dobStart.AddMonths(12).AddSeconds(-1);

                                            //int dobMonthStart = dobStart.Month;
                                            //int dobMonthEnd = dobEnd.Month;


                                            //int dobDateStart = dobStart.Day;
                                            //int dobDateEnd = dobEnd.Day;

                                            //int year = System.DateTime.Now.Year;
                                            //int nextYear = System.DateTime.Now.AddYears(1).Year;

                                            //valid for start of the last year dob month till end of current month
                                            var dobStart = memberDOB.AddDays(1 - memberDOB.Day);
                                            var dobEnd = dobStart.AddMonths(13).AddSeconds(-1);

                                            int dobMonthStart = dobStart.Month;
                                            int dobMonthEnd = dobEnd.Month;


                                            int dobDateStart = dobStart.Day;
                                            int dobDateEnd = dobEnd.Day;

                                            int year = System.DateTime.Now.AddYears(-1).Year;
                                            int nextYear = System.DateTime.Now.Year;

                                            //dob spin valid 1 year??
                                            string dateStart = Convert.ToString(year) + "-" + Convert.ToString(dobMonthStart) + "-" + Convert.ToString(dobDateStart) + " 00:00:00";
                                            string dateEnd = Convert.ToString(nextYear) + "-" + Convert.ToString(dobMonthEnd) + "-" + Convert.ToString(dobDateEnd) + " 23:59:59";

                                            int spinCountDOB = sClient.getMemberSpinWheelCountDOB(AppSettings.operator_id.ToString(), strMemberCode, dateStart, dateEnd);

                                            if (spinCountDOB == 0)
                                            {
                                                HttpContext.Current.Session["dobPrize"] = true;
                                                totalSpin = totalSpin + 1;
                                            }
                                            else
                                            {
                                                HttpContext.Current.Session["dobPrize"] = false;
                                            }
                                        }
                                        //else
                                        //{
                                        //    HttpContext.Current.Session["dobPrize"] = false;
                                        //}
                                    }
                                    //else
                                    //{
                                    //    HttpContext.Current.Session["dobPrize"] = false;
                                    //}
                                }
                                else
                                {
                                    HttpContext.Current.Session["dobPrize"] = false;
                                }


                                int spinCount = sClient.getMemberSpinWheelCount(AppSettings.operator_id.ToString(), strMemberCode, dateFrom, dateTo);

                                //System.Web.HttpContext.Current.Session["PlayRemain"] = totalSpin - spinCount;
                                dr["remainPlay"] = totalSpin - spinCount;
                                
                                if (spinCount >= totalSpin)
                                {
                                    dr["reachLimit"] = "true";
                                }
                                else
                                {
                                    dr["reachLimit"] = "false";
                                }

                                if (totalSpin == 0)
                                {
                                    dr["notEligible"] = "true";
                                }
                                else
                                {
                                    dr["notEligible"] = "false";
                                }

                                #endregion

                                //no empty item
                                if (!(itemCount > 8) && !(itemCount < 8))
                                {
                                    dr["emptyItem"] = 0;
                                }
                                else
                                {
                                    dr["emptyItem"] = 1;
                                }
                            }
                            else
                            {
                                //set to zero when not found
                               
                                dr["emptyItem"] = 1;
                            }

                        }

                    }

                    HttpContext.Current.Session["SpinWheelItem"] = ds.Tables[0];
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