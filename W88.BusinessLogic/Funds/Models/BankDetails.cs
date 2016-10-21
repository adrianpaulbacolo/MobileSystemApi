using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W88.BusinessLogic.Shared.Models;

namespace W88.BusinessLogic.Funds.Models
{
    public class BankDetails
    {
        public LOV Bank { get; set; }
        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BankName { get; set; }
        public string BankBranch { get; set; }
        public string BankAddress { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public bool IsPreferred { get; set; }

        public BankDetails()
        {
            Bank = new LOV();
        }
    }
}
