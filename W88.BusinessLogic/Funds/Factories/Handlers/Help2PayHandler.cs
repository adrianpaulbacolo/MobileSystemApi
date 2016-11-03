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
using W88.Utilities.Extensions;
using W88.Utilities.Geo;
using W88.Utilities.Log.Helpers;
using W88.Utilities.Security;
using W88.WebRef.wsDummy;


namespace W88.BusinessLogic.Funds.Factories.Handlers
{
    public class Help2PayHandler : FundsBase
    {
        private UserSessionInfo _userInfo;
        private FundsInfo _fundsInfo;
        private PaymentSettingInfo _setting;
        private List<LOV> _banks;

        public Help2PayHandler(UserSessionInfo userInfo, FundsInfo fundInfo, PaymentSettingInfo setting)
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

            this._banks = new ListOfValuesHelper().GetBanksList(Convert.ToInt32(this._setting.Id), this._userInfo.CurrencyCode);
        }

        protected override ProcessCode ValidateData(ref ProcessCode process)
        {
            base.ValidateData(ref process);

            if (this._setting.Type.Equals(Convert.ToString(Constants.PaymentTransactionType.Deposit), StringComparison.OrdinalIgnoreCase))
            {
                if (!this._banks.Any(b => b.Text == _fundsInfo.Bank.Text && b.Value == _fundsInfo.Bank.Value))
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_MissingBank"));
                    process.IsAbort = true;
                }
            }

            return process;
        }

        protected override void LogResult(ProcessCode process, Constants.PaymentTransactionType paymentType)
        {
            process.Remark = string.Format("IsSuccess: {0} | PaymentType: {1} | TransactionId: {2} | Amount: {3} | BankText: {4} | BankValue: {5}",
                process.IsSuccess, Convert.ToString(paymentType), process.IsSuccess ? process.Data.TransactionId : "", this._fundsInfo.Amount, this._fundsInfo.Bank.Text, this._fundsInfo.Bank.Value);

            AuditTrail.AppendLog(this._userInfo.MemberCode, Constants.PageNames.FundsPage,
               Constants.TaskNames.CreateBOTransaction, Constants.PageNames.ComponentName, Convert.ToString(process.Code),
                string.Join(" | ", process.Message), string.Empty, string.Empty, process.Remark,
               Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }

        protected override ProcessCode CreateVendorParameter(ref ProcessCode process)
        {
            string[] merchantAccount = base.GetPaymentGatewayMerchantSetting(this._setting);
            string merchantKey = "", merchant = "";
            if (merchantAccount.Count() == 2)
            {
                merchantKey = merchantAccount[0];
                merchant = merchantAccount[1];
            }

            string reference = process.Data.TransactionId;
            string customer = Convert.ToString(this._userInfo.MemberId);
            string currency = this._userInfo.CurrencyCode.Equals("RMB", StringComparison.OrdinalIgnoreCase) ? "CNY" : this._userInfo.CurrencyCode;
            string note = this._fundsInfo.Bank.Value; //use to store bankCode in callback response
            string bank = this._fundsInfo.Bank.Value;
            string language = "en-us";
            string transferMethod = "auto";

            string amount = this._fundsInfo.Amount.ToW88StringFormat();
            if (this._userInfo.CurrencyCode.Equals("VND", StringComparison.OrdinalIgnoreCase) || this._userInfo.CurrencyCode.Equals("IDR", StringComparison.OrdinalIgnoreCase))
            {
                amount = (this._fundsInfo.Amount * 1000m).ToW88StringFormat();
            }

            OperatorSettings operatorSettings = new OperatorSettings(Settings.OperatorName);

            string frontURI = _fundsInfo.ThankYouPage;
            string postUrl = operatorSettings.Values.Get("Help2Pay_posturl");
            string backURI = operatorSettings.Values.Get("Help2Pay_serverreturnurl");

            var builder = new StringBuilder();
            builder.AppendFormat("{0}{1}{2}{3}{4}{5}{6}", merchant, reference, customer, amount, currency, DateTime.Now.ToString("yyyyMMddHHmmss"), merchantKey);

            string key = Common.GetObject<Md5Hash>().Encrypt(builder.ToString()).ToUpper();

            process.Data = new
            {
                TransactionId = reference,
                PostUrl = postUrl,
                FormData = new
                {
                    FrontURI = frontURI,
                    BackURI = backURI,
                    Merchant = merchant,
                    Currency = currency,
                    Customer = customer,
                    Reference = reference,
                    Key = key,
                    Amount = amount,
                    Note = note,
                    Datetime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:sstt"),
                    Language = language,
                    Bank = bank,
                    TransferMethod = transferMethod
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