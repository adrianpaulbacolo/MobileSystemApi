using System;
using System.Collections.Generic;
using System.Linq;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Funds.Models;
using W88.BusinessLogic.Shared.Helpers;

namespace W88.BusinessLogic.Account.Helpers
{
    public sealed class Wallets
    {
        public List<WalletInfo> WalletInfo;
        public string FundsPageNote;
        private readonly UserSessionInfo _user;

        /// <summary>
        /// Get all wallet settings
        /// </summary>
        /// <param name="user">User session info for member code</param>
        /// <param name="getAll">Set to true to get all wallet settings. Will bypass Enabled property settings</param>
        public Wallets(UserSessionInfo user, bool getAll)
        {
            _user = user;
            GetInfo(getAll);
        }

        private void GetInfo(bool getAll)
       {
            WalletInfo = new List<WalletInfo>();
            var jsonFile = CultureHelpers.AppData.GetLocale_i18n_Resource("Shared/Wallets", false);
            var walletList = Utilities.Common.ParseJsonString<WalletInfo>(jsonFile, "Wallets").OrderBy(info => info.OrderBy);

            foreach (var setting in walletList)
            {
                var item = new WalletInfo
                {
                    Id = Convert.ToInt16(setting.Id),
                    OrderBy = Convert.ToInt16(setting.OrderBy),
                    SelectOrder = Convert.ToInt16(setting.SelectOrder),
                    CurrRestriction = setting.CurrRestriction.ToUpper(),
                    CurrAllowOnly = setting.CurrAllowOnly.ToUpper(),
                    CurrencyLabel = setting.CurrencyLabel.ToUpper(),
                    Enabled = setting.Enabled,
                    Name = setting.Lang.GetValue(_user.LanguageCode).Value
                };

                var curr = _user.CurrencyCode;
                
                if (getAll)
                {
                    WalletInfo.Add(item);
                }
                else 
                {
                    if (!item.Enabled) continue;

                    if (!string.IsNullOrWhiteSpace(item.CurrAllowOnly))
                    {
                        if (item.CurrAllowOnly.Contains(curr))
                            WalletInfo.Add(item);
                    }
                    else if (!string.IsNullOrWhiteSpace(item.CurrRestriction))
                    {
                        if (!item.CurrRestriction.Contains(curr))
                            WalletInfo.Add(item);
                    }
                    else
                    {
                        WalletInfo.Add(item);
                    }
                }
            }

        }
    }
}
