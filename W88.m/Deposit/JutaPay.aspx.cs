using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;


public partial class Deposit_JutaPay : PaymentBasePage
{
    protected string lblTransactionId;
    protected string lblTransactionFailed;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.JutaPay);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.JutaPay);

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
        txtMinMaxLimit.Text = base.strtxtMinMaxLimit;

        lblDailyLimit.Text = base.strlblDailyLimit;
        txtDailyLimit.Text = base.strtxtDailyLimit;

        lblTotalAllowed.Text = base.strlblTotalAllowed;
        txtTotalAllowed.Text = base.strtxtTotalAllowed;

        lblDepositAmount.Text = base.strlblAmount;

        btnSubmit.Text = base.strbtnSubmit;

        lblTransactionId = base.strlblTransactionId;

        lblTransactionFailed = base.GetErrors("/TransferFail").AlertMessage;
    }
}