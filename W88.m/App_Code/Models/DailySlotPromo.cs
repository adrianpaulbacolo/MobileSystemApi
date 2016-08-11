using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Models
{
    /// <summary>
    /// Summary description for DailySlotPromo
    /// </summary>
    public class DailySlotPromoResponse
    {
        public Info info { get; set; }
        public List<JsonPromoSettings> detail { get; set; }
    }

    public class Info
    {
        public int TotalRows { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class JsonPromoSettings
    {
        public long SlotPromoSettingsId { get; set; }
        public decimal Rollover { get; set; }
        public string PromoPeriodStart { get; set; }
        public string PromoPeriodEnd { get; set; }
        public JsonSlotGamePromoListing GamePromoListing { get; set; }
        public DataTable RiskCategory { get; set; }
        public DataTable Instructions { get; set; }
        public DataTable Amounts { get; set; }
    }

    public class JsonSlotGamePromoListing
    {
        public long SlotGamePromoListingId { get; set; }
        public string OperatorCode { get; set; }
        public string ProductCode { get; set; }
        public string GameTitle { get; set; }
        public string WebGameId { get; set; }
        public string WebGameCode { get; set; }
        public string HtmlGameId { get; set; }
        public string HtmlGameCode { get; set; }
        public string IosGameId { get; set; }
        public string IosGameCode { get; set; }
        public string AndroidGameId { get; set; }
        public string AndroidGameCode { get; set; }
        public string WindowsGameId { get; set; }
        public string WindowsGameCode { get; set; }
    }

    public enum SlotPromoClaimStatus
    {
        Pending = 1,
        ForApproval = 2,
        AutoApproved = 3,
        Approved = 4,
        Rejected = 5,
        Voided = 6
    }

    public class PromoClaimRequest
    {
        public decimal TotalStake { get; set; }
        public decimal TotalWinLost { get; set; }
        public decimal ClaimAmount { get; set; }
        public decimal RolloverAmount { get; set; }
        public SlotPromoSetup SlotPromoSetup { get; set; }
        public Info info { get; set; }
    }

    public class SlotPromoSetup
    {
        public decimal Stake { get; set; }
        public decimal Percentage { get; set; }
        public decimal MinimumBonus { get; set; }
        public decimal MaximumBonus { get; set; }
        public decimal Rollover { get; set; }
        public decimal EscalationThreshold { get; set; }
        public string CurrencyCode { get; set; }
        public string RiskCategoryId { get; set; }
        public string GameTitle { get; set; }
    }
    public class PromoClaimedResult
    {
        public long SlotPromoClaimId { get; set; }
        public SlotPromoClaimStatus ClaimStatus { get; set; }
        public PromoClaimRequest SlotPromoClaimRequest { get; set; }
        public Info info { get; set; }

        public PromoClaimedResult()
        {
            SlotPromoClaimId = 0;
            ClaimStatus = SlotPromoClaimStatus.Pending;
            SlotPromoClaimRequest = new PromoClaimRequest();
        }
    }
}