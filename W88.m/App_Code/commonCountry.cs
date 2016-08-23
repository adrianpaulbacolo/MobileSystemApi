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

    public class HeaderKeys
    {
        public const string HTTP_X_AKAMAI_EDGESCAPE = "HTTP_X_AKAMAI_EDGESCAPE";
        public const string HTTP_CF_IPCOUNTRY = "HTTP_CF_IPCOUNTRY";
        public const string HTTP_GEO_COUNTRY = "HTTP_GEO_COUNTRY";
        public const string TRUE_CLIENT_IP = "TRUE_CLIENT_IP";
        public const string HOST = "HOST";
        public const string COUNTRY_DOMAIN_CN = "country_domain_cn";
        public const string COUNTRY_DOMAIN_VN = "country_domain_vn";
        public const string COUNTRY_DOMAIN_TH = "country_domain_th";
        public const string COUNTRY_DOMAIN_ID = "country_domain_id";
        public const string COUNTRY_DOMAIN_MY = "country_domain_my";
        public const string COUNTRY_DOMAIN_KR = "country_domain_kr";
        public const string COUNTRY_DOMAIN_JP = "country_domain_jp";
        public const string COUNTRY_DOMAIN_KH = "country_domain_kh";
    }

    public static string GetLanguageByCountry(string CountryCode)
    {
        switch (CountryCode.ToLower())
        {
            case "us":
                return "en-us";
            case "id":
                return "id-id";
            case "kh":
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
    public static string GetCountryByLanguage(string lang)
    {
        switch (lang)
        {
            case "en-us":
                return "en";
            case "id-id":
                return "id";
            case "km-kh":
                return "kh";
            case "ko-kr":
                return "kr";
            case "th-th":
                return "th";
            case "vi-vn":
                return "vn";
            case "zh-cn":
                return "cn";
            case "ja-jp":
                return "jp";
            default:
                return "en";
        }
    }

}