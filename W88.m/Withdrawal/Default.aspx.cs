using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.XPath;

public partial class Withdrawal_Default : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CheckAgentAndRedirect(V2WithdrawalPath);
        }
    }
}
