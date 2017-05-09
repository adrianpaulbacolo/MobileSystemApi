using System;
using System.Web.UI.WebControls;

public partial class v2_Deposit_Pay110394 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        PageName = Convert.ToString(commonVariables.DepositMethod.PayGo);
        PaymentType = commonVariables.PaymentTransactionType.Deposit;
        PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.PayGo);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitialiseDepositDateTime();
        }
    }

    private void InitialiseDepositDateTime()
    {
        for (var dtDepositDateTime = DateTime.Today.AddHours(-72); dtDepositDateTime <= DateTime.Today.AddHours(72); dtDepositDateTime = dtDepositDateTime.AddHours(24))
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
    }

}