using System;
using System.Data;
using System.Web;
using System.Text;
using W88.Rewards.BusinessLogic.Rewards.Models;
using W88.Rewards.BusinessLogic.Shared.Helpers;
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
        GetProductDetails();     
    }

    private void GetProductDetails()
    {
        try
        {
            var currencyCode = MemberSession == null ? "" : MemberSession.CurrencyCode;
            var riskId = MemberSession == null ? "" : MemberSession.RiskId;
            var productId = HttpContext.Current.Request.QueryString.Get("id");
            var productDetails = new ProductDetails();
            
            productDetails.ProductId = productId;

            RedirectUri = !HasSession
                ? string.Format("/_Secure/Login.aspx?redirect=/Catalogue/Redeem&productId={0}", productId)
                : string.Format("/Catalogue/Redeem.aspx?productId={0}", productId);

            var detailsSet = RewardsHelper.GetProductDetails(MemberSession, productId);
            if (detailsSet.Tables.Count == 0 || detailsSet.Tables[0].Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow dataRow in detailsSet.Tables[0].Rows)
            {
                productDetails.ProductType = dataRow["productType"].ToString();
                productDetails.AmountLimit = dataRow["amountLimit"].ToString();
                productDetails.CategoryId = dataRow["categoryId"].ToString();
                productDetails.CurrencyCode = currencyCode;

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

                if (dataRow["discountPoints"] != DBNull.Value)
                {
                    productDetails.PointsRequired = dataRow["discountPoints"].ToString();
                }
                else
                {
                    if (!HasSession && dataRow["productType"].ToString() != "1")
                    {
                        //grab member point level
                        var pointLevelDiscount = RewardsHelper.GetMemberPointLevelDiscount(MemberSession);
                        var percentage = Convert.ToDouble(pointLevelDiscount)/100;
                        var normalPoint = int.Parse(dataRow["pointsRequired"].ToString());
                        var points = Math.Floor(normalPoint*(1 - percentage));
                        var pointAfterLevelDiscount = Convert.ToInt32(points);

                        dataRow["pointsRequired"] = pointAfterLevelDiscount;
                        dataRow["pointsLeveldiscount"] = pointAfterLevelDiscount;
                        dataRow["discountPercentage"] = pointLevelDiscount;

                        productDetails.PointsRequired = dataRow["pointsRequired"].ToString();
                        productDetails.PointsLevelDiscount = dataRow["pointsRequired"].ToString();
                    }
                    else
                    {
                        productDetails.PointsRequired = dataRow["pointsRequired"].ToString();
                    }
                }

                dataRow["currencyValidity"] = currencyCode;
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
                        if (!((string) dataRow["redemptionValidityCat"]).Contains(riskId + ","))
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
                        if (!((string) dataRow["redemptionValidity"]).Contains(riskId + ","))
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

                if (dataRow["redemptionValidity"].ToString() == "1" && dataRow["redemptionValidityCat"].ToString() == "1")
                {
                    IsValidRedemption = true;
                    var vipCategoryId = System.Configuration.ConfigurationManager.AppSettings.Get("vipCategoryId");                    
                    if (dataRow["categoryId"].ToString().Equals(vipCategoryId))
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


                imgPic.ImageUrl = dataRow["imageName"].ToString();

                if (!string.IsNullOrEmpty(dataRow["discountPoints"].ToString()) &&
                    int.Parse(dataRow["discountPoints"].ToString()) != 0)
                {
                    lblPointCenter.Text = String.Format("{0:#,###,##0.##}", dataRow["discountPoints"]) + " " + 
                        HttpContext.GetLocalResourceObject(LocalResx, "lbl_points");
                }
                else
                {
                    lblPointCenter.Text = String.Format("{0:#,###,##0.##}", dataRow["pointsRequired"]) + " " + 
                        HttpContext.GetLocalResourceObject(LocalResx, "lbl_points");
                }

                lblName.Text = dataRow["productName"].ToString();
                lblDescription.Text = dataRow["productDescription"].ToString();
                if (!string.IsNullOrEmpty(dataRow["deliveryPeriod"].ToString()))
                {
                    lblDelivery.Text = HttpContext.GetLocalResourceObject(LocalResx, "lbl_delivery_period") + (dataRow["deliveryPeriod"].ToString());
                }
            }
            // Set product cookie
            CookieHelpers.ProductCookie = productDetails;
        }
        catch (Exception exception)
        {
            var memberCode = UserSessionInfo == null ? string.Empty : UserSessionInfo.MemberCode;
            AuditTrail.AppendLog(memberCode, "/Catalogue/Detail.aspx", "RedeemProduct", "Catalogue", string.Empty, string.Empty, string.Empty, exception.Message, (new Guid()).ToString(), string.Empty, string.Empty, true);
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
    }
}