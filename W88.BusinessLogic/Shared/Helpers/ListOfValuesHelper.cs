using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Funds.Models;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities.Constant;
using W88.Utilities.Geo;
using W88.Utilities.Geo.Models;
using W88.WebRef.svcPayMember;
using W88.WebRef.svcPayMS1;

namespace W88.BusinessLogic.Shared.Helpers
{
    public class ListOfValuesHelper : BaseHelper
    {
        private IEnumerable<string> GetCurrencySettings()
        {
            var arrStrCurrencies = new OperatorSettings(Settings.OperatorName).Values.Get("Currencies");
            var lstCurrencies =
                arrStrCurrencies.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .ToList();
            return lstCurrencies;
        }

        public PageHeaders GetCountryInfo()
        {
            var headers = new Location().CheckCdn(HttpContext.Current.Request);
            var ipHelper = new IpHelper();
            var permission = string.Empty;
            var country = headers.CountryCode;
            headers.Ip = !string.IsNullOrWhiteSpace(headers.Ip) ? headers.Ip : ipHelper.User;

            if (string.IsNullOrWhiteSpace(country))
            {
                var ipLoc = new Location().Ip2LocScript();
                if (ipLoc != null)
                {
                    country = ipLoc.Country ?? string.Empty;
                    headers.Ip = ipLoc.Ip;
                    permission = ipLoc.Permission ?? string.Empty;
                }
            }

            if (string.IsNullOrWhiteSpace(country))
            {
                using (var wsInstance = new WebRef.wsIP2Loc.ServiceSoapClient())
                {
                    wsInstance.location(headers.Ip, ref country, ref permission);
                }
            }

            headers.Country = country;
            headers.Permission = permission;
            return headers;
        }

        public List<LOV> GetCountryPhone(out string selectedValue)
        {
            selectedValue = null;
            var countries = new List<LOV>();
            using (var wsInstance = new WebRef.wsMemberMS1.memberWSSoapClient())
            {
                var dsCountryInfo = wsInstance.GetCountryInfo(base.OperatorId);

                foreach (DataRow drPhoneCountryCode in dsCountryInfo.Tables[0].Select("", "countryPhoneCode ASC"))
                {
                    countries.Add(new LOV()
                    {
                        Text = string.Format("+ {0}", Convert.ToString(drPhoneCountryCode["countryPhoneCode"])),
                        Value = Convert.ToString(drPhoneCountryCode["countryPhoneCode"])

                    });
                }

                var headers = GetCountryInfo();
                if (!string.IsNullOrEmpty(headers.CountryCode))
                {
                    DataRow[] countrySearchResult =
                        dsCountryInfo.Tables[0].Select("countryCode='" + headers.CountryCode + "'");
                    if (countrySearchResult.Any())
                        selectedValue = countrySearchResult[0]["countryPhoneCode"].ToString();
                }

                if (selectedValue == null && countries.Any())
                    selectedValue = countries[0].Value;

                return countries;
            }
        }

        public List<LOV> GetCurrencies(out string selectedCurrency)
        {
            var currencies = new List<LOV>();

            var currencySettings = GetCurrencySettings();

            var currencyList = base.GetListOfValues<LOV>("shared/Currencies", "Currency", false);

            foreach (var item in currencySettings)
            {
                currencies.Add(currencyList.FirstOrDefault(c => c.Value == item));
            }

            var countryCode = GetCountryInfo();

            switch (countryCode.Country.ToUpper())
            {
                case "AU":
                    selectedCurrency = "AUD";
                    break;
                case "SG":
                    selectedCurrency = "UUS";
                    break;
                case "CN":
                    selectedCurrency = "RMB";
                    break;
                case "TH":
                    selectedCurrency = "THB";
                    break;
                case "VN":
                    selectedCurrency = "VND";
                    break;
                case "ID":
                    selectedCurrency = "IDR";
                    break;
                case "MY":
                    selectedCurrency = "MYR";
                    break;
                case "KR":
                    selectedCurrency = "KRW";
                    break;
                case "JP":
                    selectedCurrency = "JPY";
                    break;
                default:
                    selectedCurrency = "";
                    break;
            }

            string currency = selectedCurrency;

            selectedCurrency = currencies.FirstOrDefault(x => x.Value == currency) != null ? selectedCurrency : "";

            return currencies;
        }

        public List<LOV> GetBanksList(int methodId, string currencyCode)
        {
            if (methodId == 120227)
            {
                if (!string.IsNullOrWhiteSpace(currencyCode))
                {
                    var banks = base.GetListOfValues<BankInfo>("payments/Banks/banks", methodId.ToString(), true).FirstOrDefault(x => x.Currency.Equals(currencyCode, StringComparison.OrdinalIgnoreCase));
                    return banks == null ? new List<LOV>() : banks.Banks;
                }
                else
                {
                    return new List<LOV>();
                }
            }

            return base.GetListOfValues<LOV>("payments/Banks/banks", methodId.ToString(), true);
        }

        public List<LOV> GetSystemBankAccounts(UserSessionInfo userInfo)
        {
            var bankList = new List<LOV>();

            using (var svcInstance = new MS1Client())
            {
                string strProcessCode = string.Empty;
                string strProcessText = string.Empty;

                var response = svcInstance.getSystemBankAccount(base.OperatorId, userInfo.MemberCode, false, out strProcessCode, out strProcessText);

                if (strProcessCode == "00")
                {
                    foreach (var item in response)
                    {
                        var bank = new LOV
                        {
                            Value = item.accountId.ToString(),
                            Text = item.descriptionExternal
                        };

                        bankList.Add(bank);
                    }
                }
            }

            return bankList;
        }

        public async Task<List<LOV>> GetMemberBankAccounts(UserSessionInfo userInfo)
        {
            var bankList = new List<LOV>();

            bool isNameNative = false;
            if ((userInfo.LanguageCode.Equals("ZH-CN", StringComparison.OrdinalIgnoreCase) && userInfo.CurrencyCode.Equals("RMB", StringComparison.OrdinalIgnoreCase)) ||
                (userInfo.LanguageCode.Equals("VI-VN", StringComparison.OrdinalIgnoreCase) && userInfo.CurrencyCode.Equals("VND", StringComparison.OrdinalIgnoreCase)) ||
                (userInfo.LanguageCode.Equals("TH-TH", StringComparison.OrdinalIgnoreCase) && userInfo.CurrencyCode.Equals("THB", StringComparison.OrdinalIgnoreCase)) ||
                (userInfo.LanguageCode.Equals("ID-ID", StringComparison.OrdinalIgnoreCase) && userInfo.CurrencyCode.Equals("IDR", StringComparison.OrdinalIgnoreCase)) ||
                (userInfo.LanguageCode.Equals("KM-KH", StringComparison.OrdinalIgnoreCase) && userInfo.CurrencyCode.Equals("USD", StringComparison.OrdinalIgnoreCase)) ||
                (userInfo.LanguageCode.Equals("KO-KR", StringComparison.OrdinalIgnoreCase) && userInfo.CurrencyCode.Equals("KRW", StringComparison.OrdinalIgnoreCase)) ||
                (userInfo.LanguageCode.Equals("JA-JP", StringComparison.OrdinalIgnoreCase) && userInfo.CurrencyCode.Equals("JPY", StringComparison.OrdinalIgnoreCase)))
            {
                isNameNative = true;
            }

            using (var svcInstance = new MemberClient())
            {
                var response = await svcInstance.getBankAccountsAsync(new getBankAccountsRequest()
                {
                    operatorId = OperatorId,
                    countryCode = userInfo.CountryCode,
                    currencyCode = userInfo.CurrencyCode,
                });

                if (response.statusCode == "00")
                {
                    foreach (var item in response.getBankAccountsResult)
                    {
                        var bank = new LOV
                        {
                            Value = item.bankCode,
                            Text = isNameNative ? item.bankNameNative : item.bankName
                        };

                        bankList.Add(bank);
                    }
                }
            }

            bankList.Add(new LOV
            {
                Text = base.GetMessage("Pay_OtherBank"),
                Value = Constants.VarNames.OtherBankValue
            });

            return bankList;
        }

        public async Task<List<LOV>> GetMemberSecondaryBankAccounts(UserSessionInfo userInfo)
        {
            var bankList = new List<LOV>();

            bool isNameNative = false;
            if (userInfo.LanguageCode.Equals("VI-VN", StringComparison.OrdinalIgnoreCase) && userInfo.CurrencyCode.Equals("VND", StringComparison.OrdinalIgnoreCase))
            {
                isNameNative = true;
            }

            using (var svcInstance = new MemberClient())
            {
                var response = await svcInstance.getSecondaryBankAccountsAsync(new getSecondaryBankAccountsRequest()
                {
                    operatorId = OperatorId,
                    currencyCode = userInfo.CurrencyCode,
                    countryCode = userInfo.CountryCode,
                });

                if (response.statusCode == "00")
                {
                    foreach (var item in response.getSecondaryBankAccountsResult)
                    {
                        var bank = new LOV
                        {
                            Value = item.bankId.ToString(),
                            Text = isNameNative ? item.bankNameNative : item.bankName
                        };

                        bankList.Add(bank);
                    }
                }
            }

            bankList.Add(new LOV
            {
                Text = base.GetMessage("Pay_OtherBank"),
                Value = Constants.VarNames.OtherBankValue
            });

            return bankList;
        }

        public async Task<List<LOV>> GetMemberBankLocation(long bankId)
        {
            var locations = new List<LOV>();

            using (var client = new MemberClient())
            {
                var response = await client.getBankLocationsAsync(new getBankLocationsRequest
                {
                    bankId = bankId
                });

                if (response.statusCode == "00")
                {
                    foreach (DataRow row in response.getBankLocationsResult.Rows)
                    {
                        locations.Add(new LOV
                        {
                            Value = row["bankLocationId"].ToString(),
                            Text = row["description"].ToString()
                        });
                    }
                }
            }

            return locations;
        }

        public async Task<List<LOV>> GetMemberBankBranches(long bankId, long bankLocationId)
        {
            var branches = new List<LOV>();

            using (var svcInstance = new MemberClient())
            {
                var response = await svcInstance.getBankBranchesAsync(new getBankBranchesRequest
                {
                    bankId = bankId,
                    bankLocationId = bankLocationId
                });

                if (response.statusCode == "00")
                {
                    foreach (DataRow row in response.getBankBranchesResult.Rows)
                    {
                        branches.Add(new LOV
                        {
                            Value = row["bankBranchId"].ToString(),
                            Text = row["description"].ToString()
                        });
                    }
                }
            }

            return branches;
        }

        public List<LOV> GetDepositChannel()
        {
            string channels = CultureHelpers.AppData.GetLocale_i18n_Resource("/payments/depositchannel/depositchannel", true);
            return Utilities.Common.ParseJsonString<LOV>(channels, "DepositChannel");
        }

        public List<LOV> GetCardType()
        {
            string cards = CultureHelpers.AppData.GetLocale_i18n_Resource("/payments/banks/cardType", false);
            return Utilities.Common.ParseJsonString<LOV>(cards, "CardType");
        }

    }
}
