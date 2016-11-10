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

            var productDetails = await RewardsHelper.GetProductDetails(UserSessionInfo, productId);
            if (productDetails == null)
            {
                return;
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
            builder.Append(string.Format(@"{0:#,###,##0.##} ", points))
                .Append(RewardsHelper.GetTranslation(TranslationKeys.Label.Points));
            lblPointCenter.Text = builder.ToString();

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
        var pointsLabelText = RewardsHelper.GetTranslation(TranslationKeys.Label.Points);
        var stringBuilder = new StringBuilder();

        stringBuilder.Append(pointsLabelText)
            .Append(": ")
            .Append(MemberRewardsInfo != null ? Convert.ToString(MemberRewardsInfo.CurrentPoints) : "0");
        pointsLabel.InnerText = stringBuilder.ToString();

        var pointLevelLabelText = RewardsHelper.GetTranslation(TranslationKeys.Label.PointLevel);
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