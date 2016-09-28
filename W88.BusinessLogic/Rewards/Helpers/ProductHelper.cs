using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.Utilities;

namespace W88.BusinessLogic.Rewards.Helpers
{
    public class ProductHelper
    {
        public static ProductDetails SelectedProduct
        {
            get
            {
                var product = CookieHelpers.CookieProduct;
                return string.IsNullOrEmpty(product) ? null : Common.DeserializeObject<ProductDetails>(product);
            }
            set
            {
                CookieHelpers.CookieProduct = Common.SerializeObject(value);
            }
        }
    }
}
