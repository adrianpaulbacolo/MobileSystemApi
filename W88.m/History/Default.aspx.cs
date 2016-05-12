using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class History_Default : PaymentBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        base.InitialiseVariables();

        base.GetMainWalletBalance("0");
    }
}
