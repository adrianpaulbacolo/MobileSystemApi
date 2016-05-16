using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml;
using System.Web;
using System.Xml.XPath;
using System.Linq;
using System.Xml.Linq;

namespace commonCulture
{
    internal static class directory
    {
        internal static string directoryPath
        {
            get
            {
                string filePath = string.Empty;
                string fileDirectory = string.Empty;
                string fileName = string.Empty;

                string xmlDirectoryPath = string.Empty;
                string languageCode = string.Empty;

                filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
                fileName = System.IO.Path.GetFileName(filePath);
                fileDirectory = System.Web.HttpContext.Current.Server.MapPath(filePath).Replace(fileName, "");

                languageCode = commonVariables.SelectedLanguage;

                xmlDirectoryPath = fileDirectory + "App_LocalResources\\" + languageCode;

                return xmlDirectoryPath;
            }
        }

        #region localResourceDataset
        internal static void getLocalResource(out System.Data.DataSet dataSet)
        {
            dataSet = null;

            string filePath = string.Empty;
            string fileDirectory = string.Empty;
            string filename = string.Empty;
            string languageCode = string.Empty;
            string xmlFilePath = string.Empty;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            filename = System.IO.Path.GetFileName(filePath);
            fileDirectory = System.Web.HttpContext.Current.Server.MapPath(filePath).Replace(filename, "");

            languageCode = commonVariables.SelectedLanguage;

            xmlFilePath = fileDirectory + @"App_LocalResources\" + languageCode + @"\" + filename + ".xml";

            if (System.IO.File.Exists(xmlFilePath))
            {
                dataSet = new System.Data.DataSet();
                dataSet.ReadXml(xmlFilePath);
            }
            else
            {
                xmlFilePath = fileDirectory + @"App_LocalResources\en-us\" + filename + ".xml";
                dataSet = new System.Data.DataSet();
                dataSet.ReadXml(xmlFilePath);
            }
        }
        internal static void getLocalResource(string languageCode, out System.Data.DataSet dataSet)
        {
            dataSet = null;

            string filePath = string.Empty;
            string fileDirectory = string.Empty;
            string fileName = string.Empty;

            string xmlFilePath = string.Empty;
            string xmlDirectoryPath = string.Empty;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            fileName = System.IO.Path.GetFileName(filePath);
            fileDirectory = System.Web.HttpContext.Current.Server.MapPath(filePath).Replace(fileName, "");

            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlDirectoryPath = fileDirectory + @"App_LocalResources\" + languageCode;

            if (System.IO.Directory.Exists(xmlDirectoryPath)) { xmlFilePath = xmlDirectoryPath + @"\" + fileName + ".xml"; }
            else { xmlFilePath = fileDirectory + @"App_LocalResources\en-us\" + fileName + ".xml"; }

            if (System.IO.File.Exists(xmlFilePath)) { dataSet = new System.Data.DataSet(); dataSet.ReadXml(xmlFilePath); }
        }
        internal static void getLocalResource(string filePath, string languageCode, out System.Data.DataSet dataSet)
        {
            dataSet = null;

            string filename = string.Empty;
            string fileDirectory = string.Empty;

            string xmlFilePath = string.Empty;

            if (string.IsNullOrEmpty(filePath)) { filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath; }

            filename = System.IO.Path.GetFileName(filePath);
            fileDirectory = System.Web.HttpContext.Current.Server.MapPath(filePath).Replace(filename, "");

            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlFilePath = fileDirectory + @"App_LocalResources\" + languageCode + @"\" + filename + ".xml";

            if (System.IO.File.Exists(xmlFilePath))
            {
                dataSet = new System.Data.DataSet();
                dataSet.ReadXml(xmlFilePath);
            }
            else
            {
                xmlFilePath = fileDirectory + @"App_LocalResources\en-us\" + filename + ".xml";
                dataSet = new System.Data.DataSet();
                dataSet.ReadXml(xmlFilePath);
            }
        }
        #endregion

        #region localResourceDatatable
        internal static void getLocalResource(out System.Data.DataTable dataTable)
        {
            dataTable = null;

            string filePath = string.Empty;
            string fileDirectory = string.Empty;
            string filename = string.Empty;
            string languageCode = string.Empty;

            string xmlFilePath = string.Empty;

            System.Data.DataSet dataSet = null;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            filename = System.IO.Path.GetFileName(filePath);
            fileDirectory = System.Web.HttpContext.Current.Server.MapPath(filePath).Replace(filename, "");

            languageCode = commonVariables.SelectedLanguage;

            xmlFilePath = fileDirectory + @"App_LocalResources\" + languageCode + @"\" + filename + ".xml";

            if (System.IO.File.Exists(xmlFilePath))
            {
                using (dataSet = new System.Data.DataSet()) { dataSet.ReadXml(xmlFilePath); dataTable = dataSet.Tables[0]; }
            }
            else
            {
                xmlFilePath = fileDirectory + @"App_LocalResources\en-us\" + filename + ".xml";
                using (dataSet = new System.Data.DataSet()) { dataSet.ReadXml(xmlFilePath); dataTable = dataSet.Tables[0]; }
            }
        }
        internal static void getLocalResource(string languageCode, out System.Data.DataTable dataTable)
        {
            dataTable = null;

            string filePath = string.Empty;
            string fileDirectory = string.Empty;
            string fileName = string.Empty;
            string xmlFilePath = string.Empty;

            System.Data.DataSet dataSet = null;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            fileName = System.IO.Path.GetFileName(filePath);
            fileDirectory = System.Web.HttpContext.Current.Server.MapPath(filePath).Replace(fileName, "");

            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlFilePath = fileDirectory + @"App_LocalResources\" + languageCode + @"\" + fileName + ".xml";

            if (System.IO.File.Exists(xmlFilePath))
            {
                using (dataSet = new System.Data.DataSet()) { dataSet.ReadXml(xmlFilePath); dataTable = dataSet.Tables[0]; }
            }
            else
            {
                xmlFilePath = fileDirectory + @"App_LocalResources\en-us\" + fileName + ".xml";
                using (dataSet = new System.Data.DataSet()) { dataSet.ReadXml(xmlFilePath); dataTable = dataSet.Tables[0]; }
            }
        }
        internal static void getLocalResource(string filePath, string languageCode, out System.Data.DataTable dataTable)
        {
            dataTable = null;

            string fileName = string.Empty;
            string fileDirectory = string.Empty;
            string xmlFilePath = string.Empty;
            System.Data.DataSet dataSet = null;

            if (string.IsNullOrEmpty(filePath)) { filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath; }

            fileName = System.IO.Path.GetFileName(filePath);
            fileDirectory = System.Web.HttpContext.Current.Server.MapPath(filePath).Replace(fileName, "");

            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlFilePath = fileDirectory + @"App_LocalResources\" + languageCode + @"\" + fileName + ".xml";

            if (System.IO.File.Exists(xmlFilePath))
            {
                using (dataSet = new System.Data.DataSet()) { dataSet.ReadXml(xmlFilePath); dataTable = dataSet.Tables[0]; }
            }
            else
            {
                xmlFilePath = fileDirectory + @"App_LocalResources\en-us\" + fileName + ".xml";
                using (dataSet = new System.Data.DataSet()) { dataSet.ReadXml(xmlFilePath); dataTable = dataSet.Tables[0]; }
            }
        }
        #endregion

        #region localResourceXmlDocument
        internal static void getLocalResource(out System.Xml.XmlDocument xmlDocument)
        {
            xmlDocument = null;

            string filePath = string.Empty;
            string fileDirectory = string.Empty;
            string filename = string.Empty;
            string languageCode = string.Empty;
            string xmlFilePath = string.Empty;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            filename = System.IO.Path.GetFileName(filePath);
            fileDirectory = System.Web.HttpContext.Current.Server.MapPath(filePath).Replace(filename, "");

            languageCode = commonVariables.SelectedLanguage;

            xmlFilePath = fileDirectory + @"App_LocalResources\" + languageCode + @"\" + filename + ".xml";

            if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }

            else
            {
                xmlFilePath = fileDirectory + @"App_LocalResources\en-us\" + filename + ".xml";
                if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            }
        }
        internal static void getLocalResource(string languageCode, out System.Xml.XmlDocument xmlDocument)
        {
            xmlDocument = null;

            string filePath = string.Empty;
            string fileDirectory = string.Empty;
            string fileName = string.Empty;

            string xmlFilePath = string.Empty;
            string xmlDirectoryPath = string.Empty;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            fileName = System.IO.Path.GetFileName(filePath);
            fileDirectory = System.Web.HttpContext.Current.Server.MapPath(filePath).Replace(fileName, "");

            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlDirectoryPath = fileDirectory + @"App_LocalResources\" + languageCode;

            if (System.IO.Directory.Exists(xmlDirectoryPath)) { xmlFilePath = xmlDirectoryPath + @"\" + fileName + ".xml"; }
            else { xmlFilePath = fileDirectory + @"App_LocalResources\en-us\" + fileName + ".xml"; }

            if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
        }
        internal static void getLocalResource(string filePath, string languageCode, out System.Xml.XmlDocument xmlDocument)
        {
            xmlDocument = null;

            string filename = string.Empty;
            string fileDirectory = string.Empty;

            string xmlFilePath = string.Empty;

            if (string.IsNullOrEmpty(filePath)) { filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath; }

            filename = System.IO.Path.GetFileName(filePath);
            fileDirectory = System.Web.HttpContext.Current.Server.MapPath(filePath).Replace(filename, "");

            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlFilePath = fileDirectory + @"App_LocalResources\" + languageCode + @"\" + filename + ".xml";

            if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            else
            {
                xmlFilePath = fileDirectory + @"App_LocalResources\en-us\" + filename + ".xml";
                if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            }
        }
        #endregion

        #region rootResourceDataset
        internal static void getRootResource(string fileName, out System.Data.DataSet dataSet)
        {
            dataSet = null;

            string fileDirectory = string.Empty;
            string languageCode = string.Empty;
            string xmlFilePath = string.Empty;

            fileDirectory = directory.appPath;
            languageCode = commonVariables.SelectedLanguage;

            xmlFilePath = fileDirectory + @"App_LocalResources\" + languageCode + @"\" + fileName + ".xml";

            if (System.IO.File.Exists(xmlFilePath)) { dataSet = new System.Data.DataSet(); dataSet.ReadXml(xmlFilePath); }
            else
            {
                xmlFilePath = fileDirectory + @"App_LocalResources\en-us\" + fileName + ".xml";
                dataSet = new System.Data.DataSet(); dataSet.ReadXml(xmlFilePath);
            }
        }
        internal static void getRootResource(string fileName, string languageCode, out System.Data.DataSet dataSet)
        {
            dataSet = null;

            string fileDirectory = string.Empty;
            string xmlFilePath = string.Empty;

            fileDirectory = directory.appPath;

            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlFilePath = fileDirectory + @"App_LocalResources\" + languageCode + @"\" + fileName + ".xml";

            if (System.IO.File.Exists(xmlFilePath)) { dataSet = new System.Data.DataSet(); dataSet.ReadXml(xmlFilePath); }
            else
            {
                xmlFilePath = fileDirectory + @"App_LocalResources\en-us\" + fileName + ".xml";
                dataSet = new System.Data.DataSet(); dataSet.ReadXml(xmlFilePath);
            }
        }
        #endregion

        #region rootResourceDatatable
        internal static void getRootResource(string fileName, out System.Data.DataTable dataTable)
        {
            dataTable = null;

            string fileDirectory = string.Empty;
            string languageCode = string.Empty;
            string xmlFilePath = string.Empty;

            System.Data.DataSet data_set = null;

            fileDirectory = directory.appPath;

            languageCode = commonVariables.SelectedLanguage;

            xmlFilePath = fileDirectory + @"App_LocalResources\" + languageCode + @"\" + fileName + ".xml";

            if (System.IO.File.Exists(xmlFilePath))
            {
                using (data_set = new System.Data.DataSet()) { data_set.ReadXml(xmlFilePath); dataTable = data_set.Tables[0]; }
            }
            else
            {
                xmlFilePath = fileDirectory + @"App_LocalResources\en-us\" + fileName + ".xml";
                using (data_set = new System.Data.DataSet()) { data_set.ReadXml(xmlFilePath); dataTable = data_set.Tables[0]; }
            }
        }
        internal static void getRootResource(string fileName, string languageCode, out System.Data.DataTable dataTable)
        {
            dataTable = null;
            string fileDirectory = string.Empty;
            string xmlFilePath = string.Empty;
            System.Data.DataSet dataSet = null;

            fileDirectory = directory.appPath;

            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlFilePath = fileDirectory + @"App_LocalResources\" + languageCode + @"\" + fileName + ".xml";

            if (System.IO.File.Exists(xmlFilePath))
            {
                using (dataSet = new System.Data.DataSet()) { dataSet.ReadXml(xmlFilePath); dataTable = dataSet.Tables[0]; }
            }
            else
            {
                xmlFilePath = fileDirectory + @"App_LocalResources\en-us\" + fileName + ".xml";
                using (dataSet = new System.Data.DataSet()) { dataSet.ReadXml(xmlFilePath); dataTable = dataSet.Tables[0]; }
            }
        }
        #endregion

        #region rootResourceXmlDocument
        internal static void getRootResource(string fileName, out System.Xml.XmlDocument xmlDocument)
        {
            xmlDocument = null;

            string fileDirectory = string.Empty;
            string languageCode = string.Empty;
            string xmlFilePath = string.Empty;

            fileDirectory = directory.appPath;
            languageCode = commonVariables.SelectedLanguage;
            xmlFilePath = fileDirectory + @"App_LocalResources\" + languageCode + @"\" + fileName + ".xml";

            if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            else
            {
                xmlFilePath = fileDirectory + @"App_LocalResources\en-us\" + fileName + ".xml";
                if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            }
        }
        internal static void getRootResource(string fileName, string languageCode, out System.Xml.XmlDocument xmlDocument)
        {
            xmlDocument = null;
            string fileDirectory = string.Empty;
            string xmlFilePath = string.Empty;

            fileDirectory = directory.appPath;
            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlFilePath = fileDirectory + @"App_LocalResources\" + languageCode + @"\" + fileName + ".xml";

            if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            else
            {
                xmlFilePath = fileDirectory + @"App_LocalResources\en-us\" + fileName + ".xml";
                if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            }
        }
        #endregion

        private static string appPath { get { return System.AppDomain.CurrentDomain.BaseDirectory; } }
    }

    public static class appData
    {
        #region localResourceDatatable
        internal static void getLocalResource(out System.Data.DataTable dataTable)
        {
            dataTable = null;
            string filePath = string.Empty;
            string languageCode = string.Empty;
            string xmlFilePath = string.Empty;
            System.Data.DataSet dataSet = null;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            languageCode = commonVariables.SelectedLanguage;

            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + filePath + ".xml");

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
        internal static void getLocalResource(string languageCode, out System.Data.DataTable dataTable)
        {
            dataTable = null;

            string filePath = string.Empty;
            string xmlFilePath = string.Empty;
            System.Data.DataSet data_set = null;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;

            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + filePath + ".xml");

            if (System.IO.File.Exists(xmlFilePath))
            {
                using (data_set = new System.Data.DataSet()) { data_set.ReadXml(xmlFilePath); dataTable = data_set.Tables[0]; }
            }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + filePath + ".xml");
                using (data_set = new System.Data.DataSet()) { data_set.ReadXml(xmlFilePath); dataTable = data_set.Tables[0]; }
            }
        }
        internal static void getLocalResource(string filePath, string languageCode, out System.Data.DataTable dataTable)
        {
            dataTable = null;
            string xmlFilePath = string.Empty;
            System.Data.DataSet dataSet = null;

            if (string.IsNullOrEmpty(filePath)) { filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath; }
            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + filePath + ".xml");

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
        #endregion

        #region localResourceDataset
        internal static void getLocalResource(out System.Data.DataSet dataSet)
        {
            dataSet = null;
            string filePath = string.Empty;
            string languageCode = string.Empty;
            string xmlFilePath = string.Empty;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            languageCode = commonVariables.SelectedLanguage;
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + filePath + ".xml");

            if (System.IO.File.Exists(xmlFilePath))
            {
                dataSet = new System.Data.DataSet(); dataSet.ReadXml(xmlFilePath);
            }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + filePath + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { dataSet = new System.Data.DataSet(); dataSet.ReadXml(xmlFilePath); }
            }
        }
        internal static void getLocalResource(string languageCode, out System.Data.DataSet dataSet)
        {
            dataSet = null;
            string filePath = string.Empty;
            string xmlFilePath = string.Empty;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;

            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + filePath + ".xml");

            if (System.IO.File.Exists(xmlFilePath)) { dataSet = new System.Data.DataSet(); dataSet.ReadXml(xmlFilePath); }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + filePath + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { dataSet = new System.Data.DataSet(); dataSet.ReadXml(xmlFilePath); }
            }
        }
        internal static void getLocalResource(string filePath, string languageCode, out System.Data.DataSet dataSet)
        {
            dataSet = null;
            string xmlFilePath = string.Empty;

            if (string.IsNullOrEmpty(filePath)) { filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath; }
            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + filePath + ".xml");

            if (System.IO.File.Exists(xmlFilePath)) { dataSet = new System.Data.DataSet(); dataSet.ReadXml(xmlFilePath); }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + filePath + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { dataSet = new System.Data.DataSet(); dataSet.ReadXml(xmlFilePath); }
            }
        }
        #endregion

        #region localResourceXmlDocument
        internal static void getLocalResource(out System.Xml.XmlDocument xmlDocument)
        {
            xmlDocument = null;
            string filePath = string.Empty;
            string languageCode = string.Empty;
            string xmlFilePath = string.Empty;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            languageCode = commonVariables.SelectedLanguage;
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + filePath + ".xml");

            if (System.IO.File.Exists(xmlFilePath))
            {
                xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath);
            }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + filePath + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            }
        }
        internal static void getLocalResource(out System.Xml.Linq.XDocument xDocument)
        {
            xDocument = null;
            string filePath = string.Empty;
            string languageCode = string.Empty;
            string xmlFilePath = string.Empty;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            languageCode = commonVariables.SelectedLanguage;
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + filePath + ".xml");

            if (System.IO.File.Exists(xmlFilePath))
            {
                xDocument = new System.Xml.Linq.XDocument(); xDocument = System.Xml.Linq.XDocument.Load(xmlFilePath);
            }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + filePath + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { xDocument = new System.Xml.Linq.XDocument(); xDocument = System.Xml.Linq.XDocument.Load(xmlFilePath); }
            }
        }
        public static void getLocalResource(out System.Xml.Linq.XElement xElement)
        {
            xElement = null;
            string filePath = string.Empty;
            string languageCode = string.Empty;
            string xmlFilePath = string.Empty;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            languageCode = commonVariables.SelectedLanguage;
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + filePath + ".xml");
            //G:\Work\Projects\integrationServices\App_Data\en-us\Services\services.svc.xml
            if (System.IO.File.Exists(xmlFilePath)) { xElement = System.Xml.Linq.XElement.Load(xmlFilePath); }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + filePath + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { xElement = System.Xml.Linq.XElement.Load(xmlFilePath); }
            }
        }
        internal static void getLocalResource(string languageCode, out System.Xml.Linq.XDocument xDocument)
        {
            xDocument = null;
            string filePath = string.Empty;
            string xmlFilePath = string.Empty;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            languageCode = string.IsNullOrEmpty(languageCode) ? commonVariables.SelectedLanguage : languageCode;
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + filePath + ".xml");

            if (System.IO.File.Exists(xmlFilePath))
            {
                xDocument = new System.Xml.Linq.XDocument(); xDocument = System.Xml.Linq.XDocument.Load(xmlFilePath);
            }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + filePath + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { xDocument = new System.Xml.Linq.XDocument(); xDocument = System.Xml.Linq.XDocument.Load(xmlFilePath); }
            }
        }
        internal static void getLocalResource(string languageCode, out System.Xml.Linq.XElement xElement)
        {
            xElement = null;
            string filePath = string.Empty;
            string xmlFilePath = string.Empty;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            languageCode = string.IsNullOrEmpty(languageCode) ? commonVariables.SelectedLanguage : languageCode;
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + filePath + ".xml");

            if (System.IO.File.Exists(xmlFilePath)) { xElement = System.Xml.Linq.XElement.Load(xmlFilePath); }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + filePath + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { xElement = System.Xml.Linq.XElement.Load(xmlFilePath); }
            }
        }

        internal static void getLocalResource(string languageCode, System.Xml.XmlDocument xmlDocument)
        {
            xmlDocument = null;
            string filepath = string.Empty;
            string xmlFilePath = string.Empty;

            filepath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;

            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + filepath + ".xml");

            if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + filepath + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            }
        }
        internal static void getLocalResource(string filePath, string languageCode, out System.Xml.Linq.XDocument xDocument)
        {
            xDocument = null;
            string xmlFilePath = string.Empty;

            if (string.IsNullOrEmpty(filePath)) { filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath; }
            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + filePath + ".xml");

            if (System.IO.File.Exists(xmlFilePath)) { xDocument = new System.Xml.Linq.XDocument(); xDocument = System.Xml.Linq.XDocument.Load(xmlFilePath); }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + filePath + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { xDocument = new System.Xml.Linq.XDocument(); xDocument = System.Xml.Linq.XDocument.Load(xmlFilePath); }
            }
        }
        internal static void getLocalResource(string filePath, string languageCode, out System.Xml.XmlDocument xmlDocument)
        {
            xmlDocument = null;
            string xmlFilePath = string.Empty;

            if (string.IsNullOrEmpty(filePath)) { filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath; }
            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }

            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + filePath + ".xml");

            if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + filePath + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            }
        }
        #endregion

        #region localResourceJsonDocument
        internal static void getLocalResource(out string jsonDocument)
        {
            jsonDocument = null;
            string filePath = string.Empty;
            string languageCode = string.Empty;
            string jsonFilePath = string.Empty;

            filePath = System.Web.HttpContext.Current.Request.CurrentExecutionFilePath;
            languageCode = commonVariables.SelectedLanguage;
            jsonFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + filePath + ".json");

            if (System.IO.File.Exists(jsonFilePath))
            {
                jsonDocument = System.IO.File.ReadAllText(jsonFilePath);
            }
            else
            {
                jsonFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + filePath + ".json");
                if (System.IO.File.Exists(jsonFilePath)) { jsonDocument = System.IO.File.ReadAllText(jsonFilePath); }
            }
        }

        #endregion

        #region rootResourceDatatable
        internal static void getRootResource(string fileName, out System.Data.DataTable dataTable)
        {
            dataTable = null;
            string xmlFilePath = string.Empty;
            string languageCode = string.Empty;
            System.Data.DataSet dataSet = null;

            languageCode = commonVariables.SelectedLanguage;
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + fileName + ".xml");

            if (System.IO.File.Exists(xmlFilePath))
            {
                using (dataSet = new System.Data.DataSet()) { dataSet.ReadXml(xmlFilePath); dataTable = dataSet.Tables[0]; }
            }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + fileName + ".xml");
                using (dataSet = new System.Data.DataSet()) { dataSet.ReadXml(xmlFilePath); dataTable = dataSet.Tables[0]; }
            }
        }
        internal static void getRootResource(string fileName, string languageCode, out System.Data.DataTable dataTable)
        {
            dataTable = null;
            string xmlFilePath = string.Empty;
            System.Data.DataSet dataSet = null;

            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + fileName + ".xml");

            if (System.IO.File.Exists(xmlFilePath))
            {
                using (dataSet = new System.Data.DataSet()) { dataSet.ReadXml(xmlFilePath); dataTable = dataSet.Tables[0]; }
            }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + fileName + ".xml");
                using (dataSet = new System.Data.DataSet()) { dataSet.ReadXml(xmlFilePath); dataTable = dataSet.Tables[0]; }
            }
        }
        #endregion

        #region rootResourceDataset
        internal static void getRootResource(string fileName, out System.Data.DataSet dataSet)
        {
            dataSet = null;
            string languageCode = string.Empty;
            string xmlFilePath = string.Empty;
            languageCode = commonVariables.SelectedLanguage;
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + fileName + ".xml");
            if (System.IO.File.Exists(xmlFilePath)) { dataSet = new System.Data.DataSet(); dataSet.ReadXml(xmlFilePath); }
            else { dataSet.ReadXml(System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + fileName + ".xml")); }
        }
        internal static void getRootResource(string fileName, string languageCode, out System.Data.DataSet dataSet)
        {
            dataSet = null;
            string xmlFilePath = string.Empty;
            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + fileName + ".xml");
            if (System.IO.File.Exists(xmlFilePath)) { dataSet = new System.Data.DataSet(); dataSet.ReadXml(xmlFilePath); }
            else { dataSet.ReadXml(System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + fileName + ".xml")); }
        }
        #endregion

        #region rootResourceXmlDocument
        internal static void getRootResource(string fileName, out System.Xml.XmlDocument xmlDocument)
        {
            xmlDocument = null;
            string languageCode = string.Empty;
            string xmlFilePath = string.Empty;

            languageCode = commonVariables.SelectedLanguage;
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + fileName + ".xml");
            if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + fileName + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            }
        }
        internal static void getRootResource(string fileName, out System.Xml.Linq.XDocument xDocument)
        {
            xDocument = null;
            string languageCode = string.Empty;
            string xmlFilePath = string.Empty;

            languageCode = commonVariables.SelectedLanguage;
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + fileName + ".xml");
            if (System.IO.File.Exists(xmlFilePath)) { xDocument = System.Xml.Linq.XDocument.Load(xmlFilePath); }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + fileName + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { xDocument = System.Xml.Linq.XDocument.Load(xmlFilePath); }
            }
        }
        public static void getRootResource(string fileName, out System.Xml.Linq.XElement xElement)
        {
            xElement = null;
            string languageCode = string.Empty;
            string xmlFilePath = string.Empty;

            languageCode = commonVariables.SelectedLanguage;
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + fileName + ".xml");
            if (System.IO.File.Exists(xmlFilePath)) { xElement = System.Xml.Linq.XElement.Load(xmlFilePath); }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + fileName + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { xElement = System.Xml.Linq.XElement.Load(xmlFilePath); }
            }
        }


        public static System.Xml.Linq.XElement getRootResource(string fileName)
        {
            System.Xml.Linq.XElement xElement = null;
            string languageCode = string.Empty;
            string xmlFilePath = string.Empty;

            languageCode = commonVariables.SelectedLanguage;
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + fileName + ".xml");
            if (System.IO.File.Exists(xmlFilePath)) { xElement = System.Xml.Linq.XElement.Load(xmlFilePath); }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + fileName + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { xElement = System.Xml.Linq.XElement.Load(xmlFilePath); }
            }

            return xElement;
        }

        internal static void getRootResource(string fileName, string languageCode, out System.Xml.XmlDocument xmlDocument)
        {
            xmlDocument = null;
            string xmlFilePath = string.Empty;

            if (string.IsNullOrEmpty(languageCode)) { languageCode = commonVariables.SelectedLanguage; }
            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + fileName + ".xml");
            if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            else
            {
                xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + fileName + ".xml");
                if (System.IO.File.Exists(xmlFilePath)) { xmlDocument = new System.Xml.XmlDocument(); xmlDocument.Load(xmlFilePath); }
            }
        }

        public static void GetRootResourceNonLanguage(string fileName, out System.Xml.Linq.XElement xElement)
        {
            xElement = null;
            string xmlFilePath = string.Empty;

            xmlFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + fileName + ".xml");
            if (System.IO.File.Exists(xmlFilePath)) { xElement = System.Xml.Linq.XElement.Load(xmlFilePath); }
        }

        #endregion

        #region rootResourceJsonDocument
        internal static void getJsonRootResource(string fileName, out string xJson)
        {
            xJson = string.Empty;
            string languageCode = string.Empty;
            string jsonFilePath = string.Empty;

            languageCode = commonVariables.SelectedLanguage;
            jsonFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + fileName + ".json");
            if (System.IO.File.Exists(jsonFilePath)) { xJson = System.Text.RegularExpressions.Regex.Unescape(System.IO.File.ReadAllText(jsonFilePath, System.Text.Encoding.UTF8)); }
            else
            {
                jsonFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + fileName + ".json");
                if (System.IO.File.Exists(jsonFilePath)) { xJson = System.Text.RegularExpressions.Regex.Unescape(System.IO.File.ReadAllText(jsonFilePath, System.Text.Encoding.UTF8)); }
            }
        }
        #endregion
    }

    public static class ElementValues {
        public static string getResourceString(string elementName, System.Xml.Linq.XElement xElement)
        {
            try { return Convert.ToString(xElement.Elements(elementName).SingleOrDefault().Value); }
            catch (Exception) { return string.Empty; }
        }
        public static string getResourceXPathString(string elementXPath, System.Xml.Linq.XElement xElement)
        {
            try { return Convert.ToString(xElement.XPathSelectElement(elementXPath).Value); }
            catch (Exception ex) { return string.Empty; }
        }
        public static string getResourceXPathName(string elementXPath, string elementValue, System.Xml.Linq.XElement xElement)
        {
            try { return Convert.ToString(xElement.XPathSelectElement(elementXPath).Elements().Where(x => x.Value == elementValue).SingleOrDefault().Name); }
            catch (Exception) { return string.Empty; }
        }
        internal static string getResourceAttribute(string elementName, string attributeName, System.Xml.Linq.XElement xElement)
        {
            try { return Convert.ToString(xElement.Elements(elementName).SingleOrDefault().Attribute(attributeName).Value); }
            catch (Exception) { return string.Empty; }
        }
        public static string GetResourceXPathAttribute(string elementXPath, string attributeName, System.Xml.Linq.XElement xElement)
        {
            try { return Convert.ToString(xElement.XPathSelectElement(elementXPath).Attribute(attributeName).Value); }
            catch (Exception) { return string.Empty; }
        }
        public static string GetResourceXPathAttribute(string elementName, string attributeName, string attributeValue, XElement xElement)
        {
            try
            {
                var list = xElement.Elements(elementName);
                foreach (var el in list.Where(el => String.Equals(el.Attribute(attributeName).Name.ToString(), attributeName, StringComparison.CurrentCultureIgnoreCase) && String.Equals(el.Attribute(attributeName).Value, attributeValue, StringComparison.CurrentCultureIgnoreCase)))
                {
                    return el.Value;
                }

                return string.Empty;
            }
            catch (Exception) { return string.Empty; }
        }
        
    }
}