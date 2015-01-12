using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common
{
    public class validation
    {
        #region roundDown

        public static decimal roundDown(double value, int decimalPlaces)
        {
            string valueStr = Convert.ToString(value);

            int lastIndex = valueStr.LastIndexOf('.');

            if (lastIndex > 0)
            {
                decimalPlaces++;

                valueStr = valueStr.PadRight(lastIndex + decimalPlaces, '0').Substring(0, lastIndex + decimalPlaces);
            }

            return Convert.ToDecimal(valueStr);
        }

        public static decimal roundDown(decimal value, int decimalPlaces)
        {
            string valueStr = Convert.ToString(value);

            int lastIndex = valueStr.LastIndexOf('.');

            if (lastIndex > 0)
            {
                decimalPlaces++;

                valueStr = valueStr.PadRight(lastIndex + decimalPlaces, '0').Substring(0, lastIndex + decimalPlaces);
            }

            return Convert.ToDecimal(valueStr);
        }

        public static decimal roundDown(string value, int decimalPlaces)
        {
            int lastIndex = value.LastIndexOf('.');

            if (lastIndex > 0)
            {
                decimalPlaces++;

                value = value.PadRight(lastIndex + decimalPlaces, '0').Substring(0, lastIndex + decimalPlaces);
            }

            return Convert.ToDecimal(value);
        }

        #endregion

        #region validation

        internal static string sqlsafe(string sqlStr)
        {
            if (!string.IsNullOrEmpty(sqlStr)) { return sqlStr.Replace("'", "''"); }
            else { return ""; }
        }

        internal static string sqlsafe(string sqlStr, bool returnNull)
        {
            string return_value = string.Empty;

            if (!string.IsNullOrEmpty(sqlStr))
            {
                return_value = sqlStr.Replace("'", "''");
                return_value.Insert(0, "'").Insert(return_value.Length, "'");
            }

            if (returnNull) { return_value = "NULL"; }

            return return_value;
        }

        public static bool _isinjection(string text)
        {
            bool functionReturnValue = false;

            functionReturnValue = false;

            if (string.IsNullOrEmpty(text))
            {
                return false;
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(text, "/(\\%27)|(\\')|(\\-\\-)|(\\%23)|(#)/ix"))
            {
                functionReturnValue = true;
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text, "/((\\%3D)|(=))[^\\n]*((\\%27)|(\\')|(\\-\\-)|(\\%3B)|(;))/i"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text, "/\\w*((\\%27)|(\\'))((\\%6F)|o|(\\%4F))((\\%72)|r|(\\%52))/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))union/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))join/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))where/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))select/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))update/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))insert/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))delete/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))create/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))drop/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))alter/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))exec/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))call/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))truncate/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))replace/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))declare/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))shutdown/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))waitfor/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/((\\%27)|(\\'))kill/ix"))
                {
                    functionReturnValue = true;
                }
            }

            if (functionReturnValue == false)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text.ToLower(), "/exec(\\s|\\+)+(s|x)p\\w+/ix"))
                {
                    functionReturnValue = true;
                }
            }
            return functionReturnValue;
        }

        public static bool _isalpha(string text)
        {
            System.Text.RegularExpressions.Regex mc = new System.Text.RegularExpressions.Regex("([a-zA-Z])");

            return mc.Match(text).Success;
        }

        public static bool _isalphanumeric(string text)
        {
            System.Text.RegularExpressions.Regex mc = new System.Text.RegularExpressions.Regex("([a-zA-Z0-9])");

            return mc.Match(text).Success;
        }

        public static bool _isdecimal(string text)
        {
            decimal decVal;

            if (decimal.TryParse(text, out decVal)) { return true; }
            else { return false; }
        }

        public static bool _isnumeric(string text)
        {
            decimal decVal; int intVal;
            if (decimal.TryParse(text, out decVal)) { return true; }
            else if (int.TryParse(text, out intVal)) { return true; }
            else { return false; }
            //System.Text.RegularExpressions.Regex mc = new System.Text.RegularExpressions.Regex(@"^\d+$");
            //return mc.Match(text).Success;
        }

        internal static bool _isemail(string text)
        {
            System.Text.RegularExpressions.MatchCollection mc = null;
            mc = System.Text.RegularExpressions.Regex.Matches(text, "([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})");

            if (mc.Count == 1) { return true; }
            else { return false; }
        }

        internal static bool isphone(string text)
        {
            System.Text.RegularExpressions.MatchCollection mc = null;
            mc = System.Text.RegularExpressions.Regex.Matches(text, "([0-9\\-\\+])");

            if (mc.Count == text.Length) { return true; }
            else { return false; }
        }

        #endregion
    }
}