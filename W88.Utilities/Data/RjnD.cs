using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace W88.Utilities.Data
{
    public class RjnD : IEncrypt
    {
        public string Encrypt(string clearText, string key)
        {
            if (string.IsNullOrWhiteSpace(clearText))
                return string.Empty;

            //get the byte code of the string
            byte[] toEncryptArray = Encoding.Unicode.GetBytes(clearText);

            byte[] encryptedData;

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(key,
                  new byte[]
                    {
                        0x49, 0x49, 0x35, 0x6e, 0x76, 0x4d,
                        0x65, 0x64, 0x76, 0x76, 0x64, 0x65, 0x76
                    });

            //Create stream
            using (MemoryStream memStream = new MemoryStream())
            {
                //Create a new instance of the Rijndael. 
                using (Rijndael alg = Rijndael.Create())
                {
                    //get the byte code of the key and set the secret key for the DES algorithm
                    alg.Key = pdb.GetBytes(32);

                    //initialization vector (IV)
                    alg.IV = pdb.GetBytes(16);

                    ICryptoTransform encryptor = alg.CreateEncryptor();

                    //Forming cryptostream to link with data stream.
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        //Write all data to stream.
                        cryptoStream.Write(toEncryptArray, 0, toEncryptArray.Length);
                    }

                    encryptedData = memStream.ToArray();
                }
            }

            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(encryptedData);
        }

        public string Decrypt(string encryptedText, string key)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
                return string.Empty;

            //get the byte code of the string
            byte[] toEncryptArray = Convert.FromBase64String(encryptedText);
           
            byte[] decryptedData;

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(key,
                 new byte[]
                    {
                        0x49, 0x49, 0x35, 0x6e, 0x76, 0x4d,
                        0x65, 0x64, 0x76, 0x76, 0x64, 0x65, 0x76
                    });

            byte[] dataBytes;

            //Create stream
            using (MemoryStream memStream = new MemoryStream())
            {
                //Create a new instance of the Rijndael. 
                //This generates a new key and initialization vector (IV).
                using (Rijndael alg = Rijndael.Create())
                {
                    //get the byte code of the key and set the secret key for the DES algorithm
                    alg.Key = pdb.GetBytes(32);

                    //initialization vector (IV)
                    alg.IV = pdb.GetBytes(16);

                    ICryptoTransform encryptor = alg.CreateDecryptor();

                    //Forming cryptostream to link with data stream.
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        //Write all data to stream.
                        cryptoStream.Write(toEncryptArray, 0, toEncryptArray.Length);
                    }

                    decryptedData = memStream.ToArray(); 
                }
            }

            //return the Clear decrypted TEXT
            return Encoding.Unicode.GetString(decryptedData);
        }
    }
}

   