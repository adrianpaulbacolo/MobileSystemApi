using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Xml.Linq;
using W88.Utilities.Data;

namespace W88.Utilities
{
    public static class Common 
    {
        public static string GetSessionVariable(string key)
        {
            return SessionHandler.GetSessionVariable(key);
        }

        public static void SetSessionVariable(string key, string value)
        {
            SessionHandler.SetSessionVariable(key, value);
        }

        public static T GetAppSetting<T>(string key)
        {
            return new ObjHelper().GetAppSetting<T>(key);
        }
        public static T GetValue<T>(object obj)
        {
            return new ObjHelper().GetValue<T>(obj);
        }

        public static T DeserializeObject<T>(string obj)
        {
            return new ObjHelper().Deserialize<T>(obj);
        }

        public static string SerializeObject(object obj)
        {
            return new ObjHelper().SerializeObject(obj);
        }
        
        public static List<T> ParseJsonString<T>(string json, string arrayName)
        {
            return new ObjHelper().ParseJsonString<T>(json, arrayName);
        }
        public static T GetAppData<T>(string filename) where T : XElement
        {
            return new ObjHelper().GetAppData<T>(filename);
        }

        public static string GetJsonAppData(string filename) 
        {
            return new ObjHelper().GetJsonAppData(filename);
        }

        public static T GetObject<T>() where T : class, new()
        {
            return (T) Convert.ChangeType(new T(), typeof (T));
        }


    }
}
