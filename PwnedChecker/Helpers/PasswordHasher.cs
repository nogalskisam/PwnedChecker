using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace PwnedChecker.Helpers
{
    public static class PasswordHasher
    {
        public static byte[] HashPassword(string password)
        {
            var hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(password));

            return hash;
        }

        public static string ConvertHashToString(byte[] passwordHash)
        {
            var hashString = BitConverter.ToString(passwordHash).Replace("-", "");

            return hashString;
        }
    }
}
