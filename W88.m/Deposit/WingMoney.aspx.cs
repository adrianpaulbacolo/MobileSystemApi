using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.XPath;

public partial class Deposit_WingMoney : PaymentBasePage
{
    protected string strTitle = string.Empty;
    protected string strStatusCode = string.Empty;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {

        base.PageName = "WingMoney";
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.WingMoney);

        base.CheckLogin();
        base.InitialiseVariables();

        base.InitialisePaymentLimits();

        base.GetMainWalletBalance("0");

        this.InitialiseDepositDateTime();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CancelUnexpectedRePost();

        HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");
        commonPaymentMethodFunc.getDepositMethodList(strMethodsUnAvailable, depositTabs, base.PageName, sender.ToString().Contains("app"));

        if (!Page.IsPostBack)
        {
            strTitle = commonCulture.ElementValues.getResourceString("lblTitle", xeResources);

            lblDepositAmount.Text = commonCulture.ElementValues.getResourceString("lblDepositAmount", xeResources);
            lblReferenceId.Text = commonCulture.ElementValues.getResourceString("lblReferenceId", xeResources);
            lblAccountName.Text = commonCulture.ElementValues.getResourceString("lblAccountName", xeResources);
            lblAccountNumber.Text = commonCulture.ElementValues.getResourceString("lblAccountNumber", xeResources);

            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);

            if (string.Compare(strCurrencyCode, "krw", true) == 0)
            {
                divDepositDateTime.Visible = false;
            }

            txtDepositAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} {1}({2} / {3})", lblDepositAmount.Text, strCurrencyCode, strMinLimit, strMaxLimit));
            lblDailyLimit.Text = string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("lblDailyLimit", xeResources), strDailyLimit);
            lblTotalAllowed.Text = string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("lblTotalAllowed", xeResources), strTotalAllowed);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsPageRefresh)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        DateTime dtDepositDateTime = DateTime.MinValue;

        string strDepositAmount = txtDepositAmount.Text.Trim();
        string strReferenceId = txtReferenceId.Text;
        string strDepositDate = drpDepositDate.SelectedValue;
        string strDepositHour = drpHour.SelectedValue;
        string strDepositMinute = drpMinute.SelectedValue;
        string strAccountName = txtAccountName.Text;
        string strAccountNumber = txtAccountNumber.Text;

        decimal decDepositAmount = commonValidation.isDecimal(strDepositAmount) ? Convert.ToDecimal(strDepositAmount) : 0;
        decimal decMinLimit = Convert.ToDecimal(strMinLimit);
        decimal decMaxLimit = Convert.ToDecimal(strMaxLimit);

        #region initialiseDeposit
        if (!isProcessAbort)
        {
            try
            {
                if (string.Compare(strCurrencyCode, "krw", true) == 0)
                {
                    dtDepositDateTime = DateTime.Now;
                }

                if (decDepositAmount == 0)
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/MissingDepositAmount", xeErrors);
                    isProcessAbort = true;
                }
                else if (commonValidation.isInjection(strDepositAmount))
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/InvalidDepositAmount", xeErrors);
                    isProcessAbort = true;
                }
                else if (string.IsNullOrEmpty(strReferenceId))
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/MissingReferenceId", xeErrors);
                    isProcessAbort = true;
                }
                else if (commonValidation.isInjection(strReferenceId))
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/InvalidReferenceId", xeErrors);
                    isProcessAbort = true;
                }
                else if (string.IsNullOrEmpty(strAccountName))
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/MissingAccountName", xeErrors);
                    isProcessAbort = true;
                }
                else if (commonValidation.isInjection(strAccountName))
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/InvalidAccountName", xeErrors);
                    isProcessAbort = true;
                }
                else if (string.IsNullOrEmpty(strAccountNumber))
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/MissingAccountNumber", xeErrors);
                    isProcessAbort = true;
                }
                else if (commonValidation.isInjection(strAccountNumber))
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/InvalidAccountNumber", xeErrors);
                    isProcessAbort = true;
                }
                else if (string.IsNullOrEmpty(strDepositDate))
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/InvalidDateTime", xeErrors);
                    isProcessAbort = true;
                }
                else if (!string.IsNullOrEmpty(strDepositDate))
                {
                    dtDepositDateTime = DateTime.Parse(strDepositDate).AddHours(double.Parse(strDepositHour)).AddMinutes(double.Parse(strDepositMinute));
                    if ((dtDepositDateTime - DateTime.Now).TotalHours > 72 || (dtDepositDateTime - DateTime.Now).TotalHours < -72)
                    {
                        strAlertCode = "-1";
                        strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/InvalidDateTime", xeErrors);
                        isProcessAbort = true;
                    }
                }
                else if (decDepositAmount < decMinLimit)
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
                    using (svcPayDeposit.DepositClient svcInstance = new svcPayDeposit.DepositClient())
                    {
                        xeResponse = svcInstance.createWingDepositTransactionV1(Convert.ToInt64(strOperatorId), strMemberCode, Convert.ToInt64(commonVariables.DepositMethod.WingMoney),
                                    strCurrencyCode, Convert.ToDecimal(strDepositAmount), strAccountName, strAccountNumber, dtDepositDateTime,
                                    strReferenceId, Convert.ToString(commonVariables.TransactionSource.Mobile));

                        if (xeResponse == null)
                        {
                            strAlertCode = "-1";
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferFail", xeErrors);
                        }
                        else
                        {
                            bool isDepositSuccessful = Convert.ToBoolean(commonCulture.ElementValues.getResourceString("result", xeResponse));
                            string strTransferId = commonCulture.ElementValues.getResourceString("invId", xeResponse);

                            if (isDepositSuccessful)
                            {
                                strAlertCode = "0";
                                strAlertMessage = string.Format("{0}\\n{1}: {2}", commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferSuccess", xeErrors), strlblTransactionId, strTransferId);
                            }
                            else
                            {
                                strAlertCode = "-1";
                                strAlertMessage = string.Format("{0}\\n{1}", commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferFail", xeErrors), commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/error" + strTransferId, xeErrors));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strAlertCode = "-1";
                strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/Exception", xeErrors);

                strErrorDetail = ex.Message;
            }

            string strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | DepositAmount: {3} | AccountName: {4} | AccountNumber: {5} | ReferenceID: {6} | DepositDateTime: {7} | MinLimit: {8} | MaxLimit: {9} | TotalAllowed: {10} | DailyLimit: {11} | Response: {12}",
                Convert.ToInt64(strOperatorId), strMemberCode, strCurrencyCode, strDepositAmount, strAccountName, strAccountNumber, strReferenceId, dtDepositDateTime.ToString("yyyy-MM-dd HH:mm:ss"), decMinLimit, decMaxLimit, strTotalAllowed, strDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", PageName, "InitiateDeposit", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        }
        #endregion
    }

    private void InitialiseDepositDateTime()
    {
        #region DepositDateTime

        drpDepositDate.Items.Add(new ListItem(commonCulture.ElementValues.getResourceString("drpDepositDateTime", xeResources), string.Empty));

        for (System.DateTime dtDepositDateTime = System.DateTime.Today.AddHours(-72); dtDepositDateTime < System.DateTime.Today.AddHours(72); dtDepositDateTime = dtDepositDateTime.AddHours(24))
        {
            drpDepositDate.Items.Add(new ListItem(dtDepositDateTime.ToString("dd / MMM / yyyy"), dtDepositDateTime.ToString("yyyy-MM-dd")));
        }

        for (int intHour = 0; intHour < 24; intHour++) { drpHour.Items.Add(new ListItem((intHour).ToString("0#"), Convert.ToString(intHour))); }
        for (int intMinute = 0; intMinute < 60; intMinute++) { drpMinute.Items.Add(new ListItem((intMinute).ToString("0#"), Convert.ToString(intMinute))); }
        #endregion
    }

}