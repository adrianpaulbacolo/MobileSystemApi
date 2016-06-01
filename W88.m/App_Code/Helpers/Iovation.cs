using System;
using System.Web;
using Models;

namespace Helpers
{
    /// <summary>
    /// Summary description for Iovation
    /// </summary>
    public class Iovation
    {

        public void IovationSubmit(ref int intProcessSerialId, string strProcessId, string strIPAddress, LoginInfo info)
        {
            var iovationObj = new IovationModel(info.Username);

            if (string.Compare(iovationObj.ServiceEnabled, "true", true) != 0)
            {
                return;
            }

            iovationObj.ProcessRemark =
                string.Format(
                    "CheckTransactionURL: {0} | GetEvidenceURL: {1} | AccountPrefix: {2} | SubscriberID: {3} | SubscriberAccount: {4} | SubscriberPassCode: {5} | UserAccountCode : {6}",
                    iovationObj.CheckTransactionUrl, iovationObj.GetEvidenceUrl, iovationObj.AccountPrefix,
                    iovationObj.SubscriberID, iovationObj.SubscriberAccount, iovationObj.SubscriberPassCode,
                    iovationObj.UserAccountCode);

            try
            {
                using (var ioInstance = new CheckTransactionDetailsService(iovationObj.CheckTransactionUrl))
                {
                    var ioRequest = new CheckTransactionDetails
                    {
                        accountcode = iovationObj.UserAccountCode,
                        enduserip = strIPAddress,
                        beginblackbox = info.ioBlackBox,
                        subscriberid = iovationObj.SubscriberID,
                        subscriberpasscode = iovationObj.SubscriberPassCode,
                        subscriberaccount = iovationObj.SubscriberAccount,
                        type = info.PageName
                    };

                    var ioResponse = new CheckTransactionDetailsResponse();

                    ioResponse = ioInstance.CheckTransactionDetails(ioRequest);

                    #region setIovationCookie
                    var cookie = new HttpCookie(info.Username);
                    cookie.Value = ioResponse.result;
                    if (!string.IsNullOrEmpty(commonIp.DomainName)) { cookie.Domain = commonIp.DomainName; }
                    cookie.Expires = System.DateTime.Now.AddDays(Convert.ToInt32(iovationObj.ServiceDays));
                    HttpContext.Current.Response.Cookies.Add(cookie);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                iovationObj.ResultCode = "31";
                iovationObj.ResultDetail = "Error:Iovation";
                iovationObj.ErrorCode = Convert.ToString(ex.HResult);
                iovationObj.ErrorDetail = ex.Message;

                iovationObj.isSystemError = true;
            }


            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", info.PageName, "Iovation", "DataBaseManager.DLL", iovationObj.ResultCode, iovationObj.ResultDetail, iovationObj.ErrorCode, iovationObj.ErrorDetail, iovationObj.ProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, iovationObj.isSystemError);
        }
    }
}