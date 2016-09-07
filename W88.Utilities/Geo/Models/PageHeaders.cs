
using System;

namespace W88.Utilities.Geo.Models
{
    public class PageHeaders
    {
        public string Host { get; set; }
        
        public string Ip { get; set; }
        
        public string Cdn { get; set; }
        
        public string Key { get; set; }
        
        public string CountryCode { get; set; }

        public string Country { get; set; }

        public string Permission = string.Empty;
    }
}
