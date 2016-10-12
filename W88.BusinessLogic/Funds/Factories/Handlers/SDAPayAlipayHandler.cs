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
    public class SDAPayAlipayHandler : FundsBase
    {
        private UserSessionInfo _userInfo;
        private BaseFundsInfo _fundsInfo;
        private PaymentSettingInfo _setting;
        private List<LOV> _banks;

        public SDAPayAlipayHandler(UserSessionInfo userInfo, BaseFundsInfo fundInfo, PaymentSettingInfo setting)
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

        protected override async Task<ProcessCode> CreateVendorBOTransaction(ProcessCode process)
        {
            using (DepositClient client = new DepositClient())
            {
                var request = new createSDAPayTransactionV1Request()
                {
                    bank = this._fundsInfo.Bank.Value,
                    invId = Convert.ToInt64(process.Data.TransactionId),
                    memberId = Convert.ToInt64(this._userInfo.MemberId),
                    memberName = this._userInfo.MemberName,
                    merchantId = this._setting.MerchantId,
                    payMethodId = Convert.ToInt64(this._setting.Id),
                    requestAmount = this._fundsInfo.Amount
                };

                createSDAPayTransactionV1Response response = await client.createSDAPayTransactionV1Async(request);

                if (response.statusCode == "00")
                {
                    process.IsSuccess = Convert.ToBoolean(CultureHelpers.ElementValues.GetResourceString("result", response.createSDAPayTransactionV1Result));

                    if (process.IsSuccess)
                    {
                        decimal amount = Convert.ToDecimal(CultureHelpers.ElementValues.GetResourceString("amount", response.createSDAPayTransactionV1Result));

                        process.IsSuccess = await client.updateDepositAmountAsync(request.invId, amount);
                    }
                }

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