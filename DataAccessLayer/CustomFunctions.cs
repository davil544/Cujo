using System;
using System.Security.Cryptography;
using System.Text;

namespace CujoPasswordManager.DataAccessLayer
{
    public static class CustomFunctions
    {
        public static string TruncateString(string str, int maxlength)
        {
            String truncated = str.Substring(0, Math.Min(str.Length, maxlength));
            if (str.Length > maxlength)
            {
                truncated += "...";
            }
            return truncated;
        }

        public static string HashToSHA512(string msg)
        {
            var hash = new SHA512CryptoServiceProvider();
            byte[] hashedArray = hash.ComputeHash(Encoding.UTF8.GetBytes(msg));
            string hashed = null;
            foreach (byte thing in hashedArray)
            {
                hashed += thing.ToString("x2");
            }
            return hashed;
        }

        public static string GeneratePassword(int length, string includedCharacters)
        {
            string valid = null;  // Maybe remove " from the symbols string, might mess up length of password returned
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ", lowercase = "abcdefghijklmnopqrstuvwxyz", numbers = "1234567890", symbols = "~`!@#$%^&*()_-+={[}]:;\"\',>.?/|";
            if (includedCharacters.Contains("A")) { valid += uppercase; }
            if (includedCharacters.Contains("z")) { valid += lowercase; }
            if (includedCharacters.Contains("09")) { valid += numbers; }
            if (includedCharacters.Contains("@#")) { valid += symbols; }
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                sb.Append(valid[rnd.Next(valid.Length)]);
            }
            return sb.ToString();
        }
    }
}