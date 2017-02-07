using System;
using System.Collections.Generic;
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

public partial class Deposit_Neteller : PaymentBasePage
{
    protected string lblTransactionId;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = "Neteller";
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.Neteller);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitializeLabels();
        }
    }

    private void InitializeLabels()
    {
        XElement _xeRegisterResources;
        commonCulture.appData.getRootResource("/_Secure/Register.aspx", out _xeRegisterResources);

        lblAmount.Text = base.strlblAmount;

        lblAccountName.Text = "Neteller " + commonCulture.ElementValues.getResourceString("lblUsername", _xeRegisterResources);
        lblAccountNumber.Text = "Neteller " + commonCulture.ElementValues.getResourceString("lblPassword", _xeRegisterResources);

        lblTransactionId = base.strlblTransactionId;
    }
}
