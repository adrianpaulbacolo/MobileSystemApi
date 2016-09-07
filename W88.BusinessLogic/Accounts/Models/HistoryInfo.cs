using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W88.BusinessLogic.Account.Models
{

    public enum ReportType
    {
        DepositWidraw,
        FundTransfer,
        PromoClaim,
        RefferalBonus
    }

    public class HistoryInfoRequest
    {
        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public string Status { get; set; }

        public int Type { get; set; }

        public ReportType ReportType { get; set; }
    }

    public class HistoryInfoDepositWidrawResponse : HistoryInfoBase
    {
        public string PaymentMethod { get; set; }

        public string PaymentType { get; set; }

        public string SubmittedAmount { get; set; }

        public string ReceivedAmount { get; set; }
    }

    public class HistoryInfoFundTransferResponse : HistoryInfoBase
    {
        public string TransferAmount { get; set; }

        public string FromWalletId { get; set; }

        public string ToWalletId { get; set; }
        
        public string CreatedBy { get; set; }
    }

    public class HistoryInfoPromoClaimResponse
    {
        public DateTime SubmissionDate { get; set; }

        public string SubjectCode { get; set; }
    }

    public class HistoryInfoReferralBonusResponse
    {
        public decimal TotalInvitees { get; set; }

        public decimal TotalRegistered { get; set; }

        public decimal TotalSuccessRef { get; set; }

        public decimal TotalBonus { get; set; }

        public List<HistoryInfoReferralBonusList> Records = new List<HistoryInfoReferralBonusList>();
    }

    public class HistoryInfoReferralBonusList : HistoryInfoBase
    {
        public string Amount { get; set; }
    }

    public class HistoryInfoBase
    {
        public DateTime DateTime { get; set; }

        public string TransactionId { get; set; }

        public string Status { get; set; }
    }
}
