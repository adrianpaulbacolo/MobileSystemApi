using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.BusinessLogic.Shared.Models;

namespace W88.API.Controllers
{
    [RoutePrefix("rewards")]
    public class RewardsController : BaseController
    {
        [HttpPost]
        [Route("search")]
        public async Task<HttpResponseMessage> Search(SearchInfo searchInfo)
        {
            try
            {
                var response = await new RewardsHelper().SearchProducts(searchInfo);

                return ReturnResponse(response);
            }
            catch (Exception ex)
            {
                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Error,
                    Message = ex.Message
                }, ex);
            }
        }
    }
}