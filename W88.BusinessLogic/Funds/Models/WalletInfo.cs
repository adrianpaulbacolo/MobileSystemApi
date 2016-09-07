
namespace W88.BusinessLogic.Funds.Models
{
    public class WalletInfo
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public bool Enabled { get; set; }

        public int OrderBy { get; set; }

        public int SelectOrder { get; set; }

        public dynamic Balance { get; set; }

        public string CurrRestriction { get; set; }

        public string CurrAllowOnly { get; set; }

        public string CurrencyLabel { get; set; }

        public dynamic Lang { get; set; }

        public dynamic Selection { get; set; }

    }

    public class WalletInfoResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public dynamic Balance { get; set; }

        public string CurrencyLabel { get; set; }
    }
}
