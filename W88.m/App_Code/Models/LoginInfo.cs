
namespace Models
{
    /// <summary>
    /// Summary description for LoginInfo
    /// </summary>

    public class LoginInfo : CommonProperties
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Captcha { get; set; }

        public string SessionCaptcha = commonVariables.GetSessionVariable("vCode");

        public string PageName = "Login";
    }

    public class ForgotInfo
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}