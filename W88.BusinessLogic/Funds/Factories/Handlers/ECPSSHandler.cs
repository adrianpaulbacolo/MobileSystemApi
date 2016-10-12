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
using W88.Utilities.Geo;
using W88.Utilities.Log.Helpers;
using W88.Utilities.Security;
using W88.WebRef.wsDummy;


namespace W88.BusinessLogic.Funds.Factories.Handlers
{
    public class ECPSSHandler : FundsBase
    {
        private UserSessionInfo _userInfo;
        private BaseFundsInfo _fundsInfo;
        private PaymentSettingInfo _setting;
        private List<LOV> _banks;

        public ECPSSHandler(UserSessionInfo userInfo, BaseFundsInfo fundInfo, PaymentSettingInfo setting)
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
            process.Remark = string.Format("IsSuccess: {0} | PaymentType: {1} | Amount: {2} | DummyURL: {3}",
                process.IsSuccess, Convert.ToString(paymentType), this._fundsInfo.Amount, process.Data.DummyURL);

            AuditTrail.AppendLog(this._userInfo.MemberCode, Constants.PageNames.FundsPage,
               Constants.TaskNames.ProcessDummyUrl, Constants.PageNames.ComponentName, Convert.ToString(process.Code),
                string.Join(" | ", process.Message), string.Empty, string.Empty, process.Remark,
               Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }

        protected override ProcessCode ProcessDummyUrl(ref ProcessCode process)
        {
            using (dummyWSSoapClient client = new dummyWSSoapClient())
            {
                var result = client.DummyURLs_Mobile(OperatorId, Convert.ToInt64(this._setting.Id), this._userInfo.PaymentGroup);

                if (result.Tables[0].Rows.Count > 0)
                {
                    var cookie = Encryption.Encrypting(this._userInfo.CurrentSessionId, Constants.VarNames.PaymentPrivateKey);
                    var ip = Encryption.Encrypting(new IpHelper().Remote, Constants.VarNames.PaymentPrivateKey);
                    var domain = Convert.ToString(result.Tables[0].Rows[0][Constants.VarNames.RedirectUrl]);

                    process.Data = new { DummyURL = domain + "api/ECPSSHandler.ashx?requestAmount=" + this._fundsInfo.Amount + "&bankCode=" + this._fundsInfo.Bank.Value + "&cookie=" + cookie + "&ip=" + ip + "&isMobile=true" };
                    process.Code = (int)Constants.StatusCode.Success;
                }
                else
                {
                    process.Data = new { DummyURL = string.Empty };
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message = base.GetMessage("Pay_ErrEmptyDummyURL");
                }
            }

            Constants.PaymentTransactionType paymentType;
            Enum.TryParse(this._setting.Type, true, out paymentType);

            process.ProcessSerialId += 1;

            this.LogResult(process, paymentType);

            return process;
        }

        protected override bool IsBOTranction
        {
            get { return false; }
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