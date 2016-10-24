using System;
using System.Threading.Tasks;
using W88.WebRef.RewardsServices;
using RedemptionRequest = W88.BusinessLogic.Rewards.Redemption.Model.RedemptionRequest;

namespace W88.BusinessLogic.Rewards.Redemption.Factories.Handlers
{
    class OnlineRedemptionHandler : RedemptionBase
    {
        public OnlineRedemptionHandler(RedemptionRequest request) : base(request)
        {
            
        }

        protected override async Task<RedemptionResponse> ProcessRedemption()
        {
            using (var client = new RewardsServicesClient())
            {
                var request = (RedemptionOnlineRequest)CreateRequest();
                return await client.RedemptionOnlineAsync(request);
            }
        }

        protected override dynamic CreateRequest()
        {
            var request = new RedemptionOnlineRequest();
            try
            {
                request.OperatorId = Request.OperatorId;
                request.MemberCode = Request.MemberCode;
                request.ProductId = Request.ProductId;
                request.CategoryId = int.Parse(Request.CategoryId.Trim());
                request.RiskId = Request.RiskId;
                request.Currency = Request.Currency;
                request.PointRequired = int.Parse(Request.PointRequired);
                request.Quantity = int.Parse(Request.Quantity);
                request.AimId = Request.AimId;
                return request;
            }
            catch (Exception exception)
            {
                return request;
            }
        }
    }
}
