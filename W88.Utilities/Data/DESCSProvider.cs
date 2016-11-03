using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace W88.Utilities.Data
{
    public class DESCSProvider
    {
        public string Encrypt(string clearText, string key1, string key2)
        {
            if (string.IsNullOrWhiteSpace(clearText))
                return string.Empty;

            string _data = clearText + Common.GetObject<Md5Hash>().Encrypt(getMac() + DateTime.Now.ToFileTime());

            byte[] clearBytes = Encoding.UTF8.GetBytes(_data);

            byte[] encryptedData;

            //Create stream
            using (MemoryStream memStream = new MemoryStream())
            {
                //Create a new instance of the DESCryptoServiceProvider. 
                using (DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider())
                {
                    //get the byte code of the key and set the secret key for the DES algorithm
                    desProvider.Key = Convert.FromBase64String(key1);

                    //initialization vector (IV)
                    desProvider.IV =  Convert.FromBase64String(key2);

                    ICryptoTransform encryptor = desProvider.CreateEncryptor();

                    //Forming cryptostream to link with data stream.
                    using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                    {
                        //Write all data to stream.
                        cryptoStream.Write(clearBytes, 0, clearBytes.Length);
                    }

                    encryptedData = memStream.ToArray();
                }
            }

            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(encryptedData);
        }

        private static string getMac()
        {
            Random r = new Random();

            string mac = System.DateTime.Now.ToFileTime().ToString() + r.Next().ToString();

            return mac;
        }
    }
}
