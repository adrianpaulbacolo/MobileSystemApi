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


public partial class Deposit_EGHL : PaymentBasePage
{
    protected string lblTransactionId;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.EGHL);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.EGHL);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        drpBank.Items.AddRange(base.InitializeBank("EGHLBank").ToArray());

        if (!Page.IsPostBack)
        {
            this.InitializeLabels();
        }
    }

    private void InitializeLabels()
    {
        lblAmount.Text = base.strlblAmount;

        lblBank.Text = base.strlblBank;

        lblMessage.Text = commonCulture.ElementValues.getResourceString("bankNotice", xeResources);

        lblTransactionId = base.strlblTransactionId;
    }
}