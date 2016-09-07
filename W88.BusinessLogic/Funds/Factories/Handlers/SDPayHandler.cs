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
    public class SDPayHandler : FundsBase
    {
        private UserSessionInfo _userInfo;
        private BaseFundsInfo _fundsInfo;
        private PaymentSettingInfo _setting;

        public SDPayHandler(UserSessionInfo userInfo, BaseFundsInfo fundInfo, PaymentSettingInfo setting)
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

        protected override void LogResult(ProcessCode process, Constants.PaymentTransactionType paymentType)
        {
            process.Remark = string.Format("IsSuccess: {0} | PaymentType: {1} | TransactionId: {2} | Amount: {3}",
                process.IsSuccess, Convert.ToString(paymentType), process.IsSuccess ? process.Data.TransactionId : "", this._fundsInfo.Amount);

            AuditTrail.AppendLog(this._userInfo.MemberCode, Constants.PageNames.FundsPage,
               Constants.TaskNames.CreateBOTransaction, Constants.PageNames.ComponentName, Convert.ToString(process.Code),
                string.Join(" | ", process.Message), string.Empty, string.Empty, process.Remark,
               Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }

        protected override ProcessCode CreateVendorParameter(ref ProcessCode process)
        {
            string[] merchantAccount = base.GetPaymentGatewayMerchantSetting(this._setting);

            string merchantKey1 = "", merchantKey2 = "", md5Key = "";
            if (merchantAccount.Count() == 3)
            {
                merchantKey1 = merchantAccount[0];
                merchantKey2 = merchantAccount[1];
                md5Key = merchantAccount[2];
            }

            string merchantID = _setting.MerchantId;
            string order = process.Data.TransactionId;
            string username = Convert.ToString(this._userInfo.MemberId);
            string money = this._fundsInfo.Amount.ToW88StringFormat();
            string language = "zh-cn";
            string cmd = "6009";
            string unit = "1";

            OperatorSettings operatorSettings = new OperatorSettings(Settings.OperatorName);
            string postUrl = operatorSettings.Values.Get("SDPay_PostUrl");
            string backurl = operatorSettings.Values.Get("SDPay_ServerReturnUrl");


            var builder = new StringBuilder();
            builder.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            builder.Append("<message>");
            builder.AppendFormat("<cmd>{0}</cmd>", cmd);
            builder.AppendFormat("<merchantid>{0}</merchantid>", merchantID);
            builder.AppendFormat("<language>{0}</language>", language);
            builder.Append("<userinfo>");
            builder.AppendFormat("<order>{0}</order>", order);
            builder.AppendFormat("<username>{0}</username>", username);
            builder.AppendFormat("<money>{0}</money>", money);
            builder.AppendFormat("<unit>{0}</unit>", unit);
            builder.AppendFormat("<time>{0}</time>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            builder.Append("<remark></remark>");
            builder.AppendFormat("<backurl>{0}</backurl>", backurl);
            builder.Append("</userinfo>");
            builder.Append("</message>");

            string md5 = Common.GetObject<Md5Hash>().Encrypt(builder.ToString() + md5Key);

            string d = builder.ToString() + md5;

            string des = Common.GetObject<DES>().Encrypt(d, merchantKey1, merchantKey2);

            process.Data = new
            {
                TransactionId = order,
                PostUrl = postUrl,
                FormData = new
                {
                    cmd = cmd,
                    pid = merchantID,
                    des = des
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