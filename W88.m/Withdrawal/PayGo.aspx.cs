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
    protected string lblTransactionFailed;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.WithdrawalMethod.PayGo);
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.WithdrawalMethod.PayGo);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl withdrawalTabs = (HtmlGenericControl)FindControl("withdrawalTabs");
        commonPaymentMethodFunc.GetWithdrawalMethodList(strMethodsUnAvailable, withdrawalTabs, base.PageName, sender.ToString().Contains("app"));

        if (!Page.IsPostBack)
        {
            this.InitializeLabels();
        }
    }

    private void InitializeLabels()
    {
        lblMode.Text = base.strlblMode;
        txtMode.Text = base.strtxtMode;

        lblMinMaxLimit.Text = base.strlblMinMaxLimit;
        txtMinMaxLimit.Text = base.strtxtMinMaxLimit;

        lblDailyLimit.Text = base.strlblDailyLimit;
        txtDailyLimit.Text = base.strtxtDailyLimit;

        lblTotalAllowed.Text = base.strlblTotalAllowed;
        txtTotalAllowed.Text = base.strtxtTotalAllowed;

        lblWithdrawAmount.Text = base.strlblAmount;

        lblAccountName.Text = base.strlblAccountName;
        lblAccountNumber.Text = base.strlblAccountNumber;

        btnSubmit.Text = base.strbtnSubmit;

        lblTransactionId = base.strlblTransactionId;

        XElement xeResources;
        commonCulture.appData.getRootResource("_Secure/Register.aspx", out xeResources);

        lblContact.Text = commonCulture.ElementValues.getResourceString("lblContact", xeResources);
    }

}