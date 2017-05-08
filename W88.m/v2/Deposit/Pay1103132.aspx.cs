using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class v2_Deposit_Pay1103132 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        PageName = Convert.ToString(commonVariables.DepositMethod.TrueMoney);
        PaymentType = commonVariables.PaymentTransactionType.Deposit;
        PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.TrueMoney);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}