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

        public string TranslationKey { get; set; }

        public string PaymentMode { get; set; }

        public decimal MinAmount { get; set; }

        public decimal MaxAmount { get; set; }

        public string TotalAllowed { get; set; }

        public string LimitDaily { get; set; }

        public string MerchantId { get; set; }

        public dynamic Amount { get; set; }

        public dynamic Banks { get; set; }

        public dynamic SecondBanks { get; set; }

        public dynamic BankBranch { get; set; }

        public dynamic BankBranchList { get; set; }

        public dynamic BankAddress { get; set; }

        public dynamic BankAddressList { get; set; }

        public dynamic CreditCard { get; set; }

        public dynamic AccountName { get; set; }

        public dynamic AccountNumber { get; set; }

        public dynamic ReferenceId { get; set; }

        public dynamic SystemBankAccount { get; set; }

        public dynamic DepositDateTime { get; set; }

        public dynamic DepositChannel { get; set; }

        public dynamic Message { get; set; }

        public dynamic MobileNumber { get; set; }
    }
}
