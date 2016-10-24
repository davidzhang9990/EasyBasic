using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumDescriptionAttribute : Attribute
    {
        private readonly string _text = "";
        public string Text
        {
            get { return this._text; }
        }
        public EnumDescriptionAttribute(string text)
        {
            _text = text;
        }

    }

    public class EnumStringHelper
    {
        public static string ToString(object o)
        {
            Type t = o.GetType();
            string s = o.ToString();
            var os = (EnumDescriptionAttribute[])t.GetField(s).GetCustomAttributes(typeof(EnumDescriptionAttribute), false);
            if (os.Length == 1)
            {
                return os[0].Text;
            }
            return s;
        }
    }

    public class EnumHelper
    {
        public static int GetValueOf(string enumName, string enumConst)
        {
            Type enumType = Type.GetType(enumName);
            if (enumType == null)
            {
                throw new ArgumentException("Specified enum type could not be found", "enumName");
            }

            object value = Enum.Parse(enumType, enumConst);
            return Convert.ToInt32(value);
        }
    }
}
