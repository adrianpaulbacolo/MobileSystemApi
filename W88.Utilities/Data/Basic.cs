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
            string functionReturnValue = null;
            if (!string.IsNullOrEmpty(clearText))
            {
                encryption_manager.encryption encrypt = new encryption_manager.encryption();
                encrypt.private_key = key;
                encrypt.message = clearText;
                functionReturnValue = encrypt.encrypting();
                encrypt = null;
            }
            else { functionReturnValue = string.Empty; }
            return functionReturnValue;
        }

        public string Decrypt(string encryptedText, string key)
        {
            string functionReturnValue = null;
            if (!string.IsNullOrEmpty(encryptedText))
            {
                encryption_manager.encryption decrypt = new encryption_manager.encryption();
                decrypt.private_key = key;
                decrypt.message = encryptedText;
                functionReturnValue = decrypt.decrypting();
                decrypt = null;
            }
            else { functionReturnValue = string.Empty; }
            return functionReturnValue;
        }

     
    }
}
