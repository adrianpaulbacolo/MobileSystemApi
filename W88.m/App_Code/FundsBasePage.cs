using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Linq;

/// <summary>
/// Summary description for FundsBasePage
/// </summary>
public class FundsBasePage : BasePage
{
    protected string PageName { get; set; }
    protected commonVariables.PaymentTransactionType PaymentType { get; set; }
    protected string PaymentMethodId { get; set; }

    protected override void OnPreInit(EventArgs e)
    {
        this.isPublic = false;
        UserSession.checkSession();
    }

    protected override void OnPreLoad(EventArgs e)
    {
        base.OnPreLoad(e);

        if (!string.IsNullOrEmpty(PaymentMethodId))
        {
            try
            {
                Page.Items.Add("Parent", "/v2/Funds.aspx");
            }
            catch (Exception ex) { }
        }
    }
}