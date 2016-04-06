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
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;
    protected string strRedirectUrl = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = Convert.ToString(commonVariables.DepositMethod.ECPSS);
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.ECPSS);

        base.CheckLogin();
        base.InitialiseVariables();

        base.InitialisePaymentLimits();

        base.GetMainWalletBalance("0");

        drpBank.Items.AddRange(base.InitializeBank("ECPSSBank").ToArray());

        this.GetDummyURL();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CancelUnexpectedRePost();

        HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");
        commonPaymentMethodFunc.GetDepositMethodList(strMethodsUnAvailable, depositTabs, base.PageName, sender.ToString().Contains("app"));

        if (!Page.IsPostBack)
        {
            this.InitializeLabels();
        }
    }

    private void InitializeLabels()
    {
        lblMode.Text = base.strlblMode;
        txtMode.Text = base.strtxtMode;
        lblMinMaxLimit.Text = base.strlblMinMaxLimit;
        lblDailyLimit.Text = base.strlblDailyLimit;
        lblTotalAllowed.Text = base.strlblTotalAllowed;
        lblDepositAmount.Text = base.strlblDepositAmount;
        lblMessage.Text = base.strlblMessage;
        lblBank.Text = base.strlblBank;

        btnSubmit.Text = base.strbtnSubmit;

        txtDepositAmount.Attributes.Add("PLACEHOLDER", base.strtxtDepositAmount);

        txtMinMaxLimit.Text = base.strtxtMinMaxLimit;
        txtDailyLimit.Text = base.strtxtDailyLimit;
        txtTotalAllowed.Text = base.strtxtTotalAllowed;
    }

    private void GetDummyURL()
    {
        using (wsDummy.dummyWSSoapClient client = new wsDummy.dummyWSSoapClient())
        {
            DataSet result = client.DummyURLs(Convert.ToInt64(strOperatorId), Convert.ToInt64(base.PaymentMethodId), Convert.ToString(HttpContext.Current.Session["PaymentGroup"]));

            this.strRedirectUrl = result.Tables[0].Rows.Count == 0 ? string.Empty : Convert.ToString(result.Tables[0].Rows[0]["redirectUrl"]);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsPageRefresh)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        string strDepositAmount = txtDepositAmount.Text.Trim();
        string selectedBank = drpBank.SelectedItem.Value;

        decimal decDepositAmount = commonValidation.isDecimal(strDepositAmount) ? Convert.ToDecimal(strDepositAmount) : 0;
        decimal decMinLimit = Convert.ToDecimal(strMinLimit);
        decimal decMaxLimit = Convert.ToDecimal(strMaxLimit);

        CommonStatus status = new CommonStatus();

            try
            {
                if (decDepositAmount == 0)
                {
                    status = base.GetErrors("/MissingDepositAmount");
                }
                else if (selectedBank == "-1")
                {
                    status = base.GetErrors("/SelectBank");
                }
                else if (decDepositAmount < decMinLimit)
                {
                    status = base.GetErrors("/AmountMinLimit");
                }
                else if (decDepositAmount > decMaxLimit)
                {
                    status = base.GetErrors("/AmountMaxLimit");
                }
            else if ((strTotalAllowed != strUnlimited) && (decDepositAmount > Convert.ToDecimal(strTotalAllowed)) && Convert.ToDecimal(strTotalAllowed) > 0)
                {
                    status = base.GetErrors("/TotalAllowedExceeded");
                }

            if (!status.IsProcessAbort)
                {
                    status.AlertCode = "0";
                }
            }
            catch (Exception ex)
            {
                status = base.GetErrors("/Exception");

                strErrorDetail = ex.Message;
            }

            strAlertCode = status.AlertCode;
            strAlertMessage = status.AlertMessage;

            string strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | DepositAmount: {3} | BankName: {4} | MinLimit: {5} | MaxLimit: {6} | TotalAllowed: {7} | DailyLimit: {8} | Response: {9}",
               Convert.ToInt64(strOperatorId), strMemberCode, strCurrencyCode, strDepositAmount, drpBank.SelectedValue, decMinLimit, decMaxLimit, strTotalAllowed, strDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

            intProcessSerialId += 1;
        commonAuditTrail.appendLog("system", base.PageName, "InitiateDeposit", string.Empty, strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
    }
}