using Models;
using System;
 using System.Data;
 using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Helpers
{
    /// <summary>
    /// Summary description for Members
    /// </summary>
    public class Members
    {
        public int CheckMemberSession(string memberSessionId = null, string password = null)
        {
            try
            {
                using (var svcInstance = new wsMemberMS1.memberWSSoapClient())
                {
                    var id = memberSessionId ?? commonVariables.CurrentMemberSessionId;
                    var dsMember = svcInstance.MemberSessionCheck(id, commonIp.UserIP);

                    if (dsMember != null)
                    {
                        if (dsMember.Tables[0].Rows.Count > 0)
                        {
                            SetSessions(dsMember.Tables[0], password);
                        }
                    }

                    return dsMember == null ? 0 : Convert.ToInt32(dsMember.Tables[0].Rows[0]["RETURN_VALUE"]);
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void SetSessions(DataTable dTable, string password)
        {
            MemberData(dTable);

            var memberSessionId = dTable.Rows[0]["memberSessionId"];
            var currencyCode = dTable.Columns["currencyCode"] != null ? dTable.Rows[0]["currencyCode"].ToString() : dTable.Rows[0]["currency"].ToString();
            commonVariables.SetSessionVariable("MemberSessionId", memberSessionId.ToString());
            commonVariables.SetSessionVariable("MemberId", dTable.Rows[0]["memberId"].ToString());
            commonVariables.SetSessionVariable("MemberCode", dTable.Rows[0]["memberCode"].ToString());
            commonVariables.SetSessionVariable("CountryCode", dTable.Rows[0]["countryCode"].ToString());
            commonVariables.SetSessionVariable("CurrencyCode", currencyCode);
            commonVariables.SetSessionVariable("LanguageCode", dTable.Rows[0]["languageCode"].ToString());
            commonVariables.SetSessionVariable("RiskId", dTable.Rows[0]["riskId"].ToString());
            commonVariables.SetSessionVariable("PaymentGroup", dTable.Rows[0]["paymentGroup"].ToString());
            commonVariables.SetSessionVariable("PartialSignup", dTable.Rows[0]["partialSignup"].ToString());
            commonVariables.SetSessionVariable("ResetPassword", dTable.Rows[0]["resetPassword"].ToString());

            if (dTable.Columns["Lastname"] != null)
            commonVariables.SetSessionVariable("MemberName", Convert.ToString(dTable.Rows[0]["Lastname"]) + Convert.ToString(dTable.Rows[0]["Firstname"]));

            commonCookie.CookieS = Convert.ToString(memberSessionId);
            commonCookie.CookieG = Convert.ToString(memberSessionId);
            commonCookie.CookieCurrency = currencyCode;

            if (password != null)
                commonCookie.CookiePalazzo = password;
        }

        public void MemberData(DataTable dTable)
        {
            var user = new MemberSession.UserSessionInfo
            {
                CurrentSessionId = dTable.Rows[0]["memberSessionId"].ToString(),
                MemberId = dTable.Rows[0]["memberId"].ToString(),
                MemberCode = dTable.Rows[0]["memberCode"].ToString()
            };

            var serializer = new JavaScriptSerializer();
            var userData = serializer.Serialize(user);
            var ticket = new FormsAuthenticationTicket(1, user.MemberCode, DateTime.Now, DateTime.Now.AddDays(1), true,
                userData, FormsAuthentication.FormsCookiePath);
            var encTicket = FormsAuthentication.Encrypt(ticket);
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));
        }

        public MemberSession.UserSessionInfo MemberData()
        {
            var userData = new MemberSession.UserSessionInfo();
            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                if (!string.IsNullOrWhiteSpace(authCookie.Value))
                {
                    var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null)
                    {
                        var serializer = new JavaScriptSerializer();
                        userData = serializer.Deserialize<MemberSession.UserSessionInfo>(ticket.UserData);
                    }
                }
            }

            //@todo will remove soon if session dependency is no more
            if (string.IsNullOrEmpty(userData.CurrentSessionId))
            {
                userData.CurrentSessionId = commonVariables.GetSessionVariable("MemberSessionId");
            }

            if (string.IsNullOrEmpty(userData.MemberCode))
            {
                userData.MemberCode = commonVariables.GetSessionVariable("MemberCode");
            }

            if (string.IsNullOrEmpty(userData.MemberId))
            {
                userData.MemberId = commonVariables.GetSessionVariable("MemberId");
            }

            return userData;
        }
    }
}