using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace W88.Utilities.Data
{
    public class TripleDESCSProvider : IEncrypt
    {
        public string Encrypt(string clearText, string key)
        {
            if (string.IsNullOrWhiteSpace(clearText))
                return string.Empty;

            //get the byte code of the string
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(clearText);

            byte[] encryptedData;

            //Create stream
            using (MemoryStream memStream = new MemoryStream())
            {
                //Create a new instance of the TripleDESCryptoServiceProvider. 
                using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
                {
                    //get the byte code of the key and set the secret key for the tripleDES algorithm
                    tdes.Key = UTF8Encoding.UTF8.GetBytes(key);

                    //mode of operation. there are other 4 modes.
                    //We choose ECB(Electronic code Book)
                    tdes.Mode = CipherMode.ECB;

                    //padding mode(if any extra byte added)
                    tdes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform encryptor = tdes.CreateEncryptor();

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
            return Convert.ToBase64String(encryptedData, 0, encryptedData.Length);
        }

        public string Decrypt(string encryptedText, string key)
        {
            if (string.IsNullOrWhiteSpace(encryptedText))
                return string.Empty;

            //get the byte code of the string
            byte[] toEncryptArray = Convert.FromBase64String(encryptedText);
           
            byte[] decryptedData;

            //Create stream
            using (MemoryStream memStream = new MemoryStream())
            {
                //Create a new instance of the TripleDESCryptoServiceProvider. 
                using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
                {
                    //get the byte code of the key and set the secret key for the tripleDES algorithm
                    tdes.Key = UTF8Encoding.UTF8.GetBytes(key);

                    //mode of operation. there are other 4 modes.
                    //We choose ECB(Electronic code Book)
                    tdes.Mode = CipherMode.ECB;

                    //padding mode(if any extra byte added)
                    tdes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform encryptor = tdes.CreateDecryptor();

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
            return UTF8Encoding.UTF8.GetString(decryptedData);
        }
    }
}