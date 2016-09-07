using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace W88.BusinessLogic.Funds.Models
{
    public class PaymentSettingInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public dynamic TabName { get; set; }
        public string Amount { get; set; }
        public string BankList { get; set; }
        public string BankBranch { get; set; }
        public string BankAddress { get; set; }
        public string CreditCard { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string Passcode { get; set; }
        public string ReferenceId { get; set; }
        public string SystemBankAccount { get; set; }
        public string DepositDateTime { get; set; }
        public string DepositChannel { get; set; }
        public string Message { get; set; }
        public string PaymentMode { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public string TotalAllowed { get; set; }
        public string LimitDaily { get; set; }
        public string MerchantId { get; set; }

    }
}
