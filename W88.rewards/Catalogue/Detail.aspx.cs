using System;
using System.Web;
using System.Text;
using W88.BusinessLogic.Rewards.Helpers;
using W88.Utilities;
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
        SetProductInfo();     
    }

    private async void SetProductInfo()
    {
        try
        {
            var productId = HttpContext.Current.Request.QueryString.Get("id");
            var productDetails = await RewardsHelper.GetProductDetails(MemberSession, productId, HasSession);

            if (productDetails == null)
            {
                return;
            }

            RedirectUri = !HasSession
                ? string.Format(@"/_Secure/Login.aspx?redirect=/Catalogue/Redeem.aspx&productId={0}", productId)
                : string.Format(@"/Catalogue/Redeem.aspx?productId={0}", productId);

            if (productDetails.RedemptionValidity == "1" && productDetails.RedemptionValidityCategory == "1")
            {
                IsValidRedemption = true;
                var vipCategoryId = Common.GetAppSetting<string>("vipCategoryId");
                if (productDetails.CategoryId.Equals(vipCategoryId))
                {
                    var redemptionLimitResult = await RewardsHelper.CheckRedemptionLimitForVipCategory(UserSessionInfo.MemberCode, vipCategoryId);

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

    protected override void SetLabels()
    {
        VipOnly = HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem_vip").ToString();
        const string colon = ":";
        lbcurr.Text = HttpContext.GetLocalResourceObject(LocalResx, "lbl_currency") + colon;
        lbperiod.Text = HttpContext.GetLocalResourceObject(LocalResx, "lbl_delivery_period").ToString();

        #region labels
        if (!HasSession)
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