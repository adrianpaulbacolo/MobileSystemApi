using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for commonCountry
/// </summary>
public static class commonCountry
{

    internal static void getLocalResource(out System.Data.DataTable dataTable)
    {
        dataTable = null;
        string filePath = string.Empty;
        string xmlFilePath = string.Empty;
        System.Data.DataSet dataSet = null;

        filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;

        xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/operator/" + commonVariables.OperatorCode + @"/CountryRoutes.xml");

        if (System.IO.File.Exists(xmlFilePath))
        {
            using (dataSet = new System.Data.DataSet()) { dataSet.ReadXml(xmlFilePath); dataTable = dataSet.Tables[0]; }
        }
        else
        {
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + filePath + ".xml");
            using (dataSet = new System.Data.DataSet()) { dataSet.ReadXml(xmlFilePath); dataTable = dataSet.Tables[0]; }
        }
    }
    public static string getISportURL()
    {
        string ISportURL = "";
        DataTable dt = null;
        getLocalResource(out dt);

        var results =
        from DataRow myRow in dt.Rows
        where myRow.Field<string>("WebDomain").Contains(commonIp.DomainName)
        select myRow.Field<string>("ISportURL");
        if (!results.Any())
        {
            var resultsDefault =
            from DataRow myRow in dt.Rows
            where myRow.Field<string>("Country") == "US"
            select myRow.Field<string>("ISportURL");

            ISportURL = resultsDefault.ElementAt(0);
        }
        else
        {
            ISportURL = results.ElementAt(0);
        }
        return ISportURL;
    }
}