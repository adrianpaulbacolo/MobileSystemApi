using System;
using System.Data;
using System.Threading.Tasks;
using W88.BusinessLogic.Shared.Models;
using W88.Rewards.BusinessLogic.Accounts.Models;
using W88.Utilities.Extensions;
using W88.Utilities.Geo;

namespace W88.Rewards.BusinessLogic.Accounts.Helpers
{
    public class Members : W88.BusinessLogic.Accounts.Helpers.Members
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

        public async Task<ProcessCode> MembersSessionCheck(string token)
        {
            using (var svcInstance = new W88.WebRef.wsMemberMS1.memberWSSoapClient())
            {
                var dsMemberCheck = await svcInstance.MemberSessionCheckAsync(token, new IpHelper().User);

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