using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace W88.Utilities.Data
{
    public class DES 
    {
        public string Encrypt(string data, string key1, string key2)
        {
            string _data = data + Common.GetObject<Md5Hash>().Encrypt(getMac() + DateTime.Now.ToFileTime());

            byte[] keyBytes = Convert.FromBase64String(key1); // ASCIIEncoding.ASCII.GetBytes(key);
            byte[] keyIV = Convert.FromBase64String(key2);
            byte[] inputByteArray = Encoding.UTF8.GetBytes(_data);

            DESCryptoServiceProvider provider = new DESCryptoServiceProvider();

            MemoryStream mStream = new MemoryStream();

            CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);

            cStream.Write(inputByteArray, 0, inputByteArray.Length);

            cStream.FlushFinalBlock();

            return Convert.ToBase64String(mStream.ToArray());
        }

        private static string getMac()
        {
            Random r = new Random();

            string mac = System.DateTime.Now.ToFileTime().ToString() + r.Next().ToString();

            return mac;
        }

       
    }
}
