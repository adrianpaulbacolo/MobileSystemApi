using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using W88.BusinessLogic.Shared.Models;

namespace W88.BusinessLogic.Funds.Models
{
    public class BankInfo
    {
        public string Currency { get; set; }

        public List<LOV> Banks { get; set; }

        public BankInfo()
        {
            Banks = new List<LOV>();
        }
    }
}
