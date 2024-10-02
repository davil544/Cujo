using System;

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
    }
}