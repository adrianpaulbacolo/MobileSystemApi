using System;
using System.Web;
using System.Text;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities;
using W88.Utilities.Log.Helpers;

public partial class Catalogue_Detail : CatalogueBasePage
{
    protected string RedirectUri = "/Catalogue/Detail.aspx";
    protected bool IsValidRedemption = false;
    protected bool IsLimitReached = false;
    protected bool IsPending = false;
    protected bool IsVipOnly = false;
    protected string VipOnlyMessage = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }
        SetLabels();
        SetProductInfo();     
    }

    private async void SetProductInfo()
    {
        try
        {
            var productId = HttpContext.Current.Request.QueryString.Get("id");

            if (string.IsNullOrEmpty(productId))
            {
                Response.Redirect("/Catalogue?categoryId=0&sortBy=2", false);
                return;
            }

            RedirectUri = !HasSession
                            ? string.Format(@"/_Secure/Login.aspx?redirect=/Catalogue/Redeem.aspx&productId={0}", productId)
                            : string.Format(@"/Catalogue/Redeem.aspx?productId={0}", productId);

            var process = await RewardsHelper.GetProductDetails(UserSessionInfo, productId);
            if (process == null || process.Data == null)
            {
                return;
            }

            var productDetails = (ProductDetails) process.Data;
            // Set label and image values
            if (!string.IsNullOrEmpty(productDetails.CurrencyCode))
            {
                lblCurrency.Text = productDetails.CurrencyCode;
                CurrencyDiv.Visible = true;
            }

            imgPic.ImageUrl = productDetails.ImageUrl;

            var pointsLabelText = RewardsHelper.GetTranslation(TranslationKeys.Label.Points);
            if (!string.IsNullOrEmpty(productDetails.DiscountPoints) && int.Parse(productDetails.DiscountPoints) != 0)
            {
                lblPointCenter.Text = string.Format(@"{0:#,###,##0.##} {1}", productDetails.DiscountPoints, pointsLabelText);
                lblBeforeDiscount.Text = string.Format(@"{0:#,###,##0.##} {1}", productDetails.PointsRequired, pointsLabelText);
            }
            else
            {
                lblPointCenter.Text = string.Format(@"{0:#,###,##0.##} {1}", productDetails.PointsRequired, pointsLabelText);
                lblBeforeDiscount.Text = string.Empty;
            }

            if (!string.IsNullOrEmpty(productDetails.DeliveryPeriod))
            {
                lblDelivery.Text = productDetails.DeliveryPeriod;
                DeliveryDiv.Visible = true;
            }

            lblDescription.Text = productDetails.ProductDescription;
            lblName.Text = productDetails.ProductName;

            IsVipOnly = productDetails.Status == (int)Constants.ProductStatus.VipOnly;
            IsValidRedemption = !IsVipOnly;
            VipOnlyMessage = RewardsHelper.GetTranslation(TranslationKeys.Redemption.VipOnly);

            var vipCategoryId = Common.GetAppSetting<string>("vipCategoryId");
            if (!productDetails.CategoryId.Equals(vipCategoryId))
            {
                return;
            }
            var redemptionLimitResult = await RewardsHelper.CheckRedemptionLimitForVipCategory(UserSessionInfo.MemberCode, vipCategoryId);
            switch (redemptionLimitResult)
            {
                case 0:
                    VipOnlyMessage = RewardsHelper.GetTranslation(TranslationKeys.Redemption.BirthdayItemRedeemed);
                    IsLimitReached = true;
                    break;
                case 1:
                    VipOnlyMessage = RewardsHelper.GetTranslation(TranslationKeys.Redemption.BirthdayItemPending);
                    IsPending = true;
                    break;
            }          
        }
        catch (Exception exception)
        {
            AuditTrail.AppendLog(exception);
        }
    }

    protected override void SetLabels()
    {
        const string colon = ":";
        lbcurr.Text = RewardsHelper.GetTranslation(TranslationKeys.Redemption.Currency) + colon;
        lbperiod.Text = RewardsHelper.GetTranslation(TranslationKeys.Redemption.Delivery);

        #region labels
        if (!HasSession)
        {
            return;
        }

        if (string.IsNullOrEmpty(MemberSession.FullName))
        {
            usernameLabel.Visible = false;
        }
        else
        {
            usernameLabel.InnerText = UserSessionInfo.MemberCode;
        }

        pointsLabel.InnerText = string.Format("{0}: {1}",
            RewardsHelper.GetTranslation(TranslationKeys.Label.Points),
            MemberRewardsInfo != null ? Convert.ToString(MemberRewardsInfo.CurrentPoints) : "0");
        pointLevelLabel.InnerText = string.Format("{0} {1}",
            RewardsHelper.GetTranslation(TranslationKeys.Label.PointLevel),
            MemberRewardsInfo != null ? Convert.ToString(MemberRewardsInfo.CurrentPointLevel) : "0");
        divLevel.Visible = true;
        #endregion

        if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("id")))
        {
            lblDescription.Text = HttpContext.Current.Request.QueryString.Get("id");
        }
    }
}