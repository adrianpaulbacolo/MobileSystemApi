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
                    userInfo.MemberName = dsMemberCheck.Tables[0].Rows[0][Constants.VarNames.Lastname].ToString() + dsMemberCheck.Tables[0].Rows[0][Constants.VarNames.Firstname].ToString();
                }

                int returnCode;
                userInfo.Status.ReturnMessage = GetSessionCheckMsg(userInfo.Status.ReturnValue, out returnCode);
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

                case 20:
                    translateCode = (int)Constants.StatusCode.MemberVip;
                    return "Member is in VIP club";

                default:
                    translateCode = (int)Constants.StatusCode.SessionExpired;
                    return GetMessage("SessionExpired");
            }

        }

        public string ParseToken(HttpRequestMessage request)
        {
            IEnumerable<string> tokens;
            request.Headers.TryGetValues(Constants.VarNames.Token, out tokens);
            var enumurable = tokens as string[] ?? tokens.ToArray();
            return enumurable.FirstOrDefault();
        }
    }
}