using System;
using System.Threading.Tasks;
using W88.WebRef.RewardsServices;
using RedemptionRequest = W88.BusinessLogic.Rewards.Redemption.Model.RedemptionRequest;

namespace W88.BusinessLogic.Rewards.Redemption.Factories.Handlers
{
    class NormalRedemptionHandler : RedemptionBase
    {
        public NormalRedemptionHandler(RedemptionRequest request) : base(request)
        {
            
        }

        protected override async Task<RedemptionResponse> ProcessRedemption()
        {
            using (var client = new RewardsServicesClient())
            {
                var request = (RedemptionNormalRequest)CreateRequest();
                return await client.RedemptionNormalAsync(request);
            }
        }

        protected override dynamic CreateRequest()
        {
            var request = new RedemptionNormalRequest();
            try
            {
                request.OperatorId = Request.OperatorId;
                request.MemberCode = Request.MemberCode;
                request.ProductId = Request.ProductId;
                request.CategoryId = int.Parse(Request.CategoryId.Trim());
                request.RiskId = Request.RiskId;
                request.Currency = Request.Currency;
                request.PointRequired = int.Parse(Request.PointRequired.Trim());
                request.Quantity = int.Parse(Request.Quantity.Trim());
                request.Name = Request.Name;
                request.ContactNumber = Request.ContactNumber;
                request.Address = Request.Address;
                request.PostalCode = Request.PostalCode;
                request.City = Request.City;
                request.Country = Request.Country;
                return request;
            }
            catch (Exception exception)
            {
                return request;
            }
        }
    }
}
