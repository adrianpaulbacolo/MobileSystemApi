using W88.BusinessLogic.Rewards.Redemption.Factories.Handlers;
using W88.WebRef.RewardsServices;
using RedemptionRequest = W88.BusinessLogic.Rewards.Redemption.Model.RedemptionRequest;

namespace W88.BusinessLogic.Rewards.Redemption.Factories
{
    public class RedemptionStrategy
    {
        public static RedemptionBase Initialize(RedemptionRequest request)
        {
            switch (request.ProductType)
            {
                case ProductTypeEnum.Freebet:
                    return new FreeBetRedemptionHandler(request);
                case ProductTypeEnum.Normal:
                    return new NormalRedemptionHandler(request);
                case ProductTypeEnum.Online:
                    return new OnlineRedemptionHandler(request);
                case ProductTypeEnum.Wishlist:
                    return new WishlistRedemptionHandler(request);
                default:
                    return null;
            }
        }
    }
}
