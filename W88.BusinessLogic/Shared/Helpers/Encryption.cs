
using W88.Utilities.Data;

namespace W88.BusinessLogic.Shared.Helpers
{
    internal static class Encryption 
    {
        public static string Encrypting(string clearText, string privateKey)
        {
            var key = Utilities.Common.GetAppSetting<string>(privateKey);
            return Utilities.Common.GetObject<Basic>().Encrypt(clearText, key);
        }

        public static string Encrypting(string clearText)
        {
            var key = Utilities.Common.GetAppSetting<string>("PrivateKey");
            return Utilities.Common.GetObject<Basic>().Encrypt(clearText, key);
        }

        public static string Decrypting(string encryptedText, string privateKey)
        {
            var key = Utilities.Common.GetAppSetting<string>(privateKey);
            return Utilities.Common.GetObject<Basic>().Decrypt(encryptedText, key);
        }

        public static string Decrypting(string encryptedText)
        {
            var key = Utilities.Common.GetAppSetting<string>("PrivateKey");
            return Utilities.Common.GetObject<Basic>().Decrypt(encryptedText, key);
        }

        public static string Encrypt(string clearText)
        {
            var key = Utilities.Common.GetAppSetting<string>("EncryptionKey");
            return Utilities.Common.GetObject<ApiEncyption>().Encrypt(clearText, key);
        }

        public static string Decrypt(string encryptedText)
        {
            var key = Utilities.Common.GetAppSetting<string>("EncryptionKey");
            return Utilities.Common.GetObject<ApiEncyption>().Decrypt(encryptedText, key);
        }
    }
}
