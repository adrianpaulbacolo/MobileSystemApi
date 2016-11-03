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
    public class NextPayHandler : FundsBase
    {
        private UserSessionInfo _userInfo;
        private FundsInfo _fundsInfo;
        private PaymentSettingInfo _setting;
        private List<LOV> _banks;

        public NextPayHandler(UserSessionInfo userInfo, FundsInfo fundInfo, PaymentSettingInfo setting)
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

            string merchantID = merchantAccount[0];
            string inv = process.Data.TransactionId;
            string cID = Convert.ToString(this._userInfo.MemberId);
            string amt = this._fundsInfo.Amount.ToW88StringFormat();
            string bm = this._fundsInfo.Bank.Value;

            OperatorSettings operatorSettings = new OperatorSettings(Settings.OperatorName);
            string postUrl = operatorSettings.Values.Get("NextPay_posturl");
            string returnURL = operatorSettings.Values.Get("NextPay_callbackurl");

            process.Data = new
            {
                TransactionId = inv,
                PostUrl = postUrl,
                FormData = new
                {
                    returnURL = returnURL,
                    merchantID = merchantID,
                    inv = inv,
                    amt = amt,
                    bm = bm,
                    cID = cID,
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