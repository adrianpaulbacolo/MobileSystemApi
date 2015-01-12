using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;


public class SdPayUtility
{
    private string Key1;
    private string Key2;

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="data">加密数据</param>
    /// <param name="key1">key1</param>
    /// <param name="key2">key2</param>
    /// <returns>密文</returns>
    public static string EncryptData(string data, string key1, string key2)
    {

        SdPayUtility encrypt = new SdPayUtility();
        encrypt.Key1 = key1;
        encrypt.Key2 = key2;
        return encrypt.EncryptData(data + getMd5Hash(getMac() + DateTime.Now.ToFileTime()));

    }
    #region
    public void getKeys()
    {
        DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        provider.GenerateIV();
        provider.GenerateKey();
        Key1 = Convert.ToBase64String(provider.Key);
        Key2 = Convert.ToBase64String(provider.IV);
    }

    public string EncryptData(string data)
    {
        byte[] keyBytes = Convert.FromBase64String(Key1);//ASCIIEncoding.ASCII.GetBytes(key);
        byte[] keyIV = Convert.FromBase64String(Key2);
        byte[] inputByteArray = Encoding.UTF8.GetBytes(data);
        DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
        MemoryStream mStream = new MemoryStream();
        CryptoStream cStream = new CryptoStream(mStream, provider.CreateEncryptor(keyBytes, keyIV), CryptoStreamMode.Write);
        cStream.Write(inputByteArray, 0, inputByteArray.Length);
        cStream.FlushFinalBlock();
        return Convert.ToBase64String(mStream.ToArray());

    }
    public static string getMd5Hash(string input)
    {
        MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();
        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }
        // Return the hexadecimal string.
        return sBuilder.ToString();
    }
    private static string getMac()
    {
        string mac = "";
        Random r = new Random();
        mac = System.DateTime.Now.ToFileTime().ToString() + r.Next().ToString();
        return mac;
    }
    #endregion
}