using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;


namespace mServices
{
    public class Common
    {
        public static string wcAmountFormat = System.Configuration.ConfigurationManager.AppSettings["amountFormat"];
        public static string wcDateFormat = System.Configuration.ConfigurationManager.AppSettings["dateFormat"];
        public static string wcDateTimeFormat = System.Configuration.ConfigurationManager.AppSettings["dateTimeFormat"];
        public static string wcDateTimeDisplayFormat = System.Configuration.ConfigurationManager.AppSettings["dateTimeDisplayFormat"];
        public static string wcDateTimeDisplayFormatShort = System.Configuration.ConfigurationManager.AppSettings["dateTimeDisplayFormatShort"];

        //public static string connCRM = System.Configuration.ConfigurationManager.ConnectionStrings["connCRM"].ConnectionString.Replace("[xxx]", Encryption.Decrypt(System.Configuration.ConfigurationManager.AppSettings["databasePassword"]));
        //public static string connCore = System.Configuration.ConfigurationManager.ConnectionStrings["connCore"].ConnectionString.Replace("[xxx]", Encryption.Decrypt(System.Configuration.ConfigurationManager.AppSettings["databasePassword"]));
        //public static string connPayment =System.Configuration.ConfigurationManager.ConnectionStrings["connPayment"].ConnectionString.Replace("[xxx]", Encryption.Decrypt(System.Configuration.ConfigurationManager.AppSettings["databasePassword"]));
        //public static string connProduct = System.Configuration.ConfigurationManager.ConnectionStrings["connProduct"].ConnectionString.Replace("[xxx]", Encryption.Decrypt(System.Configuration.ConfigurationManager.AppSettings["databasePassword"]));
        //public static string connMessage = System.Configuration.ConfigurationManager.ConnectionStrings["connMessage"].ConnectionString.Replace("[xxx]", Encryption.Decrypt(System.Configuration.ConfigurationManager.AppSettings["databasePassword"]));

        public static string connMessage()
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["connMessage"].ConnectionString.Replace("[xxx]", Encryption.Decrypt(System.Configuration.ConfigurationManager.AppSettings["databasePassword"]));
            }
            catch
            {
                return "";
            }
        }

        public static string connProduct()
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["connProduct"].ConnectionString.Replace("[xxx]", Encryption.Decrypt(System.Configuration.ConfigurationManager.AppSettings["databasePassword"]));
            }
            catch
            {
                return "";
            }
        }

        public static string connPayment()
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["connPayment"].ConnectionString.Replace("[xxx]", Encryption.Decrypt(System.Configuration.ConfigurationManager.AppSettings["databasePassword"]));
            }
            catch
            {
                return "";
            }
        }

        public static string connCore
        {
            get
            {
                try
                {
                    return System.Configuration.ConfigurationManager.ConnectionStrings["connCore"].ConnectionString.Replace("[xxx]", Encryption.Decrypt(System.Configuration.ConfigurationManager.AppSettings["databasePassword"]));
                }
                catch
                {
                    return "";
                }
            }            
        }

        public static string connWarehouse()
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["connWarehouse"].ConnectionString.Replace("[xxx]", Encryption.Decrypt(System.Configuration.ConfigurationManager.AppSettings["databasePassword"]));
            }
            catch
            {
                return "";
            }
        }

        public static string connCRM()
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["connCRM"].ConnectionString.Replace("[xxx]", Encryption.Decrypt(System.Configuration.ConfigurationManager.AppSettings["databasePassword"]));
            }
            catch
            {
                return "";
            }
        }

        public static string connAffiliate()
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["connAffiliate"].ConnectionString.Replace("[xxx]", Encryption.Decrypt(System.Configuration.ConfigurationManager.AppSettings["databasePasswordAffiliate"]));
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static DateTime DateParse(string dateString)
        {
            return DateTime.ParseExact(dateString, wcDateFormat, null);
        }

        public static DateTime DateTimeParse(string dateTimeString)
        {
            return DateTime.ParseExact(dateTimeString, wcDateTimeFormat, null);
        }

        public static bool IsAlphaNumeric(string text)
        {
            Regex pattern = new Regex("[^a-zA-Z0-9]");
            return !pattern.IsMatch(text);
        }

        //public static void mapOperatorCode(DataControlFieldCollection dcfc)
        //{
        //    foreach (DataControlField dcf in dcfc)
        //    {
        //        if (dcf.HeaderText.Equals("SOP"))
        //        {
        //            dcf.HeaderText = Localization.GetString("SOP");
        //        }
        //        else if (dcf.HeaderText.Equals("1"))
        //        {
        //            dcf.HeaderText = Localization.GetString("BET8");
        //        }
        //    }
        //}
    }

    public class Encryption
    {
        private static string Password = "M41n$YsT3mVone";
        public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
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
            byte[] clearBytes =
              System.Text.Encoding.Unicode.GetBytes(clearText);
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x49, 0x35, 0x6e, 0x76, 0x4d,
            0x65, 0x64, 0x76, 0x76, 0x64, 0x65, 0x76});

            byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            return Convert.ToBase64String(encryptedData);
        }

        public static byte[] Encrypt(byte[] clearData)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x49, 0x35, 0x6e, 0x76, 0x4d,
            0x65, 0x64, 0x76, 0x76, 0x64, 0x65, 0x76});

            return Encrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));
        }

        public static void Encrypt(string fileIn, string fileOut)
        {
            FileStream fsIn = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
            FileStream fsOut = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);

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

        public static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();
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
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

        public static byte[] Decrypt(byte[] cipherData)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                new byte[] {0x49, 0x49, 0x35, 0x6e, 0x76, 0x4d,
            0x65, 0x64, 0x76, 0x76, 0x64, 0x65, 0x76});

            return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
        }

        public static void Decrypt(string fileIn, string fileOut)
        {
            FileStream fsIn = new FileStream(fileIn, FileMode.Open, FileAccess.Read);
            FileStream fsOut = new FileStream(fileOut, FileMode.OpenOrCreate, FileAccess.Write);

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
    }
}
