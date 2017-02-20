using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.XPath;

public partial class Deposit_MoneyTransfer : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        var money = this.RouteData.DataTokens["money"].ToString();
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        switch (money)
        {
            case "wing":
                base.PageName = Convert.ToString(commonVariables.DepositMethod.WingMoney);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.WingMoney);
                break;
            case "true":
                base.PageName = Convert.ToString(commonVariables.DepositMethod.TrueMoney);
                base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.TrueMoney);
                break;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!Page.IsPostBack)
        {
            this.InitialiseDepositDateTime();
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
}