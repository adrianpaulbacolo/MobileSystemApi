using System;
using System.Data;
using System.Threading.Tasks;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities;
using W88.Utilities.Constant;
using W88.Utilities.Geo;
using W88.Utilities.Geo.Models;
using W88.Utilities.Log.Helpers;
using W88.Utilities.Security;
using W88.WebRef.wsMemberMS1;

namespace W88.BusinessLogic.Accounts.Helpers
{
    /// <summary>
    /// Process of Registering a user account
    /// </summary>
    public sealed class RegisterHelper : BaseHelper
    {
        private readonly RegisterInfo _registerInfo;
        public PageHeaders Headers;
        private IpHelper _ipHelper;

        public RegisterHelper(RegisterInfo info)
        {
            _registerInfo = info;

            SetValues();
        }

        public async Task<ProcessCode> Process()
        {
            var process = this.ValidateData();

            if (!process.IsAbort)
            {
                using (var svc = new memberWSSoapClient())
                {
                    var dsRegister = await svc.MemberRegistrationNewAsync(base.OperatorId, _registerInfo.UserInfo.Username,
                        _registerInfo.UserInfo.Password, _registerInfo.Email, _registerInfo.ContactNumber,
                        _registerInfo.Address, _registerInfo.City, _registerInfo.Postal, _registerInfo.Country,
                        _registerInfo.CurrencyCode, _registerInfo.Gender, _registerInfo.OddsType,
                        string.IsNullOrEmpty(_registerInfo.LanguageCode) ? "en-us" : _registerInfo.LanguageCode,
                        Convert.ToInt32(_registerInfo.AffiliateId), _registerInfo.ReferBy, _registerInfo.IpAddress, _registerInfo.SignUpUrl,
                        base.DeviceId, _registerInfo.IsTestAccount, _registerInfo.Firstname, _registerInfo.Lastname,
                        _registerInfo.DateOfBirth, string.Empty);

                    if (dsRegister.Tables[0].Rows.Count > 0)
                    {
                        process.Code = Convert.ToInt32(dsRegister.Tables[0].Rows[0]["RETURN_VALUE"]);

                        this.CheckResult(ref process);

                        if (process.Code != 1)
                            return process;

                        var loginInfo = new LoginInfo
                        {
                            UserInfo =
                            {
                                Username = _registerInfo.UserInfo.Username,
                                Password = _registerInfo.UserInfo.Password
                            }
                        };

                        process = await new LoginHelper().Login(loginInfo);
                    }
                }
            }

            return process;
        }

        private ProcessCode ValidateData()
        {
            DateTime dtDob;
            var msg = new ProcessCode { Code = (int)Constants.StatusCode.Success, Message = "OK:ParameterValidation" };

            if (string.IsNullOrEmpty(_registerInfo.UserInfo.Username))
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message = base.GetMessage("Login_MissingUsername");
                msg.IsAbort = true;
            }
            else if (string.IsNullOrEmpty(_registerInfo.UserInfo.Password))
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message = base.GetMessage("Login_MissingPassword");
                msg.IsAbort = true;
            }
            else if (string.IsNullOrEmpty(_registerInfo.Email))
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message = base.GetMessage("Reg_MissingEmail");
                msg.IsAbort = true;
            }
            else if (string.IsNullOrEmpty(_registerInfo.Phone))
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message = base.GetMessage("Reg_MissingContact");
                msg.IsAbort = true;
            }
            else if (!_registerInfo.RexgexContact.IsMatch(_registerInfo.ContactNumber))
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message = base.GetMessage("Reg_InvalidContact");
                msg.IsAbort = true;
            }
            else if (string.IsNullOrEmpty(_registerInfo.CurrencyCode) || string.Compare(_registerInfo.CurrencyCode, "-1", true) == 0)
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message = base.GetMessage("Reg_MissingCurrency");
                msg.IsAbort = true;
            }
            else if (string.IsNullOrEmpty(_registerInfo.Firstname))
            {
                // This changes is for the combined name on frontend only but on the BO everything will be saved in firstname
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message = base.GetMessage("Reg_MissingName");
                msg.IsAbort = true;
            }
            else if (Validation.IsInjection(_registerInfo.UserInfo.Username) || _registerInfo.UserInfo.Username.IndexOf(' ') >= 0 || !Validation.IsAlphanumeric(_registerInfo.UserInfo.Username) || _registerInfo.UserInfo.Username.Length < 5 || _registerInfo.UserInfo.Username.Length > 16)
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message = base.GetMessage("Login_InvalidUsernamePassword");
                msg.IsAbort = true;
            }
            else if (Validation.IsInjection(Encryption.Decrypt(_registerInfo.UserInfo.Password)) || Encryption.Decrypt(_registerInfo.UserInfo.Password).Length < 8 || Encryption.Decrypt(_registerInfo.UserInfo.Password).Length > 10)
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message = base.GetMessage("Login_InvalidUsernamePassword");
                msg.IsAbort = true;
            }
            else if (Validation.IsInjection(_registerInfo.Email))
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message = base.GetMessage("Reg_InvalidEmail");
                msg.IsAbort = true;
            }
            else if (Validation.IsInjection(_registerInfo.Phone))
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message = base.GetMessage("Reg_InvalidContact");
                msg.IsAbort = true;
            }
            else if (Validation.IsInjection(_registerInfo.CurrencyCode))
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message = base.GetMessage("Reg_MissingCurrency");
                msg.IsAbort = true;
            }
            else if (Validation.IsInjection(_registerInfo.Firstname))
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message = base.GetMessage("Reg_MissingFName");
                msg.IsAbort = true;
            }
            else if (Validation.IsInjection(_registerInfo.Lastname))
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message = base.GetMessage("Reg_MissingLName");
                msg.IsAbort = true;
            }
            else if (!DateTime.TryParse(_registerInfo.DateOfBirth.ToShortDateString(), out dtDob))
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message = base.GetMessage("Reg_InvalidDOB");
                msg.IsAbort = true;
            }

            msg.Id = Guid.NewGuid();
            msg.ProcessSerialId += 1;
            msg.Remark = string.Format("HiddenValues: {0}", _registerInfo.HiddenValues);

            AuditTrail.AppendLog(_registerInfo.UserInfo.Username, Constants.PageNames.RegisterPage,
                Constants.TaskNames.ParameterValidation, Constants.PageNames.ComponentName, Convert.ToString(msg.Code),
                msg.Message, string.Empty, string.Empty, msg.Remark,
                Convert.ToString(msg.ProcessSerialId), Convert.ToString(msg.Id), false);

            return msg;
        }

        private void CheckResult(ref ProcessCode process)
        {
            switch (process.Code)
            {
                case 0:
                    process.Message = base.GetMessage("Exception");
                    break;

                case 1:
                    process.Message = base.GetMessage("Reg_Success");
                    break;

                case 10:
                    process.Message = base.GetMessage("Reg_DuplicateUsername");
                    break;

                case 11:
                    process.Message = base.GetMessage("Reg_DuplicateEmail");
                    break;
                case 50:
                    process.Message = base.GetMessage("Reg_DuplicateContact");
                    break;
                default:
                    process.Message = base.GetMessage("Exception");
                    break;
            }

            LogRegister(process);
        }

        private void SetValues()
        {
            _ipHelper = new IpHelper();

            var headers = new ListOfValuesHelper().GetCountryInfo();

            _registerInfo.IpAddress = headers.Ip;
            _registerInfo.Country = headers.Country;
            _registerInfo.Permission = headers.Permission;

            _registerInfo.HiddenValues = string.Format("{0}|{1}|{2}|{3}", _registerInfo.Country, _ipHelper.DomainName, _registerInfo.IpAddress, _registerInfo.Permission);
            _registerInfo.ContactNumber = string.Format("{0}-{1}", _registerInfo.CountryCode, _registerInfo.Phone);

            switch (_registerInfo.Country.ToUpper())
            {
                case "MY":
                case "TH":
                case "VN":
                case "KH":
                    _registerInfo.OddsType = 1;
                    break;
                case "CN":
                    _registerInfo.OddsType = 2;
                    break;
                case "IN":
                case "KR":
                case "JP":
                case "AU":
                    _registerInfo.OddsType = 3;
                    break;
                case "ID":
                    _registerInfo.OddsType = 4;
                    break;

                default:
                    _registerInfo.OddsType = 3;
                    break;
            }


            var opSettings = new OperatorSettings(Settings.OperatorName);
            if (opSettings.Values.Get("DemoDomains").IndexOf(_ipHelper.DomainName) >= 0)
            {
                _registerInfo.IsTestAccount = true;
            }

            string affId;
            int intAffiliateId;
            if (string.IsNullOrEmpty(Common.GetSessionVariable("AffiliateId")))
            {
                affId = (string.IsNullOrEmpty(_registerInfo.AffiliateId) ? "0" : _registerInfo.AffiliateId);
            }
            else
                affId = Common.GetSessionVariable("AffiliateId");

            try
            {
                int.TryParse(affId, out intAffiliateId);
            }
            catch
            {
                intAffiliateId = 0;
            }

            _registerInfo.AffiliateId = Convert.ToString(intAffiliateId);
            _registerInfo.Address = _registerInfo.Country;
            _registerInfo.City = _registerInfo.Country;
            _registerInfo.Postal = "000000";
            _registerInfo.Gender = "M";
        }

        private void LogRegister(ProcessCode process)
        {
            process.Remark =
                string.Format(
                    "OperatorId: {0} | MemberCode: {1} | Email: {2} | Contact: {3} | Address: {4} | City: {5} | Postal: {6} | Country: {7} | Currency: {8} | Gender: {9} | OddsType: {10} | Language: {11} | Affiliate: {12} | ReferBy: {13} | IP: {14} | SignUpUrl: {15} | DeviceID: {16} | TestAccount: {17} | FName: {18} | LName: {19} | DOB: {20} | REMOTEIP: {21} | FORWARDEDIP: {22} | REQUESTERIP: {23} | AffiliateID: {24}",
                    base.OperatorId, _registerInfo.UserInfo.Username, _registerInfo.Email, _registerInfo.ContactNumber, _registerInfo.Address, _registerInfo.City, _registerInfo.Postal, 
                    _registerInfo.Country, _registerInfo.CurrencyCode, _registerInfo.Gender, _registerInfo.OddsType, _registerInfo.LanguageCode, _registerInfo.AffiliateId, _registerInfo.ReferBy,
                    _registerInfo.IpAddress, _registerInfo.SignUpUrl, base.DeviceId, _registerInfo.IsTestAccount, _registerInfo.Firstname, _registerInfo.Lastname, _registerInfo.DateOfBirth, 
                    _ipHelper.Remote, _ipHelper.Forwarded, _ipHelper.Requester, _registerInfo.AffiliateId);

            process.ProcessSerialId += 1;
            
            AuditTrail.AppendLog(_registerInfo.UserInfo.Username, Constants.PageNames.RegisterPage,
                Constants.TaskNames.RegistrationNew, Constants.PageNames.ComponentName, Convert.ToString(process.Code),
                process.Message, string.Empty, string.Empty, process.Remark,
                Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }
    }
}