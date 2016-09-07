using System.Collections.Specialized;
using System.Configuration;

namespace W88.BusinessLogic.Shared.Helpers
{
    internal class OperatorSettings
    {
        private NameValueCollection nvcSettings = null;
        public OperatorSettings(string operatorCode)
        {
            nvcSettings = ConfigurationManager.GetSection("OperatorGroupSettings/" + operatorCode) as NameValueCollection;
        }
        public NameValueCollection Values
        {
            get { return nvcSettings; }
        }
    }

    internal class IovationSettings
    {
        private NameValueCollection nvcSettings = null;
        public IovationSettings(string operatorCode)
        {
            nvcSettings = ConfigurationManager.GetSection("OperatorGroupSettings/" + operatorCode + "_Iovation") as NameValueCollection;
        }

        public NameValueCollection Values
        {
            get { return nvcSettings; }
        }
    }
}