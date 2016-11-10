using System.Security.Cryptography;
using System.Text;

namespace W88.Utilities.Data
{
    public class Sha256Hash
    {
        public string Encrypt(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            byte[] data;

            // Create a new instance of the SHA256Managed.
            using (SHA256 sha = SHA256Managed.Create())
            {
                // Convert the input string to a byte array and compute the hash.
                data = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
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
