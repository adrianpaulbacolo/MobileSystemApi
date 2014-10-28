using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Points : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
     
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
                    System.Data.DataSet ds = sClient.getRedemptionDetail(AppSettings.operator_id.ToString(), strMemberCode);

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