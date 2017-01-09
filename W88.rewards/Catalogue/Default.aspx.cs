using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Services;
using W88.BusinessLogic.Accounts.Models;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.BusinessLogic.Shared.Helpers;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities;

public partial class Default : CatalogueBasePage
{
    protected string CategoryId = string.Empty;
    protected int MinPoints = 0;
    protected int MaxPoints = 0;
    protected string PageSize = string.Empty;
    protected string SearchText = string.Empty;
    protected string SortBy = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        SetLabels();
        SetCatalogueList();
        GetProducts();
    }

    private async void SetCatalogueList()
    {
        try
        {
            var catalogueSet = await RewardsHelper.GetCatalogueSet(UserSessionInfo);
            CategoryListView.DataSource = catalogueSet.Tables[0];
            var dataTable = (DataTable) CategoryListView.DataSource;
            var dataRowAll = dataTable.NewRow();
            dataRowAll["categoryId"] = "0";
            dataRowAll["categoryName"] = RewardsHelper.GetTranslation(TranslationKeys.Label.All);
            dataRowAll["imagePathOn"] = string.Empty;
            dataRowAll["imagePathOff"] = string.Empty;
            dataTable.Rows.InsertAt(dataRowAll, 0);
            CategoryListView.DataBind();
        }
        catch (Exception)
        {
            lblnodata.Text = RewardsHelper.GetTranslation(TranslationKeys.Redemption.NoAvailableItems);
            lblnodata.Visible = true; 
        }
    }

    private async void GetProducts()
    {        
        try {
            CategoryId = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("categoryId")) ? "0" : HttpContext.Current.Request.QueryString.Get("categoryId");
            MinPoints = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("min")) ? 0 : int.Parse(HttpContext.Current.Request.QueryString.Get("min"));
            MaxPoints = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("max")) ? 2000000 : int.Parse(HttpContext.Current.Request.QueryString.Get("max"));
            PageSize = string.IsNullOrEmpty(Common.GetAppSetting<string>("catalogueSize")) ? "10" : Common.GetAppSetting<string>("catalogueSize");
            SearchText = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("search")) ? "" : HttpContext.Current.Request.QueryString.Get("search");
            SortBy = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("sortBy")) ? string.Empty : HttpContext.Current.Request.QueryString.Get("sortBy");

            var searchInfo = new SearchInfo
            {
                CategoryId = CategoryId,
                Index = "0",
                MinPoints = MinPoints,
                MaxPoints = MaxPoints,
                PageSize = PageSize,
                SearchText = SearchText,
                SortBy = SortBy
            };

            if (UserSessionInfo == null)
            {
                UserSessionInfo = new UserSessionInfo();
                UserSessionInfo.CountryCode = RewardsHelper.GetCountryCode();
            }
            var result = await RewardsHelper.SearchProducts(searchInfo, UserSessionInfo);
            var products = result.Data != null ? (List<ProductDetails>) result.Data : null;
            if (result.Code == (int)Constants.StatusCode.Error || products == null)
            {
                lblnodata.Text = RewardsHelper.GetTranslation(TranslationKeys.Redemption.NoAvailableItems);
                lblnodata.Visible = true;
                return;            
            }

            lblnodata.Visible = false;
            listContainer.InnerHtml = CreateHtml(products);
        }
        catch (Exception)
        {
            lblnodata.Text = RewardsHelper.GetTranslation(TranslationKeys.Redemption.NoAvailableItems);
            lblnodata.Visible = true;
        }
    }

    protected override void SetLabels()
    {
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
    }

    [WebMethod]
    public static string CreateHtml(List<ProductDetails> products)
    {
        try
        {
            var htmlBuilder = new StringBuilder();
            if (products == null || products.Count == 0)
            {
                return string.Empty;
            }

            foreach (var product in products)
            {
                var id = string.Format("detailsButton_{0}", product.ProductId);
                htmlBuilder.Append(string.Format("<div id=\"{0}\" class=\"col-xs-6 col-sm-3\">", id))
                    .Append("<div class=\"catalog-box\">")
                    .Append("<script>$(function(){")
                    .Append(string.Format("$('#{0}').on('click',function(){{window.location.href='/Catalogue/Detail.aspx?id={1}';}});", id, product.ProductId))
                    .Append(string.Format("var labelTag = $('#labelTag_{0}');", product.ProductId))
                    .Append("if(_.isEmpty(labelTag.html().trim()))labelTag.removeClass('tag-label');});</script>")
                    .Append(string.Format("<div class=\"catalog-image\"><img src=\"{0}\" alt=\"\"></div>",
                        product.ImageUrl))
                    .Append("<div class=\"catalog-details\">")
                    .Append(string.Format("<h4>{0}</h4>", product.ProductName))
                    .Append("<small>")
                    .Append(string.Format("<span class=\"points\" style=\"{0}\">", !string.IsNullOrEmpty(product.DiscountPoints) ? "text-decoration:line-through;" : "text-decoration:none;"))
                    .Append(string.Format("{0:#,###,##0.##}", product.PointsRequired))
                    .Append(string.Format(" {0}</span>", RewardsHelper.GetTranslation(TranslationKeys.Label.Points).ToLower()))
                    .Append(string.Format("<span class=\"newpoints\" style=\"{0}\">", !string.IsNullOrEmpty(product.DiscountPoints) ? "visibility:visible;" : "visibility:hidden;"))
                    .Append(string.Format("{0:#,###,##0.##}", product.DiscountPoints))
                    .Append(string.Format(" {0}</span>", RewardsHelper.GetTranslation(TranslationKeys.Label.Points).ToLower()))
                    .Append(string.Format("<span id=\"labelTag_{0}\" class=\"tag-label\">", product.ProductId));

                switch (product.ProductIcon)
                {
                    case "2":
                        htmlBuilder.Append(RewardsHelper.GetTranslation(TranslationKeys.Label.Hot));
                        break;
                    case "3":
                        htmlBuilder.Append(RewardsHelper.GetTranslation(TranslationKeys.Label.New));
                        break;
                }

                htmlBuilder.Append("</span></small></div></div></div>");
            }
            return htmlBuilder.ToString();
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }
}






