using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using W88.BusinessLogic.Accounts.Helpers;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities;
using W88.Utilities.Constant;
using W88.Utilities.Extensions;
using W88.Utilities.Geo;

namespace W88.BusinessLogic.Base.Helpers
{

    public sealed class InitializeRequest
    {
        private string _token;
        public UserSessionInfo UserInfo = new UserSessionInfo();
        public ProcessCode Process = new ProcessCode();
        public bool TokenIsValid = false;
        public bool IsValidTokenHeader = false;

        public InitializeRequest(HttpRequestMessage request)
        {
            _token = request.GetHeader(Constants.VarNames.Token);
            UserInfo = new UserSessionInfo();

            Guid tokenGuid;
            var isValid = Guid.TryParse(_token, out tokenGuid);
            if (isValid) IsValidTokenHeader = true;
        }

        public async Task GetMember()
        {
            if (string.IsNullOrWhiteSpace(_token))
                SetTokenNotFound();
            else
            {
                UserInfo = await new Members().GetMemberInfo(_token);

                if (UserInfo.Status.ReturnValue != 1)
                {
                    Process.Code = UserInfo.Status.ReturnValue;
                    Process.Message = UserInfo.Status.ReturnMessage;
                    TokenIsValid = false;
                }
                else TokenIsValid = true;
            }
        }

        private void SetTokenNotFound()
        {
            Process = new ProcessCode
            {
                Code = (int)Constants.StatusCode.Error,
                Message = Constants.StatusCode.TokenNotFound.ToString()
            };
        }
    }

    public abstract class BaseHelper
    {
        protected int OperatorId
        {
            get
            {
                return Settings.OperatorId;
            }
        }

        protected string OperatorName
        {
            get
            {
                return Settings.OperatorName;
            }
        }

        protected string DeviceId
        {
            get
            {
                return Settings.DeviceId;
            }
        }

        protected string SiteUrl
        {
            get
            {
                return new ServerHelpers().SiteUrl;
            }
        }

        protected string GetMessage(string messageKey)
        {
            var keyValue = SystemMessages.Find(x => x.Text == messageKey);
            return keyValue == null ? string.Empty : keyValue.Value;
        }

        private static List<LOV> SystemMessages
        {
            get
            {
                var key = string.Format("{0}-{1}", Constants.VarNames.MessageKey, LanguageHelpers.SelectedLanguageShort);
                var msg = HttpContext.Current.Cache.Get(key) as dynamic;
                if (msg == null)
                {
                    msg = CultureHelpers.AppData.Messages;
                    HttpContext.Current.Cache.Add(key, msg, null, Cache.NoAbsoluteExpiration, new TimeSpan(6, 0, 0), CacheItemPriority.AboveNormal, null);
                }
                
                var list = new List<LOV>();
                foreach (var e in msg)
                {
                    list.Add(new LOV
                    {
                        Text = e.Name,
                        Value = e.Value
                    });
                }

                return list;
            }
        }

        protected List<T> GetListOfValues<T>(string filepath, string arrayName, bool useLanguage)
        {
            string settings = CultureHelpers.AppData.GetLocale_i18n_Resource(filepath, useLanguage);
            return Common.ParseJsonString<T>(settings, arrayName);
        }
    }
}
