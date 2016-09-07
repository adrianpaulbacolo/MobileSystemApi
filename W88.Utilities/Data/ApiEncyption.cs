using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace W88.Utilities.Data
{
    public class ApiEncyption : IEncrypt
    {
        public string Encrypt(string clearText, string key)
        {
            byte[] clearBytes =
                System.Text.Encoding.Unicode.GetBytes(clearText);
            System.Security.Cryptography.PasswordDeriveBytes pdb =
                new System.Security.Cryptography.PasswordDeriveBytes(key,
                    new byte[]
                    {
                        0x49, 0x49, 0x35, 0x6e, 0x76, 0x4d,
                        0x65, 0x64, 0x76, 0x76, 0x64, 0x65, 0x76
                    });

            byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            return Convert.ToBase64String(encryptedData);
        }

        public string Decrypt(string cipherText, string key)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            System.Security.Cryptography.PasswordDeriveBytes pdb =
                new System.Security.Cryptography.PasswordDeriveBytes(key,
                    new byte[]
                    {
                        0x49, 0x49, 0x35, 0x6e, 0x76, 0x4d,
                        0x65, 0x64, 0x76, 0x76, 0x64, 0x65, 0x76
                    });

            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }


        private byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Security.Cryptography.Rijndael alg = System.Security.Cryptography.Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;

            System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms,
                alg.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write);
            cs.Write(clearData, 0, clearData.Length);
            cs.Close();

            byte[] encryptedData = ms.ToArray();
            return encryptedData;
        }

        private byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Security.Cryptography.Rijndael alg = System.Security.Cryptography.Rijndael.Create();
            alg.Key = Key;
            alg.IV = IV;

            System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms,
                alg.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);
            cs.Write(cipherData, 0, cipherData.Length);
            cs.Close();
            byte[] decryptedData = ms.ToArray();
            return decryptedData;
        }


    }
}
