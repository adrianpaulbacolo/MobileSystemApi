using System.EnterpriseServices.Internal;

namespace Models
{
    /// <summary>
    /// Summary description for IovationModel
    /// </summary>
    public class IovationModel
    {

        public IovationModel(string username)
        {
            Username = username;
            isSystemError = false;
        }

        public string ResultCode { get; set; }

        public string ResultDetail { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorDetail { get; set; }

        public string ProcessRemark { get; set; }

        public bool isSystemError { get; set; }

        public string Username { get; set; }

        private readonly customConfig.IovationSettings ioSettings = new customConfig.IovationSettings("W88");

        public string CheckTransactionUrl
        {
            get { return ioSettings.Values.Get("CheckTransactionUrl"); }
        }

        public string GetEvidenceUrl
        {
            get { return ioSettings.Values.Get("GetEvidenceUrl"); }
        }

        public string AccountPrefix
        {
            get { return ioSettings.Values.Get("AccountPrefix"); }
        }

        public string SubscriberID
        {
            get { return ioSettings.Values.Get("SubscriberId"); }
        }

        public string SubscriberAccount
        {
            get { return ioSettings.Values.Get("SubscriberAccount"); }
        }

        public string SubscriberPassCode
        {
            get { return ioSettings.Values.Get("SubscriberPassCode"); }
        }

        public string ServiceEnabled
        {
            get { return ioSettings.Values.Get("ServiceEnabled"); }
        }

        public string UserAccountCode
        {
            get { return string.Format("{0}{1}", AccountPrefix, Username); }
        }

        public string Exceptions
        {
            get { return ioSettings.Values.Get("Exceptions"); }
        }

        public string ServiceDays
        {
            get { return ioSettings.Values.Get("ServiceDays"); }
        }
    }
}