using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using W88.BusinessLogic.Shared.Models;

namespace W88.BusinessLogic.Funds.Models
{
    public class BaseFundsInfo
    {
        public BaseFundsInfo()
        {
            Bank = new LOV();
            SystemBank = new LOV();
            DepositChannel = new LOV();
            CardType = new LOV();
        }

        public decimal Amount { get; set; }

        public LOV SystemBank { get; set; }

        public LOV Bank { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string BankAddress { get; set; }

        public string AccountName { get; set; }
        public string AccountNumber { get; set; }

        public string ReferenceId { get; set; }
        public DateTime DepositDateTime { get; set; }
        public LOV DepositChannel { get; set; }

        public string ThankYouPage { get; set; }

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
    }
}
