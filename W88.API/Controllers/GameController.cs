using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using W88.BusinessLogic.Games.Factories;
using W88.BusinessLogic.Games.Models;
using W88.BusinessLogic.Shared.Models;

namespace W88.API
{
    [RoutePrefix("game")]
    public class GameController : BaseController
    {
        // GET api/<controller>
        /// <summary>
        ///     Games per Game Provider
        /// </summary>
        /// <param name="request"></param>
        /// <param name="gameProvider">BS, CTXM, GPI, ISB, MGS, PNG, PT, QT</param>
        /// <param name="gameInfo">Game configuration</param>
        /// <returns></returns>
        [Route("{gameProvider}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetGames(HttpRequestMessage request, string gameProvider, [FromUri] GameConfigInfo gameInfo)
        {
            try
            {
                await CheckToken(request);

                UserRequest.UserInfo.LanguageCode = getLanguage(request);

                var strategy = GameStrategy.Initialize(gameProvider, UserRequest.UserInfo, gameInfo.Lobby, gameInfo.Cashier, gameInfo.LiveChat, gameInfo.Logout, gameInfo.Device);

                var games = strategy == null ? null : strategy.Process(UserRequest.UserInfo.CurrencyCode, gameInfo.TotalItemCount, gameInfo.Categories);

                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = games
                });
            }
            catch (Exception ex)
            {
                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Error,
                    Message = ex.Message,
                }, ex);
            }
        }
    }
}
