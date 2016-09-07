using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using W88.BusinessLogic.Account.Helpers;
using W88.BusinessLogic.Account.Models;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Funds.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities.Extensions;
using W88.WebRef.svcPayMember;

namespace W88.BusinessLogic.Accounts.Helpers
{
    public sealed class HistoryReports : BaseHelper
    {
        private readonly HistoryInfoRequest _historyInfoRequest;
        private readonly UserSessionInfo _user;
        private readonly Wallets _wallets;

        private readonly List<LOV> _paymentType;
        private readonly List<LOV> _status;
        private readonly List<LOV> _ft_status;
        private readonly List<PaymentSettingInfo> _paymentSettings;

        public HistoryReports(HistoryInfoRequest historyInfoRequest, UserSessionInfo user) 
        {
            _historyInfoRequest = historyInfoRequest;
            _user = user;

            _paymentType = base.GetListOfValues<LOV>("history/status", "PaymentType", true);
            _status = base.GetListOfValues<LOV>("history/status", "Status", true);
            _ft_status = base.GetListOfValues<LOV>("history/status", "FT_Status", true);
            _paymentSettings = base.GetListOfValues<PaymentSettingInfo>("shared/PaymentSettings", "PaymentGateway", false);

            _wallets = new Wallets(_user, true);
        }

        public async Task<List<T>> GetDepositWidrawal<T>() where T :  HistoryInfoDepositWidrawResponse
        {
            using (var svc = new MemberClient())
            {
                var req = new getDepositWithdrawalHistoryRequest
                {
                    operatorId = Convert.ToInt32(OperatorId),
                    memberCode = _user.MemberCode,
                    datetimeFrom = _historyInfoRequest.DateFrom,
                    datetimeTo = _historyInfoRequest.DateTo,
                    paymentStatus = _historyInfoRequest.Status,
                    paymentType = _historyInfoRequest.Type
                };

                var dtHistory = await svc.getDepositWithdrawalHistoryAsync(req);
                
                var response = new List<HistoryInfoDepositWidrawResponse>();
                if (dtHistory.getDepositWithdrawalHistoryResult.Rows.Count <= 0) return null;

                foreach (DataRow row in dtHistory.getDepositWithdrawalHistoryResult.Rows)
                {
                    var type = GetPaymentTranslation(row["paymenttype"].ToString().ToUpper());
                    var status = GetStatusTranslation(row["status"].ToString());
                    var method = GetPaymentMethodTranslation(row["methodid"].ToString());

                    var info = new HistoryInfoDepositWidrawResponse
                    {
                        DateTime = Convert.ToDateTime(row["requestDate"]),
                        PaymentMethod = method,
                        PaymentType = type,
                        ReceivedAmount = Convert.ToDecimal(row["transAmount"]).ToW88StringFormat(),
                        Status = status,
                        SubmittedAmount = Convert.ToDecimal(row["requestAmount"]).ToW88StringFormat(),
                        TransactionId = row["invId"].ToString(),
                    };

                    response.Add(info);
                }

                return (List<T>)Convert.ChangeType(response, typeof(List<T>));
            }
        }

        public async Task<List<T>> GetFundTransfer<T>() where T : HistoryInfoFundTransferResponse
        {
            using (var svc = new MemberClient())
            {
                var req = new getFundTransferHistoryRequest
                {
                    operatorId = base.OperatorId,
                    memberCode = _user.MemberCode,
                    datetimeFrom = _historyInfoRequest.DateFrom,
                    datetimeTo = _historyInfoRequest.DateTo,
                    transferStatus = _historyInfoRequest.Status,
                    transferType = _historyInfoRequest.Type
                };

                var dtHistory = await svc.getFundTransferHistoryAsync(req);

                var response = new List<HistoryInfoFundTransferResponse>();
                if (dtHistory.getFundTransferHistoryResult.Rows.Count <= 0) return null;
               
                foreach (DataRow row in dtHistory.getFundTransferHistoryResult.Rows)
                {
                    var status = GetFtStatusTranslation(row["transferStatus"].ToString());

                    var info = new HistoryInfoFundTransferResponse
                    {
                        DateTime = Convert.ToDateTime(row["createdDateTime"]),
                        TransferAmount = Convert.ToDecimal(row["transferAmount"]).ToW88StringFormat(),
                        Status = status,
                        TransactionId = row["transferId"].ToString(),
                        CreatedBy = row["createdBy"].ToString(),
                        FromWalletId = GetWalletName(Convert.ToInt32(row["transferFromWalletId"])),
                        ToWalletId = GetWalletName(Convert.ToInt32(row["transferToWalletId"])),
                    };

                    response.Add(info);
                }

                return (List<T>)Convert.ChangeType(response, typeof(List<T>));
            }

        }

        public async Task<List<T>> GetPromotionClaim<T>() where T : HistoryInfoPromoClaimResponse
        {
            using (var svc = new WebRef.wsMemberMS1.memberWSSoapClient())
            {
                var dtHistory = await svc.MemberPromotionRegistrationHistoryAsync(OperatorId, _user.MemberId, _historyInfoRequest.DateFrom, _historyInfoRequest.DateTo);

                var response = new List<HistoryInfoPromoClaimResponse>();
                if (dtHistory.Tables.Count <= 0 || dtHistory.Tables[0].Rows.Count <= 0) return null;

                foreach (DataRow row in dtHistory.Tables[0].Rows)
                {

                    var info = new HistoryInfoPromoClaimResponse
                    {
                        SubmissionDate = Convert.ToDateTime(row["submissionDate"]),
                        SubjectCode = row["subjectCode"].ToString(),
                    };

                    response.Add(info);
                }

                return (List<T>)Convert.ChangeType(response, typeof(T));
            }
        }

        public async Task<T> GetReferralBonus<T>() where T : HistoryInfoReferralBonusResponse
        {
            using (var svc = new WebRef.wsMemberMS1.memberWSSoapClient())
            {
                var dtHistory = await svc.MemberReferralHistoryAsync(_user.MemberId, _historyInfoRequest.DateFrom, _historyInfoRequest.DateTo);

                var response = new HistoryInfoReferralBonusResponse();
                if (dtHistory.Tables.Count <= 0 || dtHistory.Tables[0].Rows.Count <= 0) return null;

                foreach (DataRow row in dtHistory.Tables[0].Rows)
                {
                    response.TotalInvitees = Convert.ToDecimal(row["totInvitees"]);
                    response.TotalRegistered = Convert.ToDecimal(row["totRegistered"]);
                    response.TotalSuccessRef = Convert.ToDecimal(row["totSuccessful"]);
                    response.TotalBonus = Convert.ToDecimal(row["totBonus"]);
                }

                foreach (DataRow row2 in dtHistory.Tables[1].Rows)
                {
                    response.Records.Add(new HistoryInfoReferralBonusList
                    {
                        Amount = Convert.ToDecimal(row2["requestAmount"]).ToW88StringFormat(),
                        DateTime = Convert.ToDateTime(row2["requestDate"]),
                        Status = Convert.ToString(row2["status"]),
                        TransactionId = Convert.ToString(row2["invId"])
                    });
                }

                return (T)Convert.ChangeType(response, typeof(T));
            }
        }

        private string GetWalletName(int walletId)
        {
            var name = _wallets.WalletInfo.Where(x => x.Id == walletId).ToList();
            return name.Any() ? name[0].Name : Convert.ToString(walletId);
        }

        private string GetPaymentTranslation(string type)
        {
            if (type == Constants.PaymentTransactionType.Deposit.ToString().ToUpper())
            {
                const int id = (int) Constants.PaymentTransactionType.Deposit;
                var payment = _paymentType.FirstOrDefault(x => x.Value == Convert.ToString(id));
                return payment == null ? type : payment.Text;
            }
            
            if (type == Constants.PaymentTransactionType.Withdrawal.ToString().ToUpper()) 
            {
                const int id = (int) Constants.PaymentTransactionType.Withdrawal;
                var payment = _paymentType.FirstOrDefault(x => x.Value == Convert.ToString(id));
                return payment == null ? type : payment.Text;
            }

            return type;
        }

        private string GetStatusTranslation(string status)
        {
            var statusFirstOrDefault = _status.FirstOrDefault(x => x.Value.ToUpper() == status.ToUpper());
            return statusFirstOrDefault != null ? statusFirstOrDefault.Text : status;
        }

        private string GetPaymentMethodTranslation(string methodId)
        {
            var settingInfo = _paymentSettings.FirstOrDefault(x => x.Id == methodId);
            return settingInfo != null ? settingInfo.TabName.GetValue(LanguageHelpers.SelectedLanguageShort).Value : methodId;
        }

        private string GetFtStatusTranslation(string status)
        {
            var statusFirstOrDefault = _ft_status.FirstOrDefault(x => x.Value.ToUpper() == status.ToUpper());
            return statusFirstOrDefault != null ? statusFirstOrDefault.Text : status;
        }
    }
}
