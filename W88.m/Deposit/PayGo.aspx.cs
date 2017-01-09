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


public partial class Deposit_PayGo : PaymentBasePage
{
    protected string lblTransactionId;
    protected string lblTransactionFailed;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.PayGo);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.PayGo);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");
        commonPaymentMethodFunc.GetDepositMethodList(strMethodsUnAvailable, depositTabs, base.PageName, sender.ToString().Contains("app"), base.strCurrencyCode);

        if (!Page.IsPostBack)
        {
            this.InitializeLabels();
            this.InitialiseDepositDateTime();
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

        lblAccountName.Text = base.strlblAccountName;
        lblAccountNumber.Text = base.strlblAccountNumber;

        btnSubmit.Text = base.strbtnSubmit;

        lblTransactionId = base.strlblTransactionId;
    }

    private void InitialiseDepositDateTime()
    {
        #region DepositDateTime

        for (System.DateTime dtDepositDateTime = System.DateTime.Today.AddHours(-72); dtDepositDateTime <= System.DateTime.Today.AddHours(72); dtDepositDateTime = dtDepositDateTime.AddHours(24))
        {
            drpDepositDate.Items.Add(new ListItem(dtDepositDateTime.ToString("dd / MM / yyyy"), dtDepositDateTime.ToString("yyyy-MM-dd")));
        }

        drpDepositDate.SelectedValue = DateTime.Now.ToString("yyyy-MM-dd");

        for (int intHour = 0; intHour < 24; intHour++)
        {
            drpHour.Items.Add(new ListItem((intHour).ToString("0#"), Convert.ToString(intHour)));
        }
        for (int intMinute = 0; intMinute < 60; intMinute++)
        {
            drpMinute.Items.Add(new ListItem((intMinute).ToString("0#"), Convert.ToString(intMinute)));
        }
        #endregion
    }
}