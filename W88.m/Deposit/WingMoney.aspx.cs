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
    protected string strStatusCode = string.Empty;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {

        base.PageName = "WingMoney";
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.WingMoney);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.InitialiseDepositDateTime();

            this.InitializeLabels();

            if (string.Compare(strCurrencyCode, "krw", true) == 0)
            {
                divDepositDateTime.Visible = false;
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        DateTime dtDepositDateTime = DateTime.MinValue;

        string strDepositAmount = txtAmount.Text.Trim();
        string strReferenceId = txtReferenceId.Text;
        string strDepositDate = drpDepositDate.SelectedValue;
        string strDepositHour = drpHour.SelectedValue;
        string strDepositMinute = drpMinute.SelectedValue;
        string strAccountName = txtAccountName.Text;
        string strAccountNumber = txtAccountNumber.Text;

        decimal decDepositAmount = commonValidation.isDecimal(strDepositAmount) ? Convert.ToDecimal(strDepositAmount) : 0;
        decimal decMinLimit = commonValidation.isDecimal(strMinLimit) ? Convert.ToDecimal(strMinLimit) : 0;
        decimal decMaxLimit = commonValidation.isDecimal(strMaxLimit) ? Convert.ToDecimal(strMaxLimit) : 0;

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
                    if (!IsValidDepositTime(dtDepositDateTime))
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

        for (System.DateTime dtDepositDateTime = System.DateTime.Today.AddHours(-72); dtDepositDateTime <= System.DateTime.Today.AddHours(72); dtDepositDateTime = dtDepositDateTime.AddHours(24))
        {
            drpDepositDate.Items.Add(new ListItem(dtDepositDateTime.ToString("dd / MM / yyyy"), dtDepositDateTime.ToString("yyyy-MM-dd")));
        }

        drpDepositDate.SelectedValue = DateTime.Now.ToString("yyyy-MM-dd");

        for (int intHour = 0; intHour < 24; intHour++)
        {
            drpHour.Items.Add(new ListItem((intHour).ToString("0#"), Convert.ToString(intHour)));
        }
        for (int intMinute = 0; intMinute < 60; intMinute++)
        {
            drpMinute.Items.Add(new ListItem((intMinute).ToString("0#"), Convert.ToString(intMinute)));
        }
        #endregion
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

        lblAccountName.Text = base.strlblAccountName;
        lblAccountNumber.Text = base.strlblAccountNumber;

        btnSubmit.Text = base.strbtnSubmit;

        lblReferenceId.Text = commonCulture.ElementValues.getResourceString("lblReferenceId", xeResources);

        lblDepositDateTime.Text = commonCulture.ElementValues.getResourceString("drpDepositDateTime", xeResources);
    }

    private bool IsValidDepositTime(DateTime validDateTime)
    {
        DateTime minDate = DateTime.Now.AddHours(-72);
        DateTime maxDate = DateTime.Now.AddHours(72);

        if (validDateTime < minDate || validDateTime > maxDate)
        {
            return false;
        }

        return true;
    }
}