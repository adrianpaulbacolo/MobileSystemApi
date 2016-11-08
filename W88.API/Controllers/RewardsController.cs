using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.BusinessLogic.Shared.Models;

namespace W88.API
{
    [RoutePrefix("rewards")]
    public class RewardsController : BaseController
    {
        // GET api/<controller>
        /// <summary>
        ///     Search catalogue products
        /// </summary>
        /// <param name="request"></param>
        /// <param name="searchInfo">SearchInfo object that contains search parameters</param>
        /// <returns>HttpResponseMessage</returns>
        [Route("search")]
        [HttpGet]
        public async Task<HttpResponseMessage> Search(HttpRequestMessage request, [FromUri] SearchInfo searchInfo)
        {
            try
            {
                await CheckToken(request);           
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