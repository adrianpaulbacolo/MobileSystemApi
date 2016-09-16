using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Shared.Helpers;
using W88.Rewards.BusinessLogic.Accounts.Models;
using W88.WebRef.RewardsServices;

namespace W88.Rewards.BusinessLogic.Rewards.Helpers
{
    /// <summary>
    /// Summary description for RewardsHelper
    /// </summary>
    public class RewardsHelper : BaseHelper
    {
        public DataSet GetCatalogueSet(MemberSession memberSession)
        {
            using (RewardsServicesClient client = new RewardsServicesClient())
            {
                var countryCode = memberSession == null ? "0" : memberSession.CountryCode;
                var currencyCode = memberSession == null ? "0" : memberSession.CurrencyCode;
                var riskId = memberSession == null ? "0" : memberSession.RiskId;

                DataSet dataSet = client.getCatalogueSearch(
                    OperatorId.ToString(CultureInfo.InvariantCulture)
                    , LanguageHelpers.SelectedLanguage
                    , countryCode
                    , currencyCode
                    , riskId);

                if (dataSet.Tables.Count == 0)
                {
                    return null;
                }
                if (!dataSet.Tables[0].Columns.Contains("redemptionValidity"))
                {
                    dataSet.Tables[0].Columns.Add("redemptionValidity");
                }

                foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                {
                    string imgNameOn = dataRow["imageNameOn"].ToString().Split('.')[0];
                    string imgPathOn = imgNameOn + ".png";
                    string imgPathOff = imgNameOn + ".png";

                    dataRow["imagePathOn"] = Convert.ToString(ConfigurationManager.AppSettings.Get("ImagesDirectoryPath") + "Category/" + imgPathOn);
                    dataRow["imagePathOff"] = Convert.ToString(ConfigurationManager.AppSettings.Get("ImagesDirectoryPath") + "Category/" + imgPathOff);

                    if (!riskId.Equals("0"))
                    {
                        dataRow["redemptionValidity"] += ",";
                        if (!dataRow["redemptionValidity"].ToString().ToUpper().Equals("ALL,"))
                        {
                            if (((string)dataRow["redemptionValidity"]).Contains(memberSession.RiskId.ToUpper() + ","))
                            {
                                dataRow["redemptionValidity"] = "0";
                            }
                            else
                            {
                                dataRow["redemptionValidity"] = "1";
                            }
                        }
                        else
                        {
                            dataRow["redemptionValidity"] = "1";
                        }
                    }
                    else
                    {
                        dataRow["redemptionValidity"] += "0";
                    }
                }
                return dataSet;
            }
        }

        public int GetPointLevel(string memberId)
        {
            try
            {
                if (string.IsNullOrEmpty(memberId))
                {
                    return 0;
                }

                using (RewardsServicesClient sClient = new RewardsServicesClient())
                {
                    string pointLevel = sClient.getMemberPointLevelFE(memberId);
                    return int.Parse(pointLevel);
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}