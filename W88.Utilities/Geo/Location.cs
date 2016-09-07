using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using W88.Utilities.Constant;
using W88.Utilities.Geo.Models;

namespace W88.Utilities.Geo
{
    public class Location
    {
        readonly PageHeaders _headers = new PageHeaders();

        public PageHeaders CheckCdn(HttpRequest context)
        {
            if (!string.IsNullOrEmpty(Common.GetValue<string>(context.ServerVariables[HeaderKeys.HTTP_X_AKAMAI_EDGESCAPE])))
            {
                _headers.Cdn = context.ServerVariables[HeaderKeys.HTTP_X_AKAMAI_EDGESCAPE];
                _headers.Key = HeaderKeys.HTTP_X_AKAMAI_EDGESCAPE;
            }

            if (!string.IsNullOrEmpty(Common.GetValue<string>(context.ServerVariables[HeaderKeys.HTTP_CF_IPCOUNTRY])))
            {
                _headers.Cdn = context.ServerVariables[HeaderKeys.HTTP_CF_IPCOUNTRY];
                _headers.Key = HeaderKeys.HTTP_CF_IPCOUNTRY;
            }

            if (!string.IsNullOrEmpty(Common.GetValue<string>(context.ServerVariables[HeaderKeys.HTTP_GEO_COUNTRY])))
            {
                _headers.Cdn = context.ServerVariables[HeaderKeys.HTTP_GEO_COUNTRY];
                _headers.Key = HeaderKeys.HTTP_GEO_COUNTRY;
            }

            if (!string.IsNullOrEmpty(Common.GetValue<string>(context.ServerVariables[HeaderKeys.HOST])))
            {
                _headers.Host = context.ServerVariables[HeaderKeys.HOST];
            }

            if (!string.IsNullOrEmpty(Common.GetValue<string>(context.ServerVariables[HeaderKeys.TRUE_CLIENT_IP])))
            {
                _headers.Ip = context.ServerVariables[HeaderKeys.TRUE_CLIENT_IP];
            }

            if (!string.IsNullOrWhiteSpace(_headers.Cdn) && !string.IsNullOrWhiteSpace(_headers.Key))
                _headers.CountryCode = GetCountryCode(_headers.Cdn, _headers.Key);

            return _headers;
        }

        private string GetCountryCode(string CDN_Value, string key)
        {
            string CountryCode = string.Empty;

            if (key == HeaderKeys.HTTP_X_AKAMAI_EDGESCAPE)
            {
                string[] Values = new string[100];
                Values = CDN_Value.Split(',');
                CountryCode = Values[1].Split('=')[1];
            }
            if (key == HeaderKeys.HTTP_CF_IPCOUNTRY)
            {
                CountryCode = CDN_Value;
            }
            if (key == HeaderKeys.HTTP_GEO_COUNTRY)
            {
                CountryCode = CDN_Value;
            }
            return CountryCode;
        }

        public PageHeaders Ip2LocScript()
        {
            var request = (HttpWebRequest)WebRequest.Create(new Uri("https://ip2loc.w2script.com/IP2LOC?v=" + DateTime.Now));
            var response = (HttpWebResponse)request.GetResponse();

            string content = string.Empty;
            if (!response.ResponseUri.AbsoluteUri.Contains("forbidden"))
            {
                var receiveStream = response.GetResponseStream();
                var readStream = new StreamReader(receiveStream, Encoding.UTF8);
                content = readStream.ReadToEnd();
            }
            return Common.DeserializeObject<PageHeaders>(content);
        }
    }
}
