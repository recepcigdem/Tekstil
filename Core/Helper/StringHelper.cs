using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helper
{
    public static class StringHelper
    {
        public static string[] MonthNames = new string[12]
        {
      "Ocak",
      "Şubat",
      "Mart",
      "Nisan",
      "Mayıs",
      "Haziran",
      "Temmuz",
      "Ağustos",
      "Eylül",
      "Ekim",
      "Kasım",
      "Aralık"
        };
        private static Random random = new Random();

        public static string EnglishUrl(string originalUrl) => originalUrl.Replace('Ç', 'C').Replace('ç', 'c').Replace('İ', 'I').Replace('ı', 'i').Replace('Ö', 'O').Replace('ö', 'o').Replace('Ş', 'S').Replace('ş', 's').Replace('Ü', 'U').Replace('ü', 'u').Replace(" ", "-");

        public static string TurkishUppercase(string originalText) => originalText.Replace('ç', 'Ç').Replace('i', 'İ').Replace('ı', 'I').Replace('ö', 'Ö').Replace('ş', 'Ş').Replace('ü', 'Ü');

        public static string Encrypt(string text, string key)
        {
            using (MD5CryptoServiceProvider cryptoServiceProvider1 = new MD5CryptoServiceProvider())
            {
                using (TripleDESCryptoServiceProvider cryptoServiceProvider2 = new TripleDESCryptoServiceProvider())
                {
                    cryptoServiceProvider2.Key = cryptoServiceProvider1.ComputeHash(Encoding.UTF8.GetBytes(key));
                    cryptoServiceProvider2.Mode = CipherMode.ECB;
                    cryptoServiceProvider2.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform encryptor = cryptoServiceProvider2.CreateEncryptor())
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(text);
                        byte[] inArray = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
                        return Convert.ToBase64String(inArray, 0, inArray.Length);
                    }
                }
            }
        }

        public static string Decrypt(string cipher, string key)
        {
            using (MD5CryptoServiceProvider cryptoServiceProvider1 = new MD5CryptoServiceProvider())
            {
                using (TripleDESCryptoServiceProvider cryptoServiceProvider2 = new TripleDESCryptoServiceProvider())
                {
                    cryptoServiceProvider2.Key = cryptoServiceProvider1.ComputeHash(Encoding.UTF8.GetBytes(key));
                    cryptoServiceProvider2.Mode = CipherMode.ECB;
                    cryptoServiceProvider2.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform decryptor = cryptoServiceProvider2.CreateDecryptor())
                    {
                        byte[] inputBuffer = Convert.FromBase64String(cipher);
                        return Encoding.UTF8.GetString(decryptor.TransformFinalBlock(inputBuffer, 0, inputBuffer.Length));
                    }
                }
            }
        }

        public static string Base64Encode(string plainText) => Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

        public static string Base64Decode(string base64EncodedData) => Encoding.UTF8.GetString(Convert.FromBase64String(base64EncodedData));

        public static string RandomString(int length) => new string(Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", length).Select<string, char>((Func<string, char>)(s => s[StringHelper.random.Next(s.Length)])).ToArray<char>());
    }
}
