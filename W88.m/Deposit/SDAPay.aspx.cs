using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class Deposit_SDAPay : PaymentBasePage
{
    protected string strStatusCode = string.Empty;
    protected string strStatusText = string.Empty;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;
    protected long transactionId = 0;

    protected void Page_Init(object sender, EventArgs e)
    {

        base.PageName = "SDAPayAlipay";
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.SDAPayAlipay);

        base.CheckLogin();
        base.InitialiseVariables();

        base.InitialisePaymentLimits();

        base.GetMainWalletBalance("0");

        drpBank.Items.AddRange(base.InitializeBank("SDAPayAlipayBank").Where(bank => bank.Value.Equals("alipay", StringComparison.OrdinalIgnoreCase)).ToArray());
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");
        commonPaymentMethodFunc.GetDepositMethodList(strMethodsUnAvailable, depositTabs, base.PageName, sender.ToString().Contains("app"));


        if (!Page.IsPostBack)
        {
            lblMode.Text = commonCulture.ElementValues.getResourceString("lblMode", xeResources);
            txtMode.Text = string.Format(": {0}", commonCulture.ElementValues.getResourceString("txtMode", xeResources));
            lblMinMaxLimit.Text = commonCulture.ElementValues.getResourceString("lblMinMaxLimit", xeResources);
            lblDailyLimit.Text = commonCulture.ElementValues.getResourceString("lblDailyLimit", xeResources);
            lblTotalAllowed.Text = commonCulture.ElementValues.getResourceString("lblTotalAllowed", xeResources);
            lblDepositAmount.Text = commonCulture.ElementValues.getResourceString("lblAmount", xeResources);
            lblIndicatorMessage.Text = commonCulture.ElementValues.getResourceString("lblIndicatorMessage", xeResources);

            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);

            txtDepositAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} ({1})", lblDepositAmount.Text, strCurrencyCode));

            txtMinMaxLimit.Text = string.Format(": {0} / {1}", strMinLimit, strMaxLimit);
            txtDailyLimit.Text = string.Format(": {0}", strDailyLimit);
            txtTotalAllowed.Text = string.Format(": {0}", strTotalAllowed);

            drpBank.SelectedValue = "alipay";
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {

        string strDepositAmount = txtDepositAmount.Text.Trim();
        string selectedBank = drpBank.SelectedItem.Value;
        selectedBank = "cmb";

        decimal decDepositAmount = commonValidation.isDecimal(strDepositAmount) ? Convert.ToDecimal(strDepositAmount) : 0;
        decimal decMinLimit = commonValidation.isDecimal(strMinLimit) ? Convert.ToDecimal(strMinLimit) : 0;
        decimal decMaxLimit = commonValidation.isDecimal(strMaxLimit) ? Convert.ToDecimal(strMaxLimit) : 0;

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
                    using (svcPayDeposit.DepositClient client = new svcPayDeposit.DepositClient())
                    {
                        xeResponse = client.createOnlineDepositTransactionV1(Convert.ToInt64(strOperatorId), Convert.ToInt64(strMemberID), strMemberCode, Convert.ToInt64(base.PaymentMethodId), strCurrencyCode, decDepositAmount, svcPayDeposit.DepositSource.Mobile, string.Empty);

                        if (xeResponse == null)
                        {
                            strAlertCode = "-1";
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferFail", xeErrors);
                        }
                        else
                        {
                            bool isTransactionSuccessful = Convert.ToBoolean(commonCulture.ElementValues.getResourceString("result", xeResponse));
                            long transferId = Convert.ToInt64(commonCulture.ElementValues.getResourceString("invId", xeResponse));

                            if (isTransactionSuccessful)
                            {
                                XElement xeSDAPayResponse = client.createSDAPayTransactionV1(transferId, decDepositAmount, Convert.ToInt64(strMemberID), strMemberName, selectedBank, strMerchantId, Convert.ToInt64(PaymentMethodId), out strStatusCode, out strStatusText);

                                if (xeSDAPayResponse == null)
                                {
                                    strAlertCode = "-1";
                                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferFail", xeErrors);
                                }
                                else
                                {
                                    bool isSuccess = Convert.ToBoolean(commonCulture.ElementValues.getResourceString("result", xeSDAPayResponse));
                                    decimal amount = Convert.ToDecimal(commonCulture.ElementValues.getResourceString("amount", xeSDAPayResponse));

                                    if (isSuccess)
                                    {
                                        if (client.updateDepositAmount(transferId, amount))
                                        {
                                            strAlertCode = "0";
                                            transactionId = transferId;
                                        }
                                        else
                                        {
                                            strAlertCode = "-1";
                                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferFail", xeErrors);
                                        }
                                    }
                                    else
                                    {
                                        strAlertCode = "-1";
                                        strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferFail", xeErrors);
                                    }
                                }
                            }
                            else
                            {
                                strAlertCode = "-1";
                                strAlertMessage = string.Format("{0}\\n{1}", commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferFail", xeErrors), commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/error" + transferId, xeErrors));
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

            string strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | DepositAmount: {3} | BankName: {4} | MinLimit: {5} | MaxLimit: {6} | TotalAllowed: {7} | DailyLimit: {8} | Response: {9}",
               Convert.ToInt64(strOperatorId), strMemberCode, strCurrencyCode, strDepositAmount, drpBank.SelectedValue, decMinLimit, decMaxLimit, strTotalAllowed, strDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", PageName, "InitiateDeposit", string.Empty, strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        }
    }
}
