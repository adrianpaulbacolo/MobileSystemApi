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
using W88.Utilities.Security;
using W88.WebRef.svcPayDeposit;
using W88.WebRef.svcPayWithdrawal;


namespace W88.BusinessLogic.Funds.Factories.Handlers
{
    public class WingMoneyHandler : FundsBase
    {
        private UserSessionInfo _userInfo;
        private BaseFundsInfo _fundsInfo;
        private PaymentSettingInfo _setting;

        public WingMoneyHandler(UserSessionInfo userInfo, BaseFundsInfo fundInfo, PaymentSettingInfo setting)
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

            if (Validation.IsInjection(_fundsInfo.AccountName))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_InvalidAccountName"));
                process.IsAbort = true;
            }

            if (string.IsNullOrWhiteSpace(this._fundsInfo.AccountNumber))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_MissingAccountNumber"));
                process.IsAbort = true;
            }

            if (Validation.IsInjection(_fundsInfo.AccountNumber))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_InvalidAccountNumber"));
                process.IsAbort = true;
            }

            if (this._setting.Type.Equals(Convert.ToString(Constants.PaymentTransactionType.Deposit), StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(this._fundsInfo.ReferenceId))
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_MissingReferenceId"));
                    process.IsAbort = true;
                }

                if (Validation.IsInjection(_fundsInfo.ReferenceId))
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_InvalidReferenceId"));
                    process.IsAbort = true;
                }

                if (this._userInfo.CurrencyCode.Equals("krw", StringComparison.OrdinalIgnoreCase))
                {
                    this._fundsInfo.DepositDateTime = DateTime.Now;
                }

                if ((this._fundsInfo.DepositDateTime - DateTime.Now).TotalHours > 72 || (this._fundsInfo.DepositDateTime - DateTime.Now).TotalHours < -72)
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_InvalidDateTime"));
                    process.IsAbort = true;
                }
            }

            return process;
        }

        protected override void LogResult(ProcessCode process, Constants.PaymentTransactionType paymentType)
        {
            process.Remark = string.Format("IsSuccess: {0} | PaymentType: {1} | TransactionId: {2} | Amount: {3} | AccountName: {4} | AccountNumber: {5} | ReferenceId: {6} | DepositDateTime: {7}",
                process.IsSuccess, Convert.ToString(paymentType), process.IsSuccess ? process.Data.TransactionId : "", this._fundsInfo.Amount, this._fundsInfo.AccountName, this._fundsInfo.AccountNumber,
                    this._fundsInfo.ReferenceId, this._fundsInfo.DepositDateTime);

            AuditTrail.AppendLog(this._userInfo.MemberCode, Constants.PageNames.FundsPage,
               Constants.TaskNames.CreateBOTransaction, Constants.PageNames.ComponentName, Convert.ToString(process.Code),
                string.Join(" | ", process.Message), string.Empty, string.Empty, process.Remark,
               Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }

        protected override async Task<XElement> CreateDeposit(ProcessCode process)
        {
            using (DepositClient client = new DepositClient())
            {
                return await client.createWingDepositTransactionV1Async(OperatorId, this._userInfo.MemberCode, Convert.ToInt64(this._setting.Id), this._userInfo.CurrencyCode,
                    this._fundsInfo.Amount, this._fundsInfo.AccountName, this._fundsInfo.AccountNumber, this._fundsInfo.DepositDateTime, this._fundsInfo.ReferenceId, Convert.ToString(DepositSource.Mobile));
            }
        }

        protected override async Task<XElement> CreateWithdrawal(ProcessCode process)
        {
            string memberMobile = string.Empty;

            using (WithdrawalClient client = new WithdrawalClient())
            {
                return await client.createWingMoneyTransactionV1Async(OperatorId, this._userInfo.MemberCode, Convert.ToInt64(this._setting.Id), this._userInfo.CurrencyCode,
                    this._fundsInfo.Amount, this._fundsInfo.AccountName, this._fundsInfo.AccountNumber, memberMobile, Convert.ToString(WithdrawalSource.Mobile));
            }
        }

        protected override bool IsBOTranction
        {
            get { return true; }
        }

        protected override bool IsVendorTransaction
        {
            get { return false; }
        }
    }
}