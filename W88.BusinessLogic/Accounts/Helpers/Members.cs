using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.SqlServer.Server;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Funds.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities.Geo;
using W88.Utilities.Extensions;
using W88.Utilities.Security;
using W88.Utilities.Log.Helpers;
using W88.WebRef.svcPayMember;
using W88.WebRef.wsMemberMS1;

namespace W88.BusinessLogic.Accounts.Helpers
{
    /// <summary>
    /// Summary description for Members
    /// </summary>
    public class Members : BaseHelper
    {
        private readonly ServerHelpers _serverHelper = new ServerHelpers();

        public MemberSession GetData(DataTable dTable)
        {
            var memberSession = new MemberSession
            {
                Token = dTable.Rows[0]["memberSessionId"].ToString(),
                MemberId = dTable.Rows[0]["MemberId"].ToString(),
                CurrencyCode = dTable.Columns["currencyCode"] != null ? dTable.Rows[0]["currencyCode"].ToString() : dTable.Rows[0]["currency"].ToString(),
                CountryCode = dTable.Columns["countryCode"] != null ? dTable.Rows[0]["countryCode"].ToString() : string.Empty,
                FirstName = dTable.Columns["FirstName"] != null ? Convert.ToString(dTable.Rows[0]["Firstname"]) : string.Empty,
                LanguageCode = dTable.Columns["languageCode"] != null ? Convert.ToString(dTable.Rows[0]["languageCode"].ToString()) : string.Empty,
                PartialSignup = dTable.Columns["partialSignup"] != null ? Convert.ToString(dTable.Rows[0]["partialSignup"].ToString()) : string.Empty,
                Balance = dTable.Columns["MemberBalance"] != null ? dTable.Rows[0]["MemberBalance"].ToString() : "0.00",
                ResetPassword = Convert.ToBoolean(dTable.Rows[0]["resetPassword"].ToString()),
                RiskId = dTable.Rows[0]["riskId"].ToString()
            };

            memberSession.Balance = Convert.ToDecimal(memberSession.Balance).ToW88StringFormat();
            return memberSession;
        }

        public async Task<List<WalletInfoResponse>> GetWalletBalancesAsync(UserSessionInfo userInfo)
        {
            using (var svcInstance = new WebRef.svcPayMember.MemberClient())
            {
                var wallets = new List<WalletInfoResponse>();
                var balances = await svcInstance.getBalancesAsync(base.OperatorId.ToString(), base.SiteUrl, userInfo.MemberCode);
                var xe = XElement.Parse(balances);
                var balanceSection = xe.Elements("balance");

                ICollection<KeyValuePair<int, string>> balanceCollection = new Collection<KeyValuePair<int, string>>();
                foreach (var pair in balanceSection)
                {
                    var value = (pair.Value == "*" || pair.Value == "-")
                        ? "0.00"
                        : Convert.ToString(Convert.ToDecimal(pair.Value));
                    balanceCollection.Add(new KeyValuePair<int, string>(Convert.ToInt16(pair.Attribute("id").Value),
                        value));
                }

                if (balanceCollection.Any())
                {
                    var config = CultureHelpers.AppData.GetLocale_i18n_Resource("Shared/Wallets", false);
                    var walletSettings = Utilities.Common.ParseJsonString<WalletInfo>(config, "Wallets").OrderBy(info => info.OrderBy);

                    foreach (var c in walletSettings)
                    {
                        if (!c.Enabled) continue;

                        var id = c.Id;
                        var firstValue = balanceCollection.Where(pair => pair.Key == id).Select(pair => pair.Value).ToList();

                        if (firstValue.Any())
                            c.Balance = Convert.ToDecimal(firstValue[0]).ToW88StringFormat();

                        var item = new WalletInfoResponse
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Balance = c.Balance
                        };

                        if (!string.IsNullOrWhiteSpace(c.CurrAllowOnly))
                        {
                            if (c.CurrAllowOnly.Contains(userInfo.CurrencyCode))
                                wallets.Add(item);
                        }
                        else if (!string.IsNullOrWhiteSpace(c.CurrRestriction))
                        {
                            if (!c.CurrRestriction.Contains(userInfo.CurrencyCode))
                                wallets.Add(item);
                        }
                        else
                        {
                            wallets.Add(item);
                        }
                    }
                }

                return wallets;
            }
        }

        public async Task<WalletInfoResponse> GetWalletBalanceAsync(int walletId, UserSessionInfo userInfo)
        {
            using (var svcInstance = new WebRef.svcPayMember.MemberClient())
            {
                var request = new WebRef.svcPayMember.getWalletBalanceRequest(base.OperatorId.ToString(),
                    base.SiteUrl, userInfo.MemberCode, Convert.ToString(walletId));
                var balance = await svcInstance.getWalletBalanceAsync(request);

                var config = CultureHelpers.AppData.GetJsonRootResource("Shared/Wallets");
                var wallets = base.GetListOfValues<WalletInfo>("Shared/Wallets", "Wallets", false).OrderBy(x => x.OrderBy);

                var info = wallets.Where(walletInfo => walletInfo.Id == walletId).ToList();
                var wallet = new WalletInfoResponse
                {
                    Id = info[0].Id,
                    Balance = Convert.ToDecimal(balance.getWalletBalanceResult).ToW88StringFormat(),
                    Name = info[0].Name,
                    CurrencyLabel = string.IsNullOrWhiteSpace(info[0].CurrencyLabel) ? userInfo.CurrencyCode.ToUpper() : info[0].CurrencyLabel
                };

                return wallet;
            }
        }

        public async Task<decimal> GetRewardsPoints(UserSessionInfo userInfo)
        {
            using (var svc = new WebRef.RewardsServices.RewardsServicesClient())
            {
                var total = 0;
                var claim = 0;
                var cart = 0;

                var ds = await svc.getRedemptionDetailAsync(base.OperatorId.ToString(), userInfo.MemberCode);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    total = Convert.ToInt32(ds.Tables[0].Rows[0][0]);

                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        claim = int.Parse(ds.Tables[1].Rows[0][0].ToString());
                    }

                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        cart = int.Parse(ds.Tables[2].Rows[0][0].ToString());
                    }

                    claim = claim + cart;
                }

                return total - claim;
            }
        }

        public async Task<ProcessCode> MembersSessionCheck(string token)
        {
            using (var svcInstance = new WebRef.wsMemberMS1.memberWSSoapClient())
            {
                var dsMemberCheck = await svcInstance.MemberSessionCheckAsync(token, new IpHelper().User);

                var process = new ProcessCode
                {
                    Code = Convert.ToInt32(dsMemberCheck.Tables[0].Rows[0]["RETURN_VALUE"])
                };

                int returnCode;
                process.Message = GetSessionCheckMsg(process.Code, out returnCode);

                if (returnCode == 1)
                {
                    var member = new Members();
                    process.Data = member.GetData(dsMemberCheck.Tables[0]);
                    process.Message = string.Empty;
                }

                return process;
            }
        }

        public async Task<UserSessionInfo> GetMemberInfo(string token)
        {
            using (var sessionCheck = new WebRef.wsMemberMS1.memberWSSoapClient())
            {
                var dsMemberCheck = await sessionCheck.MemberSessionCheckAsync(token, new IpHelper().User);

                var userInfo = new UserSessionInfo();
                if (dsMemberCheck.Tables.Count <= 0) return userInfo;

                if (dsMemberCheck.Tables[0].Columns[Constants.VarNames.MemberCode] != null)
                {
                    userInfo.MemberCode = dsMemberCheck.Tables[0].Rows[0][Constants.VarNames.MemberCode].ToString();
                    userInfo.CurrencyCode = dsMemberCheck.Tables[0].Rows[0][Constants.VarNames.CurrencyCode].ToString();
                    userInfo.CurrentSessionId = dsMemberCheck.Tables[0].Rows[0][Constants.VarNames.MemberSessionId].ToString();
                    userInfo.Token = userInfo.CurrentSessionId;
                    userInfo.MemberId = Convert.ToInt64(dsMemberCheck.Tables[0].Rows[0][Constants.VarNames.MemberId]);
                    userInfo.CountryCode = dsMemberCheck.Tables[0].Rows[0][Constants.VarNames.CountryCode].ToString();
                    userInfo.PaymentGroup = dsMemberCheck.Tables[0].Rows[0][Constants.VarNames.PaymentGroup].ToString();

                    string lastName = dsMemberCheck.Tables[0].Rows[0][Constants.VarNames.Lastname].ToString();
                    string firstName = dsMemberCheck.Tables[0].Rows[0][Constants.VarNames.Firstname].ToString();

                    userInfo.MemberName = lastName + firstName;
                    userInfo.AccountName = SetAccountName(userInfo.CurrencyCode, lastName, firstName);
                }

                int returnCode;
                userInfo.Status.ReturnMessage = GetSessionCheckMsg(Convert.ToInt32(dsMemberCheck.Tables[0].Rows[0][Constants.VarNames.ReturnValue]), out returnCode);
                userInfo.Status.ReturnValue = returnCode;

                return userInfo;
            }
        }

        private string GetSessionCheckMsg(int returnValue, out int translateCode)
        {
            switch (returnValue)
            {
                case 0:
                    translateCode = (int)Constants.StatusCode.Error;
                    return GetMessage("Exception");

                case 1:
                    translateCode = (int)Constants.StatusCode.Success;
                    return string.Empty;

                case 10:
                    translateCode = (int)Constants.StatusCode.NotLogin;
                    return "Member is not login";

                case 13:
                    translateCode = (int)Constants.StatusCode.MultipleLogin;
                    return "Member is login at another session. (Multiple Login)";

                default:
                    translateCode = (int)Constants.StatusCode.SessionExpired;
                    return GetMessage("SessionExpired");
            }

        }
        private string SetAccountName(string currencyCode, string lastName, string firstName)
        {
            string accountName = string.Empty;

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                if (string.IsNullOrWhiteSpace(lastName))
                    accountName = firstName;
                else
                {
                    switch (currencyCode)
                    {
                        case "RMB":
                        case "KRW":
                            accountName = lastName + firstName;
                            break;
                        case "VND":
                        case "JPY":
                            accountName = lastName + " " + firstName;
                            break;
                        case "USD":
                        case "MYR":
                        case "IDR":
                            accountName = firstName + " " + lastName;
                            break;
                        default:
                            break;
                    }
                }
            }

            return accountName;
        }

        public string ParseToken(HttpRequestMessage request)
        {
            IEnumerable<string> tokens;
            request.Headers.TryGetValues(Constants.VarNames.Token, out tokens);
            var enumurable = tokens as string[] ?? tokens.ToArray();
            return enumurable.FirstOrDefault();
        }

        public async Task<ProcessCode> CreateBankDetails(UserSessionInfo userInfo, BankDetails details)
        {
            var process = new ProcessCode() { Id = Guid.NewGuid() };
            process = ValidateBankDetails(userInfo.MemberCode, details, await new ListOfValuesHelper().GetMemberBankAccounts(userInfo));

            if (!process.IsAbort)
            {
                using (var client = new MemberClient())
                {
                    var response = await client.createBankDetailsAsync(Convert.ToInt64(OperatorId), userInfo.MemberCode, userInfo.CurrencyCode,
                        details.BankBranch, details.BankAddress, details.AccountName, details.AccountNumber, details.Bank.Value, details.Bank.Text, details.BankName, details.IsPreferred);

                    process.IsSuccess = response;

                    if (process.IsSuccess)
                    {
                        process.Code = (int)Constants.StatusCode.Success;
                        process.Message = base.GetMessage("Pay_Success");
                    }
                    else
                    {
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Message = base.GetMessage("Pay_Fail");
                    }

                    process.ProcessSerialId += 1;
                    process.Remark = string.Format("IsSuccess: {0} | BankCode: {1} | BankName: {2}  | BankBranch: {3} | BankAddress: {4} | AccountName: {5} | AccountNumber: {6} | IsPreferred: {7}",
                        process.IsSuccess, details.Bank.Value, details.BankName, details.BankBranch, details.BankAddress, details.AccountName, details.AccountNumber, details.IsPreferred);

                    AuditTrail.AppendLog(userInfo.MemberCode, Constants.PageNames.FundsPage,
                        Constants.TaskNames.CreateBankDetails, Constants.PageNames.ComponentName,
                        Convert.ToString(process.Code), process.Message, string.Empty, string.Empty, process.Remark,
                        Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
                }
            }

            return process;
        }

        private ProcessCode ValidateBankDetails(string memberCode, BankDetails details, List<LOV> banks)
        {
            var process = new ProcessCode() { Message = new List<string>() };

            if (details.Bank == null)
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_MissingBank"));
                process.IsAbort = true;
            }
            else
            {
                if (Validation.IsNumeric(details.Bank.Text) || Validation.IsNumeric(details.Bank.Value))
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_MissingBank"));
                    process.IsAbort = true;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(details.Bank.Text) || string.IsNullOrWhiteSpace(details.Bank.Value))
                    {
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Message.Add(base.GetMessage("Pay_MissingBank"));
                        process.IsAbort = true;
                    }
                    else
                    {
                        if (details.Bank.Text.Contains("other") || details.Bank.Value.Equals("other", StringComparison.OrdinalIgnoreCase))
                        {
                            if (string.IsNullOrWhiteSpace(details.BankName))
                            {
                                process.Code = (int)Constants.StatusCode.Error;
                                process.Message.Add(base.GetMessage("Pay_MissingBankName"));
                                process.IsAbort = true;
                            }

                            if (Validation.IsInjection(details.BankName))
                            {
                                process.Code = (int)Constants.StatusCode.Error;
                                process.Message.Add(base.GetMessage("Pay_InvalidBankName"));
                                process.IsAbort = true;
                            }
                        }
                    }

                    if (Validation.IsInjection(details.Bank.Text) || Validation.IsInjection(details.Bank.Value))
                    {
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Message.Add(base.GetMessage("Pay_MissingBank"));
                        process.IsAbort = true;
                    }

                    if (!banks.Any(b => b.Text == details.Bank.Text && b.Value == details.Bank.Value))
                    {
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Message.Add(base.GetMessage("Pay_MissingBank"));
                        process.IsAbort = true;
                    }
                }
            }

            if (string.IsNullOrWhiteSpace(details.BankBranch))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_MissingBankBranch"));
                process.IsAbort = true;
            }

            if (Validation.IsInjection(details.BankBranch))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_InvalidBankBranch"));
                process.IsAbort = true;
            }

            if (string.IsNullOrWhiteSpace(details.BankAddress))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_MissingBankAddress"));
                process.IsAbort = true;
            }

            if (Validation.IsInjection(details.BankAddress))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_InvalidBankAddress"));
                process.IsAbort = true;
            }

            if (string.IsNullOrWhiteSpace(details.AccountName))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_MissingAccountName"));
                process.IsAbort = true;
            }

            if (Validation.IsInjection(details.AccountName))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_InvalidAccountName"));
                process.IsAbort = true;
            }

            if (string.IsNullOrWhiteSpace(details.AccountNumber))
            {
                process.Code = (int)Constants.StatusCode.Error;
                process.Message.Add(base.GetMessage("Pay_MissingAccountNumber"));
                process.IsAbort = true;
            }
            else
            {
                if (!Validation.IsNumeric(details.AccountNumber))
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_InvalidAccountNumber"));
                    process.IsAbort = true;
                }

                if (Validation.IsInjection(details.AccountNumber))
                {
                    process.Code = (int)Constants.StatusCode.Error;
                    process.Message.Add(base.GetMessage("Pay_InvalidAccountNumber"));
                    process.IsAbort = true;
                }
            }

            process.ProcessSerialId += 1;

            process.Remark = string.Format("IsValid: {0}", !process.IsAbort);

            AuditTrail.AppendLog(memberCode, Constants.PageNames.FundsPage,
                Constants.TaskNames.ParameterValidation, Constants.PageNames.ComponentName,
                Convert.ToString(process.Code), string.Join(" | ", process.Message), string.Empty, string.Empty, process.Remark,
                Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);

            return process;
        }

        public async Task<ProcessCode> GetBankDetails(UserSessionInfo userInfo)
        {
            var process = new ProcessCode();

            using (var client = new MemberClient())
            {
                var response = await client.getBankDetailsAsync(Convert.ToInt64(OperatorId), userInfo.MemberCode);

                if (response != null)
                {
                    if (response.Rows.Count > 0)
                    {
                        DataRow dr = response.Rows[0];
                        process.Code = (int)Constants.StatusCode.Success;

                        var banks = await new ListOfValuesHelper().GetMemberBankAccounts(userInfo);
                        var bank = banks.FirstOrDefault(b => b.Value == dr["bankCode"].ToString());

                        process.Data = new BankDetails()
                        {
                            Bank = bank,
                            BankName = dr["bankNameNative"].ToString(),
                            BankBranch = dr["bankBranch"].ToString(),
                            BankAddress = dr["bankAddress"].ToString(),
                            AccountName = dr["bankAccountName"].ToString(),
                            AccountNumber = dr["bankAccountNumber"].ToString(),
                            IsPreferred = Convert.ToBoolean(dr["preferred"]),
                        };
                    }
                }
            }

            return process;
        }

        public async Task<ProcessCode> ChangePassword(ChangePasswordInfo changePasswordInfo)
        {
            var process = new ProcessCode();
            var memberId = changePasswordInfo.MemberId;
            var password = changePasswordInfo.Password;
            var newPassword = changePasswordInfo.NewPassword;
            var confirmPassword = changePasswordInfo.ConfirmPassword;

            if (string.IsNullOrEmpty(password))
            {
                process.ProcessSerialId += 1;
                process.Code = (int)Constants.StatusCode.Error;
                process.Message = GetMessage("ChangePassword_EnterCurrent");
                LogProcess(process, memberId);
                return process;
            }

            if (string.IsNullOrEmpty(newPassword))
            {
                process.ProcessSerialId += 1;
                process.Code = (int)Constants.StatusCode.Error;
                process.Message = GetMessage("ChangePassword_EnterNew");
                LogProcess(process, memberId);
                return process;
            }

            if (string.IsNullOrEmpty(confirmPassword))
            {
                process.ProcessSerialId += 1;
                process.Code = (int)Constants.StatusCode.Error;
                process.Message = GetMessage("ChangePassword_EnterConfirm");
                LogProcess(process, memberId);
                return process;
            }

            if (!Encryption.Decrypt(confirmPassword).Equals(Encryption.Decrypt(newPassword)))
            {
                process.ProcessSerialId += 1;
                process.Code = (int)Constants.StatusCode.Error;
                process.Message = GetMessage("ChangePassword_Mismatch");
                LogProcess(process, memberId);
                return process;
            }

            if (Validation.IsInjection(Encryption.Decrypt(password))
                || Validation.IsInjection(Encryption.Decrypt(newPassword))
                || Validation.IsInjection(Encryption.Decrypt(confirmPassword)))
            {
                process.ProcessSerialId += 1;
                process.Code = (int)Constants.StatusCode.Error;
                process.Message = GetMessage("ChangePassword_Invalid");
                LogProcess(process, memberId);
                return process;
            }

            using (var client = new memberWSSoapClient())
            {

                process.Id = Guid.NewGuid();
                process.ProcessSerialId += 1;

                var result = await client.MemberChangePasswordAsync(long.Parse(changePasswordInfo.MemberId),
                    changePasswordInfo.Password, changePasswordInfo.NewPassword);

                switch (result)
                {
                    case 1:
                        process.Code = (int)Constants.StatusCode.Success;
                        process.Message = GetMessage("ChangePassword_Success");
                        break;
                    case 10:
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Message = GetMessage("ChangePassword_Fail");
                        break;
                    case 11:
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Message = GetMessage("ChangePassword_Invalid");
                        break;
                    default:
                        process.Code = (int)Constants.StatusCode.Error;
                        process.Message = GetMessage("Exception");
                        break;
                }

                LogProcess(process, memberId);
                return process;
            }
        }

        private void LogProcess(ProcessCode process, string memberCode)
        {
            AuditTrail.AppendLog(memberCode, Constants.PageNames.ChangePasswordPage,
                Constants.TaskNames.ChangePassword, Constants.PageNames.ComponentName,
                Convert.ToString(process.Code), string.Join(" | ", process.Message), string.Empty, string.Empty, process.Remark,
                Convert.ToString(process.ProcessSerialId), Convert.ToString(process.Id), false);
        }
    }
}