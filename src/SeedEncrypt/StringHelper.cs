using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeedEncrypt
{
    public class StringHelper
    {
        public static string TrimAndRemoveEndings(string value) =>
            RemoveEndings(value).Trim();

        public static string RemoveEndings(string value) => value
            .Replace("\r", String.Empty)
            .Replace("\n", String.Empty);

        public static bool IsBase64(string value)
        {
            if (ContainsInvalidBase64Char(value))
                return false;

            try
            {
                return Convert.FromBase64String(value) != null;
            }
            catch
            {
                return false;
            }
        }

        public static bool ContainsInvalidBase64Char(string value) =>
            value?.Any(ch => !IsValidBase64Char(ch)) == true;

        public static bool IsValidBase64Char(char ch) =>
            ch >= 'a' && ch <= 'z' ||
            ch >= 'A' && ch <= 'Z' ||
            ch >= '0' && ch <= '9' ||
            ch == '+' || 
            ch == '/' || 
            ch == '=';
    }
}
