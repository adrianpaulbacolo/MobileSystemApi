using System;
using System.Data;
using System.Globalization;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities.Extensions;
using W88.Utilities.Geo;
using MemberSession = W88.Rewards.BusinessLogic.Accounts.Models.MemberSession;

namespace W88.Rewards.BusinessLogic.Accounts.Helpers
{
    public class Members : BaseHelper
    {
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

        public UserSessionInfo GetMemberInfo(string token)
        {
            using (var sessionCheck = new WebRef.wsMemberMS1.memberWSSoapClient())
            {
                var dsMemberCheck = sessionCheck.MemberSessionCheck(token, new IpHelper().User);

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

                userInfo.Status.ReturnValue = Convert.ToInt32(dsMemberCheck.Tables[0].Rows[0][Constants.VarNames.ReturnValue]);
                userInfo.Status.ReturnMessage = GetSessionCheckMsg(userInfo.Status.ReturnValue);

                return userInfo;
            }
        }

        public decimal GetRewardsPoints(UserSessionInfo userInfo)
        {
            using (var svc = new WebRef.RewardsServices.RewardsServicesClient())
            {
                var total = 0;
                var claim = 0;
                var cart = 0;

                var ds = svc.getRedemptionDetail(base.OperatorId.ToString(CultureInfo.InvariantCulture), userInfo.MemberCode);

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

        public ProcessCode MembersSessionCheck(string token)
        {
            using (var svcInstance = new W88.WebRef.wsMemberMS1.memberWSSoapClient())
            {
                var dsMemberCheck = svcInstance.MemberSessionCheck(token, new IpHelper().User);

                var process = new ProcessCode
                {
                    Code = Convert.ToInt32(dsMemberCheck.Tables[0].Rows[0]["RETURN_VALUE"])
                };

                process.Message = GetSessionCheckMsg(process.Code);

                if (process.Code == 1)
                {
                    var member = new Members();
                    process.Data = member.GetData(dsMemberCheck.Tables[0]);
                    process.Message = string.Empty;
                }

                return process;
            }
        }

        private string GetSessionCheckMsg(int returnValue)
        {
            switch (returnValue)
            {
                case 0:
                    return GetMessage("Exception");
                case 1:
                    return string.Empty;
                case 10:
                    return "Member is not login";
                case 13:
                    return "Member is login at another session. (Multiple Login)";
                case 20:
                    return "Member is in VIP club";
                default:
                    return GetMessage("SessionExpired");
            }
        }
    }
}