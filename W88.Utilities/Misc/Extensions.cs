using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using W88.Utilities.Constant;

namespace W88.Utilities.Extensions
{
    public static class DateTimeExtension
    {
        public static string ToW88StringFormat(this DateTime item)
        {
            return item.ToString(Settings.DateTimeFormat);
        }

        public static bool IsOver18(this DateTime dob)
        {
            DateTime now = DateTime.Today;
            int age = now.Year - dob.Year;

            if (now.Month < dob.Month || (now.Month == dob.Month && now.Day < dob.Day))
                age--;

            return age >= 18;
        }
    }

    public static class DecimalExtension
    {
        public static string ToW88StringFormat(this decimal item)
        {
            return item.ToString(Settings.DecimalFormat);
        }
    }

    public static class HttpExtension
    {
        public static string GetHeader(this HttpRequestMessage item, string headerName)
        {
            IEnumerable<string> headers;
            item.Headers.TryGetValues(headerName, out headers);

            if (headers == null) return "";

            var enumurable = headers as string[] ?? headers.ToArray();
            return enumurable.FirstOrDefault();
        }
    }

    public static class StringExtension
    {
        public static bool IsContactNumberMatch(this string item)
        {
            var contactRegEx = new Regex("([0-9]{1,4})[-]([0-9]{6,12})$");

            return contactRegEx.IsMatch(item);
        }
    }

}
