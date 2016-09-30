using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Text;
using System.Web.UI;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Redemption.Factories;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities;
using W88.Utilities.Constant;
using W88.Utilities.Log.Helpers;
using W88.WebRef.RewardsServices;
using W88.BusinessLogic.Rewards.Models;
using RedemptionRequest = W88.BusinessLogic.Rewards.Redemption.Model.RedemptionRequest;

public partial class Catalogue_Redeem : CatalogueBasePage
{
    protected string AlertCode = string.Empty;
    protected string AlertMessage = string.Empty;
    protected string ProductType = string.Empty;
    protected string VipOnly = string.Empty;
    private readonly string _operatorId = Settings.OperatorId.ToString(CultureInfo.InvariantCulture);
    protected ProductDetails ProductDetails = ProductHelper.SelectedProduct;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }
        SetLabels();
        SetProductInfo();
        InitFields();       
    }

    protected async void RedeemButtonOnClick(object sender, EventArgs e)
    {
        AlertCode = string.Empty;
        AlertMessage = string.Empty;
        var memberCode = UserSessionInfo == null ? string.Empty : UserSessionInfo.MemberCode;
        var productType = (ProductTypeEnum)int.Parse(ProductDetails.ProductType);
        var quantitytext = tbQuantity.Text.Trim();
        int quantity;

        var isParsed = int.TryParse(quantitytext, out quantity);
        if (!isParsed)
        {
            AlertCode = Result.Fail;
            AlertMessage = (string)HttpContext.GetLocalResourceObject(LocalResx, "lbl_invalid_number");     
            return;
        }

        quantity = int.Parse(quantitytext);
        if (quantity < 1)
        {
            AlertCode = Result.Fail;
            AlertMessage = (string)HttpContext.GetLocalResourceObject(LocalResx, "lbl_invalid_at_least");
            return;
        }

        #region redeem product
        try
        {
            var process = await RedemptionStrategy.Initialize(GetRequest(productType)).Redeem();
            if (process.Code != (int)Constants.StatusCode.Success)
            {
                AlertCode = Result.Fail;
                AlertMessage = (string)HttpContext.GetLocalResourceObject(LocalResx, "lblPointCheckError");
                return;
            }

            var response = (RedemptionResponse)process.Data;
            switch (response.Result)
            {
                case RedemptionResultEnum.ConcurrencyDetected:
                    AlertCode = Result.Fail;
                    AlertMessage = (string)HttpContext.GetLocalResourceObject(LocalResx, "lblPointCheckError");
                    break;
                case RedemptionResultEnum.LimitReached:
                    AlertCode = Result.Fail;
                    AlertMessage = (string)HttpContext.GetLocalResourceObject(LocalResx, "lbl_redemption_limit_reached");
                    break;
                case RedemptionResultEnum.VIPSuccessLimitReached:
                    AlertCode = Result.Fail;
                    AlertMessage = (string)HttpContext.GetLocalResourceObject(LocalResx, "lbl_redemption_success_limit_reached");
                    break;
                case RedemptionResultEnum.VIPProcessingLimitReached:
                    AlertCode = Result.Fail;
                    AlertMessage = (string)HttpContext.GetLocalResourceObject(LocalResx, "lblPoints_insufficient");
                    break;
                case RedemptionResultEnum.PointIsufficient:
                    AlertCode = Result.Fail;
                    AlertMessage = (string)HttpContext.GetLocalResourceObject(LocalResx, "lblPoints_insufficient");
                    break;
                case RedemptionResultEnum.Success:
                    AlertCode = Result.Success;
                    if (productType == ProductTypeEnum.Freebet) //Freebet success
                    {
                        foreach (var redemptionItemId in response.RedemptionIds)
                        {
                            SendMail(memberCode, redemptionItemId.ToString(CultureInfo.InvariantCulture));
                        }
                        AlertMessage = (string)HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem_success_processed");
                    }
                    else
                    {
                        AlertMessage = (string)HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem_success_submit");
                    }
                    break;
                case RedemptionResultEnum.UnknownError:
                    AlertCode = Result.Fail;
                    AlertMessage = (string)HttpContext.GetLocalResourceObject(LocalResx, "lblPointCheckError");
                    break;
                case RedemptionResultEnum.PointCheckError:
                    AlertCode = Result.Fail;
                    AlertMessage = (string)HttpContext.GetLocalResourceObject(LocalResx, "lblPointCheckError");
                    break;
            }            
        }
        catch (Exception exception)
        {
            AlertCode = Result.Fail;
            AlertMessage = (string)HttpContext.GetLocalResourceObject(LocalResx, "lbl_Exception");
            AuditTrail.AppendLog(exception);
        }
        finally
        {
            RefreshPoints();
            ShowMessage(AlertCode, AlertMessage);
        }
        #endregion
    }

    protected override void SetLabels()
    {
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
    }

    private void InitFields()
    {
        #region fields
        const string colon = ":";
        lbcat.Text = HttpContext.GetLocalResourceObject(LocalResx, "lbl_category") + colon;
        lbproduct.Text = HttpContext.GetLocalResourceObject(LocalResx, "lbl_product") + colon;
        lbcurr.Text = HttpContext.GetLocalResourceObject(LocalResx, "lbl_currency") + colon;
        lbpoint.Text = HttpContext.GetLocalResourceObject(LocalResx, "lbl_points") + colon;
        lbperiod.Text = HttpContext.GetLocalResourceObject(LocalResx, "lbl_delivery_period") + colon;
        lbqty.Text = HttpContext.GetLocalResourceObject(LocalResx, "lbl_quantity") + colon;
        lbaccount.Text = HttpContext.GetLocalResourceObject(LocalResx, "lbl_account") + colon;
        tbRName.Attributes.Add("PLACEHOLDER", HttpContext.GetLocalResourceObject(LocalResx, "lbl_enter_name").ToString());
        tbAddress.Attributes.Add("PLACEHOLDER", HttpContext.GetLocalResourceObject(LocalResx, "lbl_enter_address").ToString());
        tbPostal.Attributes.Add("PLACEHOLDER", HttpContext.GetLocalResourceObject(LocalResx, "lbl_enter_postal").ToString());
        tbCity.Attributes.Add("PLACEHOLDER", HttpContext.GetLocalResourceObject(LocalResx, "lbl_enter_city").ToString());
        tbCountry.Attributes.Add("PLACEHOLDER", HttpContext.GetLocalResourceObject(LocalResx, "lbl_enter_country").ToString());
        tbContact.Attributes.Add("PLACEHOLDER", HttpContext.GetLocalResourceObject(LocalResx, "lbl_enter_contact").ToString());
        redeemButton.Text = HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem_now").ToString();
        redeemButton.Visible = true;
        #endregion
    }

    private async void SetProductInfo()
    {
        var memberCode = UserSessionInfo == null ? "" : UserSessionInfo.MemberCode;
        var countryCode = MemberSession == null ? "0" : MemberSession.CountryCode;
        var currencyCode = MemberSession == null ? "0" : MemberSession.CurrencyCode;

        try
        {
            ProductType = ProductDetails.ProductType;
            if (ProductType == "1" && (ProductDetails.CurrencyCode != currencyCode))
            {
                Response.Redirect("/Catalogue?categoryId=53&sortBy=2", false);
                return;
            }

            if (ProductDetails.CountryCode.Trim() != "All")
            {
                if (!ProductDetails.CountryCode.Contains(countryCode))
                {
                    Response.Redirect(string.Format("/Catalogue?categoryId={0}&sortBy=2", ProductDetails.CategoryId), false);
                    return;
                }
            }

            if (!(ProductDetails.RedemptionValidity == "1" && ProductDetails.RedemptionValidityCategory == "1"))
            {
                AlertCode = Result.Vip;
                VipOnly = HttpContext.GetLocalResourceObject(LocalResx, "lbl_redeem_vip").ToString();
                return;
            }

            lblproductid.Value = ProductDetails.ProductId;
            /**
                freebet show  currency, hide recipient panel , hide delivery, hide account
                normal product show recipient, show delivery if any,  hide currency, hide account
                online show account, hide delivery, hide recipient, hide currency
            **/
            switch (ProductType)
            {
                case "1": //freebet
                    CurrencyDiv.Visible = true;
                    RecipientDiv.Visible = false;
                    DeliveryDiv.Visible = false;
                    AccountDiv.Visible = false;
                    break;
                case "2": //normal
                    RecipientDiv.Visible = true;
                    CurrencyDiv.Visible = false;
                    AccountDiv.Visible = false;
                    break;
                case "3": //wishlist same as normal
                    RecipientDiv.Visible = true;
                    CurrencyDiv.Visible = false;
                    AccountDiv.Visible = false;
                    break;
                case "4": //online
                    AccountDiv.Visible = true;
                    CurrencyDiv.Visible = false;
                    RecipientDiv.Visible = false;
                    DeliveryDiv.Visible = false;
                    break;
            }

            //vip cannot select quantity
            var vipCategoryId = Common.GetAppSetting<string>("vipCategoryId");
            if (ProductDetails.CategoryId == vipCategoryId)
            {
                tbQuantity.Enabled = false;
            }

            imgPic.ImageUrl = ProductDetails.ImageUrl;

            if (!string.IsNullOrEmpty(ProductDetails.DiscountPoints) && int.Parse(ProductDetails.DiscountPoints) != 0)
            {
                var builder = new StringBuilder();
                // Before discount
                builder.Append(string.Format(@"{0:#,###,##0.##}", ProductDetails.PointsRequired))
                    .Append(" ")
                    .Append(HttpContext.GetLocalResourceObject(LocalResx, "lbl_points"));
                lblBeforeDiscount.Text = builder.ToString();

                builder = new StringBuilder();
                builder.Append(string.Format(@"{0:#,###,##0.##}", ProductDetails.DiscountPoints))
                    .Append(" ")
                    .Append((string) HttpContext.GetLocalResourceObject(LocalResx, "lblPoints"));
                lblPointCenter.Text = builder.ToString();
            }
            else
            {
                lblBeforeDiscount.Text = string.Empty;
                var builder = new StringBuilder();
                // Before discount
                builder.Append(string.Format(@"{0:#,###,##0.##}", ProductDetails.PointsRequired))
                    .Append(" ")
                    .Append(HttpContext.GetLocalResourceObject(LocalResx, "lbl_points"));
                lblPointCenter.Text = builder.ToString();
            }

            lblName.Text = ProductDetails.ProductName;
            lblCategory.Text = ProductDetails.ProductCategoryName;

            if (!string.IsNullOrEmpty(ProductDetails.DeliveryPeriod))
            {
                lblDelivery.Text = ProductDetails.DeliveryPeriod;
                DeliveryDiv.Visible = true;
            }

            if (!string.IsNullOrEmpty(ProductDetails.CurrencyCode))
            {
                lblCurrency.Text = ProductDetails.CurrencyCode;
            }

            #region memberInfo
            var redemptionDetails = await RewardsHelper.GetMemberRedemptionDetails(memberCode);
            if (redemptionDetails == null)
            {
                return;
            }

            if (ProductType == "2" || ProductType == "3") //normal & wishlist
            {
                tbRName.Text = redemptionDetails.FullName;
                tbAddress.Value = redemptionDetails.Address;                   
                tbPostal.Text = redemptionDetails.Postal;
                tbCity.Text = redemptionDetails.City;
                tbCountry.Text = redemptionDetails.CountryCode;
                tbContact.Text = redemptionDetails.Mobile;
            }
            #endregion
        }
        catch (Exception exception)
        {
            AuditTrail.AppendLog(exception);
        }
    }

    private RedemptionRequest GetRequest(ProductTypeEnum type)
    {
        var request = new RedemptionRequest();
        request.ProductType = type;
        request.MemberCode = UserSessionInfo == null ? string.Empty : UserSessionInfo.MemberCode;
        request.ProductId = lblproductid.Value;
        request.CategoryId = string.IsNullOrEmpty(ProductDetails.CategoryId) ? "0" : ProductDetails.CategoryId;
        request.RiskId = MemberSession == null ? "0" : MemberSession.RiskId;
        request.Currency = string.IsNullOrEmpty(ProductDetails.CurrencyCode) ? "0" : ProductDetails.CurrencyCode;
        request.PointRequired = string.IsNullOrEmpty(ProductDetails.PointsRequired) ? string.Empty : ProductDetails.PointsRequired;
        request.Quantity = tbQuantity.Text.Trim();

        switch (type)
        {
            case ProductTypeEnum.Freebet:
                request.CreditAmount = string.IsNullOrEmpty(ProductDetails.AmountLimit) ? string.Empty : ProductDetails.AmountLimit;
                break;
            case ProductTypeEnum.Normal:
                request.Name = tbRName.Text.Trim();
                request.ContactNumber = tbContact.Text.Trim();
                request.Address = tbAddress.Value.Trim();
                request.PostalCode = tbPostal.Text.Trim();
                request.City = tbCity.Text.Trim();
                request.Country = tbCountry.Text.Trim();
                break;
            case ProductTypeEnum.Online:
                request.AimId = tbAccount.Text.Trim();
                break;
        }

        return request;
    }

    private async void RefreshPoints()
    {
        if (MemberRewardsInfo == null)
        {
            MemberRewardsInfo = new MemberRewardsInfo();
        }
        MemberRewardsInfo.CurrentPoints = await MembersHelper.GetRewardsPoints(UserSessionInfo);
        MemberRewardsInfo.CurrentPointLevel = await RewardsHelper.GetPointLevel(MemberSession.MemberId);
        SetLabels();
        SetProductInfo();
    }

    private void ShowMessage(string alertCode, string alertMessage)
    {
        var scriptBuilder = new StringBuilder();
        scriptBuilder.Append("setTimeout(function() {showMessage('")
            .Append(alertCode + "','")
            .Append(alertMessage + "');}, 300);");
        ScriptManager.RegisterStartupScript(Page, GetType(), (new Guid()).ToString(), scriptBuilder.ToString(), true);
    }

    private void SendMail(string memberCode, string redemptionId)
    {
        var fields = new Dictionary<string, string>();
        var language = MemberSession.LanguageCode;
        var localResxMail = "~/redemption_mail.{0}.aspx";
        localResxMail = string.Format(localResxMail, (string.IsNullOrEmpty(language) ? LanguageHelpers.SelectedLanguage : language));
        fields["Subject"] = HttpContext.GetLocalResourceObject(localResxMail, "lbl_subject") == null 
            ? string.Empty : (string)HttpContext.GetLocalResourceObject(localResxMail, "lbl_subject");
        fields["Body"] = HttpContext.GetLocalResourceObject(localResxMail, "lbl_body") == null 
            ? string.Empty : string.Format((string)HttpContext.GetLocalResourceObject(localResxMail, "lbl_body"), memberCode.Trim(), redemptionId);
        // Send mail
        RewardsHelper.SendMail(fields, memberCode);
    }

    static private class Result
    {
        public const string Success = "SUCCESS";
        public const string Fail = "FAIL";
        public const string Vip = "VIP";
    }
}





