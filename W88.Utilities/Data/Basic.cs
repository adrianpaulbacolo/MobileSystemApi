using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace W88.Utilities.Data
{
    public class Basic : IEncrypt
    {
        public string Encrypt(string clearText, string key)
        {
            if (string.IsNullOrWhiteSpace(clearText))
                return string.Empty;

            encryption_manager.encryption encrypt = new encryption_manager.encryption();
            encrypt.private_key = key;
            encrypt.message = clearText;

            return encrypt.encrypting();
        }

        public string Decrypt(string encryptedText, string key)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
                return string.Empty;

            encryption_manager.encryption decrypt = new encryption_manager.encryption();
            decrypt.private_key = key;
            decrypt.message = encryptedText;

            return decrypt.decrypting();
        }
    }
}
