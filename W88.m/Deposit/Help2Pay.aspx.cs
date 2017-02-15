using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;


public partial class Deposit_Help2Pay : PaymentBasePage
{
    protected string lblTransactionId;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.Help2Pay);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.Help2Pay);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.InitializeBank();
            this.InitializeLabels();
        }
    }

    private void InitializeBank()
    {
        try
        {
            XElement xElementBank = null;

            commonCulture.appData.getRootResource("/Deposit/Help2PayBank", out xElementBank);

            XElement xElementBankPath = xElementBank.Element(strCurrencyCode);
            var banks = from bank in xElementBankPath.Elements("bank") select new { value = bank.Attribute("id").Value, text = bank.Value };

            drpBank.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpBank", xeResources), "-1"));

            foreach (var b in banks)
            {
                drpBank.Items.Add(new ListItem(b.text, b.value));
            }
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", "help2pay", "InitializeBank", string.Empty, string.Empty, string.Empty, "-99", "exception", ex.Message, string.Empty, string.Empty, true);
        }
    }

    private void InitializeLabels()
    {
        lblAmount.Text = base.strlblAmount;

        lblBank.Text = base.strlblBank;

        lblTransactionId = base.strlblTransactionId;
    }
}