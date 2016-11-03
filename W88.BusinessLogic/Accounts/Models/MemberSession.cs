
namespace W88.BusinessLogic.Accounts.Models
{
    public class MemberSession
    {
        public string FirstName = string.Empty;

        public string CurrencyCode = string.Empty;

        public string PartialSignup = string.Empty;

        public bool ResetPassword{ get; set; }

        public string Balance { get; set; }

        public string LanguageCode = string.Empty;

        public string CountryCode = string.Empty;

        public string Token = string.Empty;

        public string MemberId = string.Empty;

        public string RiskId = string.Empty;
    }

    public class UserSessionInfo 
    {

        public string CurrentSessionId = string.Empty;

        public long MemberId = 0;

        public string MemberCode = string.Empty;

        public string AccountName = string.Empty;

        public string MemberName = string.Empty;

        public string CurrencyCode = string.Empty;

        public string CountryCode = string.Empty;

        public string PaymentGroup = string.Empty;

        public string Token = string.Empty;

        public TokenStatus Status = new TokenStatus();

        public string LanguageCode = string.Empty;

    }

    public class TokenStatus
    {
        public int ReturnValue = 0;

        public string ReturnMessage = string.Empty;
    }
}
