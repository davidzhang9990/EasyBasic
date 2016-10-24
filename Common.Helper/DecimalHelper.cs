using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helper
{
    public static class DecimalHelper
    {
        public static decimal FormatDoubleToDecimal(this double value, int point)
        {
            return Decimal.Round((decimal)value, point, MidpointRounding.AwayFromZero);
        }

        public static decimal FormatDecimal(this decimal value, int point)
        {
            return Decimal.Round(value, point, MidpointRounding.AwayFromZero);
        }
    }
}
