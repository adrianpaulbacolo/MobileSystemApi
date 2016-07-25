using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Linq;

public partial class Deposit_SDAPay2 : PaymentBasePage
{
    protected string strStatusCode = string.Empty;
    protected string strStatusText = string.Empty;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;
    protected string strTransactionId = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {

        base.PageName = "SDAPayAlipay";
        base.PaymentType = commonVariables.PaymentTransactionType.Deposit;
        base.PaymentMethodId = Convert.ToString((int)commonVariables.DepositMethod.SDAPayAlipay);

        base.CheckLogin();
        base.InitialiseVariables();

        base.InitialisePaymentLimits();

        base.GetMainWalletBalance("0");

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        HtmlGenericControl depositTabs = (HtmlGenericControl)FindControl("depositTabs");
        commonPaymentMethodFunc.GetDepositMethodList(strMethodsUnAvailable, depositTabs, base.PageName, sender.ToString().Contains("app"), base.strCurrencyCode);

        if (!Page.IsPostBack)
        {
            string id = Request.QueryString["id"];

            long transactionId = string.IsNullOrWhiteSpace(id) ? 0 : Convert.ToInt64(id);

            if (transactionId <= 0)
            {
                strAlertCode = "-1";
                strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/TransferFail", xeErrors);
            }
            else
            {
                lblStatus.Text = commonCulture.ElementValues.getResourceString("lblStatus", xeResources);
                txtStatus.Text = string.Format(": {0}", commonCulture.ElementValues.getResourceString("litStatusPending", xeResources));
                lblTransactionId.Text = commonCulture.ElementValues.getResourceString("lblTransactionId", xeResources);
                lblAmount.Text = commonCulture.ElementValues.getResourceString("lblAmount", xeResources);
                lblAmountNote.Text = commonCulture.ElementValues.getResourceString("lblAmtNote", xeResources);
                lblBankName.Text = commonCulture.ElementValues.getResourceString("lblBankName", xeResources);
                lblBankHolderName.Text = commonCulture.ElementValues.getResourceString("lblBankHolderName", xeResources);
                lblBankAccountNo.Text = commonCulture.ElementValues.getResourceString("lblBankAccountNo", xeResources);

                btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);
                btnSubmit.NavigateUrl = ConfigurationManager.AppSettings["SDAPayAlipayBankUrl"];

                GetCurrentDeposit(transactionId);
            }
        }
    }

    private void GetCurrentDeposit(long transactionId)
    {
        try
        {
            using (svcPayDeposit.DepositClient client = new svcPayDeposit.DepositClient())
            {
                DataTable dt = client.getSDAPayDepositTransaction(transactionId);

                if (dt == null || dt.Rows[0]["state"].ToString() != "0")
                {
                    strAlertCode = "-1";
                }
                else
                {
                    DataRow dr = dt.Rows[0];

                    strTransactionId = dr["invId"].ToString();
                    txtTransactionId.Text = string.Format(": {0}", dr["invId"].ToString());

                    string ePrice = Convert.ToDecimal(dr["ePrice"]).ToString("N2");
                    txtAmount.Text = string.Format(": {0}", ePrice);

                    txtBankName.Text = string.Format(": {0}", base.InitializeBank("SDAPayAlipayBank").FirstOrDefault(bank => bank.Value.Equals(dr["eBank"].ToString(), StringComparison.OrdinalIgnoreCase)).Text);
                    txtBankHolderName.Text = string.Format(": {0}", dr["eName"].ToString());

                    string eBankAccount = dr["eBankAccount"].ToString();
                    if (eBankAccount.Length == 16)
                        eBankAccount = eBankAccount.Substring(0, 4) + " " + eBankAccount.Substring(4, 4) + " " + eBankAccount.Substring(8, 4) + " " + eBankAccount.Substring(12, 4);

                    txtBankAccountNo.Text = string.Format(": {0}", eBankAccount);

                }
            }
        }
        catch (Exception ex)
        {
            strAlertCode = "-1";
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString(base.PaymentType.ToString() + "/Exception", xeErrors);

            strErrorDetail = ex.Message;
        }

        string strProcessRemark = string.Format("OperatorId: {0} | MemberCode: {1} | CurrencyCode: {2} | DepositAmount: {3} | BankName: {4} | BankAccountNo: {5} | BankHolderName: {6} | MinLimit: {7} | MaxLimit: {8} | TotalAllowed: {9} | DailyLimit: {10} | Response: {11}",
            Convert.ToInt64(strOperatorId), strMemberCode, strCurrencyCode, txtAmount.Text, txtBankName.Text, txtBankAccountNo.Text, txtBankHolderName.Text, strMinLimit, strMaxLimit, strTotalAllowed, strDailyLimit, string.Empty);

        intProcessSerialId += 1;

        commonAuditTrail.appendLog("system", PageName, "GetCurrentDeposit", string.Empty, strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    public static string CheckDeposit(string strTransactionId)
    {
        string result = string.Empty;
        try
        {
            using (svcPayDeposit.DepositClient client = new svcPayDeposit.DepositClient())
            {
                long transactionId = string.IsNullOrWhiteSpace(strTransactionId) ? 0 : Convert.ToInt64(strTransactionId);

                DataTable dt = client.getDepositTransaction(transactionId);

                if (dt != null)
                {
                    result = dt.Rows[0]["status"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", "CheckDeposit", "CheckDeposit", string.Empty, string.Empty, string.Empty, "-99", "exception", ex.Message, string.Empty, string.Empty, true);
        }

        return result;
    }
}
