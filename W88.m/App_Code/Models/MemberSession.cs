using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public class MemberSession
    {
        public class UserSessionInfo
        {

            public string CurrentSessionId = string.Empty;

            public string MemberId = string.Empty;

            public string MemberCode = string.Empty;

            public string RiskId = string.Empty;

            public string CountryCode = string.Empty;

            public bool IsTestAccount = false;

        }
    }
}