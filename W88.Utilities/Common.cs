using System;
using System.Collections.Generic;
using System.Xml.Linq;
using W88.Utilities.Data;

namespace W88.Utilities
{
    public static class Common 
    {
        public static string GetSessionVariable(string key)
        {
            return SessionHandler.GetSessionVariable(key);
        }

        public static void SetSessionVariable(string key, string value)
        {
            SessionHandler.SetSessionVariable(key, value);
        }

        public static T GetAppSetting<T>(string key)
        {
            return new ObjHelper().GetAppSetting<T>(key);
        }
        public static T GetValue<T>(object obj)
        {
            return new ObjHelper().GetValue<T>(obj);
        }

        public static T DeserializeObject<T>(string obj)
        {
            return new ObjHelper().Deserialize<T>(obj);
        }

        public static string SerializeObject(object obj)
        {
            return new ObjHelper().SerializeObject(obj);
        }
        
        public static List<T> ParseJsonString<T>(string json, string arrayName)
        {
            return new ObjHelper().ParseJsonString<T>(json, arrayName);
        }
        public static T GetAppData<T>(string filename) where T : XElement
        {
            return new ObjHelper().GetAppData<T>(filename);
        }

        public static string GetJsonAppData(string filename) 
        {
            return new ObjHelper().GetJsonAppData(filename);
        }

        public static T GetObject<T>() where T : class, new()
        {
            return (T) Convert.ChangeType(new T(), typeof (T));
        }
    }

    public static class Encryption
    {
        public static string Encrypt(Constant.EncryptionType type, string clearText, string keyName = "")
        {
            var encryptedText = string.Empty;

            switch (type)
            {
                case Constant.EncryptionType.Basic:
                    encryptedText = Common.GetObject<Basic>().Encrypt(clearText, 
                        string.IsNullOrWhiteSpace(keyName) ? Common.GetAppSetting<string>("PrivateKey") : Common.GetAppSetting<string>(keyName));
                    break;
                case Constant.EncryptionType.Md5Hash:
                    encryptedText = Common.GetObject<Md5Hash>().Encrypt(clearText);
                    break;
                case Constant.EncryptionType.RjnD:
                    encryptedText = Common.GetObject<RjnD>().Encrypt(clearText,
                        string.IsNullOrWhiteSpace(keyName) ? Common.GetAppSetting<string>("EncryptionKey") : Common.GetAppSetting<string>(keyName));
                    break;
                case Constant.EncryptionType.Sha256Hash:
                    encryptedText = Common.GetObject<Sha256Hash>().Encrypt(clearText);
                    break;
                case Constant.EncryptionType.TripleDESCS:
                    var privateKey = Decrypt(Constant.EncryptionType.RjnD, Common.GetAppSetting<string>("PrivateKeyToken"));
                    encryptedText = Common.GetObject<TripleDESCSProvider>().Encrypt(clearText, privateKey);
                    break;
            }

            return encryptedText;
        }

        public static string Decrypt(Constant.EncryptionType type, string encryptedText, string keyName = "")
        {
            var decryptedText = string.Empty;

            switch (type)
            {
                case Constant.EncryptionType.Basic:
                    decryptedText = Common.GetObject<Basic>().Decrypt(encryptedText, 
                        string.IsNullOrWhiteSpace(keyName) ? Common.GetAppSetting<string>("PrivateKey") : Common.GetAppSetting<string>(keyName));
                    break;
                case Constant.EncryptionType.RjnD:
                    decryptedText = Common.GetObject<RjnD>().Decrypt(encryptedText, 
                        string.IsNullOrWhiteSpace(keyName) ? Common.GetAppSetting<string>("EncryptionKey") : Common.GetAppSetting<string>(keyName));
                    break;
                case Constant.EncryptionType.TripleDESCS:
                    var privateKey = Decrypt(Constant.EncryptionType.RjnD, Common.GetAppSetting<string>("PrivateKeyToken"));
                    decryptedText = Common.GetObject<TripleDESCSProvider>().Decrypt(encryptedText, privateKey);
                    break;
            }

            return decryptedText;
        }
    }
}
