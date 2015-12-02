using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace customConfig
{
    public class DBVariables : System.Configuration.ConfigurationSection
    {
        [System.Configuration.ConfigurationProperty("serverIp")]
        public string serverIp { get { return Convert.ToString(base["serverIp"]); } }
        [System.Configuration.ConfigurationProperty("Username")]
        public string Username { get { return Convert.ToString(base["Username"]); } }
        [System.Configuration.ConfigurationProperty("Password")]
        public string Password { get { return Convert.ToString(base["Password"]); } }
        [System.Configuration.ConfigurationProperty("dbSchema")]
        public string dbSchema { get { return Convert.ToString(base["dbSchema"]); } }
        [System.Configuration.ConfigurationProperty("Timeout")]
        public string Timeout { get { return Convert.ToString(base["Timeout"]); } }
        [System.Configuration.ConfigurationProperty("Encrypted")]
        public string Encrypted { get { return Convert.ToString(base["Encrypted"]); } }
    }

    public class OperatorSettings
    {
        private System.Collections.Specialized.NameValueCollection nvcSettings = null;
        public OperatorSettings(string operatorCode) { nvcSettings = System.Configuration.ConfigurationManager.GetSection("OperatorGroupSettings/" + operatorCode) as System.Collections.Specialized.NameValueCollection; }
        public System.Collections.Specialized.NameValueCollection Values { get { return nvcSettings; } }
    }

    public class IovationSettings
    {
        private System.Collections.Specialized.NameValueCollection nvcSettings = null;
        public IovationSettings(string operatorCode) { nvcSettings = System.Configuration.ConfigurationManager.GetSection("OperatorGroupSettings/" + operatorCode + "_Iovation") as System.Collections.Specialized.NameValueCollection; }
        public System.Collections.Specialized.NameValueCollection Values { get { return nvcSettings; } }
    }


    /*public class SMSSettings
    {
        private System.Collections.Specialized.NameValueCollection nvcSMS = null;
        public SMSSettings(string opCode)
        {
            nvcSMS = System.Configuration.ConfigurationManager.GetSection("SMSGroupSettings/" + opCode) as System.Collections.Specialized.NameValueCollection;
        }
        public System.Collections.Specialized.NameValueCollection Values { get { return nvcSMS; } }
    }*/

    public class WebClientWithTimeOut : System.Net.WebClient
    {
        public int Timeout { get; set; }
        public WebClientWithTimeOut()
        {
            this.Timeout = 10000;
        }
        public WebClientWithTimeOut(int timeout)
        {
            this.Timeout = timeout;
        }
        protected override System.Net.WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            if (request != null)
            {
                request.Timeout = this.Timeout;
            }
            return request;
        }
    }

    public class Configs
    {
        public static System.Configuration.ConfigurationSectionGroup GetSettings(string sectionGroupName)
        {
            if (HttpContext.Current == null)
                return System.Configuration.ConfigurationManager.OpenExeConfiguration(null).GetSectionGroup(sectionGroupName); // whatever you are doing currently
            else
                return System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath).GetSectionGroup(sectionGroupName); //this should do the trick
        }
        /*public static System.Configuration.ConfigurationSection get(string sectionName)
        {
            if (HttpContext.Current == null)
                return System.Configuration.ConfigurationManager.OpenExeConfiguration(null).GetSection(sectionName); // whatever you are doing currently
            else
                return System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath).GetSection(sectionName); //this should do the trick
        }*/
    }

    public class WalletVariables : System.Configuration.ConfigurationSection
    {
        [System.Configuration.ConfigurationProperty("walletId")]
        public string walletId { get { return Convert.ToString(base["walletId"]); } }
        [System.Configuration.ConfigurationProperty("orderBy")]
        public string orderBy { get { return Convert.ToString(base["orderBy"]); } }
    }
}

