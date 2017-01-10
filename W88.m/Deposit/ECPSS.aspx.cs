using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;


public partial class Deposit_ECPSS : PaymentBasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.ECPSS);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.ECPSS);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            drpBank.Items.AddRange(base.InitializeBank("ECPSSBank").ToArray());
            this.InitializeLabels();
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

        lblMessage.Text = base.strlblMessage;

        lblBank.Text = base.strlblBank;

        btnSubmit.Text = base.strbtnSubmit;
    }
}