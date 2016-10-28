using System;
using System.Threading.Tasks;
using W88.WebRef.RewardsServices;
using RedemptionRequest = W88.BusinessLogic.Rewards.Redemption.Model.RedemptionRequest;

namespace W88.BusinessLogic.Rewards.Redemption.Factories.Handlers
{
    class WishlistRedemptionHandler : RedemptionBase
    {
        public WishlistRedemptionHandler(RedemptionRequest request) : base(request)
        {
            
        }

        protected override async Task<RedemptionResponse> ProcessRedemption()
        {
            using (var client = new RewardsServicesClient())
            {
                var request = CreateWishListRequest();
                return await client.RedemptionWishlistAsync(request);
            }
        }

        private RedemptionWishlistRequest CreateWishListRequest()
        {
            var request = new RedemptionWishlistRequest();
            try
            {
                request.OperatorId = Request.OperatorId;
                request.MemberCode = Request.MemberCode;
                request.ProductId = Request.ProductId;
                request.CategoryId = int.Parse(Request.CategoryId.Trim());
                request.RiskId = Request.RiskId;
                request.Currency = Request.Currency;
                request.PointRequired = int.Parse(Request.PointRequired.Trim());
                request.Quantity = Request.Quantity;
                request.Name = Request.Name;
                request.ContactNumber = Request.ContactNumber;
                request.Address = Request.Address;
                request.PostalCode = Request.PostalCode;
                request.City = Request.City;
                request.Country = Request.Country;
                request.Remark = Request.Remarks;
                return request;
            }
            catch (Exception)
            {
                return request;
            }
        }
    }
}
