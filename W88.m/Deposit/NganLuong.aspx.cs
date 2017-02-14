using System;
using System.Web.UI;


public partial class Deposit_NganLuong : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.NganLuong);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.NganLuong);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CheckAgentAndRedirect(V2DepositPath + "Pay120212.aspx");
            lblMessage.Text = base.strlblVendorNote;
            btnSubmit.Text = commonCulture.ElementValues.getResourceString("proceed", commonVariables.LeftMenuXML);
        }
    }
}