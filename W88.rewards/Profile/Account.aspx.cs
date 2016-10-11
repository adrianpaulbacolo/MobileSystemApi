using System;
using System.Data;
using System.Threading.Tasks;
using System.Web;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.Utilities.Constant;
using W88.WebRef.RewardsServices;

public partial class Account : BasePage
{
    protected string Title = string.Empty;
    protected DataSet DataSet = null;

    protected async void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }
       
        if (!HasSession)
        {
            Response.Redirect("/Index.aspx", false);
        }

        var type = HttpContext.Current.Request.QueryString.Get("type");
        var memberCode = UserSessionInfo == null ? string.Empty : UserSessionInfo.MemberCode;

        if (!string.IsNullOrEmpty(type))
        {
            var walletId = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("walletid")) ? "" : HttpContext.Current.Request.QueryString.Get("walletid");
            var yearMonth = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("yearmonth")) ? "" : HttpContext.Current.Request.QueryString.Get("yearmonth");
            var html = string.Empty;

            switch (type)
            {
                case "stake":
                    if (string.IsNullOrEmpty(walletId)) //all stakes
                    {
                        // "Earning Listing by Product";
                        Title = string.Format(@"{0} ({1})",
                            RewardsHelper.GetTranslation(TranslationKeys.Redemption.EarningListing),
                            RewardsHelper.GetTranslation(TranslationKeys.Redemption.Product));
                        html = await GetTotalStake(memberCode);
                    }
                    else if (!string.IsNullOrEmpty(walletId) && string.IsNullOrEmpty(yearMonth)) //stakes by walletid
                    {
                        // "Earning Listing by Month";
                        Title = string.Format(@"{0} ({1})",
                            RewardsHelper.GetTranslation(TranslationKeys.Redemption.EarningListing),
                            RewardsHelper.GetTranslation(TranslationKeys.Label.Month));                      
                        html = await GetTotalStakeByMonth(memberCode, walletId);
                    }
                    else //stakes detail
                    {
                        // "Earning Listing Detail";
                        Title = string.Format(@"{0} ({1})",
                            RewardsHelper.GetTranslation(TranslationKeys.Redemption.EarningListing),
                            RewardsHelper.GetTranslation(TranslationKeys.Label.Details));
                        html = await GetTotalStakeDetail(memberCode, walletId, yearMonth);
                    }

                    if (!string.IsNullOrEmpty(html))
                    {
                        resultpanel.InnerHtml = html;
                    }
                    else
                    {
                        lblNoRecord.Visible = true;
                        ListviewHistory.Visible = false;
                    }
                    break;
                case "redeemed":
                    Title = RewardsHelper.GetTranslation(TranslationKeys.Redemption.RedemptionListing);
                    resultpanel.InnerHtml = html;
                    break;
                case "expired":
                    Title = RewardsHelper.GetTranslation(TranslationKeys.Redemption.PointsExpired);
                    resultpanel.InnerHtml = html;
                    break;
                case "adjusted":
                    Title = RewardsHelper.GetTranslation(TranslationKeys.Redemption.PointsAdjusted);
                    resultpanel.InnerHtml = html;
                    break;
                case "cart":
                    Title = "Points in your cart";
                    resultpanel.InnerHtml = html;
                    break;
                default:
                    SetAccountSummary(memberCode);
                    break;
            }
        }
        else
        {
            SetAccountSummary(memberCode);
        }           
    }

    private static async Task<string> GetTotalStake(string memberCode)
    {
        using (var client = new RewardsServicesClient())
        {
            var dataSet = await client.getEarnProductFEAsync(Convert.ToString(Settings.OperatorId), memberCode);
            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                return string.Empty;
            }

            var dataTable = dataSet.Tables[0];
            var headers = string.Empty;
            var rows = string.Empty;

            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                rows += "<tr>";

                for (var j = 0; j < dataTable.Columns.Count; j++)
                {
                    if (i == 0)
                    {
                        switch (dataTable.Columns[j].ColumnName)
                        {
                            case "walletName":
                                headers += string.Format(@"<td><div class='pointDetailMainHeaderHor'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Label.Wallet));
                                break;
                            case "totalStake":
                                headers += string.Format(@"<td><div class='pointDetailMainHeaderHor'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Redemption.TotalStake));
                                break;
                            case "pointsAwarded":
                                headers += string.Format(@"<td><div class='pointDetailMainHeaderHor'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Redemption.PointsEarned));
                                break;
                        }
                    }

                    switch (j)
                    {
                        case 0:
                            rows += string.Empty;
                            break;
                        case 2:
                            rows += string.Format(@"<td><div class='points'><span>{0}</span></div></td>", string.Format("{0:#,###,##0.##}", dataTable.Rows[i].ItemArray[j]));
                            break;
                        case 3:
                            rows += string.Format(@"<td><a href='/Account?type=stake&walletid='{0}'><div class='points'><span>{1}</span></div></a></td>",
                                    dataTable.Rows[i].ItemArray[0],
                                    string.Format("{0:#,###,##0.##}", 
                                    dataTable.Rows[i].ItemArray[j]));
                            break;
                        default:
                            rows += string.Format(@"<td><div class='points'><span>{0}</span></div></td>", dataTable.Rows[i].ItemArray[j]);
                            break;
                    }
                }

                rows += "</tr>";
            }
            
            return string.Format(@"<table width='100%'><tr>{0}</tr>{1}</table>", headers, rows);             
        }
    }

    private static async Task<string> GetTotalStakeByMonth(string memberCode, string walletId)
    {
        using (var client = new RewardsServicesClient())
        {
            var dataSet = await client.getEarnMonthFEAsync(Convert.ToString(Settings.OperatorId), memberCode, walletId);
            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                return string.Empty;
            }

            var dataTable = dataSet.Tables[0];
            var headers = string.Empty;
            var rows = string.Empty;

            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                rows += "<tr>";

                for (var j = 0; j < dataTable.Columns.Count; j++)
                {
                    if (i == 0)
                    {
                        switch (dataTable.Columns[j].ColumnName)
                        {
                            case "pointsYear":
                                headers += string.Format(@"<td><div class='pointDetailMainHeaderHor'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Label.Year));
                                break;
                            case "pointsMonth":
                                headers += string.Format(@"<td><div class='pointDetailMainHeaderHor'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Label.Month));
                                break;
                            case "totalStake":
                                headers += string.Format(@"<td><div class='pointDetailMainHeaderHor'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Redemption.TotalStake));
                                break;
                            case "pointsAwarded":
                                headers += string.Format(@"<td><div class='pointDetailMainHeaderHor'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Redemption.PointsEarned));
                                break;
                        }
                    }

                    switch (j)
                    {
                        case 2:
                            rows += string.Format(@"<td><div class='points'><span>{0}</span></div></td>", string.Format("{0:#,###,##0.##}", dataTable.Rows[i].ItemArray[j]));
                            break;
                        case 3: 
                            rows += string.Format(@"<td><a href='/Account?type=stake&walletid={0}&yearmonth={1}'><div class='points'><span>{2}</span></div></a></td>", 
                                walletId, 
                                ((string)dataTable.Rows[i].ItemArray[0] + (string)dataTable.Rows[i].ItemArray[1]), 
                                string.Format("{0:#,###,##0.##}", dataTable.Rows[i].ItemArray[j]));
                            break;
                        default:
                            rows += string.Format(@"<td><div class='points'><span>{0}</span></div></td>", dataTable.Rows[i].ItemArray[j]);
                            break;
                    }
                }

                rows += "</tr>";
            }

            return string.Format(@"<table width='100%'><tr>{0}</tr>{1}</table>", headers, rows);                         
        }
    }

    public async Task<string> GetTotalStakeDetail(string memberCode, string walletId, string yearMonth)
    {
        using (var client = new RewardsServicesClient())
        {
            var year = int.Parse(yearMonth.Substring(0, 4));
            var month = (yearMonth.Length > 5) ? int.Parse(yearMonth.Substring(4, 2)) : int.Parse(yearMonth.Substring(4, 1));
            var dataSet = await client.getEarnDetailFEAsync(Convert.ToString(Settings.OperatorId), memberCode, walletId, month, year);

            if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
            {
                return string.Empty;
            }

            var dataTable = dataSet.Tables[0];
            var headers = string.Empty;
            var rows = string.Empty;

            for (var i = 0; i < dataTable.Rows.Count; i++)
            {
                rows += "<tr>";

                for (var j = 0; j < dataTable.Columns.Count; j++)
                {
                    if (i == 0)
                    {
                        switch (dataTable.Columns[j].ColumnName)
                        {
                            case "createdDateTime":
                                headers += string.Format(@"<td><div class='pointDetailMainHeaderHor'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Label.Date));
                                break;
                            case "transactionDateTime":
                                headers += string.Format(@"<td><div class='pointDetailMainHeaderHor'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Redemption.BetDate));
                                break;
                            case "totalStake":
                                headers += string.Format(@"<td><div class='pointDetailMainHeaderHor'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Redemption.TotalStake));
                                break;
                            case "pointsAwarded":
                                headers += string.Format(@"<td><div class='pointDetailMainHeaderHor'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Redemption.PointsEarned));
                                break;
                        }
                    }

                    switch (j)
                    {
                        case 2:
                            rows += string.Format(@"<td><div class='points'><span>{0}</span></div></td>", string.Format("{0:#,###,##0.##}", dataTable.Rows[i].ItemArray[j]));
                            break;
                        case 3:
                            rows += string.Format(@"<td><div class='points'><span>{0}</span></div></td>", string.Format("{0:#,###,##0.##}", dataTable.Rows[i].ItemArray[j]));
                            break;
                        default:
                            rows += string.Format(@"<td><div class='points'><span>{0}</span></div></td>", Convert.ToString(DateTime.Parse(dataTable.Rows[i].ItemArray[j].ToString()).ToString("yyyy/MM/dd")));
                            break;
                    }
                }

                rows += "</tr>";
            }

            return string.Format(@"<table width='100%'><tr>{0}</tr>{1}</table>", headers, rows);                                           
        }      
    }

    private async void SetAccountSummary(string memberCode)
    {
        Title = RewardsHelper.GetTranslation(TranslationKeys.Redemption.AccountSummary);
        DataSet = await RewardsHelper.GetAccountSummary(memberCode);
        if (DataSet == null)
        {
            lblNoRecord.Visible = true;
        }
        else
        {
            lblNoRecord.Visible = false;
            ListviewHistory.DataSource = DataSet.Tables[0];
            ListviewHistory.DataBind();
        }
    }
}