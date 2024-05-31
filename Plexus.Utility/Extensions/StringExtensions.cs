using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Plexus.Utility.Extensions
{
	public static class StringExtensions
	{
        private static Regex _initialCharacters = new Regex("^[A-Za-zกขฃคฅฆงจฉชซฌญฎฏฐฑฒณดตถทธนบปผฝพฟภมยรลวศษสหฬอฮ]$");

        public static string GenerateRandomString(int length)
        {
            using (var crypto = RandomNumberGenerator.Create())
            {
                var bits = (length * 6);
                var byte_size = ((bits + 7) / 8);
                var bytesarray = new byte[byte_size];
                crypto.GetBytes(bytesarray);
                return Convert.ToBase64String(bytesarray);
            }
        }

        public static string GetInitial(this string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            var initial = name.FirstOrDefault(x => _initialCharacters.IsMatch($"{x}"));

            return initial == default ? string.Empty
                                      : $"{initial}.";
        }

        public static IEnumerable<string>? SplitWithCommaSeparator(this string? text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return null;
            }

            var response = text.Split(",")
                               .Select(x => x.Trim())
                               .Where(x => !string.IsNullOrEmpty(x))
                               .ToList();
            
            return response;
        }

        public static string? ToStringWithCommaSeparator(this IEnumerable<string>? array)
        {
            if (array is null || !array.Any())
            {
                return null;
            }

            var response = string.Join(",", array);
            
            return response;
        }

        public static string Base64Encode(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }

        public static string Base64Decode(this string base64string)
        {
            if (string.IsNullOrEmpty(base64string))
            {
                return string.Empty;
            }

            return Encoding.UTF8.GetString(Convert.FromBase64String(base64string));
        }

        public static string HashHMACSHA256(this string signature, string? key = null)
        {
            if (string.IsNullOrEmpty(signature))
            {
                return string.Empty;
            }

            using (var hasher = new HMACSHA256(Encoding.UTF8.GetBytes(key ?? string.Empty)))
            {
                var signatureByted = hasher.ComputeHash(Encoding.UTF8.GetBytes(signature));
                return Convert.ToBase64String(signatureByted);
            }
        }

        public static bool IsHashHMACSHA256Match(this string signature, string expectedHashValue, string? key = null)
        {
            if (string.IsNullOrEmpty(signature) || string.IsNullOrEmpty(expectedHashValue))
            {
                return false;
            }

            var hashValues = signature.HashHMACSHA256(key);

            return string.Equals(expectedHashValue, hashValues);
        }
    }
}

