using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Accounts.Models;
using W88.WebRef.RewardsServices;
using W88.BusinessLogic.Rewards.Models;

namespace W88.BusinessLogic.Rewards.Helpers
{

    /// <summary>
    /// Summary description for RewardsHelper
    /// </summary>
    public class RewardsHelper : BaseHelper
    {
        protected RewardsServicesClient Client = new RewardsServicesClient();

        public int CheckRedemptionLimitForVipCategory(string memberCode, string vipCategoryId)
        {
            try
            {
                return Client.CheckRedemptionLimitForVIPCategory(OperatorId.ToString(CultureInfo.InvariantCulture), 
                    memberCode,
                    vipCategoryId);
            }
            catch (Exception exception)
            {
                return 0;
            }
        }

        public DataSet GetCatalogueSet(MemberSession memberSession)
        {
            try
            {
                var countryCode = memberSession == null ? "0" : memberSession.CountryCode;
                var currencyCode = memberSession == null ? "0" : memberSession.CurrencyCode;
                var riskId = memberSession == null ? "0" : memberSession.RiskId;

                var dataSet = Client.getCatalogueSearch(
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
                            if (!((string) dataRow["redemptionValidity"]).Contains(riskId.ToUpper() + ","))
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
            catch (Exception exception)
            {
                return null;
            }
        }

        public string GetCategoryName(string categoryCode)
        {
            try
            {
                return Client.getCategoryName(categoryCode, LanguageHelpers.SelectedLanguage);               
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public int GetMemberPointLevelDiscount(MemberSession memberSession)
        {
            try
            {
                return Client.getMemberPointLevelDiscount(
                    OperatorId.ToString(CultureInfo.InvariantCulture), 
                    memberSession.CurrencyCode, 
                    GetPointLevel(memberSession.MemberId).ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception exception)
            {
                return 0;
            }
        }

        public MemberRedemptionDetails GetMemberRedemptionDetails(string memberCode)
        {
            try
            {
                var dataSet = Client.getMemberRedemptionDetail(OperatorId.ToString(CultureInfo.InvariantCulture), memberCode);
                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    return null;
                }
                var dataRow = dataSet.Tables[0].Rows[0];
                var redemptionDetails = new MemberRedemptionDetails();
                redemptionDetails.FullName = dataRow["firstName"] + " " + dataRow["lastName"];
                redemptionDetails.Address = dataRow["address"].ToString();
                redemptionDetails.Postal = dataRow["postal"].ToString();
                redemptionDetails.City = dataRow["city"].ToString();
                redemptionDetails.CountryCode = dataRow["countryCode"].ToString();
                redemptionDetails.Mobile = dataRow["mobile"].ToString();
                if (dataSet.Tables.Count > 1)
                {
                    redemptionDetails.PointsBefore = dataSet.Tables[1].Rows[0]["pointsBefore"].ToString();
                }
                return redemptionDetails;
            }
            catch (Exception exception)
            {
                return null;
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
                var pointLevel = Client.getMemberPointLevelFE(memberId);
                return int.Parse(pointLevel);               
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public DataSet GetProductDetails(MemberSession memberSession, string productId)
        {
            try
            {
                var countryCode = memberSession == null ? "0" : memberSession.CountryCode;
                var currencyCode = memberSession == null ? "0" : memberSession.CurrencyCode;
                var riskId = memberSession == null ? "0" : memberSession.RiskId;
                var dataSet = Client.getProductDetail(
                    productId, 
                    LanguageHelpers.SelectedLanguage, 
                    riskId, 
                    countryCode,
                    currencyCode, 
                    riskId);
                return dataSet;
            }
            catch (Exception exception)
            {
                return null;
            }
        }

        public DataSet GetProductSearch(MemberSession memberSession, 
            string categoryId, 
            int pointsFrom, 
            int pointsTo, 
            string searchText,
            string sortBy,
            string pageSize,
            string numberOfPages)
        {
            try
            {
                var countryCode = memberSession == null ? "0" : memberSession.CountryCode;
                var currencyCode = memberSession == null ? "0" : memberSession.CurrencyCode;
                var riskId = memberSession == null ? "0" : memberSession.RiskId;

                var dataSet = Client.getProductSearch(
                    OperatorId.ToString(CultureInfo.InvariantCulture), 
                    categoryId, 
                    LanguageHelpers.SelectedLanguage, 
                    pointsFrom, 
                    pointsTo, 
                    searchText,
                    countryCode, 
                    currencyCode, 
                    riskId, 
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), 
                    sortBy, 
                    pageSize, 
                    numberOfPages);

                return dataSet;
            }
            catch (Exception exception)
            {
                return null;
            }
        }
    }
}