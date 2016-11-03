namespace W88.BusinessLogic.Shared.Models
{
    public class Constants
    {
        public struct TaskNames
        {
            public const string ParameterValidation = "ParameterValidation";
            public const string System = "System";
            public const string MemberSignin = "MemberSignin";
            public const string RegistrationNew = "RegistrationNew";
            public const string MembersSessionCheck = "MembersSessionCheck";
            public const string CreateBOTransaction = "CreateBOTransaction";
            public const string CreateVendorBOTransaction = "CreateVendorBOTransaction";
            public const string CreateVendorParameter = "CreateVendorParameter";
            public const string ProcessDummyUrl = "ProcessDummyUrl";
            public const string CancelPendingWithdrawal = "CancelPendingWithdrawal";
            public const string SendMail = "SendMail";
            public const string RedeemRewards = "RedeemRewards";
            public const string SearchCatalogue = "SearchCatalogue";
            public const string VendorRedirection = "VendorRedirection";
            public const string CreateBankDetails = "CreateBankDetails";
            public const string ChangePassword = "ChangePassword";
        }

        public struct PageNames
        {
            public const string ComponentName = "W88.BusinessLogic";
            public const string RegisterPage = "RegisterPage";
            public const string FundsPage = "FundsPage";
            public const string LoginPage = "LoginPage";
            public const string WalletsApi = "WalletsApi";
            public const string MailApi = "MailApi";
            public const string RedeemPage = "RedeemPage";
            public const string CataloguePage = "CataloguePage";
            public const string ChangePasswordPage = "ChangePasswordPage";
        }

        public struct VarNames
        {
            public const string Token = "token";

            public const string MemberId = "memberId";
            
            public const string MemberCode = "memberCode";

            public const string Lastname = "Lastname";

            public const string Firstname = "Firstname";

            public const string CurrencyCode = "CurrencyCode";

            public const string CountryCode = "countryCode";

            public const string PaymentGroup = "paymentGroup";

            public const string MemberSessionId = "MemberSessionId";

            public const string Unlimited = "Unlimited";

            public const string LanguageCode = "LanguageCode";

            public const string MessageKey = "Messages";

            public const string RedirectUrl = "redirectUrl";

            public const string PaymentPrivateKey = "PaymentPrivateKey";

            public const string NextPayPrivateKey = "privateKey_nextPay";

            public const string ReturnValue = "Return_Value";

            public const string DefaultLanguage = "DefaultSelectedLanguage";

            public const string OtherBankValue = "OTHER";

            public const string RiskId = "riskId";	
        }

        public enum PaymentTransactionType
        {
            Deposit = 1,
            Withdrawal = 2
        }

        public enum StatusCode
        {
            SessionExpired = -7,
            MultipleLogin = -6,
            WithPendingWithdrawal = -5,
            NotImplemented = -4,
            TokenNotFound = -3,
            NotLogin = -2,
            Error = -1,
            Success = 1,
            ResetPassword = 2,
            MemberVip = 3
        }

        public enum ProductStatus
        {
            Valid = 1,
            VipOnly = 2,
        }
    }
}