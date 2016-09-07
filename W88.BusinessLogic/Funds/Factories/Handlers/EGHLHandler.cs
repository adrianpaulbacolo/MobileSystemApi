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
using W88.Utilities.Log.Helpers;
using W88.Utilities.Security;
using W88.WebRef.svcPayDeposit;


namespace W88.BusinessLogic.Funds.Factories.Handlers
{
    public class EGHLHandler : FundsBase
    {
        private UserSessionInfo _userInfo;
        private BaseFundsInfo _fundsInfo;
        private PaymentSettingInfo _setting;
        private List<LOV> _banks;

        public EGHLHandler(UserSessionInfo userInfo, BaseFundsInfo fundInfo, PaymentSettingInfo setting)
            : base(userInfo, fundInfo, setting)
        {
            if (userInfo == null)
                userInfo = new UserSessionInfo();

            this._userInfo = userInfo;

            if (fundInfo == null)
                fundInfo = new BaseFundsInfo();

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
                if (_fundsInfo.Bank == null)
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_MissingBank"));
                    process.IsAbort = true;
                }
                else
                {
                    if (Validation.IsNumeric(_fundsInfo.Bank.Text) || Validation.IsNumeric(_fundsInfo.Bank.Value))
                    {
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Message.Add(base.GetMessage("Pay_MissingBank"));
                        process.IsAbort = true;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(_fundsInfo.Bank.Text) || string.IsNullOrWhiteSpace(_fundsInfo.Bank.Value))
                        {
                            process.Code = (int)Constants.StatusCode.Error;
                            process.Message.Add(base.GetMessage("Pay_MissingBank"));
                            process.IsAbort = true;
                        }

                        if (Validation.IsInjection(_fundsInfo.Bank.Text) || Validation.IsInjection(_fundsInfo.Bank.Value))
                        {
                            process.Code = (int)Constants.StatusCode.Error;
                            process.Message.Add(base.GetMessage("Pay_MissingBank"));
                            process.IsAbort = true;
                        }

                        if (!this._banks.Any(b => b.Text == _fundsInfo.Bank.Text && b.Value == _fundsInfo.Bank.Value))
                        {
                            process.Code = (int)Constants.StatusCode.Error;
                            process.Message.Add(base.GetMessage("Pay_MissingBank"));
                            process.IsAbort = true;
                        }
                    }
                }
            }

            return process;
        }

        protected override void LogResult(ProcessCode process, Constants.PaymentTransactionType paymentType)
        {
            process.Remark = string.Format("IsSuccess: {0} | PaymentType: {1} | TransactionId: {2} | Amount: {3} | BankText: {4} | BankValue: {5}",
                process.IsSuccess, Convert.ToString(paymentType), process.IsSuccess ? process.Data.TransactionId : "", this._fundsInfo.Amount, this._fundsInfo.Bank.Text, this._fundsInfo.Bank.Text);

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
            string transactionId = process.Data.TransactionId;
            string merID = this._setting.MerchantId;
            string amt = this._userInfo.CurrencyCode.Equals("IDR", StringComparison.OrdinalIgnoreCase) ? (this._fundsInfo.Amount * 1000).ToW88StringFormat() : this._fundsInfo.Amount.ToW88StringFormat();
            string respURL = this._fundsInfo.ThankYouPage;
            string c_Email = (this._userInfo.CurrencyCode.Equals("IDR", StringComparison.OrdinalIgnoreCase) ? "GVV" + this._userInfo.MemberCode : Convert.ToString(this._userInfo.MemberId)) + "@qq.com";
            string p_Name = this._setting.Name;

            OperatorSettings operatorSettings = new OperatorSettings(Settings.OperatorName);

            string postUrl = operatorSettings.Values.Get("EGHL_posturl");

            process.Data = new
             {
                 TransactionId = transactionId,
                 PostUrl = postUrl,
                 FormData = new Dictionary<string, string>()
                 {
                     { "respURL", respURL },
                     { "merID", merID },
                     { "e-inv", transactionId },
                     { "amt", amt },
                     { "p_Name", p_Name },
                     { "c_Email", c_Email }
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
    }
}