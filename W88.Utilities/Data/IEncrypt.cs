using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace W88.Utilities.Data
{
    interface IEncrypt 
    {
        string Encrypt(string clearText, string key);

        string Decrypt(string encryptedText, string key);
        
    }
}
