using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using W88.BusinessLogic.Accounts.Helpers;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Base.Api;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities.Extensions;

namespace W88.BusinessLogic.Accounts.Api
{
    [RoutePrefix("api/user")]
    public class UserController : BaseController
    {
       
        [HttpPost]
        [Route("Register")]
        public async Task<HttpResponseMessage> Register(RegisterInfo registerInfo)
        {
            try
            {
                var register = new RegisterHelper(registerInfo);
                var response = await register.Process();

                return ReturnResponse(response);
            }
            catch (Exception ex)
            {
                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Error,
                    Data = ex.Message
                }, ex);
            }
        }


        [HttpPost]
        [Route("Login")]
        public async Task<HttpResponseMessage> Login(LoginInfo loginInfo)
        {
            try
            {
                var response = await new LoginHelper().Login(loginInfo);

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

        [HttpGet]
        [Route("Logout")]
        public async Task<HttpResponseMessage> Logout(string memberId)
        {
            try
            {
                var response = await new LoginHelper().Logout(memberId);

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

        [HttpGet]
        [Route("Wallets")]
        public async Task<HttpResponseMessage> WalletBalances(HttpRequestMessage request)
        {
            try
            {
                if (await CheckToken(request) == false) return ReturnResponse(UserRequest.Process);

                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = await new Members().GetWalletBalancesAsync(UserRequest.UserInfo),
                    Message = Constants.StatusCode.Success.ToString()
                });
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

        [HttpGet]
        [Route("Wallet/{walletId}")]
        public async Task<HttpResponseMessage> WalletBalance(int walletId, HttpRequestMessage request)
        {
            try
            {
                if (await CheckToken(request) == false) return ReturnResponse(UserRequest.Process);

                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = await new Members().GetWalletBalanceAsync(walletId, UserRequest.UserInfo)
                });

            }
            catch (Exception ex)
            {
                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Error,
                    Data = Convert.ToDecimal("0").ToW88StringFormat()
                }, ex);
            }
        }

        [HttpGet]
        [Route("Rewards")]
        public async Task<HttpResponseMessage> GetRewards(HttpRequestMessage request)
        {
            try
            {
                if (await CheckToken(request) == false) return ReturnResponse(UserRequest.Process);

                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = Convert.ToDecimal(await new Members().GetRewardsPoints(UserRequest.UserInfo)).ToW88StringFormat()
                });
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

