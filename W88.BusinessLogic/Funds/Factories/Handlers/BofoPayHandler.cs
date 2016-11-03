using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Funds.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities;
using W88.Utilities.Constant;
using W88.Utilities.Data;
using W88.Utilities.Log.Helpers;
using W88.WebRef.svcPayDeposit;


namespace W88.BusinessLogic.Funds.Factories.Handlers
{
    public class BofoPayHandler : FundsBase
    {
        private UserSessionInfo _userInfo;
        private FundsInfo _fundsInfo;
        private PaymentSettingInfo _setting;

        public BofoPayHandler(UserSessionInfo userInfo, FundsInfo fundInfo, PaymentSettingInfo setting)
            : base(userInfo, fundInfo, setting)
        {
            if (userInfo == null)
                userInfo = new UserSessionInfo();

            this._userInfo = userInfo;

            if (fundInfo == null)
                fundInfo = new FundsInfo();

            this._fundsInfo = fundInfo;

            if (setting == null)
                setting = new PaymentSettingInfo();

            this._setting = setting;
        }

        protected override void LogResult(ProcessCode process, Constants.PaymentTransactionType paymentType)
        {
            process.Remark = string.Format("IsSuccess: {0} | PaymentType: {1} | TransactionId: {2} | Amount: {3}",
                process.IsSuccess, Convert.ToString(paymentType), process.IsSuccess ? process.Data.TransactionId : "", this._fundsInfo.Amount);

            AuditTrail.AppendLog(this._userInfo.MemberCode, Constants.PageNames.FundsPage,
               Constants.TaskNames.CreateBOTransaction, Constants.PageNames.ComponentName, Convert.ToString(process.Code),
                string.Join(" | ", process.Message), string.Empty, string.Empty, process.Remark,
               Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }

        protected override async Task<XElement> CreateDeposit(ProcessCode process)
        {
            using (DepositClient client = new DepositClient())
            {
                return await client.createOnlineDepositTransactionV3Async(OperatorId, this._userInfo.MemberCode, Convert.ToInt64(this._setting.Id), this._setting.MerchantId, this._userInfo.CurrencyCode, this._fundsInfo.Amount, DepositSource.Mobile, this._fundsInfo.Bank.Value, this._fundsInfo.AccountName);
            }
        }

        protected override ProcessCode CreateVendorParameter(ref ProcessCode process)
        {
            string[] infoArray = base.GetPaymentGatewayMerchantSetting(this._setting);

            string md5Key = "", terminalID = "";
            if (infoArray.Count() == 2)
            {
                md5Key = infoArray[0];
                terminalID = infoArray[1];
            }

            string orderMoney = (this._fundsInfo.Amount * 100).ToString();
            string transID = process.Data.TransactionId;
            string memberID = this._setting.MerchantId;

            string keyType = "1"; //1 - MD5
            string tradeDate = DateTime.Now.ToString("yyyyMMddHHmmss");
            string payID = "all"; //Product Features
            string productName = "mobile";
            string amount = "1"; //quantity
            string userName = string.Empty;
            string additionalInfo = string.Empty;

            string noticeType = "1"; //0 - no redirection to pageUrl after successful payment, 1 - redirect to pageUrl after payment
            string pageUrl = this._fundsInfo.ThankYouPage;

            OperatorSettings operatorSettings = new OperatorSettings(Settings.OperatorName);

            string postUrl = operatorSettings.Values.Get("BofoPay_postUrl");
            string returnUrl = operatorSettings.Values.Get("BofoPay_serverreturnurl");
            string interfaceVersion = operatorSettings.Values.Get("Baofo_InterfaceVersion");

            var builder = new StringBuilder();
            builder.AppendFormat("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}", memberID, payID, tradeDate, transID, orderMoney, pageUrl, returnUrl, noticeType, md5Key);

            string signature = Common.GetObject<Md5Hash>().Encrypt(builder.ToString()).ToUpper();

            process.Data = new
             {
                 TransactionId = transID,
                 PostUrl = postUrl,
                 FormData = new
                 {
                     ReturnUrl = returnUrl,
                     InterfaceVersion = interfaceVersion,
                     MemberID = memberID,
                     TerminalID = terminalID,
                     KeyType = keyType,
                     PayID = payID,
                     TradeDate = tradeDate,
                     TransID = transID,
                     OrderMoney = orderMoney,
                     ProductName = productName,
                     Amount = amount,
                     Username = userName,
                     AdditionalInfo = additionalInfo,
                     PageUrl = pageUrl,
                     Signature = signature,
                     NoticeType = noticeType
                 }
             };

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