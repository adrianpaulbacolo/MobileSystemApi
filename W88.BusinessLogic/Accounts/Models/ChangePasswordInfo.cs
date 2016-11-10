
using W88.BusinessLogic.Shared.Helpers;
using W88.Utilities;
using W88.Utilities.Constant;

namespace W88.BusinessLogic.Accounts.Models
{
    public class ChangePasswordInfo
    {
        private string _password = string.Empty;
        private string _newPassword = string.Empty;
        private string _confirmPassword = string.Empty;
        
        public string MemberId { get; set; }

        public string Password
        {
            get { return _password; }
            set
            {
                var password = value.Trim();
                _password = string.IsNullOrEmpty(password) ? string.Empty : Encryption.Encrypt(EncryptionType.RjnD, password);
            }   
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                var password = value.Trim();
                _newPassword = string.IsNullOrEmpty(password) ? string.Empty : Encryption.Encrypt(EncryptionType.RjnD, password);
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                var password = value.Trim();
                _confirmPassword = string.IsNullOrEmpty(password) ? string.Empty : Encryption.Encrypt(EncryptionType.RjnD, password);
            }
        }

        public string Language { get; set; }
    }
}
