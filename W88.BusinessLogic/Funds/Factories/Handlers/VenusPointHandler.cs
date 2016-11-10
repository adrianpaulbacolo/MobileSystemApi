using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Funds.Helpers;
using W88.BusinessLogic.Funds.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities;
using W88.Utilities.Constant;
using W88.Utilities.Data;
using W88.Utilities.Extensions;
using W88.Utilities.Log.Helpers;
using W88.Utilities.Security;
using W88.WebRef.svcPayDeposit;


namespace W88.BusinessLogic.Funds.Factories.Handlers
{
    public class VenusPointHandler : FundsBase
    {
        private UserSessionInfo _userInfo;
        private FundsInfo _fundsInfo;
        private PaymentSettingInfo _setting;

        private decimal _convertedAmount;
        private decimal _exRate;

        public VenusPointHandler(UserSessionInfo userInfo, FundsInfo fundInfo, PaymentSettingInfo setting)
            : base(userInfo, fundInfo, setting)
        {
            if (userInfo == null)
                userInfo = new UserSessionInfo();

            this._userInfo = userInfo;

            if (fundInfo == null)
                fundInfo = new FundsInfo();

            this._fundsInfo = fundInfo;

            var response = new Payments().GetExchangeRate(this._fundsInfo.Amount, "JPY", "USD");

            this._convertedAmount = response.Data.Amount;
            this._exRate = response.Data.ExchangeRate;

            if (setting == null)
                setting = new PaymentSettingInfo();

            this._setting = setting;

            string[] merchantAccount = base.GetPaymentGatewayMerchantSetting(this._setting);

            this._setting.MerchantId = merchantAccount[0];
        }

        protected override ProcessCode ValidateData(ref ProcessCode process)
        {
            base.ValidateData(ref process);

            if (string.IsNullOrWhiteSpace(this._fundsInfo.AccountName))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_MissingAccountName"));
                process.IsAbort = true;
            }
            else if (Validation.IsInjection(_fundsInfo.AccountName))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_InvalidAccountName"));
                process.IsAbort = true;
            }

            if (this._setting.Type.Equals(Convert.ToString(Constants.PaymentTransactionType.Deposit), StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(this._fundsInfo.AccountNumber))
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_MissingAccountNumber"));
                    process.IsAbort = true;
                }
                else if (Validation.IsInjection(_fundsInfo.AccountNumber))
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_InvalidAccountNumber"));
                    process.IsAbort = true;
                }
            }

            return process;
        }

        protected override void LogResult(ProcessCode process, Constants.PaymentTransactionType paymentType)
        {
            process.Remark = string.Format("IsSuccess: {0} | PaymentType: {1} | TransactionId: {2} | Amount: {3} | AccountName: {4} | AccountNumber: {5}",
                process.IsSuccess, Convert.ToString(paymentType), process.IsSuccess ? process.Data.TransactionId : "", this._fundsInfo.Amount, this._fundsInfo.AccountName, this._fundsInfo.AccountNumber);

            AuditTrail.AppendLog(this._userInfo.MemberCode, Constants.PageNames.FundsPage,
               Constants.TaskNames.CreateBOTransaction, Constants.PageNames.ComponentName, Convert.ToString(process.Code),
                string.Join(" | ", process.Message), string.Empty, string.Empty, process.Remark,
               Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }

        protected override async Task<XElement> CreateDeposit(ProcessCode process)
        {
            using (DepositClient client = new DepositClient())
            {
                return await client.createOnlineDepositTransactionV4Async(OperatorId, this._userInfo.MemberCode, Convert.ToInt64(this._setting.Id), this._setting.MerchantId, this._userInfo.CurrencyCode, this._fundsInfo.Amount,
                    "USD", this._convertedAmount, this._exRate, DepositSource.Mobile, string.Empty, this._fundsInfo.AccountName);
            }
        }

        protected override ProcessCode CreateVendorParameter(ref ProcessCode process)
        {
            var transactionId = process.Data.TransactionId;

            string password = HttpUtility.UrlEncode(Common.GetObject<Md5Hash>().Encrypt(_fundsInfo.AccountNumber));

            var operatorSettings = new OperatorSettings(Settings.OperatorName);

            string postUrl = operatorSettings.Values.Get("VenusPoint_posturl");
            string returnUrl = operatorSettings.Values.Get("VenusPoint_returnurl");

            byte[] responseBytes;
            using (var client = new WebClient())
            {
                responseBytes = client.UploadValues(postUrl, new NameValueCollection()
                {
                    {"mid", _setting.MerchantId},
                    {"muid", _userInfo.MemberId.ToString()},
                    {"uid", _fundsInfo.AccountName},
                    {"password", password},
                    {"amount",  _userInfo.CurrencyCode.Equals("JPY", StringComparison.OrdinalIgnoreCase) ? this._convertedAmount.ToW88StringFormat() : _fundsInfo.Amount.ToW88StringFormat()},
                    {"tid", transactionId},
                });
            }

            string responseStr = Encoding.UTF8.GetString(responseBytes);

            byte[] callBackResponse;

            using (var wClient = new WebClient())
            {
                callBackResponse = wClient.UploadValues(returnUrl, "POST", new NameValueCollection()
                {
                    {"responseXml", HttpUtility.HtmlEncode(responseStr)}
                });
            }

            string result = Encoding.Default.GetString(callBackResponse);

            if (result.Equals("OK", StringComparison.OrdinalIgnoreCase))
            {
                process.Message = base.GetMessage("Pay_Success");
                process.Data = new { TransactionId = transactionId };
                process.Code = (int)Constants.StatusCode.Success;
            }
            else
            {
                process.Message = base.GetMessage("Pay_Fail");
                process.Data = null;
                process.Code = (int)Constants.StatusCode.Error;
            }

            process.ProcessSerialId += 1;
            process.Remark = string.Format("VendorParams : {0}", process.Data);

            AuditTrail.AppendLog(_userInfo.MemberCode, Constants.PageNames.FundsPage,
                Constants.TaskNames.CreateVendorParameter, Constants.PageNames.ComponentName, Convert.ToString(process.Code),
                 string.Join(" | ", process.Message), string.Empty, string.Empty, process.Remark,
                Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);

            return process;
        }

        protected override bool IsBOTranction
        {
            get { return true; }
        }

        protected override bool IsVendorTransaction
        {
            get { return true; }
        }

        protected override bool IsVendorRedirection
        {
            get { return false; }
        }
    }
}