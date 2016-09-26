using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using W88.BusinessLogic.Base.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities;

namespace W88.API
{
    public class BannerController : BaseController
    {
        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            try
            {
                
                var banner = CultureHelpers.AppData.GetJsonRootResource("Shared/Banner");

                return ReturnResponse(new ProcessCode
                 {
                     Code = (int)Constants.StatusCode.Success,
                     Data = Common.DeserializeObject<dynamic>(banner)
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