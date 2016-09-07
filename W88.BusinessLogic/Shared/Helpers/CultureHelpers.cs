using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.XPath;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities;

namespace W88.BusinessLogic.Shared.Helpers
{
    public class CultureHelpers
    {
        public static class AppData
        {
            private static string _i18nFolderPath = "i18n";

            public static XElement GetRootResource(string fileName)
            {
                string languageCode = LanguageHelpers.SelectedLanguage;
                string xmlFilePath = HttpContext.Current.Server.MapPath(@"~/App_Data/" + languageCode + @"/" + fileName + ".xml");

                var xElement = Common.GetAppData<XElement>(xmlFilePath);

                if (xElement != null) return xElement;
                xmlFilePath = HttpContext.Current.Server.MapPath(@"~/App_Data/en-us/" + fileName + ".xml");
                xElement = Common.GetAppData<XElement>(xmlFilePath);

                return xElement;
            }

            public static string GetJsonRootResource(string fileName)
            {
                string jsonFilePath = HttpContext.Current.Server.MapPath(@"~/App_Data/" + fileName + ".json");
                return Common.GetJsonAppData(jsonFilePath);
            }

            public static XElement GetRootResourceNonLanguage(string fileName)
            {
                string xmlFilePath = HttpContext.Current.Server.MapPath(@"~/App_Data/" + fileName + ".xml");
                return Common.GetAppData<XElement>(xmlFilePath);
            }

            /// <summary>
            /// Get Translation from i18n
            /// </summary>
            /// <param name="filePath">File path and file name. ex. errors/errors, payments/depositchannel</param>
            /// <param name="useLanguage">Set true if need to use Language e.g. filePath.en-us</param>
            /// <param name="specificLanguage"></param>
            /// <returns>Translation</returns>
            public static string GetLocale_i18n_Resource(string filePath, bool useLanguage, string specificLanguage = "")
            {
                var jsonFilePath = string.Format("{0}/{1}", _i18nFolderPath, filePath);

                if (useLanguage)
                    jsonFilePath = string.Format("{0}/{1}.{2}", _i18nFolderPath, filePath, string.IsNullOrWhiteSpace(specificLanguage) ? LanguageHelpers.SelectedLanguage : specificLanguage.ToLower());

                return GetJsonRootResource(jsonFilePath);
            }

            public static dynamic Messages
            {
                get
                {
                    string jsonFile = GetLocale_i18n_Resource("contents/messages", true);
                    return Common.DeserializeObject<dynamic>(jsonFile);
                }
            }

        }

        public static class ElementValues
        {
            public static string GetResourceString(string elementName, XElement xElement)
            {
                var elem = xElement.Elements(elementName).FirstOrDefault();
                return elem == null ? string.Empty : Convert.ToString(elem.Value);
            }

            public static string GetResourceXPathString(string elementXPath, XElement xElement)
            {
                var elem = xElement.XPathSelectElement(elementXPath);
                return elem == null ? string.Empty : Convert.ToString(elem.Value);
            }
            public static string GetResourceXPathAttribute(string attributeName, XElement xElement)
            {
                var elem = xElement.Attribute(attributeName);
                return elem == null ? string.Empty : Convert.ToString(elem.Value);
            }

            public static string GetResourceXPathName(string elementXPath, string elementValue, XElement xElement)
            {
                var elem = xElement.XPathSelectElement(elementXPath).Elements().FirstOrDefault(x => x.Value == elementValue);
                return elem == null ? string.Empty : Convert.ToString(elem.Name);
            }

            public static string GetResourceXPathAttribute(string elementXPath, string attributeName, XElement xElement)
            {
                var elem = xElement.XPathSelectElement(elementXPath).Attribute(attributeName);
                return elem == null ? string.Empty : Convert.ToString(elem.Value);
            }

            public static string GetResourceXPathAttribute(string elementName, string attributeName, string attributeValue, XElement xElement)
            {
                var elem = xElement.Elements(elementName).FirstOrDefault(el => string.Equals(el.Attribute(attributeName).Name.ToString(), attributeName, StringComparison.CurrentCultureIgnoreCase) && string.Equals(el.Attribute(attributeName).Value, attributeValue, StringComparison.CurrentCultureIgnoreCase));
                return elem == null ? string.Empty : Convert.ToString(elem.Value);
            }
        }
    }
}
