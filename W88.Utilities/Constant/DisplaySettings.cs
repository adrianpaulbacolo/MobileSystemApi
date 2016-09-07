using System;

namespace W88.Utilities.Constant
{
    public class DisplaySettings
    {
        public static decimal RoundDown(string value, int decimalPlaces)
        {
            int lastIndex = value.LastIndexOf('.');
            if (lastIndex <= 0) return Convert.ToDecimal(value);
            decimalPlaces++;
            value = value.PadRight(lastIndex + decimalPlaces, '0').Substring(0, lastIndex + decimalPlaces);
            return Convert.ToDecimal(value);
        }
        public static decimal RoundDown(decimal value, int decimalPlaces)
        {
            string valueStr = Convert.ToString(value);
            int lastIndex = valueStr.LastIndexOf('.');
            if (lastIndex <= 0) return Convert.ToDecimal(valueStr);
            decimalPlaces++;
            valueStr = valueStr.PadRight(lastIndex + decimalPlaces, '0').Substring(0, lastIndex + decimalPlaces);
            return Convert.ToDecimal(valueStr);
        }
    }
}
