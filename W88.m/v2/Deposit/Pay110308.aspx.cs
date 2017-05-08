using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_Deposit_Pay110308 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        PageName = Convert.ToString(commonVariables.DepositMethod.WingMoney);
        PaymentType = commonVariables.PaymentTransactionType.Deposit;
        PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.WingMoney);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}