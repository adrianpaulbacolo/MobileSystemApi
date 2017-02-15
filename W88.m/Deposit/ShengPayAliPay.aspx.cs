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


public partial class Deposit_ShengPayAliPay : PaymentBasePage
{
    protected string lblTransactionId;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.ShengPayAliPay);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.ShengPayAliPay);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CheckAgentAndRedirect(string.Concat(V2DepositPath, "Pay", PaymentMethodId, ".aspx"));
            InitializeLabels();

        }
    }

    private void InitializeLabels()
    {
        lblDepositAmount.Text = base.strlblAmount;
        lblTransactionId = base.strlblTransactionId;
    }
}