using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class History_DepositWithdrawalResults : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime dateFrom = DateTime.Parse(Request["dateFrom"].ToString());
        DateTime dateTo = DateTime.Parse(Request["dateTo"].ToString());
        var status = Request["status"];
        var type = Request["type"];

        
        if((dateTo-dateFrom).TotalDays > 90)
        {
            dateTo = dateFrom.AddDays(90);
        }
    }
}