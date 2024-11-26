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
    }
}