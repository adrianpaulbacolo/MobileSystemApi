
namespace W88.Utilities.Security
{
    public class Validation
    {
        public static bool IsInjection(string text)
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

        public static bool IsAlphanumeric(string text)
        {
            System.Text.RegularExpressions.Regex mc = new System.Text.RegularExpressions.Regex("([a-zA-Z0-9])");

            return mc.Match(text).Success;
        }

        public static bool IsNumeric(string text)
        {
            decimal decVal;
            int intVal;
            if (decimal.TryParse(text, out decVal))
            {
                return true;
            }

            return int.TryParse(text, out intVal);
        }
    }
}
