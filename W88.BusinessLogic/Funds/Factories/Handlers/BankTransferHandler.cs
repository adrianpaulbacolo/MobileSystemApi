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
    public class BankTransferHandler : FundsBase
    {
        private UserSessionInfo _userInfo;
        private BaseFundsInfo _fundsInfo;
        private PaymentSettingInfo _setting;
        private List<LOV> _banks;
        private List<LOV> _systemBanks;
        private List<LOV> _depositChannel;

        public BankTransferHandler(UserSessionInfo userInfo, BaseFundsInfo fundInfo, PaymentSettingInfo setting, List<LOV> banks)
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

            this._banks = banks;

            if (this._setting.Type.Equals(Convert.ToString(Constants.PaymentTransactionType.Deposit), StringComparison.OrdinalIgnoreCase))
            {
                this._systemBanks = new ListOfValuesHelper().GetSystemBankAccounts(_userInfo);

                this._depositChannel = new ListOfValuesHelper().GetDepositChannel();
            }
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
                    else
                    {
                        if (_fundsInfo.Bank.Text.Contains("other") || _fundsInfo.Bank.Value.Equals("other", StringComparison.OrdinalIgnoreCase))
                        {
                            if (string.IsNullOrWhiteSpace(_fundsInfo.BankName))
                            {
                                process.Code = (int)Constants.StatusCode.Error;
                                process.Message.Add(base.GetMessage("Pay_MissingBankName"));
                                process.IsAbort = true;
                            }

                            if (Validation.IsInjection(_fundsInfo.BankName))
                            {
                                process.Code = (int)Constants.StatusCode.Error;
                                process.Message.Add(base.GetMessage("Pay_InvalidBankName"));
                                process.IsAbort = true;
                            }
                        }
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

                if (_fundsInfo.SystemBank == null)
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_MissingSystemAccount"));
                    process.IsAbort = true;
                }
                else
                {
                    if (Validation.IsNumeric(_fundsInfo.SystemBank.Text) || !Validation.IsNumeric(_fundsInfo.SystemBank.Value))
                    {
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Message.Add(base.GetMessage("Pay_MissingSystemAccount"));
                        process.IsAbort = true;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(_fundsInfo.SystemBank.Text) || string.IsNullOrWhiteSpace(_fundsInfo.SystemBank.Value))
                        {
                            process.Code = (int)Constants.StatusCode.Error;
                            process.Message.Add(base.GetMessage("Pay_MissingSystemAccount"));
                            process.IsAbort = true;
                        }

                        if (Validation.IsInjection(_fundsInfo.SystemBank.Text) || Validation.IsInjection(_fundsInfo.SystemBank.Value))
                        {
                            process.Code = (int)Constants.StatusCode.Error;
                            process.Message.Add(base.GetMessage("Pay_MissingSystemAccount"));
                            process.IsAbort = true;
                        }


                        if (!this._systemBanks.Any(b => b.Text == _fundsInfo.SystemBank.Text && b.Value == _fundsInfo.SystemBank.Value))
                        {
                            process.Code = (int)Constants.StatusCode.Error;
                            process.Message.Add(base.GetMessage("Pay_MissingSystemAccount"));
                            process.IsAbort = true;
                        }
                    }
                }

                if (_fundsInfo.DepositChannel == null)
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_MissingDepositChannel"));
                    process.IsAbort = true;
                }
                else
                {
                    if (Validation.IsNumeric(_fundsInfo.DepositChannel.Text) || Validation.IsNumeric(_fundsInfo.DepositChannel.Value))
                    {
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Message.Add(base.GetMessage("Pay_MissingDepositChannel"));
                        process.IsAbort = true;
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(_fundsInfo.DepositChannel.Text) || string.IsNullOrWhiteSpace(_fundsInfo.DepositChannel.Value))
                        {
                            process.Code = (int)Constants.StatusCode.Error;
                            process.Message.Add(base.GetMessage("Pay_MissingDepositChannel"));
                            process.IsAbort = true;
                        }

                        if (Validation.IsInjection(_fundsInfo.DepositChannel.Text) || Validation.IsInjection(_fundsInfo.DepositChannel.Value))
                        {
                            process.Code = (int)Constants.StatusCode.Error;
                            process.Message.Add(base.GetMessage("Pay_MissingDepositChannel"));
                            process.IsAbort = true;
                        }

                        if (!this._depositChannel.Any(b => b.Text == _fundsInfo.DepositChannel.Text && b.Value == _fundsInfo.DepositChannel.Value))
                        {
                            process.Code = (int)Constants.StatusCode.Error;
                            process.Message.Add(base.GetMessage("Pay_MissingDepositChannel"));
                            process.IsAbort = true;
                        }
                    }
                }

            }
            else
            {
                if (string.IsNullOrWhiteSpace(_fundsInfo.BankBranch))
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_MissingBankBranch"));
                    process.IsAbort = true;
                }

                if (Validation.IsInjection(_fundsInfo.BankBranch))
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_InvalidBankBranch"));
                    process.IsAbort = true;
                }

                if (string.IsNullOrWhiteSpace(_fundsInfo.BankAddress))
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_MissingBankAddress"));
                    process.IsAbort = true;
                }

                if (Validation.IsInjection(_fundsInfo.BankAddress))
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_InvalidBankAddress"));
                    process.IsAbort = true;
                }
            }

            return process;
        }

        protected override void LogResult(ProcessCode process, Constants.PaymentTransactionType paymentType)
        {
            if (this._setting.Type.Equals(Convert.ToString(Constants.PaymentTransactionType.Deposit), StringComparison.OrdinalIgnoreCase))
            {
                process.Remark = string.Format("IsSuccess: {0} | PaymentType: {1} | TransactionId: {2} | Amount: {3} | AccountName: {4} | AccountNumber: {5} | ReferenceId: {6} | DepositDateTime: {7} | BankCode: {8} | BankName: {9} | SystemBank: {10}  | DepositChannel: {11}",
                    process.IsSuccess, Convert.ToString(paymentType), process.IsSuccess ? process.Data.TransactionId : "", this._fundsInfo.Amount, this._fundsInfo.AccountName, this._fundsInfo.AccountNumber, this._fundsInfo.ReferenceId,
                        this._fundsInfo.DepositDateTime, this._fundsInfo.Bank.Value, this._fundsInfo.BankName, this._fundsInfo.SystemBank.Value, this._fundsInfo.DepositChannel.Value);

            }
            else
            {
                process.Remark = string.Format("IsSuccess: {0} | PaymentType: {1} | TransactionId: {2} | Amount: {3} | BankCode: {4} | AccountName: {5} | AccountNumber: {6}",
                    process.IsSuccess, Convert.ToString(paymentType), process.IsSuccess ? process.Data.TransactionId : "", this._fundsInfo.Amount, this._fundsInfo.Bank,
                        this._fundsInfo.AccountName, this._fundsInfo.AccountNumber);
            }

            AuditTrail.AppendLog(this._userInfo.MemberCode, Constants.PageNames.FundsPage,
               Constants.TaskNames.CreateBOTransaction, Constants.PageNames.ComponentName, Convert.ToString(process.Code),
                string.Join(" | ", process.Message), string.Empty, string.Empty, process.Remark,
               Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }

        protected override async Task<XElement> CreateDeposit(ProcessCode process)
        {
            using (DepositClient client = new DepositClient())
            {
                return await client.createFastDepositTransactionV1Async(OperatorId, this._userInfo.MemberCode, this._fundsInfo.DepositChannel.Value, Convert.ToInt64(this._setting.Id),
                    this._userInfo.CurrencyCode, this._fundsInfo.Amount, Convert.ToInt64(this._fundsInfo.SystemBank.Value), this._fundsInfo.AccountName, this._fundsInfo.AccountNumber,
                    this._fundsInfo.DepositDateTime, this._fundsInfo.ReferenceId, this._fundsInfo.Bank.Value, _fundsInfo.Bank.Text, _fundsInfo.BankName, Convert.ToString(DepositSource.Mobile));
            }
        }

        protected override async Task<XElement> CreateWithdrawal(ProcessCode process)
        {
            string memberIC = string.Empty;
            string memberMobile = string.Empty;
            bool mobileNotify = false;

            using (WithdrawalClient client = new WithdrawalClient())
            {
                return await client.createBankTransferTransactionV1Async(OperatorId, this._userInfo.MemberCode, Convert.ToInt64(this._setting.Id), this._userInfo.CurrencyCode,
                    this._fundsInfo.Amount, this._fundsInfo.AccountName, this._fundsInfo.AccountNumber, this._fundsInfo.BankAddress, this._fundsInfo.BankBranch, this._fundsInfo.Bank.Value,
                    this._fundsInfo.Bank.Text, this._fundsInfo.BankName, memberIC, memberMobile, mobileNotify, Convert.ToString(WithdrawalSource.Mobile));
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