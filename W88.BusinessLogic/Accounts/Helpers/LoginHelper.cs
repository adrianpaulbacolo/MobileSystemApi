using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities;
using W88.Utilities.Constant;
using W88.Utilities.Geo;
using W88.Utilities.Log.Helpers;
using W88.Utilities.Security;
using W88.WebRef.wsMemberMS1;

namespace W88.BusinessLogic.Accounts.Helpers
{
    public sealed class LoginHelper : BaseHelper
    {
        private void CheckResult(ref ProcessCode process, LoginInfo loginInfo)
        {
            switch (process.Code)
            {
                case 0:
                    process.Message = base.GetMessage("Exception");
                    break;

                case 1:
                    process.Message = Constants.StatusCode.Success.ToString();
                    break;

                case 21:
                case 23:
                    process.Message = base.GetMessage("Login_InvalidUsernamePassword");
                    break;

                case 22:
                    process.Message = base.GetMessage("Login_InactiveAccount");
                    break;
            }

            process.ProcessSerialId += 1;
            process.Remark = string.Format("MemberCode: {0} | IP: {1} ", loginInfo.UserInfo.Username, new IpHelper().User);

            AuditTrail.AppendLog(loginInfo.UserInfo.Username, Constants.PageNames.LoginPage,
                Constants.TaskNames.MemberSignin, Constants.PageNames.ComponentName, Convert.ToString(process.Code),
                process.Message, string.Empty, string.Empty,
                process.Remark, Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }

        private ProcessCode ValidateData(LoginInfo loginInfo)
        {
            var msg = new ProcessCode { Code = (int)Constants.StatusCode.Success, Message = new List<string>() };

            if (string.IsNullOrEmpty(loginInfo.UserInfo.Username))
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message.Add(base.GetMessage("Login_MissingUsername"));
                msg.IsAbort = true;

                loginInfo.UserInfo.Username = string.Empty;
            }
            else if (Validation.IsInjection(loginInfo.UserInfo.Username))
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message.Add(base.GetMessage("Login_InvalidUsernamePassword"));
                msg.IsAbort = true;
            }

            if (string.IsNullOrEmpty(loginInfo.UserInfo.Password))
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message.Add(base.GetMessage("Login_MissingPassword"));
                msg.IsAbort = true;
            }
            else if (Validation.IsInjection(Encryption.Decrypt(EncryptionType.RjnD, loginInfo.UserInfo.Password)))
            {
                msg.Code = (int)Constants.StatusCode.Error;
                msg.Message.Add(base.GetMessage("Login_InvalidUsernamePassword"));
                msg.IsAbort = true;
            }

            msg.Id = Guid.NewGuid();
            msg.ProcessSerialId += 1;

            AuditTrail.AppendLog(loginInfo.UserInfo.Username, Constants.PageNames.LoginPage,
                Constants.TaskNames.ParameterValidation, Constants.PageNames.ComponentName, Convert.ToString(msg.Code),
                string.Join(" | ", msg.Message), string.Empty, string.Empty, msg.Remark,
                Convert.ToString(msg.ProcessSerialId), Convert.ToString(msg.Id), false);

            return msg;
        }

        public async Task<ProcessCode> Login(LoginInfo loginInfo)
        {
            var process = this.ValidateData(loginInfo);

            if (!process.IsAbort)
            {
                using (var svcInstance = new memberWSSoapClient())
                {
                    var dsSignin =
                        await svcInstance.MemberSigninAsync(base.OperatorId, loginInfo.UserInfo.Username,
                            loginInfo.UserInfo.Password,
                            base.SiteUrl, new IpHelper().User, base.DeviceId);

                    if (dsSignin.Tables[0].Rows.Count > 0)
                    {
                        process.Code = Convert.ToInt32(dsSignin.Tables[0].Rows[0]["RETURN_VALUE"]);
                        CheckResult(ref process, loginInfo);

                        if (process.Code == 1)
                        {
                            var member = await new Members().MembersSessionCheck(dsSignin.Tables[0].Rows[0]["memberSessionId"].ToString());
                            process.Code = member.Code;
                            process.Data = member.Data;
                            process.Message = member.Message;

                            if (process.Code != 1)
                            {
                                process.ProcessSerialId += 1;

                                AuditTrail.AppendLog(loginInfo.UserInfo.Username, Constants.PageNames.LoginPage,
                                    Constants.TaskNames.MembersSessionCheck, Constants.PageNames.ComponentName, Convert.ToString(process.Code),
                                    process.Message, string.Empty, string.Empty, process.Remark,
                                    Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
                            }
                        }
                    }
                }
            }

            return process;
        }

        public async Task<ProcessCode> Account(string sessionId)
        {
            var response = new ProcessCode { Code = (int)Constants.StatusCode.Success, Message = "" };

            var member = await new Members().MembersSessionCheck(sessionId);
            response.Code = member.Code;
            response.Data = member.Data;
            response.Message = member.Message;

            return response;

        }


        public async Task<ProcessCode> Logout(string memberId)
        {
            using (var svcInstance = new memberWSSoapClient())
            {
                var resp = await svcInstance.MemberSignoutAsync(memberId);

                var msg = resp != (int) Constants.StatusCode.Success
                    ? Constants.StatusCode.Error.ToString()
                    : Constants.StatusCode.Success.ToString();
                var process = new ProcessCode
                {
                    Code = resp,
                    Message = msg
                };

                return process;
            }
        }
    }
}
