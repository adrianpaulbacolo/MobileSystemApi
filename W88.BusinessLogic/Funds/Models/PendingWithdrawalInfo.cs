using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using W88.BusinessLogic.Shared.Models;

namespace W88.BusinessLogic.Funds.Models
{
    public class PendingWithdrawalInfo
    {
        public string Name { get; set; }
        public long TransactionId { get; set; }
        public long MethodId { get; set; }
        public decimal Amount { get; set; }
        public DateTime RequestDateTime { get; set; }
        public string Status { get; set; }
    }
}
