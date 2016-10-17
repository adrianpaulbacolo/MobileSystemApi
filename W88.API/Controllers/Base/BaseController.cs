using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Base.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities.Extensions;
using W88.Utilities.Log.Helpers;

namespace W88.API
{
    [BaseCorsPolicy]
    public class BaseController : ApiController
    {
        protected InitializeRequest UserRequest;

        [NonAction]
        protected async Task<bool> CheckToken(HttpRequestMessage request)
        {
         
            UserRequest = new InitializeRequest(request);

            if (UserRequest.IsValidTokenHeader)
                await UserRequest.GetMember();    
            
            return UserRequest.TokenIsValid;
        }

        protected string GetLanguage(HttpRequestMessage request)
        {
            var language = request.GetHeader(Constants.VarNames.LanguageCode);
            return (!string.IsNullOrEmpty(language)) ? language : "en-us";
        }

        protected HttpResponseMessage ReturnResponse(ProcessCode process, Exception e = null)
        {
            var response = new DataResponse
            {
                ResponseCode = process.Code,
                ResponseData = process.Data,
                ResponseMessage = process.Message
            };

            if (process.Code == (int)Constants.StatusCode.Error && e != null)
            {
                response.ResponseCode = process.Code;
                response.ResponseData = string.Empty;
                AuditTrail.AppendLog(e);
            }

            return new HttpResponseMessage(HttpStatusCode.Accepted)
            {
                Content = new StringContent(Utilities.Common.SerializeObject(response), Encoding.UTF8, "application/json"),
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
