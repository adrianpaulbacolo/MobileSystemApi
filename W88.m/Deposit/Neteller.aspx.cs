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

public partial class Deposit_Neteller : PaymentBasePage
{
    protected string strStatusCode = string.Empty;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e) { base.CheckLogin(); }

    protected void Page_Load(object sender, EventArgs e)
    {
        CancelUnexpectedRePost();

        
        commonCulture.appData.getRootResource("/Deposit/SDPay", out xeResources);

        this.paymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.Neteller);

        if (!Page.IsPostBack)
        {
            lblMode.Text = commonCulture.ElementValues.getResourceString("lblMode", xeResources);
            txtMode.Text = string.Format(": {0}", commonCulture.ElementValues.getResourceString("txtMode", xeResources));
            lblMinMaxLimit.Text = commonCulture.ElementValues.getResourceString("lblMinMaxLimit", xeResources);
            lblDailyLimit.Text = commonCulture.ElementValues.getResourceString("lblDailyLimit", xeResources);
            lblTotalAllowed.Text = commonCulture.ElementValues.getResourceString("lblTotalAllowed", xeResources);
            //lblDepositAmount.Text = commonCulture.ElementValues.getResourceString("lblDepositAmount", xeResources);
            //lblNoticeDownload.Text = commonCulture.ElementValues.getResourceString("lblNoticeDownload", xeResources);

            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);

            txtDepositAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} {1}", commonCulture.ElementValues.getResourceString("lblDepositAmount", xeResources), strCurrencyCode));
            txtAccountId.Attributes.Add("PLACEHOLDER", string.Format("{0}", commonCulture.ElementValues.getResourceString("accountId", commonVariables.LeftMenuXML)));
            txtSecureId.Attributes.Add("PLACEHOLDER", string.Format("{0}", commonCulture.ElementValues.getResourceString("secureId", commonVariables.LeftMenuXML)));


            System.Threading.Tasks.Task t1 = System.Threading.Tasks.Task.Factory.StartNew(this.InitialisePaymentLimits);

            System.Threading.Tasks.Task.WaitAll(t1);

            HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");

            commonPaymentMethodFunc.getDepositMethodList(strMethodsUnAvailable, depositTabs, "neteller");

            txtDepositAmount.Attributes.Add("PLACEHOLDER", string.Format("{0} ({1})", commonCulture.ElementValues.getResourceString("lblDepositAmount", xeResources), strCurrencyCode));

            txtMinMaxLimit.Text = string.Format(": {0} / {1}", strMinLimit, strMaxLimit);
            txtDailyLimit.Text = string.Format(": {0}", strDailyLimit);
            txtTotalAllowed.Text = string.Format(": {0}", strTotalAllowed);

            this.getMainWalletBalance("0");
        }
    }

    public static string decrypting(string str)
    {
        string privateKey = System.Configuration.ConfigurationManager.AppSettings.Get("PrivateKey_nextPay");
        string functionReturnValue = null;
        if (!string.IsNullOrEmpty(str))
        {
            encryption_manager.encryption decrypt = new encryption_manager.encryption();
            decrypt.private_key = privateKey;
            decrypt.message = str;
            functionReturnValue = decrypt.decrypting();
            decrypt = null;
        }
        else { functionReturnValue = string.Empty; }
        return functionReturnValue;
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string strPageName = "Deposit.Neteller";

        string statusCode = string.Empty;
        string statusText = string.Empty;
        string strDepositAmount = txtDepositAmount.Text.Trim();
        string accountId = txtAccountId.Text.Trim();
        string secureId = txtSecureId.Text.Trim();

        string strResultCode = string.Empty;
        string strResultDetail = string.Empty;
        string strErrorCode = string.Empty;

        System.Xml.Linq.XElement xeResponse = null;
        bool isDepositSuccessful = false;
        string strTransferId = string.Empty;


        bool isProcessAbort = false;

        decimal decMinLimit = decimal.Zero;
        decimal decMaxLimit = decimal.Zero;
        decimal decTotalAllowed = decimal.Zero;
        decimal decDailyLimit = decimal.Zero;


        string strProcessRemark = string.Empty;
        string strErrorDetail = string.Empty;
        int intProcessSerialId = 0;
        string strProcessId = Guid.NewGuid().ToString().ToUpper();
        bool isSystemError = false;



        #region initialiseDeposit
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
                else if ((Convert.ToDecimal(strDepositAmount) > Convert.ToDecimal(strTotalAllowed)) && (Convert.ToDecimal(strTotalAllowed) > 0))
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
                            isDepositSuccessful = Convert.ToBoolean(commonCulture.ElementValues.getResourceString("result", xeResponse));
                            strTransferId = commonCulture.ElementValues.getResourceString("invId", xeResponse);


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

            strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | DepositAmount: {3} | DepositChannel: {4} | NetellerAccounttId: {6} | NetellerSecureId: {6} | MinLimit: {13} | MaxLimit: {14} | TotalAllowed: {15} | DailyLimit: {16} | Response: {17}",
                Convert.ToInt64(strOperatorId), strMemberCode, strCurrencyCode, strDepositAmount, accountId, secureId, decMinLimit, decMaxLimit, decTotalAllowed, decDailyLimit, xeResponse == null ? string.Empty : xeResponse.ToString());

            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", strPageName, "InitiateDeposit", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        }

        getMainWalletBalance("0");

        System.Threading.Tasks.Task t5 = System.Threading.Tasks.Task.Factory.StartNew(this.InitialisePaymentLimits);
        #endregion
    }

}
