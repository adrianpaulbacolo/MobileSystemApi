using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;


public partial class Withdrawal_Cubits : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.WithdrawalMethod.Cubits);
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.WithdrawalMethod.Cubits);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            base.InitialisePendingWithdrawals(false);
            this.InitializeLabels();
        }
    }

    private void InitializeLabels()
    {
        lblAmount.Text = base.strlblAmount;
    }
}