using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Web.Services;
using mServices;

/// <summary>
/// Summary description for mws_affiliate
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class mws_affiliate : System.Web.Services.WebService {

    public mws_affiliate () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public System.Data.DataSet MobileMemberSignin(long operatorId, string memberCode, string password, string siteURL, string loginIP, string deviceId)
    {
        //----------------------------------------------------------------------
        SqlConnection conn = new SqlConnection(Common.connAffiliate());
        try
        {
            SqlCommand cCommand = new SqlCommand("spMobileMemberSignin", conn);
            cCommand.CommandType = CommandType.StoredProcedure;

            cCommand.Parameters.AddWithValue("@operatorId", operatorId);
            cCommand.Parameters.AddWithValue("@memberCode", memberCode);
            cCommand.Parameters.AddWithValue("@password", password);
            cCommand.Parameters.AddWithValue("@siteURL", siteURL);
            cCommand.Parameters.AddWithValue("@loginIP", loginIP);
            cCommand.Parameters.AddWithValue("@deviceId", deviceId);

            SqlDataAdapter cAdapter = new SqlDataAdapter();
            cAdapter.SelectCommand = cCommand;
            DataSet ds = new DataSet("dt");
            conn.Open();
            cAdapter.Fill(ds);
            cAdapter.Dispose();
            return ds;
        }
        catch (Exception ex)
        {
            //Log.LogError("PointsAdjustment", "sp_bo_getAdjustment(" + operatorId.ToString() + "," + memberCode.ToString() + "," + adjustmentCategoryId.ToString() + ")", ex.Message);
            string strProcessRemark = ex.Message + " | spMobileMemberSignin | " + operatorId + "," + "'" + memberCode + "'," + "'" + password + "'," + "'" + siteURL + "'," + "'" + loginIP + "'," + "'" + deviceId + "'";
            int intProcessSerialId = 0;
            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", "mws_affiliate", "ParameterValidation", "DataBaseManager.DLL", "", "", "", "", strProcessRemark, Convert.ToString(intProcessSerialId), "", true);

            return new DataSet();
        }
        finally
        {
            conn.Close();
        }
    }

    [WebMethod]
    public System.Data.DataSet GetCountryList()
    {
        //----------------------------------------------------------------------
        SqlConnection conn = new SqlConnection(Common.connAffiliate());
        try
        {
            SqlCommand cCommand = new SqlCommand("spCountryView", conn);
            cCommand.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter cAdapter = new SqlDataAdapter();
            cAdapter.SelectCommand = cCommand;
            DataSet ds = new DataSet("dt");
            conn.Open();
            cAdapter.Fill(ds);
            cAdapter.Dispose();

            return ds;
        }
        catch (Exception ex)
        {
            //Log.LogError("PointsAdjustment", "sp_bo_getAdjustment(" + operatorId.ToString() + "," + memberCode.ToString() + "," + adjustmentCategoryId.ToString() + ")", ex.Message);
            string strProcessRemark = "spCountryView | " + ex.Message;
            int intProcessSerialId = 0;
            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", "mws_affiliate", "ParameterValidation", "DataBaseManager.DLL", "", "", "", "", strProcessRemark, Convert.ToString(intProcessSerialId), "", true);

            return new DataSet();
        }
        finally
        {
            conn.Close();
        }
    }
    
}
