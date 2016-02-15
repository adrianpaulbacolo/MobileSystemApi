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
    protected string strStatusCode = string.Empty;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        base.PageName = "ShengPay";
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.ShengPay);

        base.CheckLogin();
        base.InitialiseVariables();

        base.InitialisePaymentLimits();

        base.GetMainWalletBalance("0");

        drpBank.Items.AddRange(base.InitializeBank("ShengPayBank").ToArray());
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CancelUnexpectedRePost();

        HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");
        commonPaymentMethodFunc.getDepositMethodList(strMethodsUnAvailable, depositTabs, "shengpay", sender.ToString().Contains("app"));

        if (!Page.IsPostBack)
        {
            this.InitializeLabels();
        }
    }

    private void InitializeLabels(){
        lblMode.Text = base.lblMode;
        txtMode.Text = base.txtMode;
        lblMinMaxLimit.Text = base.lblMinMaxLimit;
        lblDailyLimit.Text = base.lblDailyLimit;
        lblTotalAllowed.Text = base.lblTotalAllowed;
        lblDepositAmount.Text = base.lblDepositAmount;

        btnSubmit.Text = base.btnSubmit;
        btnCancel.InnerText = base.btnCancel;

        txtDepositAmount.Attributes.Add("PLACEHOLDER", base.txtDepositAmount);

        txtMinMaxLimit.Text = base.txtMinMaxLimit;
        txtDailyLimit.Text = base.txtDailyLimit;
        txtTotalAllowed.Text = base.txtTotalAllowed;
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

        if (!isProcessAbort)
        {

            try
            {
                if (decDepositAmount == 0)
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/MissingDepositAmount", xeErrors);
                    isProcessAbort = true;
                }
                else if (selectedBank == "-1")
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/SelectBank", xeErrors);
                    isProcessAbort = true;
                }
                if (decDepositAmount < decMinLimit)
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/AmountMinLimit", xeErrors);
                    isProcessAbort = true;
                }
                else if (decDepositAmount > decMaxLimit)
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/AmountMaxLimit", xeErrors);
                    isProcessAbort = true;
                }
                else if ((strTotalAllowed != commonCulture.ElementValues.getResourceString("unlimited", xeResources)) && (decDepositAmount > Convert.ToDecimal(strTotalAllowed)) && Convert.ToDecimal(strTotalAllowed) > 0)
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TotalAllowedExceeded", xeErrors);
                    isProcessAbort = true;
                }

                if (!isProcessAbort)
                {
                    strAlertCode = "0";
                }
            }
            catch (Exception ex)
            {
                strAlertCode = "-1";
                strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/Exception", xeErrors);

                strErrorDetail = ex.Message;
            }

            string strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | DepositAmount: {3} | BankName: {4} | MinLimit: {5} | MaxLimit: {6} | TotalAllowed: {7} | DailyLimit: {8} | Response: {9}",
               Convert.ToInt64(strOperatorId), strMemberCode, strCurrencyCode, strDepositAmount, drpBank.SelectedValue, decMinLimit, decMaxLimit, strTotalAllowed, strDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", PageName, "InitiateDeposit", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        }
    }
}