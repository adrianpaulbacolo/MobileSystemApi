using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using W88.BusinessLogic.Shared.Models;

namespace W88.BusinessLogic.Funds.Models
{
    public class FundsInfo : BankDetails
    {
        public FundsInfo()
        {
            SystemBank = new LOV();
            DepositChannel = new LOV();
            CardType = new LOV();
        }
        
        public decimal Amount { get; set; }

        public LOV SystemBank { get; set; }

        public LOV SecondBank { get; set; }

        public long BankAddressId { get; set; }

        public long BankBranchId { get; set; }

        public string ReferenceId { get; set; }


        public string CountryCode { get; set; }

        public string Phone { get; set; }

        public string MobileNumber { get; set; }

        public bool NotifyMobile { get; set; }


        public DateTime DepositDateTime { get; set; }

        public LOV DepositChannel { get; set; }


        public LOV CardType { get; set; }

        public string CardNumber { get; set; }

        public string CardExpiryMonth { get; set; }

        public string CardExpiryYear { get; set; }

        public string SanitizedCardNumber
        {
            get
            {
                if (this.CardNumber == null)
                    return this.CardNumber;

                Regex digitsOnly = new Regex(@"[^\d]");
                return digitsOnly.Replace(this.CardNumber, "");
            }
            set{
                if(value != null)
                this.CardNumber = value;
            }
        }

        public string CCV { get; set; }

        public string ThankYouPage { get; set; }
    }
}
