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


public partial class Withdrawal_VenusPoint : PaymentBasePage
{
    protected string lblTransactionId;
    protected string lblTransactionFailed;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.WithdrawalMethod.VenusPoint);
        base.PaymentType = commonVariables.PaymentTransactionType.Withdrawal;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.WithdrawalMethod.VenusPoint);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl withdrawalTabs = (HtmlGenericControl)FindControl("withdrawalTabs");
        commonPaymentMethodFunc.GetWithdrawalMethodList(strMethodsUnAvailable, withdrawalTabs, base.PageName, sender.ToString().Contains("app"));

        if (!Page.IsPostBack)
        {
            InitializeLabels();
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
        txtWithdrawAmount.Attributes.Add("PLACEHOLDER", base.strtxtAmount);

        btnSubmit.Text = base.strbtnSubmit;

        txtMinMaxLimit.Text = base.strtxtMinMaxLimit;
        txtDailyLimit.Text = base.strtxtDailyLimit;
        txtTotalAllowed.Text = base.strtxtTotalAllowed;

        lblTransactionId = base.strlblTransactionId;

        lblTransactionFailed = base.GetErrors("/TransferFail").AlertMessage;
    }
}