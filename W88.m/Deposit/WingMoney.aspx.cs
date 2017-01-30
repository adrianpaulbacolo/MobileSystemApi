using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.XPath;

public partial class Deposit_WingMoney : PaymentBasePage
{
    protected string lblTransactionId;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.WingMoney);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.WingMoney);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.InitialiseDepositDateTime();

            this.InitializeLabels();

            if (string.Compare(strCurrencyCode, "krw", true) == 0)
            {
                divDepositDateTime.Visible = false;
            }
        }
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

    private void InitializeLabels()
    {
        lblDepositAmount.Text = base.strlblAmount;

        lblAccountName.Text = base.strlblAccountName;
        lblAccountNumber.Text = base.strlblAccountNumber;

        lblReferenceId.Text = commonCulture.ElementValues.getResourceString("lblReferenceId", xeResources);

        lblDepositDateTime.Text = commonCulture.ElementValues.getResourceString("drpDepositDateTime", xeResources);

        lblTransactionId = base.strlblTransactionId;
    }
}