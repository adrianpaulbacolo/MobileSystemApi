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


public partial class Withdrawal_PayGo : PaymentBasePage
{
    protected string lblTransactionId;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.WithdrawalMethod.PayGo);
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.WithdrawalMethod.PayGo);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CheckAgentAndRedirect(string.Concat(V2WithdrawalPath, "Pay", PaymentMethodId, ".aspx"));
            this.InitializeLabels();
        }
    }

    private void InitializeLabels()
    {
        lblAmount.Text = base.strlblAmount;

        lblAccountName.Text = base.strlblAccountName;
        lblAccountNumber.Text = base.strlblAccountNumber;

        lblTransactionId = base.strlblTransactionId;

        XElement xeResources;
        commonCulture.appData.getRootResource("_Secure/Register.aspx", out xeResources);

        lblContact.Text = commonCulture.ElementValues.getResourceString("lblContact", xeResources);
    }

}