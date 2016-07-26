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

            var walletList = xeResources.Elements(commonVariables.OperatorCode).Elements("Wallets").Elements().Where(m => m.Attribute("enable").Value == "true").ToList().OrderBy(x => (int)x.Attribute("orderBy"));
            foreach (var element in walletList)
            {
                var item = new WalletInfo
                {
                    Id = Convert.ToInt16(element.Attribute("id").Value),
                    OrderBy = Convert.ToInt16(element.Attribute("orderBy").Value),
                    SelectOrder = Convert.ToInt16(element.Attribute("selectOrder").Value),
                    Restriction = element.Attribute("CurrRestriction").Value.ToUpper(),
                    AllowOnly = element.Attribute("CurrAllowOnly").Value.ToUpper(),
                    CurrencyLabel = element.Attribute("CurrStaticLabel").Value.ToUpper()
                };

                foreach (var v in element.Elements("lang").Select(x => x.Element(commonVariables.SelectedLanguageShort)))
                {
                    item.Name = Convert.ToString(v.Value);
                }

                foreach (var v in element.Elements("selection").Select(x => x.Element(commonVariables.SelectedLanguageShort)))
                {
                    item.SelectName = Convert.ToString(v.Value);
                }

                var curr = commonVariables.GetSessionVariable("CurrencyCode");

                if (!string.IsNullOrWhiteSpace(item.AllowOnly))
                {
                    if (item.AllowOnly.Contains(curr))
                        WalletInfo.Add(item);
                }
                else if (!string.IsNullOrWhiteSpace(item.Restriction))
                {
                    if (!item.Restriction.Contains(curr))
                        WalletInfo.Add(item);
                }
                else
                {
                    WalletInfo.Add(item);
                }
                    
            }

            var note = xeResources.Elements(commonVariables.OperatorCode).Elements("FundsPageNote").Select(x => x.Element(commonVariables.SelectedLanguageShort));
            foreach (var item in note)
            {
                FundsPageNote = Convert.ToString(item.Value);
            }

        }
    }
}