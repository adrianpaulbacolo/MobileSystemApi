using System.Security.Cryptography;
using System.Text;

namespace W88.Utilities.Data
{
    public class Sha256Hash
    {
        public string Encrypt(string input)
        {
            SHA256 sha = SHA256Managed.Create();
            byte[] hashData = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < hashData.Length; i++)
            {
                output.Append(hashData[i].ToString("x2"));
            }

            return output.ToString();
        } 
    }
}
