using System;
using System.Data;
using System.Web;
using System.Text;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.Utilities.Log.Helpers;

public partial class Catalogue_Detail : CatalogueBasePage
{
    protected string RedirectUri = "/Catalogue/Detail.aspx";
    protected bool IsValidRedemption = false;
    protected bool IsRedemptionLimitReached = false;
    protected bool IsProcessingLimitReached = false;
    protected string VipOnly = string.Empty;
    protected string Errormsg = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        SetLabels();
        SetProductDetails();     
    }

    private void SetProductDetails()
    {
        try
        {
            var productId = HttpContext.Current.Request.QueryString.Get("id");
            var detailsSet = GetProductDetails(MemberSession, productId);

            if (detailsSet == null)
            {
                return;
            }

            var dataRow = detailsSet.Tables[0].Rows[0];
            var productDetails = new ProductDetails();       
            productDetails.ProductId = productId;

            RedirectUri = !HasSession
                ? string.Format(@"/_Secure/Login.aspx?redirect=/Catalogue/Redeem.aspx&productId={0}", productId)
                : string.Format(@"/Catalogue/Redeem.aspx?productId={0}", productId);

            // Set product details
            productDetails.ProductType = dataRow["productType"].ToString();
            productDetails.AmountLimit = dataRow["amountLimit"].ToString();
            productDetails.CategoryId = dataRow["categoryId"].ToString();
            productDetails.CurrencyCode = dataRow["currencyValidity"].ToString();
            productDetails.CountryCode = dataRow["countryValidity"].ToString();
            productDetails.PointsRequired = dataRow["pointsRequired"].ToString();
            productDetails.ImageUrl = dataRow["imageName"].ToString();
            productDetails.ProductCategoryName = dataRow["categoryName"].ToString();
            productDetails.ProductName = dataRow["productName"].ToString();
            productDetails.ProductDescription = dataRow["productDescription"].ToString();
            productDetails.DeliveryPeriod = dataRow["deliveryPeriod"].ToString();
            productDetails.DiscountPoints = dataRow["discountPoints"] == DBNull.Value ? string.Empty : dataRow["discountPoints"].ToString();    
            productDetails.RedemptionValidity = dataRow["redemptionValidity"].ToString();
            productDetails.RedemptionValidityCategory = dataRow["redemptionValidityCat"].ToString();

            if (productDetails.RedemptionValidity == "1" && productDetails.RedemptionValidityCategory == "1")
            {
                IsValidRedemption = true;
                var vipCategoryId = System.Configuration.ConfigurationManager.AppSettings.Get("vipCategoryId");
                if (productDetails.CategoryId.Equals(vipCategoryId))
                {
                    var redemptionLimitResult = RewardsHelper.CheckRedemptionLimitForVipCategory(UserSessionInfo.MemberCode, vipCategoryId);

                    switch (redemptionLimitResult)
                    {
                        case 0:
                            Errormsg = (string)HttpContext.GetLocalResourceObject(LocalResx, "lbl_redemption_success_limit_reached");
                            IsRedemptionLimitReached = true;
                            IsValidRedemption = false;
                            break;
                        case 1:
                            Errormsg = (string)HttpContext.GetLocalResourceObject(LocalResx, "lbl_redemption_processing_limit_reached");
                            IsProcessingLimitReached = true;
                            IsValidRedemption = false;
                            break;
                    }
                }
            }

            // Set label and image values
            if (!string.IsNullOrEmpty(productDetails.CurrencyCode))
            {
                lblCurrency.Text = productDetails.CurrencyCode;
                CurrencyDiv.Visible = true;
            }

            imgPic.ImageUrl = productDetails.ImageUrl;
            var builder = new StringBuilder();
            var points = !string.IsNullOrEmpty(productDetails.DiscountPoints) &&
                         int.Parse(productDetails.DiscountPoints) != 0
                ? productDetails.DiscountPoints
                : productDetails.PointsRequired;
            builder.Append(string.Format(@"{0:#,###,##0.##}", points))
                .Append(" ")
                .Append(HttpContext.GetLocalResourceObject(LocalResx, "lbl_points"));
            lblPointCenter.Text = builder.ToString();

            if (!string.IsNullOrEmpty(productDetails.DeliveryPeriod))
            {
                lblDelivery.Text = productDetails.DeliveryPeriod;
                DeliveryDiv.Visible = true;
            }

            lblDescription.Text = productDetails.ProductDescription;
            lblName.Text = productDetails.ProductName;

            // Set product cookie
            ProductHelper.SelectedProduct = productDetails;
        }
        catch (Exception exception)
        {
            var memberCode = UserSessionInfo == null ? string.Empty : UserSessionInfo.MemberCode;
            AuditTrail.AppendLog(memberCode, "/Catalogue/Detail.aspx", "RedeemProduct", "Catalogue", string.Empty, string.Empty, string.Empty, exception.Message, (new Guid()).ToString(), string.Empty, string.Empty, true);
        }
    }

    private DataSet GetProductDetails(MemberSession memberSession, string productId)
    {
        try
        {
            var riskId = memberSession == null ? "" : memberSession.RiskId;

            RedirectUri = !HasSession
                ? string.Format(@"/_Secure/Login.aspx?redirect=/Catalogue/Redeem.aspx&productId={0}", productId)
                : string.Format(@"/Catalogue/Redeem.aspx?productId={0}", productId);

            var detailsSet = RewardsHelper.GetProductDetails(MemberSession, productId);
            if (detailsSet.Tables.Count == 0 || detailsSet.Tables[0].Rows.Count == 0)
            {
                return null;
            }

            var dataRow = detailsSet.Tables[0].Rows[0];
            if (dataRow == null)
            {
                return null;
            }
            dataRow["pointsRequired"] = Convert.ToInt32(dataRow["pointsRequired"].ToString().Replace(" ", string.Empty));

            if (!detailsSet.Tables[0].Columns.Contains("pointsLeveldiscount"))
            {
                detailsSet.Tables[0].Columns.Add("pointsLeveldiscount");
                dataRow["pointsLeveldiscount"] = 0;
            }

            if (!detailsSet.Tables[0].Columns.Contains("pointsRequired2"))
            {
                detailsSet.Tables[0].Columns.Add("pointsRequired2");
                dataRow["pointsRequired2"] = dataRow["pointsRequired"];
            }

            if (!detailsSet.Tables[0].Columns.Contains("discountPercentage"))
            {
                detailsSet.Tables[0].Columns.Add("discountPercentage");
                dataRow["discountPercentage"] = 0;
            }

            if (dataRow["discountPoints"] == DBNull.Value)
            {
                if (HasSession && dataRow["productType"].ToString() != "1")
                {
                    //grab member point level
                    var pointLevelDiscount = RewardsHelper.GetMemberPointLevelDiscount(MemberSession);
                    var percentage = Convert.ToDouble(pointLevelDiscount) / 100;
                    var normalPoint = int.Parse(dataRow["pointsRequired"].ToString());
                    var points = Math.Floor(normalPoint * (1 - percentage));
                    var pointAfterLevelDiscount = Convert.ToInt32(points);

                    dataRow["pointsRequired"] = pointAfterLevelDiscount;
                    dataRow["pointsLeveldiscount"] = pointAfterLevelDiscount;
                    dataRow["discountPercentage"] = pointLevelDiscount;
                }    
            }

            dataRow["imageName"] =
                Convert.ToString(
                    System.Configuration.ConfigurationManager.AppSettings.Get("ImagesDirectoryPath") +
                    "Product/" + dataRow["imageName"]);

            if (!string.IsNullOrEmpty(riskId))
            {
                //valid category
                dataRow["redemptionValidityCat"] += ",";
                if (dataRow["redemptionValidityCat"].ToString().ToUpper() != "ALL,")
                {
                    if (!((string)dataRow["redemptionValidityCat"]).Contains(riskId + ","))
                    {
                        dataRow["redemptionValidityCat"] = "0";
                    }
                    else
                    {
                        dataRow["redemptionValidityCat"] = "1";
                    }
                }
                else
                {
                    dataRow["redemptionValidityCat"] = "1";
                }

                dataRow["redemptionValidity"] += ",";
                if (dataRow["redemptionValidity"].ToString().ToUpper() != "ALL,")
                {
                    if (!((string)dataRow["redemptionValidity"]).Contains(riskId + ","))
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
                dataRow["redemptionValidityCat"] += "0";
            }
            
            return detailsSet;
        }
        catch (Exception exception)
        {
            var memberCode = UserSessionInfo == null ? string.Empty : UserSessionInfo.MemberCode;
            AuditTrail.AppendLog(memberCode, "/Catalogue/Detail.aspx", "RedeemProduct", "Catalogue", string.Empty, string.Empty, string.Empty, exception.Message, (new Guid()).ToString(), string.Empty, string.Empty, true);
            return null;
        }
    }

    protected override void SetLabels()
    {
        VipOnly = HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem_vip").ToString();

        #region labels
        if (!HasSession && UserSessionInfo == null)
        {
            return;
        }

        if (string.IsNullOrEmpty(MemberSession.FirstName))
        {
            usernameLabel.Visible = false;
        }
        else
        {
            usernameLabel.InnerText = UserSessionInfo.MemberCode;
        }
        var pointsLabelText = (string)HttpContext.GetLocalResourceObject(LocalResx, "lbl_points");
        var stringBuilder = new StringBuilder();

        stringBuilder.Append(pointsLabelText)
            .Append(": ")
            .Append(MemberRewardsInfo != null ? Convert.ToString(MemberRewardsInfo.CurrentPoints) : "0");
        pointsLabel.InnerText = stringBuilder.ToString();

        var pointLevelLabelText = (string)HttpContext.GetLocalResourceObject(HeaderResx, "lbl_point_level");
        stringBuilder = new StringBuilder();
        stringBuilder.Append(pointLevelLabelText)
            .Append(" ")
            .Append(MemberRewardsInfo != null ? Convert.ToString(MemberRewardsInfo.CurrentPointLevel) : "0");
        pointLevelLabel.InnerText = stringBuilder.ToString();
        divLevel.Visible = true;
        #endregion

        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("id")))
        {
            lblDescription.Text = HttpContext.Current.Request.QueryString.Get("id");
        }

        const string colon = ":";
        lbcurr.Text = HttpContext.GetLocalResourceObject(LocalResx, "lbl_currency") + colon;
        lbperiod.Text = HttpContext.GetLocalResourceObject(LocalResx, "lbl_delivery_period").ToString();
    }
}