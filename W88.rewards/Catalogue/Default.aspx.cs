using System;
using System.Data;
using System.Dynamic;
using System.Web;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.Utilities;

public partial class Default : CatalogueBasePage
{
    protected dynamic Params
    {
        get
        {
            dynamic _params = new ExpandoObject();
            _params.CategoryId = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("categoryId")) ? "0" : HttpContext.Current.Request.QueryString.Get("categoryId");
            _params.MinPoints = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("min")) ? 0 : int.Parse(HttpContext.Current.Request.QueryString.Get("min"));
            _params.MaxPoints = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("max")) ? 2000000 : int.Parse(HttpContext.Current.Request.QueryString.Get("max"));
            _params.PageSize = string.IsNullOrEmpty(Common.GetAppSetting<string>("catalogueSize")) ? "10" : Common.GetAppSetting<string>("catalogueSize");
            _params.SearchText = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("search")) ? "" : HttpContext.Current.Request.QueryString.Get("search");
            _params.SortBy = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("sortBy")) ? string.Empty : HttpContext.Current.Request.QueryString.Get("sortBy");
            _params.Index = 0;
            return Common.SerializeObject(_params);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SetLabels();
        SetCatalogueList();
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

        pointsLabel.InnerText = string.Format("{0}: {1}", 
            RewardsHelper.GetTranslation(TranslationKeys.Label.Points), 
            MemberRewardsInfo != null ? Convert.ToString(MemberRewardsInfo.CurrentPoints) : "0");
        pointLevelLabel.InnerText = string.Format("{0} {1}", 
            RewardsHelper.GetTranslation(TranslationKeys.Label.PointLevel), 
            MemberRewardsInfo != null ? Convert.ToString(MemberRewardsInfo.CurrentPointLevel) : "0");
        divLevel.Visible = true;
        #endregion
    }
}






