using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using W88.BusinessLogic.Base.Api;
using W88.BusinessLogic.Base.Models;
using W88.BusinessLogic.Games.Factories;
using W88.BusinessLogic.Shared.Models;

namespace W88.BusinessLogic.Games.Api
{
    [RoutePrefix("api/game")]
    public class GameController : BaseController
    {
        // GET api/<controller>
        /// <summary>
        ///     Games per Game Provider
        /// </summary>
        /// <param name="gameProvider">BS, CTXM, GPI, ISB, MGS, PNG, PT, QT</param>
        /// <param name="request"></param>
        /// <param name="lobby">Club Landing Page</param>
        /// <param name="cashier">Cashier Landing Page</param>
        /// <param name="liveChat">Live Chat Landing Page</param>
        /// <param name="logout">Logout Landing Page</param>
        /// <param name="device">ANDROID, IOS or WP</param>
        /// <returns></returns>
        [Route("{gameProvider}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetGames(string gameProvider, HttpRequestMessage request, string lobby = "", string cashier = "", string liveChat = "", string logout = "", string device = "")
        {
            try
            {
                await CheckToken(request);

                var strategy = GameStrategy.Initialize(gameProvider, UserRequest.UserInfo, lobby, cashier, liveChat, logout, device);

                var games = strategy == null ? null : strategy.Process(UserRequest.UserInfo.CurrencyCode);

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
