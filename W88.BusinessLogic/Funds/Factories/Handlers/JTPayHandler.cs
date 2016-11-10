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
using W88.WebRef.svcPayDeposit;
using W88.WebRef.wsDummy;


namespace W88.BusinessLogic.Funds.Factories.Handlers
{
    public class JTPayHandler : FundsBase
    {
        private UserSessionInfo _userInfo;
        private FundsInfo _fundsInfo;
        private PaymentSettingInfo _setting;

        public JTPayHandler(UserSessionInfo userInfo, FundsInfo fundInfo, PaymentSettingInfo setting)
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
        }

        protected override void LogResult(ProcessCode process, Constants.PaymentTransactionType paymentType)
        {
            process.Remark = string.Format("IsSuccess: {0} | PaymentType: {1} | Amount: {2}",
                process.IsSuccess, Convert.ToString(paymentType), this._fundsInfo.Amount);

            AuditTrail.AppendLog(this._userInfo.MemberCode, Constants.PageNames.FundsPage,
               Constants.TaskNames.CreateBOTransaction, Constants.PageNames.ComponentName, Convert.ToString(process.Code),
                string.Join(" | ", process.Message), string.Empty, string.Empty, process.Remark,
               Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }

        protected override async Task<XElement> CreateDeposit(ProcessCode process)
        {
            using (DepositClient client = new DepositClient())
            {
                return await client.createOnlineDepositTransactionV1Async(OperatorId, this._userInfo.MemberId, this._userInfo.MemberCode, Convert.ToInt64(this._setting.Id), this._userInfo.CurrencyCode, this._fundsInfo.Amount, DepositSource.Mobile, string.Empty);
            }
        }

        protected override ProcessCode CreateVendorParameter(ref ProcessCode process)
        {
            string[] infoArray = base.GetPaymentGatewayMerchantSetting(this._setting);

            string md5Key = "";
            if (infoArray.Count() == 1)
            {
                md5Key = infoArray[0];
            }

            OperatorSettings operatorSettings = new OperatorSettings(Settings.OperatorName);

            string postUrl = operatorSettings.Values.Get("JTPay_posturl");
            string notifyUrl = operatorSettings.Values.Get("JTPay_serverreturnurl");


            string p1_usercode = this._setting.MerchantId;
            string p2_order = process.Data.TransactionId;
            string p3_money = this._fundsInfo.Amount.ToString();
            string p4_returnurl = this._fundsInfo.ThankYouPage;
            string p5_notifyurl = notifyUrl;
            string p6_ordertime = DateTime.Now.ToString("yyyyMMddHHmmss");

            string signParameters = string.Format("{0}&{1}&{2}&{3}&{4}&{5}{6}", this._setting.MerchantId, process.Data.TransactionId, p3_money, p4_returnurl, p5_notifyurl, p6_ordertime, md5Key);

            string p7_sign = Common.GetObject<Md5Hash>().Encrypt(signParameters).ToUpper();
            string p8_signtype = "1";
            string p9_paymethod = "3";

            process.Data = new
            {
                PostUrl = postUrl,
                FormData = new
                {
                    p1_usercode = p1_usercode,
                    p2_order = p2_order,
                    p3_money = p3_money,
                    p4_returnurl = p4_returnurl,
                    p5_notifyurl = p5_notifyurl,
                    p6_ordertime = p6_ordertime,
                    p7_sign = p7_sign,
                    p8_signtype = p8_signtype,
                    p9_paymethod = p9_paymethod
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