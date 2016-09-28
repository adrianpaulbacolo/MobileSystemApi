using System;
using System.Data;
using System.Text;
using System.Web;

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
        var catalogueSet = await RewardsHelper.GetCatalogueSet(MemberSession);
        CategoryListView.DataSource = catalogueSet.Tables[0];
        var dataTable = (DataTable)CategoryListView.DataSource;
        DataRow dataRowAll = dataTable.NewRow();
        dataRowAll["categoryId"] = "0";
        dataRowAll["categoryName"] = HttpContext.GetLocalResourceObject(LocalResx, "lbl_all").ToString();
        dataRowAll["imagePathOn"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("ImagesDirectoryPath") + "Category/" + "clt_all_on.png");
        dataRowAll["imagePathOff"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("ImagesDirectoryPath") + "Category/" + "clt_all_off.png");
        dataTable.Rows.InsertAt(dataRowAll, 0);
        CategoryListView.DataBind();
    }

    private async void GetProducts()
    {
        try {
            var categoryId = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("categoryId")) ? "0" : HttpContext.Current.Request.QueryString.Get("categoryId");
            var sortBy = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("sortBy")) ? "" : HttpContext.Current.Request.QueryString.Get("sortBy");
            var pointsFrom = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("min")) ? 0 : int.Parse(HttpContext.Current.Request.QueryString.Get("min"));
            var pointsTo = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("max")) ? 2000000 : int.Parse(HttpContext.Current.Request.QueryString.Get("max"));
            var searchText = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("search")) ? "" : HttpContext.Current.Request.QueryString.Get("search");

            const string pageSize = "15";
            const string pageNumber = "0";
            var pointLevelDiscount = 0;
            var totalCount = 0;

            var dataSet = await RewardsHelper.GetProductSearch(
                MemberSession, 
                categoryId, 
                pointsFrom, 
                pointsTo, 
                searchText,
                sortBy, 
                pageSize, 
                pageNumber);

            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                lblnodata.Text = HttpContext.GetLocalResourceObject(LocalResx, "lbl_noRedemption").ToString();
                lblnodata.Visible = true;
                return;
            }

            if (HasSession)
            {
                pointLevelDiscount = await RewardsHelper.GetMemberPointLevelDiscount(MemberSession);
            }

            if (!dataSet.Tables[0].Columns.Contains("redemptionValidity"))
            {
                dataSet.Tables[0].Columns.Add("redemptionValidity");
            }

            foreach (DataRow dataRow in dataSet.Tables[0].Rows)
            {
                if (HasSession)
                {
                    if (dataRow["discountPoints"] == DBNull.Value && pointLevelDiscount != 0 &&
                        dataRow["productType"].ToString() != "1")
                    {
                        //reverse to new points maintain in bo Discount
                        var percentage = Convert.ToDouble(pointLevelDiscount) / 100;
                        var normalPoint = int.Parse(dataRow["pointsRequired"].ToString());
                        var points = Math.Floor(normalPoint * (1 - percentage));
                        var pointAfterLevelDiscount = Convert.ToInt32(points);
                        dataRow["pointsRequired"] = pointAfterLevelDiscount;
                    }
                }

                dataRow["imagePath"] = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("ImagesDirectoryPath") + "Product/" + dataRow["imageName"]);
                categoryId = dataRow["categoryId"].ToString(); //???? 

                if (HasSession)
                {
                    dataRow["redemptionValidity"] += ",";
                    if (dataRow["redemptionValidity"].ToString().ToUpper() != "ALL,")
                    {
                        if (!((string) dataRow["redemptionValidity"]).Contains(MemberSession.RiskId.ToUpper() + ","))
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

            if (sortBy == "2")
            {
                dataSet.Tables[0].DefaultView.Sort = "pointsRequired";
            }

            if (dataSet.Tables.Count > 1)
            {
                if (dataSet.Tables[1].Rows.Count > 0)
                    totalCount = int.Parse(dataSet.Tables[1].Rows[0][0].ToString());
            }

            ListviewProduct.DataSource = dataSet.Tables[0];
            ListviewProduct.DataBind();
            lblnodata.Visible = false;
        }
        catch (Exception ex)
        {
            lblnodata.Text = HttpContext.GetLocalResourceObject(LocalResx, "lbl_noRedemption").ToString();
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
}






