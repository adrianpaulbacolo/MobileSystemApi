using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Models
{
    /// <summary>
    /// Summary description for WalletInfo
    /// </summary>
    public class WalletInfo
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int OrderBy { get; set; }

        public string Note{ get; set; }

        public int SelectOrder { get; set; }

        public string SelectName { get; set; }
    }
}