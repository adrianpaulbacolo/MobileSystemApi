using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace W88.Utilities.Data
{
    public class Md5Hash
    {
        public string Encrypt(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            byte[] data;

            // Create a new instance of the MD5. 
            using (MD5 md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash. 
                data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            }

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            StringBuilder output = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            for (int i = 0; i < data.Length; i++)
            {
                output.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string. 
            return output.ToString();
        }
    }
}
