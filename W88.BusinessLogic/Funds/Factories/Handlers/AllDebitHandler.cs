using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    public class AllDebitHandler : FundsBase
    {
        private UserSessionInfo _userInfo;
        private FundsInfo _fundsInfo;
        private PaymentSettingInfo _setting;
        private List<LOV> _cardType;

        public AllDebitHandler(UserSessionInfo userInfo, FundsInfo fundInfo, PaymentSettingInfo setting)
            : base(userInfo, fundInfo, setting)
        {
            if (userInfo == null)
                userInfo = new UserSessionInfo();

            _userInfo = userInfo;

            if (fundInfo == null)
                fundInfo = new FundsInfo();

            _fundsInfo = fundInfo;
            
            if (setting == null)
                setting = new PaymentSettingInfo();

            _setting = setting;

            _cardType = new ListOfValuesHelper().GetCardType();
        }

        protected override void LogResult(ProcessCode process, Constants.PaymentTransactionType paymentType)
        {
            process.Remark = string.Format("IsSuccess: {0} | PaymentType: {1} | TransactionId: {2} | Amount: {3} | CardNumber: {4} | CardType: {5} | CardExpiryMonth: {6} | CardExpiryYear: {7}",
               process.IsSuccess, Convert.ToString(paymentType), process.IsSuccess ? process.Data.TransactionId : "", this._fundsInfo.Amount, _fundsInfo.CardNumber, _fundsInfo.CardType.Text, _fundsInfo.CardExpiryMonth, _fundsInfo.CardExpiryYear);

            AuditTrail.AppendLog(_userInfo.MemberCode, Constants.PageNames.FundsPage,
               Constants.TaskNames.CreateBOTransaction, Constants.PageNames.ComponentName, Convert.ToString(process.Code),
                string.Join(" | ", process.Message), string.Empty, string.Empty, process.Remark,
               Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }

        protected override ProcessCode ValidateData(ref ProcessCode process)
        {
            base.ValidateData(ref process);

            if (string.IsNullOrEmpty(_fundsInfo.AccountName))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_MissingCardName"));
                process.IsAbort = true;
            }
            else if (string.IsNullOrEmpty(_fundsInfo.CardNumber))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_MissingCardNo"));
                process.IsAbort = true;
            }
            else if (string.IsNullOrEmpty(_fundsInfo.CCV))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_MissingCCV"));
                process.IsAbort = true;
            }
            else if (string.IsNullOrWhiteSpace(_fundsInfo.CardExpiryMonth) || string.IsNullOrWhiteSpace(_fundsInfo.CardExpiryYear))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_MissingCardExpiry"));
                process.IsAbort = true;
            }
            else
            {
                if (!this._cardType.Any(b => b.Text == _fundsInfo.CardType.Text && b.Value == _fundsInfo.CardType.Value))
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_MissingCardType"));
                    process.IsAbort = true;
                }
            }

            return process;
        }

        protected override async Task<XElement> CreateDeposit(ProcessCode process)
        {
            using (var client = new DepositClient())
            {
                return await client.createCreditCardTransactionAsync(OperatorId, _userInfo.MemberId, _userInfo.MemberCode,
                            Convert.ToInt64(_setting.Id), _setting.MerchantId, _userInfo.CurrencyCode, _fundsInfo.Amount,
                            DepositSource.Mobile,
                            _fundsInfo.AccountName, _fundsInfo.SanitizedCardNumber, _fundsInfo.CardType.Text,
                            _fundsInfo.CardExpiryMonth, _fundsInfo.CardExpiryYear, _fundsInfo.CCV, "OTHER");

            }
        }

        protected override ProcessCode CreateVendorParameter(ref ProcessCode process)
        {
            var transactionId = process.Data.TransactionId;
            var cardType = _fundsInfo.CardType.Value;

            string[] merchantAccount = base.GetPaymentGatewayMerchantSetting(this._setting);
            const string email = "pgwsw88@gmail.com";

            var builder = new StringBuilder();
            builder.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}", merchantAccount[0], cardType, transactionId, _userInfo.CurrencyCode, _fundsInfo.Amount, _userInfo.MemberId, _userInfo.MemberId, _fundsInfo.CardNumber, _fundsInfo.CardExpiryYear, _fundsInfo.CardExpiryMonth, _fundsInfo.CCV, email, merchantAccount);

            string signInfo = Common.GetObject<Sha256Hash>().Encrypt(builder.ToString()).ToUpper();
            const string na = "NA";

            var operatorSettings = new OperatorSettings(Settings.OperatorName);

            string postUrl = operatorSettings.Values.Get("AllDebit_posturl");
            string returnUrl = operatorSettings.Values.Get("AllDebit_returnURL").Replace("{DOMAIN}", new Utilities.Geo.IpHelper().DomainName);

            process.Data = new
            {
                TransactionId = transactionId,
                PostUrl = postUrl,
                FormData = new
                {
                    merNo = merchantAccount[0],
                    gatewayNo = cardType,
                    orderNo = transactionId,
                    orderCurrency = _userInfo.CurrencyCode,
                    orderAmount = _fundsInfo.Amount,
                    returnUrl = returnUrl,
                    cardNo = _fundsInfo.CardNumber,
                    cardExpireMonth = _fundsInfo.CardExpiryMonth,
                    cardExpireYear = _fundsInfo.CardExpiryYear,
                    cardSecurityCode = _fundsInfo.CCV,
                    issuingBank = "OTHER",
                    firstName = _userInfo.MemberId,
                    lastName = _userInfo.MemberId,
                    phone = na,
                    country = na,
                    state = na,
                    city = na,
                    address = na,
                    zip = na,
                    signInfo = signInfo,
                    csid = string.Empty
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
