using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using W88.BusinessLogic.Account.Models;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Base.Api;
using W88.BusinessLogic.Base.Models;
using W88.BusinessLogic.Funds.Factories;
using W88.BusinessLogic.Funds.Factories.Handlers;
using W88.BusinessLogic.Funds.Helpers;
using W88.BusinessLogic.Funds.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities;

namespace W88.BusinessLogic.Funds.Api
{
    [RoutePrefix("api/payments")]
    public class FundsController : BaseController
    {
        [HttpGet]
        [Route("Settings/{transType}")]
        public async Task<HttpResponseMessage> GetPaymentSetting(string transType, HttpRequestMessage request)
        {
            try
            {
                if (await CheckToken(request) == false) return ReturnResponse(UserRequest.Process);

                var paymentSettings = await new Payments().GetPaymentSettings(UserRequest.UserInfo, transType);

                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = paymentSettings
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

        [HttpPost]
        [Route("transfer")]
        public async Task<HttpResponseMessage> FundTransfer(HttpRequestMessage request, FundTransferInfo.FtRequest funds)
        {
            try
            {
                if (await CheckToken(request) == false) return ReturnResponse(UserRequest.Process);

                var response = await new Payments().FundTransfer(funds, UserRequest.UserInfo);

                if (response.FtCode == "00")
                {
                    return ReturnResponse(new ProcessCode
                    {
                        Code = (int)Constants.StatusCode.Success,
                        Data = response,
                        Message = response.Message
                    });
                }

                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Error,
                    Data = response,
                    Message = response.Message
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

        [HttpPost]
        [Route("history")]
        public async Task<HttpResponseMessage> GetHistoryResults(HttpRequestMessage request, HistoryInfoRequest historyRequest)
        {
            try
            {
                if (await CheckToken(request) == false) return ReturnResponse(UserRequest.Process);

                var process = await new Payments().SelectHistoryReport(historyRequest, UserRequest.UserInfo);

                if (process.Data != null)
                {
                    return ReturnResponse(new ProcessCode
                    {
                        Code = (int)Constants.StatusCode.Success,
                        Data = process.Data,
                        Message = Constants.StatusCode.Success.ToString(),
                    });
                }

                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Error,
                    Data = null,
                    Message = process.Message
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

        [HttpPost]
        [Route("{methodId}")]
        public async Task<HttpResponseMessage> CreateTransaction(HttpRequestMessage request, int methodId, BaseFundsInfo fundsInfo)
        {
            try
            {
                if (await CheckToken(request) == false) return ReturnResponse(UserRequest.Process);

                var settingInfo = await new Payments().GetPaymentSettings(UserRequest.UserInfo, string.Empty, methodId);
                var setting = settingInfo.Count == 0 ? new PaymentSettingInfo() : settingInfo.FirstOrDefault();

                var strategy = await FundsStrategy.Initialize(UserRequest.UserInfo, fundsInfo, setting);

                var response = strategy == null ? new ProcessCode() { Code = (int)Constants.StatusCode.NotImplemented, Message = Convert.ToString(Constants.StatusCode.NotImplemented) } : await strategy.Process();

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
        [Route("{methodId}/{transactionId}")]
        public async Task<HttpResponseMessage> GetCurrentDeposit(HttpRequestMessage request, int methodId, long transactionId)
        {
            try
            {
                if (await CheckToken(request) == false) return ReturnResponse(UserRequest.Process);

                if (methodId == 120254)
                {
                    var response = await new Payments().GetSDAPayCurrentDeposit(transactionId);

                    return ReturnResponse(response);
                }
                else
                {
                    return ReturnResponse(new ProcessCode
                    {
                        Message = Convert.ToString(Constants.StatusCode.NotImplemented),
                        Code = (int)Constants.StatusCode.NotImplemented,
                    });
                }
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
        [Route("deposit/status/{transactionId}")]
        public async Task<HttpResponseMessage> GetDepositStatus(HttpRequestMessage request, long transactionId)
        {
            try
            {
                if (await CheckToken(request) == false) return ReturnResponse(UserRequest.Process);

                var response = await new Payments().GetDepositStatus(transactionId);

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
        [Route("LastCardTrans")]
        public async Task<HttpResponseMessage> GetLastCreditTransaction(HttpRequestMessage request)
        {
            try
            {
                if (await CheckToken(request) == false) return ReturnResponse(UserRequest.Process);

                var response = await new Payments().GetLastCreditTransaction(UserRequest.UserInfo.MemberCode);

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
        [Route("withdrawal/pending")]
        public async Task<HttpResponseMessage> CheckHasPendingWithdawal(HttpRequestMessage request)
        {
            try
            {
                if (await CheckToken(request) == false) return ReturnResponse(UserRequest.Process);

                var response = await new Payments().HasPendingWithdrawal(UserRequest.UserInfo.MemberCode);

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

        [HttpPost]
        [Route("withdrawal/pending")]
        public async Task<HttpResponseMessage> CancelPendingWithdrawal(HttpRequestMessage request, PendingWithdrawalInfo pending)
        {
            try
            {
                if (await CheckToken(request) == false) return ReturnResponse(UserRequest.Process);

                var response = await new Payments().CancelPendingWithdrawal(UserRequest.UserInfo.MemberCode, pending);

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