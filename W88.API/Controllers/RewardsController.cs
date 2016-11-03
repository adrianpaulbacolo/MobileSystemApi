using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities;

namespace W88.API.Controllers
{
    [RoutePrefix("rewards")]
    public class RewardsController : BaseController
    {
        [HttpGet]
        [Route("search")]
        public async Task<HttpResponseMessage> Search(HttpRequestMessage request)
        {
            try
            {
                await CheckToken(request);
                var parameters = request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value);
                if (parameters.Count == 0) return ReturnResponse(UserRequest.Process);
                if(string.IsNullOrEmpty(parameters["searchInfo"])) return ReturnResponse(UserRequest.Process);

                var searchInfo = Common.DeserializeObject<SearchInfo>(parameters["searchInfo"]);           
                var response = await new RewardsHelper().SearchProducts(searchInfo, UserRequest.UserInfo);

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