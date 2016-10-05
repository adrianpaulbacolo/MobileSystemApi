using System;
using System.Threading.Tasks;
using W88.WebRef.RewardsServices;
using RedemptionRequest = W88.BusinessLogic.Rewards.Redemption.Model.RedemptionRequest;

namespace W88.BusinessLogic.Rewards.Redemption.Factories.Handlers
{
    public class FreeBetRedemptionHandler : RedemptionBase
    {
        public FreeBetRedemptionHandler(RedemptionRequest request) : base(request)
        {

        }

        protected override async Task<RedemptionResponse> ProcessRedemption()
        {
            using (var client = new RewardsServicesClient())
            {
                var request = (RedemptionFreebetRequest)CreateRequest();
                return await client.RedemptionFreebetAsync(request);
            }
        }

        protected override dynamic CreateRequest()
        {
            var request = new RedemptionFreebetRequest();
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
                request.CreditAmount = Convert.ToDecimal(Request.CreditAmount.Trim());
                return request;
            }
            catch (Exception exception)
            {
                return request;
            }
        }
    }
}
