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


public partial class Deposit_PaySec : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.PaySec);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.PaySec);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");
        commonPaymentMethodFunc.GetDepositMethodList(strMethodsUnAvailable, depositTabs, base.PageName, sender.ToString().Contains("app"), base.strCurrencyCode);

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
        lblDailyLimit.Text = base.strlblDailyLimit;
        lblTotalAllowed.Text = base.strlblTotalAllowed;
        lblDepositAmount.Text = base.strlblAmount;

        btnSubmit.Text = base.strbtnSubmit;

        txtDepositAmount.Attributes.Add("PLACEHOLDER", base.strtxtAmount);

        txtMinMaxLimit.Text = base.strtxtMinMaxLimit;
        txtDailyLimit.Text = base.strtxtDailyLimit;
        txtTotalAllowed.Text = base.strtxtTotalAllowed;
    }
}