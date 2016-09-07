using W88.BusinessLogic.Base.Helpers;

namespace W88.BusinessLogic.Accounts.Models
{
    public class FundTransferInfo
    {
        public class FtRequest
        {
            public string TransferFrom { get; set; }

            public string TransferTo { get; set; }

            public decimal TransferAmount { get; set; }

            public string PromoCode { get; set; }

        }

        public class FtResponse 
        {
            public string FtCode { get; set; }

            public int TransferId { get; set; }

            public string TransferStatus { get; set; }

            public string Message { get; set; }

        }
    }
}
