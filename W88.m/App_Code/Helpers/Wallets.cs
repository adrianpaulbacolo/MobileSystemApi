using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Models;

namespace Helpers
{
    /// <summary>
    /// Summary description for Wallets
    /// </summary>
    public sealed class Wallets
    {
        public List<WalletInfo> WalletInfo;
        public string FundsPageNote;

        public Wallets()
        {
            GetInfo();
        }

        private void GetInfo()
        {
            WalletInfo = new List<WalletInfo>();
            XElement xeResources;
            commonCulture.appData.GetRootResourceNonLanguage("/Shared/Wallets", out xeResources);

            var walletList = xeResources.Elements("Wallets").Elements().Where(m => m.Attribute("enable").Value == "true").ToList().OrderBy(x => (int)x.Attribute("orderBy"));
            foreach (var element in walletList)
            {
                var item = new WalletInfo
                {
                    Id = Convert.ToInt16(element.Attribute("id").Value),
                    OrderBy = Convert.ToInt16(element.Attribute("orderBy").Value)
                };

                foreach (var v in element.Elements("lang").Select(x => x.Element(commonVariables.SelectedLanguageShort)))
                {
                    item.Name = Convert.ToString(v.Value);
                }
                
                WalletInfo.Add(item);
            }

            var note = xeResources.Elements("FundsPageNote").Select(x => x.Element(commonVariables.SelectedLanguageShort));
            foreach (var item in note)
            {
                FundsPageNote = Convert.ToString(item.Value);
            }
           
        }
    }
}