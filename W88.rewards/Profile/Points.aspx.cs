using System;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Globalization;
using W88.BusinessLogic.Rewards.Helpers;
using W88.BusinessLogic.Rewards.Models;
using W88.Utilities.Constant;
using W88.Utilities.Log.Helpers;
using W88.WebRef.RewardsServices;

public partial class Points : BasePage
{
    protected string Title = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsPostBack)
        {
            return;
        }

        lbdatefrom.InnerText = string.Format("{0}:", RewardsHelper.GetTranslation(TranslationKeys.Label.DateFrom));
        lbdateto.InnerText = string.Format("{0}:", RewardsHelper.GetTranslation(TranslationKeys.Label.DateTo));
        submit.Text = RewardsHelper.GetTranslation(TranslationKeys.Label.Search);

        if (!HasSession)
        {
            Response.Redirect("/Index.aspx", false);
        }

        var type = HttpContext.Current.Request.QueryString.Get("type");
        if (!string.IsNullOrEmpty(type))
        {
            switch (type)
            {
                case "stake":
                    break;
                case "redeemed":
                    Title = RewardsHelper.GetTranslation(TranslationKeys.Redemption.RedemptionListing);
                    break;
                case "expired":
                    Title = RewardsHelper.GetTranslation(TranslationKeys.Redemption.PointsExpired);
                    break;
                case "adjusted":
                    Title = RewardsHelper.GetTranslation(TranslationKeys.Redemption.PointsAdjusted);
                    break;
                case "cart":
                    Title = "Cart Listing";
                    break;
            }
        }
    }

    private static async Task<string> GetRedemptionList(string memberCode, DateTime dateFrom, DateTime dateTo)
    {
        try
        {
            using (var client = new RewardsServicesClient())
            {
                var dateFromString = string.Format("{0:yyyy/M/d HH:mm:ss}", dateFrom);
                var dateToString = string.Format("{0:yyyy/M/d HH:mm:ss}", dateTo);
                var dataSet = await client.getRedemptionFEAsync(Convert.ToString(Settings.OperatorId), memberCode, dateFromString, dateToString);

                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    return string.Empty;
                }

                dataSet.Tables[0].Columns.Add("rType");

                var dataTable = dataSet.Tables[0];
                var headers = string.Empty;
                var rows = string.Empty;

                for (var i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (dataTable.Rows[i]["rType"].ToString() != "0")
                    {
                        dataTable.Rows[i]["rType"] = "Spin Wheel Prize";
                    }
                    else 
                    { 
                        dataTable.Rows[i]["rType"] = "Redemption";
                    }

                    rows += "<tr>";

                    for (var j = 0; j < dataTable.Columns.Count; j++)
                    {
                        if (i == 0)
                        {
                            switch (dataTable.Columns[j].ColumnName)
                            {
                                case "createdDateTime":                   
                                    headers += string.Format(@"<td><div class='pointDetailMainHeader'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Label.Date));
                                    break;
                                case "redemptionId":
                                    headers += "<td><div class='pointDetailMainHeader'><span>ID</span></div></td>";
                                    break;
                                case "categoryCode":
                                    headers += string.Format(@"<td><div class='pointDetailMainHeader'><span>{0} ({1})</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Redemption.Category), RewardsHelper.GetTranslation(TranslationKeys.Label.Points));
                                    break;
                                case "statusName":
                                    headers += string.Format(@"<td><div class='pointDetailMainHeader'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Label.Status));
                                    break;
                            }
                        }

                        switch (j)
                        {
                            case 0:
                                rows += string.Format(@"<td><div class='points'><span>{0}</span></div></td>", string.Format("{0:d/M/yyyy}", Convert.ToDateTime(dataTable.Rows[i].ItemArray[0].ToString())));
                                break;
                            case 3:
                                rows += string.Format(@"<td><div class='points'><span>{0} ({1})</span></div></td>", dataTable.Rows[i].ItemArray[j], string.Format("{0:#,###,##0.##}", dataTable.Rows[i].ItemArray[4]));
                                break;
                            case 1:
                                rows += string.Format(@"<td><div class='points'><span>{0}</span></div></td>", dataTable.Rows[i].ItemArray[j]);
                                break;
                            case 5:
                                rows += string.Format(@"<td><div class='points'><span>{0}</span></div></td>", dataTable.Rows[i].ItemArray[j]);
                                break;
                            default:
                                rows += string.Empty;
                                break;
                        }
                    }

                    rows += "</tr>";
                }

                return string.Format(@"<table id='transactions' width='100%'><tr>{0}</tr>{1}</table>", headers, rows);                                   
            }
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    private static async Task<string> GetAdjustmentList(string memberCode, DateTime dateFrom, DateTime dateTo)
    {
        try
        {
            using (var client = new RewardsServicesClient())
            {
                var dateFromString = string.Format("{0:yyyy/M/d HH:mm:ss}", dateFrom);
                var dateToString = string.Format("{0:yyyy/M/d HH:mm:ss}", dateTo);
                var dataSet = await client.getAdjustmentFEAsync(Convert.ToString(Settings.OperatorId), memberCode, dateFromString, dateToString);

                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    return string.Empty;
                }

                var dataTable = dataSet.Tables[0];
                var headers = string.Empty;
                var rows = string.Empty;

                for (var i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(dataTable.Rows[i]["remarks"].ToString()))
                    {
                        dataTable.Rows[i]["remarks"] = "-";
                    }

                    rows += "<tr>";

                    for (var j = 0; j < dataTable.Columns.Count; j++)
                    {
                        if (i == 0)
                        {
                            switch (dataTable.Columns[j].ColumnName)
                            {
                                case "createdDateTime":
                                    headers += string.Format(@"<td><div class='pointDetailMainHeader'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Label.Date));
                                    break;
                                case "actionName":
                                    headers += string.Format(@"<td><div class='pointDetailMainHeader'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Label.AdjustmentType));
                                    break;
                                case "pointsAdjusted":
                                    headers += string.Format(@"<td><div class='pointDetailMainHeader'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Label.Points));
                                    break;
                                case "remarks":
                                    headers += string.Format(@"<td><div class='pointDetailMainHeader'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Label.Remarks));
                                    break;
                            }
                        }

                        if (j == 0)
                        {
                            rows += string.Format(@"<td><div class='points'><span>{0}</span></div></td>", string.Format("{0:d/M/yyyy}", Convert.ToDateTime(dataTable.Rows[i].ItemArray[0].ToString())));
                        }
                        else
                        {
                            rows += string.Format(@"<td><div class='points'><span>{0}</span></div></td>", dataTable.Rows[i].ItemArray[j]);
                        }
                    }

                    rows += "</tr>";
                }

                return string.Format(@"<table id='transactions' width='100%'><tr>{0}</tr>{1}</table>", headers, rows);                              
            }
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    private static async Task<string> GetExpiredList(string memberCode, DateTime dateFrom, DateTime dateTo)
    {
        try
        {
            using (var client = new RewardsServicesClient())
            {
                var dateFromString = String.Format("{0:yyyy/M/d HH:mm:ss}", dateFrom);
                var dateToString = String.Format("{0:yyyy/M/d HH:mm:ss}", dateTo);
                var dataSet = await client.getExpiredFEAsync(Convert.ToString(Settings.OperatorId), memberCode, dateFromString, dateToString);

                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    return string.Empty;
                }

                var dataTable = dataSet.Tables[0];
                var headers = string.Empty;
                var rows = string.Empty;

                for (var i = 0; i < dataTable.Rows.Count; i++)
                {
                    if (string.IsNullOrEmpty(dataTable.Rows[i]["remarks"].ToString()))
                    {
                        dataTable.Rows[i]["remarks"] = "-";
                    }

                    rows += "<tr>";

                    for (var j = 0; j < dataTable.Columns.Count; j++)
                    {
                        if (i == 0)
                        {
                            switch (dataTable.Columns[j].ColumnName)
                            {
                                case "createdDateTime":
                                    headers += string.Format(@"<td><div class='pointDetailMainHeader'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Label.Date));
                                    break;
                                case "pointsExpired":
                                    headers += string.Format(@"<td><div class='pointDetailMainHeader'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Redemption.PointsExpired));
                                    break;
                                case "remarks":
                                    headers += string.Format(@"<td><div class='pointDetailMainHeader'><span>{0}</span></div></td>", RewardsHelper.GetTranslation(TranslationKeys.Label.Remarks));
                                    break;
                            }
                        }

                        switch (j)
                        {
                            case 8:
                                rows += string.Format(@"<td><div class='points'><span>{0}</span></div></td>", string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(dataTable.Rows[i].ItemArray[j].ToString())));
                                break;
                            case 5:
                                rows += string.Format(@"<td><div class='points'><span>{0}</span></div></td>", dataTable.Rows[i].ItemArray[j]);
                                break;
                            case 6:
                                rows += string.Format(@"<td><div class='points'><span>{0}</span></div></td>", dataTable.Rows[i].ItemArray[j]);
                                break;
                            default:
                                rows += string.Empty;
                                break;
                        }
                    }
                    
                    rows += "</tr>";
                }
                
                return string.Format(@"<table id='transactions' width='100%'><tr>{0}</tr>{1}</table>", headers, rows);                                 
            }
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    protected async void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            var formats = new[] { "MM/dd/yyyy" };

            DateTime fromDate;
            var isParsed = DateTime.TryParseExact(dateFrom.Value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDate);

            if (!isParsed)
            {
                ShowMessage("wrongfromdate");
                return;
            }

            DateTime toDate;
            isParsed = DateTime.TryParseExact(dateTo.Value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out toDate);

            if (!isParsed)
            {
                ShowMessage("wrongtodate");
                return;
            }

            if (fromDate > toDate)
            {
                ShowMessage("wrongdaterange");
                return;
            }

            var memberCode = UserSessionInfo == null ? string.Empty : UserSessionInfo.MemberCode;
            toDate = toDate.AddHours(23).AddMinutes(59).AddSeconds(59);

            var type = HttpContext.Current.Request.QueryString.Get("type");
            if (string.IsNullOrEmpty(type))
            {
                return;
            }

            var html = string.Empty;             
            switch (type)
            {
                case "redeemed":
                    Title = RewardsHelper.GetTranslation(TranslationKeys.Redemption.RedemptionListing);
                    html = await GetRedemptionList(memberCode, fromDate, toDate);
                    break;
                case "expired":
                    Title = RewardsHelper.GetTranslation(TranslationKeys.Redemption.PointsExpired);
                    html = await GetExpiredList(memberCode, fromDate, toDate);
                    break;
                case "adjusted":
                    Title = RewardsHelper.GetTranslation(TranslationKeys.Redemption.PointsAdjusted);
                    html = await GetAdjustmentList(memberCode, fromDate, toDate);
                    break;
            }

            if (!string.IsNullOrEmpty(html))
            {
                resultpanel.InnerHtml = html;
            }
            else
            {
                ShowMessage("nodata");
                resultpanel.InnerHtml = string.Empty;
            }
        }
        catch (Exception exception)
        {
            ShowMessage("FAIL");
            AuditTrail.AppendLog(exception);
        }     
    }

    private void ShowMessage(string status)
    {
        var scriptBuilder = new StringBuilder();
        scriptBuilder.Append("setTimeout(function() {showMessage('")
            .Append(status + "');}, 300);");
        ScriptManager.RegisterStartupScript(Page, GetType(), (new Guid()).ToString(), scriptBuilder.ToString(), true);
    }
}