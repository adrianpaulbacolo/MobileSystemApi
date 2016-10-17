using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Funds.Helpers;
using W88.BusinessLogic.Funds.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities.Log.Helpers;
using W88.Utilities.Security;
using W88.Utilities.Extensions;
using W88.WebRef.svcPayDeposit;
using W88.Utilities.Constant;
using W88.WebRef.svcPayWithdrawal;
using W88.Utilities;
using System.Collections;
using System.Web;

namespace W88.BusinessLogic.Funds.Factories
{
    public abstract class FundsBase : BaseHelper
    {
        /// <summary>
        /// True: It will create a back office transaction
        /// False: It will create a dummy url from back office 
        /// </summary>
        protected abstract bool IsBOTranction { get; }

        /// <summary>
        /// True: It will call vendor to create the transaction
        /// </summary>
        protected abstract bool IsVendorTransaction { get; }

        /// <summary>
        /// True: It will create the vendor url and create the transaction from that
        /// </summary>
        protected abstract bool IsVendorRedirection { get; }


        private BaseFundsInfo _fundsInfo;
        private UserSessionInfo _userInfo;
        private PaymentSettingInfo _setting;

        public FundsBase(UserSessionInfo userInfo, BaseFundsInfo fundInfo, PaymentSettingInfo setting)
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

        protected virtual ProcessCode ValidateData(ref ProcessCode process)
        {
            if (_fundsInfo.Amount <= 0)
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_MissingAmount"));
                process.IsAbort = true;
            }

            if (Validation.IsInjection(_fundsInfo.Amount.ToW88StringFormat()))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_MissingAmount"));
                process.IsAbort = true;
            }

            if (_fundsInfo.Amount < this._setting.MinAmount)
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_AmountMinLimit"));
                process.IsAbort = true;
            }

            if (_fundsInfo.Amount > this._setting.MaxAmount)
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_AmountMaxLimit"));
                process.IsAbort = true;
            }

            if ((this._setting.TotalAllowed != Constants.VarNames.Unlimited) && (_fundsInfo.Amount > Convert.ToDecimal(this._setting.TotalAllowed)) && Convert.ToDecimal(this._setting.TotalAllowed) > 0)
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_TotalAllowedExceeded"));
                process.IsAbort = true;
            }

            return process;
        }

        protected virtual ProcessCode CheckResult(ref ProcessCode process, XElement response, Constants.PaymentTransactionType paymentType)
        {
            if (response == null)
            {
                process.Message = base.GetMessage("Pay_Fail");
                process.Code = (int)Constants.StatusCode.Error;

                return process;
            }

            process.IsSuccess = Convert.ToBoolean(CultureHelpers.ElementValues.GetResourceString("result", response));

            if (process.IsSuccess)
            {
                process.Message = base.GetMessage("Pay_Success");
                process.Data = new { TransactionId = CultureHelpers.ElementValues.GetResourceString("invId", response) };
                process.Code = (int)Constants.StatusCode.Success;
            }
            else
            {
                process.Code = Convert.ToInt32(CultureHelpers.ElementValues.GetResourceString("invId", response));

                switch (process.Code)
                {
                    case -1:
                    case -3:
                        process.Message.Add(base.GetMessage("Pay_Fail"));
                        break;

                    case -2:
                        if (Constants.PaymentTransactionType.Deposit == paymentType)
                        {
                            process.Message.Add(base.GetMessage("Pay_ErrDepositNotAllowed"));
                        }
                        else
                        {
                            process.Message.Add(base.GetMessage("Pay_ErrWithdrawalNotAllowed"));
                        }

                        break;

                    case -4:
                        process.Message.Add(base.GetMessage("Pay_ErrMaxLimitExceed"));
                        break;

                    case -5:
                        process.Message.Add(base.GetMessage("Pay_ErrMinLimitNotReached"));
                        break;

                    case -6:
                        process.Message.Add(base.GetMessage("Pay_ErrMaxDailyLimitExceed"));
                        break;

                    case -7:
                        process.Message.Add(base.GetMessage("Pay_ErrMaxDailyCountExceed"));
                        break;

                    case -8:
                        process.Message.Add(base.GetMessage("Pay_ErrInsufficientBalance"));
                        break;

                    case -10:
                        process.Message.Add(base.GetMessage("Pay_ErrInvalidCreditCardInfo"));
                        break;

                    case -11:
                        process.Message.Add(base.GetMessage("Pay_ErrNickNameNotAvailable"));
                        break;

                    case -99:
                        process.Message.Add(base.GetMessage("ServerError"));
                        break;
                }
            }

            process.ProcessSerialId += 1;

            this.LogResult(process, paymentType);

            return process;
        }

        protected abstract void LogResult(ProcessCode process, Constants.PaymentTransactionType paymentType);

        protected virtual async Task<XElement> CreateDeposit(ProcessCode process)
        {
            using (DepositClient client = new DepositClient())
            {
                return await client.createOnlineDepositTransactionV1Async(OperatorId, this._userInfo.MemberId, this._userInfo.MemberCode, Convert.ToInt64(this._setting.Id), this._userInfo.CurrencyCode, this._fundsInfo.Amount, DepositSource.Mobile, this._fundsInfo.Bank.Value);
            }
        }

        protected virtual async Task<XElement> CreateWithdrawal(ProcessCode process)
        {
            using (WithdrawalClient client = new WithdrawalClient())
            {
                return await client.createOnlineWithdrawalTransactionV1Async(OperatorId, this._userInfo.MemberCode, Convert.ToInt64(this._setting.Id), this._userInfo.CurrencyCode, this._fundsInfo.Amount, WithdrawalSource.Mobile, this._fundsInfo.AccountName, this._fundsInfo.AccountNumber);
            }
        }

        protected virtual async Task<ProcessCode> CreateVendorBOTransaction(ProcessCode process)
        {
            return process;
        }

        protected virtual ProcessCode CreateVendorParameter(ref ProcessCode process)
        {
            return process;
        }

        protected virtual ProcessCode ProcessDummyUrl(ref ProcessCode process)
        {
            return process;
        }

        private ProcessCode GetVendorRedirection(ref ProcessCode process)
        {
            var settings = new OperatorSettings(W88.Utilities.Constant.Settings.OperatorName);

            string url = settings.Values.Get(this.GetRedirectionUrl());

            string encryptedMemberCode = HttpUtility.UrlEncode(Encryption.Encrypting(this._userInfo.MemberCode, Constants.VarNames.PaymentPrivateKey));

            process.Data = new { VendorRedirectionUrl = url + "AutoSignIn.aspx?a=" + encryptedMemberCode + "&b=" + HttpUtility.UrlEncode(this._userInfo.CurrentSessionId) + "&isMobile=true"  };
            process.Code = (int)Constants.StatusCode.Success;

            Constants.PaymentTransactionType paymentType;
            Enum.TryParse(this._setting.Type, true, out paymentType);

            process.ProcessSerialId += 1;

            this.LogResult(process, paymentType);

            return process;
        }

        private string GetRedirectionUrl()
        {
            switch (this._setting.Id)
            {
                case "120212":
                    return "NganLuong_redirectUrl";

                default :
                    return "";
            }
        }

        private async Task<ProcessCode> CreateBOTransaction(ProcessCode process)
        {
            Constants.PaymentTransactionType paymentType;
            Enum.TryParse(this._setting.Type, true, out paymentType);

            XElement response = null;

            if (Constants.PaymentTransactionType.Deposit == paymentType)
            {
                response = await this.CreateDeposit(process);
                this.CheckResult(ref process, response, paymentType);

                if (process.IsSuccess)
                {
                    process = await this.CreateVendorBOTransaction(process);
                }
            }
            else
            {
                var hasPending = await new Payments().HasPendingWithdrawal(_userInfo.MemberCode);

                if (hasPending.Code == 0)
                {
                    response = await this.CreateWithdrawal(process);
                    this.CheckResult(ref process, response, paymentType);
                }
                else
                {
                    process.Code = (int)Constants.StatusCode.WithPendingWithdrawal;
                    process.Message = GetMessage("Pay_ErrWithdrawalNotAllowed");
                    LogResult(process, Constants.PaymentTransactionType.Withdrawal);
                }

            }

            return process;
        }

        protected string[] GetPaymentGatewayMerchantSetting(PaymentSettingInfo paymentSettings)
        {
            var merchantAccounts = Common.ParseJsonString<dynamic>(CultureHelpers.AppData.GetLocale_i18n_Resource("shared/MerchantAccounts", false), paymentSettings.Name);

            var result = string.IsNullOrWhiteSpace(paymentSettings.MerchantId) ? merchantAccounts.FirstOrDefault() : merchantAccounts.FirstOrDefault(m => m.MerchantID == paymentSettings.MerchantId);

            string key = string.Empty;
            switch (paymentSettings.Id)
            {
                case "120231":
                    key = Encryption.Decrypting(result.Key.Value, Constants.VarNames.PaymentPrivateKey) + "|" + Encryption.Decrypting(result.TerminalID.Value, Constants.VarNames.PaymentPrivateKey);
                    break;

                case "120227":
                    key = Encryption.Decrypting(result.Key.Value, Constants.VarNames.PaymentPrivateKey) + "|" + Encryption.Decrypting(result.MerchantID.Value, Constants.VarNames.PaymentPrivateKey);
                    break;

                case "120204":
                    key = Encryption.Decrypting(result.MerchantID.Value, Constants.VarNames.NextPayPrivateKey);
                    break;

                case "120223":
                    key = Encryption.Decrypting(result.Key1.Value, Constants.VarNames.PaymentPrivateKey) + "|" + Encryption.Decrypting(result.Key2.Value, Constants.VarNames.PaymentPrivateKey)
                        + "|" + Encryption.Decrypting(result.MD5Key.Value, Constants.VarNames.PaymentPrivateKey);
                    break;

                case "120236":
                    key = result.MerchantID.Value + "|" +
                          Encryption.Decrypting(result.AllDebit_Visa.Value, Constants.VarNames.PaymentPrivateKey) + "|" +
                          Encryption.Decrypting(result.AllDebit_Master.Value, Constants.VarNames.PaymentPrivateKey);
                    break;

                default:
                    key = Encryption.Decrypting(result.Key.Value, Constants.VarNames.PaymentPrivateKey);
                    break;
            }

            return key.Split('|');
        }

        public async Task<ProcessCode> Process()
        {
            var process = new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Message = new List<string>()
                };

            if (this.IsVendorRedirection)
            {
                this.GetVendorRedirection(ref process);
            }
            else
            {
                this.ValidateData(ref process);

                LogValidationResult(process);

                if (this.IsBOTranction)
                {
                    if (!process.IsAbort)
                    {
                        process = await this.CreateBOTransaction(process);

                        if (this.IsVendorTransaction)
                        {
                            if (process.IsSuccess)
                            {
                                this.CreateVendorParameter(ref process);
                            }
                        }
                    }
                }
                else
                {
                    this.ProcessDummyUrl(ref process);
                }
            }

            return process;
        }

        private void LogValidationResult(ProcessCode process)
        {
            process.Id = Guid.NewGuid();
            process.ProcessSerialId += 1;

            process.Remark = string.Format("IsValid: {0}", !process.IsAbort);

            AuditTrail.AppendLog(this._userInfo.MemberCode, Constants.PageNames.FundsPage,
                Constants.TaskNames.ParameterValidation, Constants.PageNames.ComponentName,
                Convert.ToString(process.Code),
                string.Join(" | ", process.Message), string.Empty, string.Empty, process.Remark,
                Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }
    }
}