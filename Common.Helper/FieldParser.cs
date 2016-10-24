using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Common.Exception;

namespace Common.Helper
{
    public class FieldParser<T> where T : struct, IConvertible
    {
        private readonly Regex _regex = new Regex(@"\[(\s*(\w*),?)*\]");

        public IList<T> Parse(string fields)
        {
            var match = _regex.Match(fields);

            var parsed = new HashSet<T>();
            foreach (var capture in match.Groups[2].Captures)
            {
                var value = capture.ToString();
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                T result;
                if (!System.Enum.TryParse(value, out result))
                    throw new BadRequestException(string.Format("Unknown field '{0}'", value));

                parsed.Add(result);
            }

            return parsed.ToList();
        }
    }

    public static class ParameterParaser
    {
        public static Dictionary<string, string> ParseODataParas(this string parameters)
        {
            var result = new Dictionary<string, string>();
            parameters = parameters.Replace("[", string.Empty).Replace("]", string.Empty).Trim();
            var keyValues = parameters.Split(',');
            foreach (var keyValue in keyValues)
            {
                if (keyValue.IndexOf('=') < 0)
                    throw new BadRequestException(string.Format("Wrong parameters '{0}'", keyValue));
                var keyAndValue = keyValue.Split('=');
                result.Add(keyAndValue[0].Trim(), keyAndValue[1].Trim());
            }
            return result;
        }
    }
}
