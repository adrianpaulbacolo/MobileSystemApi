using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;

public class commonEncryption
{
    #region encryption

    //private key for encryption & decryption
    private static string privateKey = string.Empty;
    //encryption using md5 algorithm with a private key
    public static string encrypting(string str)
    {
        privateKey = ConfigurationManager.AppSettings.Get("PrivateKey");
        string functionReturnValue;
        if (!string.IsNullOrEmpty(str))
        {
            encryption_manager.encryption encrypt = new encryption_manager.encryption();
            encrypt.private_key = privateKey;
            encrypt.message = str;
            functionReturnValue = encrypt.encrypting();
        }
        else { functionReturnValue = string.Empty; }
        return functionReturnValue;
    }

    //decryption using md5 algorithm with a private key
    public static string decrypting(string str)
    {
        privateKey = ConfigurationManager.AppSettings.Get("PrivateKey");
        string functionReturnValue;
        if (!string.IsNullOrEmpty(str))
        {
            encryption_manager.encryption decrypt = new encryption_manager.encryption();
            decrypt.private_key = privateKey;
            decrypt.message = str;
            functionReturnValue = decrypt.decrypting();
        }
        else { functionReturnValue = string.Empty; }
        return functionReturnValue;
    }

    public static string encrypting(string str, string privateKey)
    {
        string functionReturnValue;
        if (!string.IsNullOrEmpty(str))
        {
            encryption_manager.encryption encrypt = new encryption_manager.encryption();
            encrypt.private_key = privateKey;
            encrypt.message = str;
            functionReturnValue = encrypt.encrypting();
        }
        else { functionReturnValue = string.Empty; }
        return functionReturnValue;
    }

    //decryption using md5 algorithm with a private key
    public static string decrypting(string str, string private_key)
    {
        string functionReturnValue;
        if (!string.IsNullOrEmpty(str))
        {
            encryption_manager.encryption decrypt = new encryption_manager.encryption();
            decrypt.private_key = private_key;
            decrypt.message = str;
            functionReturnValue = decrypt.decrypting();
        }
        else { functionReturnValue = string.Empty; }
        return functionReturnValue;
    }
    #endregion

    #region cathy encryption method
    private static string Password = ConfigurationManager.AppSettings.Get("EncryptionKey");

    internal static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        Rijndael alg = Rijndael.Create();
        alg.Key = Key;
        alg.IV = IV;

        CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(clearData, 0, clearData.Length);
        cs.Close();

        byte[] encryptedData = ms.ToArray();
        return encryptedData;
    }

    public static string Encrypt(string clearText)
    {
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
            new byte[] {0x49, 0x49, 0x35, 0x6e, 0x76, 0x4d,
            0x65, 0x64, 0x76, 0x76, 0x64, 0x65, 0x76});

        byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));

        return Convert.ToBase64String(encryptedData);
    }

    internal static byte[] Encrypt(byte[] clearData)
    {
        PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
            new byte[] {0x49, 0x49, 0x35, 0x6e, 0x76, 0x4d,
            0x65, 0x64, 0x76, 0x76, 0x64, 0x65, 0x76});

        return Encrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));
    }

    internal static void Encrypt(string fileIn, string fileOut)
    {
        System.IO.FileStream fsIn = new System.IO.FileStream(fileIn, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        System.IO.FileStream fsOut = new System.IO.FileStream(fileOut, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);

        PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
            new byte[] {0x49, 0x49, 0x35, 0x6e, 0x76, 0x4d,
            0x65, 0x64, 0x76, 0x76, 0x64, 0x65, 0x76});
        Rijndael alg = Rijndael.Create();
        alg.Key = pdb.GetBytes(32);
        alg.IV = pdb.GetBytes(16);

        CryptoStream cs = new CryptoStream(fsOut,
            alg.CreateEncryptor(), CryptoStreamMode.Write);

        int bufferLen = 4096;
        byte[] buffer = new byte[bufferLen];
        int bytesRead;
        do
        {
            // read a chunk of data from the input file
            bytesRead = fsIn.Read(buffer, 0, bufferLen);
            // encrypt it
            cs.Write(buffer, 0, bytesRead);
        } while (bytesRead != 0);

        cs.Close();
        fsIn.Close();
    }

    internal static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        Rijndael alg = Rijndael.Create();
        alg.Key = Key;
        alg.IV = IV;

        CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
        cs.Write(cipherData, 0, cipherData.Length);
        cs.Close();
        byte[] decryptedData = ms.ToArray();
        return decryptedData;
    }

    public static string Decrypt(string cipherText)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
            new byte[] {0x49, 0x49, 0x35, 0x6e, 0x76, 0x4d,
            0x65, 0x64, 0x76, 0x76, 0x64, 0x65, 0x76});

        byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));
        return Encoding.Unicode.GetString(decryptedData);
    }

    internal static byte[] Decrypt(byte[] cipherData)
    {
        PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
            new byte[] {0x49, 0x49, 0x35, 0x6e, 0x76, 0x4d,
            0x65, 0x64, 0x76, 0x76, 0x64, 0x65, 0x76});

        return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
    }

    internal static void Decrypt(string fileIn, string fileOut)
    {
        System.IO.FileStream fsIn = new System.IO.FileStream(fileIn, System.IO.FileMode.Open, System.IO.FileAccess.Read);
        System.IO.FileStream fsOut = new System.IO.FileStream(fileOut, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);

        PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
            new byte[] {0x49, 0x49, 0x35, 0x6e, 0x76, 0x4d,
            0x65, 0x64, 0x76, 0x76, 0x64, 0x65, 0x76});
        Rijndael alg = Rijndael.Create();
        alg.Key = pdb.GetBytes(32);
        alg.IV = pdb.GetBytes(16);

        CryptoStream cs = new CryptoStream(fsOut, alg.CreateDecryptor(), CryptoStreamMode.Write);

        int bufferLen = 4096;
        byte[] buffer = new byte[bufferLen];
        int bytesRead;
        do
        {
            // read a chunk of data from the input file
            bytesRead = fsIn.Read(buffer, 0, bufferLen);
            // Decrypt it
            cs.Write(buffer, 0, bytesRead);
        } while (bytesRead != 0);
        cs.Close();
        fsIn.Close();
    }

    #endregion

    public static string GetMd5Hash(string input)
    {
        byte[] data;
        // Convert the input string to a byte array and compute the hash. 
        using (MD5 md5Hash = MD5.Create())
        {
            data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        }

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

    internal static string MD5Decrypt(string key, string cipherText)
    {
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //System.Security.Cryptography.PasswordDeriveBytes pdb = new System.Security.Cryptography.PasswordDeriveBytes(GetMd5Hash(key),
        //  new byte[] {0x49, 0x49, 0x35, 0x6e, 0x76, 0x4d,
        // 0x65, 0x64, 0x76, 0x76, 0x64, 0x65, 0x76});

        byte[] decryptedData = MD5Decrypt(cipherBytes, Encoding.UTF8.GetBytes(GetMd5Hash(key)), Encoding.UTF8.GetBytes(GetMd5Hash(GetMd5Hash(key))));
        return Encoding.Unicode.GetString(decryptedData);
    }


    internal static byte[] MD5Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
    {
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        Rijndael alg = Rijndael.Create();
        alg.Key = Key;
        alg.IV = IV;

        CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write);
        cs.Write(cipherData, 0, cipherData.Length);
        cs.Close();
        byte[] decryptedData = ms.ToArray();
        return decryptedData;
    }

    public static string decryptToken(string cipherString, string cipherKey)
    {
        byte[] keyArray;
        //get the byte code of the string
        byte[] toEncryptArray = Convert.FromBase64String(cipherString);

        //if hashing was not implemented get the byte code of the key
        keyArray = UTF8Encoding.UTF8.GetBytes(cipherKey);

        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        //set the secret key for the tripleDES algorithm
        tdes.Key = keyArray;
        //mode of operation. there are other 4 modes.
        //We choose ECB(Electronic code Book)

        tdes.Mode = CipherMode.ECB;
        //padding mode(if any extra byte added)
        tdes.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = tdes.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock
                (toEncryptArray, 0, toEncryptArray.Length);
        //Release resources held by TripleDes Encryptor
        tdes.Clear();
        //return the Clear decrypted TEXT
        return UTF8Encoding.UTF8.GetString(resultArray);
    }

    public static string GetSHA256Hash(string input)
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