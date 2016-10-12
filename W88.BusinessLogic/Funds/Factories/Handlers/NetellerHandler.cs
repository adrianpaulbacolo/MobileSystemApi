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


namespace W88.BusinessLogic.Funds.Factories.Handlers
{
    public class NetellerHandler : FundsBase
    {
        private UserSessionInfo _userInfo;
        private BaseFundsInfo _fundsInfo;
        private PaymentSettingInfo _setting;

        public NetellerHandler(UserSessionInfo userInfo, BaseFundsInfo fundInfo, PaymentSettingInfo setting)
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

            if (Validation.IsNumeric(_fundsInfo.AccountName))
            {
                if (Convert.ToInt64(this._fundsInfo.AccountName) == 0)
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_MissingAccountName"));
                    process.IsAbort = true;
                }
            }
            else
            {
                if (string.IsNullOrWhiteSpace(this._fundsInfo.AccountName))
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_MissingAccountName"));
                    process.IsAbort = true;
                }
                else
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_InvalidAccountName"));
                    process.IsAbort = true;
                }
            }

            if (Validation.IsInjection(_fundsInfo.AccountName))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_InvalidAccountName"));
                process.IsAbort = true;
            }

            if (this._setting.Type.Equals(Convert.ToString(Constants.PaymentTransactionType.Deposit), StringComparison.OrdinalIgnoreCase))
            {
                if (Validation.IsNumeric(_fundsInfo.AccountNumber))
                {
                    if (Convert.ToInt32(this._fundsInfo.AccountNumber) == 0)
                    {
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Message.Add(base.GetMessage("Pay_MissingAccountNumber"));
                        process.IsAbort = true;
                    }
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(this._fundsInfo.AccountNumber))
                    {
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Message.Add(base.GetMessage("Pay_MissingAccountNumber"));
                        process.IsAbort = true;
                    }
                    else
                    {
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Message.Add(base.GetMessage("Pay_InvalidAccountNumber"));
                        process.IsAbort = true;
                    }
                }

                if (Validation.IsInjection(_fundsInfo.AccountNumber))
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

        protected override async Task<ProcessCode> CreateVendorBOTransaction(ProcessCode process)
        {
            using (DepositClient client = new DepositClient())
            {
                process.IsSuccess = await client.createNetellerTransactionAsync(Convert.ToInt64(process.Data.TransactionId), OperatorId, this._fundsInfo.Amount, this._userInfo.CurrencyCode, Convert.ToInt64(this._userInfo.MemberId), this._userInfo.MemberCode.ToLower(), Convert.ToInt64(this._fundsInfo.AccountName), Convert.ToInt32(this._fundsInfo.AccountNumber));

                if (!process.IsSuccess)
                {
                    process.Message = base.GetMessage("Pay_Fail");
                    process.Code = (int)Constants.StatusCode.Error;
                }
            }

            process.ProcessSerialId += 1;
            process.Remark = string.Format("IsSuccess: {0} | Response: {1}", process.IsSuccess, process.Data);

            AuditTrail.AppendLog(this._userInfo.MemberCode, Constants.PageNames.FundsPage,
               Constants.TaskNames.CreateVendorBOTransaction, Constants.PageNames.ComponentName, Convert.ToString(process.Code),
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
            get { return false; }
        }

        protected override bool IsVendorRedirection
        {
            get { return false; }
        }
    }
}