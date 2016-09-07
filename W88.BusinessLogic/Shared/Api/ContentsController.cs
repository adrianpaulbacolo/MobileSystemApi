using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using W88.BusinessLogic.Base.Api;
using W88.BusinessLogic.Base.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;

namespace W88.BusinessLogic.Shared.Api
{
    [RoutePrefix("api")]
    public class ContentsController : BaseController
    {
        [Route("contents")]
        [HttpGet]
        public HttpResponseMessage Contents(HttpRequestMessage request)
        {
            try
            {
                bool isValid = false;
                var enumerable = new string[] {};

                IEnumerable<string> langCode;
                request.Headers.TryGetValues(Constants.VarNames.LanguageCode, out langCode);

                if (langCode != null)
                {
                    enumerable = langCode as string[] ?? langCode.ToArray();
                    if (enumerable.Any())
                    {
                        var languages = new LanguageHelpers();
                        isValid = languages.Language.ContainsKey(enumerable.FirstOrDefault().ToLower());
                    }
                }

                return ReturnResponse(new ProcessCode
                {
                    Code = (int) Constants.StatusCode.Success,
                    Data = Utilities.Common.DeserializeObject<dynamic>(CultureHelpers.AppData.GetLocale_i18n_Resource("contents/translations", true, isValid ? enumerable.FirstOrDefault() : string.Empty))
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
                XElement xeResources = CultureHelpers.AppData.GetRootResource("_Secure/Register.aspx");

                string selectedCurrency;
                var currencies = new ListOfValuesHelper().GetCurrencies(xeResources, out selectedCurrency);

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
    }
}