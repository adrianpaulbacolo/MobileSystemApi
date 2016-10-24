using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Shared.Helpers;
using W88.Utilities.Geo;

namespace W88.BusinessLogic.Accounts.Models
{
    /// <summary>
    /// Summary description for RegisterInfo
    /// </summary>
    public class RegisterInfo
    {
        public UserInfo UserInfo = new UserInfo();

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Country { get; set; }

        public string ContactNumber { get; set; }

        public string CurrencyCode { get; set; }

        public string Firstname { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Lastname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string HiddenValues { get; set; }

        public string Permission { get; set; }

        public string CountryCode { get; set; }

        public Regex RexgexContact = new Regex("([0-9]{1,4})[-]([0-9]{6,12})$");

        public bool IsTestAccount = false;

        public string IpAddress { get; set; }

        public int OddsType { get; set; }

        public string AffiliateId { get; set; }

        public string City { get; set; }

        public string Address { get; set; }

        public string Postal { get; set; }

        public string Gender { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string ReferralId { get; set; }

        public readonly string SignUpUrl = string.Format("m.{0}", new IpHelper().DomainName);
    }
}