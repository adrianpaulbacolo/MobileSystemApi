using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class v2_Deposit_Pay120244 : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.DaddyPayQR);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.DaddyPayQR);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            InitializeWeChatDenominations();
        }
    }

    private void InitializeWeChatDenominations()
    {
        XElement xElementBank;
        commonCulture.appData.getRootResource("/Deposit/DaddyPay", out xElementBank);

        List<ListItem> denoms = new List<ListItem>();
        XElement denom = xElementBank.Element("denoms");

        denoms.AddRange(denom.Elements("denom").Select(x => new ListItem(x.Value, x.Attribute("id").Value)));

        drpDepositAmount.Items.AddRange(denoms.ToArray());
    }
}