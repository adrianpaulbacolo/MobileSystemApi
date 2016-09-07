using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace W88.Utilities
{
    internal class ObjHelper
    {
        public T GetAppSetting<T>(string key)
        {
            T returnValue = default(T);
            if (System.Configuration.ConfigurationManager.AppSettings[key] != null)
            {
                returnValue = (T)Convert.ChangeType(System.Configuration.ConfigurationManager.AppSettings.Get(key), typeof(T));
            }

            return returnValue;
        }

        public T GetValue<T>(object obj)
        {
            if (obj == DBNull.Value || obj == null)
            {
                return default(T);
            }

            return (T)Convert.ChangeType(obj, typeof(T));
        }

        public T Deserialize<T>(string context)
        {
            var jsonData = context;
            var obj = JsonConvert.DeserializeObject<T>(jsonData);
            return obj;
        }

        public string SerializeObject(object context)
        {
            return JsonConvert.SerializeObject(context);
        }

        public List<T> ParseJsonString<T>(string json, string arrayName)
        {
            var jsonObject = JObject.Parse(json);
            var jArray = (JArray)jsonObject.GetValue(arrayName, StringComparison.OrdinalIgnoreCase);
            return jArray == null ? new List<T>() : jArray.ToObject<List<T>>();
        }

        public T GetAppData<T>(string filename) where T : XElement
        {
            if (!File.Exists(filename)) return default(T);
            return (T)Convert.ChangeType(XElement.Load(filename), typeof(T));
        }

        public string GetJsonAppData(string filename)
        {
            return !File.Exists(filename) ? string.Empty : File.ReadAllText(filename, Encoding.UTF8);
        }
    }
}
