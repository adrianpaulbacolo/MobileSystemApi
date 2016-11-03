using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities.Extensions;

namespace W88.API
{
    public class ContentsController : BaseController
    {
        [Route("contents")]
        [HttpGet]
        public HttpResponseMessage Contents(HttpRequestMessage request)
        {
            try
            {
                var langCode = request.GetHeader(Constants.VarNames.LanguageCode);

                return ReturnResponse(new ProcessCode
                {
                    Code = (int) Constants.StatusCode.Success,
                    Data = Utilities.Common.DeserializeObject<dynamic>(CultureHelpers.AppData.GetLocale_i18n_Resource("contents/translations", true, langCode))
                });
            }
            catch (Exception ex)
            {
                return ReturnResponse(new ProcessCode
                {
                    Code = (int) Constants.StatusCode.Error,
                    Message = ex.Message,
                }, ex);
            }
        }

        [Route("CountryPhoneList")]
        [HttpGet]
        public HttpResponseMessage GetCountryPhoneList()
        {
            try
            {
                string selected;
                var phoneList = new ListOfValuesHelper().GetCountryPhone(out selected);

                var contents = new
                {
                    PhoneList = phoneList,
                    PhoneSelected = selected,
                };

                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = contents
                });
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

        [Route("CurrencyList")]
        [HttpGet]
        public HttpResponseMessage GetCurrencyList()
        {
            try
            {
                string selectedCurrency;
                var currencies = new ListOfValuesHelper().GetCurrencies(out selectedCurrency);

                var contents = new
                {
                    CurrencyList = currencies,
                    CurrencySelected = selectedCurrency
                };

                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = contents
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

        [Route("Banks/vendor/{methodId}/{*currencyCode}")]
        [HttpGet]
        public HttpResponseMessage GetBankList(int methodId, string currencyCode)
        {
            try
            {
                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = new ListOfValuesHelper().GetBanksList(methodId, currencyCode)
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

        [Route("Banks/system")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetSystemBankAccounts(HttpRequestMessage request)
        {
            try
            {
                if (await CheckToken(request) == false) return ReturnResponse(UserRequest.Process);

                var banks = new ListOfValuesHelper().GetSystemBankAccounts(UserRequest.UserInfo);

                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = banks
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

        [Route("Banks/member")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetMemberBankAccounts(HttpRequestMessage request)
        {
            try
            {
                if (await CheckToken(request) == false) return ReturnResponse(UserRequest.Process);

                UserRequest.UserInfo.LanguageCode = GetLanguage(request);

                var banks = await new ListOfValuesHelper().GetMemberBankAccounts(UserRequest.UserInfo);

                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = banks
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

        [Route("Banks/member/secondary")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetMemberSecondaryBankAccounts(HttpRequestMessage request)
        {
            try
            {
                if (await CheckToken(request) == false) return ReturnResponse(UserRequest.Process);

                UserRequest.UserInfo.LanguageCode = GetLanguage(request);

                var banks = await new ListOfValuesHelper().GetMemberSecondaryBankAccounts(UserRequest.UserInfo);

                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = banks
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

        [Route("Banks/member/location/{bankId}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetMemberBankLocation(long bankId)
        {
            try
            {
                var locations = await new ListOfValuesHelper().GetMemberBankLocation(bankId);

                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = locations
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

        [Route("Banks/member/branch/{bankId}/{bankLocationId}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetMemberBankLocation(long bankId, long bankLocationId)
        {
            try
            {
                var branches = await new ListOfValuesHelper().GetMemberBankBranches(bankId, bankLocationId);

                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = branches
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

        [Route("DepositChannel")]
        [HttpGet]
        public HttpResponseMessage GetDepositChannel(HttpRequestMessage request)
        {
            try
            {
                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = new ListOfValuesHelper().GetDepositChannel()
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

        [Route("CardType")]
        [HttpGet]
        public HttpResponseMessage GetCardType(HttpRequestMessage request)
        {
            try
            {
                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = new ListOfValuesHelper().GetCardType()
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

        [Route("history")]
        [HttpGet]
        public HttpResponseMessage GetHistorySelections(HttpRequestMessage request)
        {
            try
            {
                UserRequest = new InitializeRequest(request) {UserInfo = {LanguageCode = GetLanguage(request)}};

                return ReturnResponse(new ProcessCode
                {
                    Code = (int)Constants.StatusCode.Success,
                    Data = Utilities.Common.DeserializeObject<dynamic>(CultureHelpers.AppData.GetLocale_i18n_Resource("/history/status", true, UserRequest.UserInfo.LanguageCode))
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