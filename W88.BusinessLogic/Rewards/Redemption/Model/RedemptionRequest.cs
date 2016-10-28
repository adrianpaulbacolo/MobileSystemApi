

using W88.WebRef.RewardsServices;

namespace W88.BusinessLogic.Rewards.Redemption.Model
{
    public class RedemptionRequest
    {
        public string OperatorId = string.Empty;

        public string MemberCode = string.Empty;

        public string ProductId = string.Empty;

        public string CategoryId = string.Empty;

        public string RiskId = string.Empty;

        public string Currency = string.Empty;

        public string PointRequired = string.Empty;

        public int Quantity = 0;

        public string CreditAmount = string.Empty;

        public string Name = string.Empty;

        public string ContactNumber = string.Empty;

        public string Address = string.Empty;

        public string PostalCode = string.Empty;

        public string City = string.Empty;

        public string Country = string.Empty;

        public string AimId = string.Empty;

        public string Remarks = string.Empty;

        public ProductTypeEnum ProductType;
    }
}
