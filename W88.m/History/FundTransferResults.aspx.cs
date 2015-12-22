using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class History_FundTransferResults : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        
            if (!string.IsNullOrEmpty(Request["dateFrom"]) && !string.IsNullOrEmpty(Request["dateTo"]) &&
                !string.IsNullOrEmpty(Request["status"]) && !string.IsNullOrEmpty(Request["type"]) &&
                !string.IsNullOrEmpty(commonVariables.OperatorId) &&
                !string.IsNullOrEmpty(commonVariables.GetSessionVariable("MemberCode")))
            {
                //Request Params
                var dateFrom = DateTime.Parse(Request["dateFrom"].ToString());
                var dateTo = DateTime.Parse(Request["dateTo"].ToString());
                var status = Request["status"];
                var type = int.Parse(Request["type"]);

                //Other Params
                var strOperatorId = int.Parse(commonVariables.OperatorId);
                var strMemberCode = commonVariables.GetSessionVariable("MemberCode");
                //if((dateTo-dateFrom).TotalDays > 90)
                //{
                //    dateTo = dateFrom.AddDays(90);
                //}


                try
                {
                    using (var svcInstance = new svcPayMember.MemberClient())
                    {
                        string statusCode;
                        DataTable history = svcInstance.getFundTransferHistory(strOperatorId, strMemberCode, type,
                            status, dateFrom, dateTo, out statusCode);

                        GridView1.DataSource = history;
                        GridView1.PagerSettings.Mode = PagerButtons.NextPrevious;
                        GridView1.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    Console.Out.Write("ex: " + ex);
                }
            }
            else
            {
                Response.Redirect((string) HttpContext.Current.Session["domain_Account"] + "/History");
            }
        

    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        GridView1.DataBind();
    }

    protected void GridView1_RowDataBound1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TableCell fromCell = e.Row.Cells[3];
            fromCell.Text = GetWallet(e.Row.Cells[3].Text);
            TableCell toCell = e.Row.Cells[4];
            toCell.Text = GetWallet(e.Row.Cells[4].Text);
            TableCell statusCell = e.Row.Cells[7];
            
            switch (e.Row.Cells[7].Text)
            {
                case "0"    :
                    statusCell.Text = "Pending";
                    break;
                case "1":
                    statusCell.Text = "Successful";
                    break;
                case "2":
                    statusCell.Text = "Failed";
                    break;
                case "3":
                    statusCell.Text = "Declined";
                    break;
                default:
                    statusCell.Text = "";
                    break;
            }
        }
    }

    protected string GetWallet(string walletId)
    {
        string retVal;
        switch (walletId)
        {
            case "0":
                retVal = "MAIN";
                break;
            case "1":
                retVal = "LOTTERY";
                break;
            case "2":
                retVal = "A-SPORTS";
                break;
            case "4":
                retVal = "PALAZZO";
                break;
            case "6":
                retVal = "POKER (USD)";
                break;
            case "7":
                retVal = "E-SPORTS";
                break;
            case "9":
                retVal = "I-SPORTS";
                break;
            case "12":
                retVal = "NUOVO";
                break;
            case "13":
                retVal = "W-SPORTS";
                break;
            case "3":
                retVal = "CLUB W, BRAVADO, APOLLO, CRESCENDO, DIVINO & MASSIMO, VIRTUAL";
                break;
            default:
                retVal = "";
                break;
        }

        return retVal;
    }
}