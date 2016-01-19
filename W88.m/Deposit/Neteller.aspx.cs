using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class Deposit_Neteller : PaymentBasePage
{
    protected string strStatusCode = string.Empty;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    private string PageName
    {
        get
        {
            return "Deposit.Neteller";
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        base.CheckLogin();
        base.InitialiseVariables("Neteller");

        this.paymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.Neteller);
        this.InitialisePaymentLimits();

        this.getMainWalletBalance("0");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CancelUnexpectedRePost();

        HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");
        commonPaymentMethodFunc.getDepositMethodList(strMethodsUnAvailable, depositTabs, "neteller");

        if (!Page.IsPostBack)
        {
            lblMode.Text = commonCulture.ElementValues.getResourceString("lblMode", xeResources);
            txtMode.Text = string.Format(": {0}", commonCulture.ElementValues.getResourceString("txtMode", xeResources));
            lblMinMaxLimit.Text = commonCulture.ElementValues.getResourceString("lblMinMaxLimit", xeResources);
            lblDailyLimit.Text = commonCulture.ElementValues.getResourceString("lblDailyLimit", xeResources);
            lblTotalAllowed.Text = commonCulture.ElementValues.getResourceString("lblTotalAllowed", xeResources);

            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);

            txtDepositAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("lblDepositAmount", xeResources), strCurrencyCode));
            txtAccountId.Attributes.Add("PLACEHOLDER", string.Format("{0}", commonCulture.ElementValues.getResourceString("accountId", commonVariables.LeftMenuXML)));
            txtSecureId.Attributes.Add("PLACEHOLDER", string.Format("{0}", commonCulture.ElementValues.getResourceString("secureId", commonVariables.LeftMenuXML)));

            txtDepositAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} ({1})", commonCulture.ElementValues.getResourceString("lblDepositAmount", xeResources), strCurrencyCode));

            txtMinMaxLimit.Text = string.Format(": {0} / {1}", strMinLimit, strMaxLimit);
            txtDailyLimit.Text = string.Format(": {0}", strDailyLimit);
            txtTotalAllowed.Text = string.Format(": {0}", strTotalAllowed);
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (IsPageRefresh)
        {
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        string strDepositAmount = txtDepositAmount.Text.Trim();
        string accountId = txtAccountId.Text.Trim();
        string secureId = txtSecureId.Text.Trim();

        decimal decMinLimit = decimal.Zero;
        decimal decMaxLimit = decimal.Zero;
        decimal decTotalAllowed = decimal.Zero;
        decimal decDailyLimit = decimal.Zero;

        if (!isProcessAbort)
        {
            try
            {
                if (Convert.ToDecimal(strDepositAmount) < Convert.ToDecimal(strMinLimit))
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/AmountMinLimit", xeErrors);
                    isProcessAbort = true;
                }
                else if (Convert.ToDecimal(strDepositAmount) > Convert.ToDecimal(strMaxLimit))
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/AmountMaxLimit", xeErrors);
                    isProcessAbort = true;
                }
                else if ((strTotalAllowed != commonCulture.ElementValues.getResourceString("unlimited", xeResources)) && (Convert.ToDecimal(strDepositAmount) > Convert.ToDecimal(strTotalAllowed)) && Convert.ToDecimal(strTotalAllowed) > 0)
                {
                    strAlertCode = "-1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/TotalAllowedExceeded", xeErrors);
                    isProcessAbort = true;
                }


                if (!isProcessAbort)
                {
                    using (DepositServices.DepositClient client = new DepositServices.DepositClient())
                    {
                        xeResponse = client.createOnlineDepositTransactionV1(Convert.ToInt64(strOperatorId), Convert.ToInt64(strMemberID), strMemberCode, Convert.ToInt64(paymentMethodId), strCurrencyCode, Convert.ToDecimal(strDepositAmount), DepositServices.DepositSource.Mobile, string.Empty);

                        if (xeResponse == null)
                        {
                            strAlertCode = "-1";
                            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/TransferFail", xeErrors);
                        }
                        else
                        {
                            bool isDepositSuccessful = Convert.ToBoolean(commonCulture.ElementValues.getResourceString("result", xeResponse));
                            string strTransferId = commonCulture.ElementValues.getResourceString("invId", xeResponse);


                            if (isDepositSuccessful)
                            {
                                bool isSuccess = client.createNetellerTransaction(Convert.ToInt64(strTransferId), Convert.ToInt64(strOperatorId), Convert.ToDecimal(strDepositAmount), strCurrencyCode, Convert.ToInt64(strMemberID), strMemberCode, Convert.ToInt64(accountId), Convert.ToInt32(secureId));

                                if (isSuccess)
                                {
                                    strAlertCode = "0";
                                    strAlertMessage = string.Format("{0}\\n{1}: {2}", commonCulture.ElementValues.getResourceXPathString("/Deposit/TransferSuccess", xeErrors), commonCulture.ElementValues.getResourceString("lblTransactionId", xeResources), strTransferId);
                                }
                                else
                                {
                                    strAlertCode = "-1";
                                    strAlertMessage = string.Format("{0}\\n{1}", commonCulture.ElementValues.getResourceXPathString("/Deposit/TransferFail", xeErrors), commonCulture.ElementValues.getResourceXPathString("/Deposit/NetellerDepositFailed", xeErrors));
                                }
                            }
                            else
                            {
                                strAlertCode = "-1";
                                strAlertMessage = string.Format("{0}\\n{1}", commonCulture.ElementValues.getResourceXPathString("/Deposit/TransferFail", xeErrors), commonCulture.ElementValues.getResourceXPathString("/Deposit/error" + strTransferId, xeErrors));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strAlertCode = "-1";
                strAlertMessage = commonCulture.ElementValues.getResourceXPathString("/Deposit/Exception", xeErrors);

                strErrorDetail = ex.Message;
            }

            txtDepositAmount.Text = string.Empty;
            txtAccountId.Text = string.Empty;
            txtSecureId.Text = string.Empty;

            string strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | DepositAmount: {3} | NetellerAccounttId: {4} | NetellerSecureId: {5} | MinLimit: {6} | MaxLimit: {7} | TotalAllowed: {8} | DailyLimit: {9} | Response: {10}",
                Convert.ToInt64(strOperatorId), strMemberCode, strCurrencyCode, strDepositAmount, accountId, secureId, decMinLimit, decMaxLimit, decTotalAllowed, decDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", PageName, "InitiateDeposit", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        }
    }

}
