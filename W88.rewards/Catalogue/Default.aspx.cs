using System;
using System.Data;
using System.Text;
using System.Web;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.Utilities;

public partial class Default : CatalogueBasePage
{

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
            var catalogueSet = await RewardsHelper.GetCatalogueSet(MemberSession);
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
            var categoryId = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("categoryId")) ? "0" : HttpContext.Current.Request.QueryString.Get("categoryId");
            var sortBy = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("sortBy")) ? "" : HttpContext.Current.Request.QueryString.Get("sortBy");
            var pointsFrom = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("min")) ? 0 : int.Parse(HttpContext.Current.Request.QueryString.Get("min"));
            var pointsTo = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("max")) ? 2000000 : int.Parse(HttpContext.Current.Request.QueryString.Get("max"));
            var searchText = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("search")) ? "" : HttpContext.Current.Request.QueryString.Get("search");
            const string pageNumber = "0";

            var catalogueSize = Common.GetAppSetting<string>("catalogueSize");
            var pageSize = string.IsNullOrEmpty(catalogueSize) ? "100" : catalogueSize;

            var dataSet = await RewardsHelper.GetProductSearch(
                MemberSession, 
                categoryId, 
                pointsFrom, 
                pointsTo, 
                searchText,
                sortBy, 
                pageSize, 
                pageNumber);

            if (dataSet == null)
            {
                lblnodata.Text = RewardsHelper.GetTranslation(TranslationKeys.Redemption.NoAvailableItems);
                lblnodata.Visible = true;
                return;            
            }
           
            ListviewProduct.DataSource = dataSet.Tables[0];
            ListviewProduct.DataBind();
            lblnodata.Visible = false;
        }
        catch (Exception ex)
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

        if (string.IsNullOrEmpty(MemberSession.FirstName))
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
}






