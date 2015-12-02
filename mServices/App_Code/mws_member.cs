using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.SqlClient;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
// [System.Web.Script.Services.ScriptService]
public class mws_member : System.Web.Services.WebService
{

    public mws_member()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld()
    {
        return "Hello World";
    }

    [WebMethod]
    public System.Data.DataSet GetMemberInfoByMemberCode(long operatorId, string memberCode)
    {
        System.Data.DataSet dsData = null;

        #region create user profile record and retrieve latest from db
        using (System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(mServices.Common.connCore))
        {
            using (SqlCommand cmd = new SqlCommand("sp_core_GetMemberByCode", conn))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@operatorId", operatorId);
                cmd.Parameters.AddWithValue("@memberCode", memberCode);

                try
                {
                    conn.Open();

                    using (System.Data.SqlClient.SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(cmd))
                    {
                        using (dsData = new System.Data.DataSet("dsMember"))
                        {
                            dataAdapter.Fill(dsData, "dtMember");
                        }
                    }
                }
                catch (System.Data.SqlClient.SqlException ex) { }
                catch (System.Data.Common.DbException ex) { }
                catch (System.Exception ex) { }
                finally { conn.Close(); }
            }
        }
        #endregion

        return dsData;
    }
}