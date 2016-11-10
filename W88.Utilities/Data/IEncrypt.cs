
namespace W88.Utilities.Data
{
    interface IEncrypt 
    {
        string Encrypt(string clearText, string key);

        string Decrypt(string encryptedText, string key);
        
    }
}
