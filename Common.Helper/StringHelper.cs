using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Helper
{
    public static class StringHelper
    {
        public static bool IsEmpty(this Guid value)
        {
            return value == Guid.Empty;
        }

        public static bool IsNotEmpty(this Guid value)
        {
            return !IsEmpty(value);
        }


        public static string ConvertToBase64(this string value)
        {
            var stringBytes = Encoding.UTF8.GetBytes(value);
            return Convert.ToBase64String(stringBytes);
        }

        public static string ConvertFromBase64(this string value)
        {
            var base64EncodedBytes = Convert.FromBase64String(value);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        private static readonly char[] Constant =   
      {   
        '0','1','2','3','4','5','6','7','8','9',   
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',   
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'   
      };

        private static readonly char[] LetterConstant =   
      {   
        'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',   
        'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'   
      };
        public static string GenerateRandomLetters(int length)
        {
            var newRandom = new StringBuilder(62);
            var rd = new Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(LetterConstant[rd.Next(52)]);
            }
            return newRandom.ToString();
        }

        public static string GenerateRandom(int length)
        {
            var newRandom = new StringBuilder(62);
            var rd = new Random();
            for (int i = 0; i < length; i++)
            {
                newRandom.Append(Constant[rd.Next(62)]);
            }
            return newRandom.ToString();
        }

        public static bool CompareVersion(String v1, String v2)
        {
            var s1 = NormalisedVersion(v1);
            var s2 = NormalisedVersion(v2);
            var cmp = s1.CompareTo(s2);
            return cmp >= 0;
        }

        public static String NormalisedVersion(String version)
        {
            return NormalisedVersion(version, '.', 4);
        }

        public static String NormalisedVersion(String version, char sep, int maxWidth)
        {
            var splitString = version.Split(sep);
            var sb = new StringBuilder();
            var placeHolder = new StringBuilder();
            for (var i = 0; i < maxWidth; i++)
            {
                placeHolder.Append("0");
            }
            foreach (var s in splitString)
            {
                sb.AppendFormat("{0:" + placeHolder.ToString() + "}", int.Parse(s));
            }

            return sb.ToString();
        }

        public static bool IsGuid(this string strSrc)
        {

            Regex reg = new Regex("^[A-F0-9]{8}(-[A-F0-9]{4}){3}-[A-F0-9]{12}$", RegexOptions.IgnoreCase);

            return reg.IsMatch(strSrc);

        }
    }
}
