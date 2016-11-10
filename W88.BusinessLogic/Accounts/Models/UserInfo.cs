using W88.Utilities;
using W88.Utilities.Constant;

namespace W88.BusinessLogic.Accounts.Models
{
    public class UserInfo 
    {
        private string _password;

        public string Username { get; set; }

        public string Password
        {
            get { return _password; }
            set { _password = Encryption.Encrypt(EncryptionType.RjnD, value); }
        }
    }
}
