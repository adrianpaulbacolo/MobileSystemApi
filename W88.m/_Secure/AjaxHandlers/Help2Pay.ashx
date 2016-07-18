<%@ WebHandler Language="C#" Class="Help2Pay" %>

using System;
using System.Web;
using System.Xml.Linq;
using System.Linq;
using System.Data;

public class Help2Pay : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.Write("Processing...");

        //    if (string.IsNullOrEmpty((string)HttpContext.Current.Session["user_MemberID"]))
        //        context.Response.Redirect((string)HttpContext.Current.Session["domain_Account"] + "/BankingOptions.aspx#logout");

        #region variables
        int processSerialId = 0;
        string processId = Guid.NewGuid().ToString().ToUpper();
        string pageName = "Pay120227Handler";
        string taskName = "ProcessRequest";
        string processDetail = string.Empty;
        decimal requestAmount = 0m;
        long invId = 0;

        //    string memberId = (string)HttpContext.Current.Session["user_MemberID"];
        //    string memberCode = (string)HttpContext.Current.Session["user_MemberCode"];
        //    string memberCurrency = (string)HttpContext.Current.Session["user_Currency"];
        string strAmount = context.Request.Params["requestAmount"];
        string bankCode = context.Request.Params["requestBank"];
        string strOperatorId = commonVariables.OperatorId;
        string strMemberId = commonVariables.GetSessionVariable("MemberId");
        string strMemberCode = commonVariables.GetSessionVariable("MemberCode");
        string strCurrencyCode = commonVariables.GetSessionVariable("CurrencyCode");

        string parameters = string.Format("{0} | {1} | {2} | {3} | {4} | {5} ", strOperatorId, strMemberId, strMemberCode, strCurrencyCode, requestAmount, bankCode);
        #endregion

        #region log parameters
        processSerialId++;
        processDetail = "log parameters";

        commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "ok", parameters, Convert.ToString(processSerialId), processId, false);
        
        #endregion

        #region parameter validation
        processSerialId++;
        processDetail = "parameter validation";

        bool bankFound = false;

        try
        {
            XElement xElementBank = null;
            commonCulture.appData.getRootResource("/Deposit/Help2PayBank", out xElementBank);
            XElement xElementBankPath = xElementBank.Element(commonVariables.GetSessionVariable("CurrencyCode"));
            var banks = from bank in xElementBankPath.Elements("bank") select new { value = bank.Attribute("id").Value, text = bank.Value };

            foreach (var b in banks)
            {
                if (b.value.ToUpper().Equals(bankCode.ToUpper()))
                {
                    bankFound = true;
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "-99", ex.ToString(), Convert.ToString(processSerialId), processId, true);
            return;
        }

        if (!bankFound)
        {
            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "invalid bank", Convert.ToString(processSerialId), processId, false);
            context.Response.Write("Invalid bank");
            return;
        }

        commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "ok", string.Empty, Convert.ToString(processSerialId), processId, false);
        #endregion

        #region check payment setting
        processSerialId++;
        processDetail = "check payment setting";

        try
        {
            DataTable dt;
            string statusCode, statusText;

            requestAmount = Convert.ToDecimal(strAmount);

            using (svcPayMember.MemberClient client = new svcPayMember.MemberClient())
            {
                dt = client.getMethodLimits_Mobile(strOperatorId, strMemberCode, Convert.ToInt32(commonVariables.DepositMethod.Help2Pay).ToString(), "1", false, out statusCode, out statusText);
                client.Close();
            }

            if (dt == null)
            {
                commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "payment setting null", Convert.ToString(processSerialId), processId, false);
                context.Response.Write("Limit error 1");
                return;
            }

            if (dt.Rows.Count != 1)
            {
                commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "payment setting row count error", Convert.ToString(processSerialId), processId, false);
                context.Response.Write("Limit error 2");
                return;
            }

            DataRow dr = dt.Rows[0];

            if (Convert.ToDecimal(dr["minDeposit"]) > requestAmount)
            {
                commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "Amount is less than min limit", Convert.ToString(processSerialId), processId, false);
                context.Response.Write("Min limit not reached");
                return;
            }

            if (Convert.ToDecimal(dr["maxDeposit"]) < requestAmount)
            {
                commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "Amount is more than max limit", Convert.ToString(processSerialId), processId, false);
                context.Response.Write("Max limit exceeded");
                return;
            }

            if (Convert.ToDecimal(dr["limitDaily"]) > 0m && Convert.ToDecimal(dr["totalAllowed"]) < requestAmount)
            {
                commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "Amount is more than total allowed", Convert.ToString(processSerialId), processId, false);
                context.Response.Write("Total allowed exceeded");
                return;
            }
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "-99", ex.ToString(), Convert.ToString(processSerialId), processId, true);
            context.Response.Write("Limit exception");
            return;
        }

        commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "ok", string.Empty, Convert.ToString(processSerialId), processId, false);
        #endregion

        #region create server deposit
        processSerialId++;
        processDetail = "create server deposit";
        System.Xml.Linq.XElement xElement;
        try
        {
            using (svcPayDeposit.DepositClient client = new svcPayDeposit.DepositClient())
            {
                xElement = client.createOnlineDepositTransactionV1(Convert.ToInt64(strOperatorId), Convert.ToInt64(strMemberId), strMemberCode, Convert.ToInt64(commonVariables.DepositMethod.Help2Pay), strCurrencyCode, requestAmount, svcPayDeposit.DepositSource.Mobile, bankCode);
            }

            if (xElement == null)
            {
                commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "deposit result is null", Convert.ToString(processSerialId), processId, false);
                context.Response.Write("Deposit error 1");
                return;
            }

            bool result = Convert.ToBoolean(xElement.Element("result").Value);
            invId = Convert.ToInt64(xElement.Element("invId").Value);

            if (!result)
            {
                commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "unable to create deposit", Convert.ToString(processSerialId), processId, false);
                context.Response.Write("Deposit error 2");
                return;
            }
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "-99", ex.ToString(), Convert.ToString(processSerialId), processId, true);
            context.Response.Write("Deposit exception");
            return;
        }

        commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "ok", string.Empty, Convert.ToString(processSerialId), processId, false);
        #endregion

        #region call vendor
        processSerialId++;
        processDetail = "call vendor";

        try
        {
            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings(System.Configuration.ConfigurationManager.AppSettings.Get("Operator"));
            string payment_encryption_key = System.Configuration.ConfigurationManager.AppSettings.Get("PaymentPrivateKey");
            string merchantAccount = commonEncryption.decrypting(opSettings.Values.Get("Help2Pay_merchantaccount"), payment_encryption_key);
            string merchantKey = commonEncryption.decrypting(opSettings.Values.Get("Help2Pay_key"), payment_encryption_key);

            var requestUrl = HttpContext.Current.Request.Url;
            string frontReturnUrl = requestUrl.Scheme + "://" + requestUrl.Host + "/Index";
            
            string serverReturnUrl = opSettings.Values.Get("Help2Pay_serverreturnurl");
            string postUrl = opSettings.Values.Get("Help2Pay_posturl");
            string username = strMemberId;
            string md5 = string.Empty;
            string amount = requestAmount.ToString("#.00");
            string currency = strCurrencyCode == "RMB" ? "CNY" : strCurrencyCode;
            string note = string.Empty;
            string language = "en-us";
            string transferMethod = "auto";
            DateTime currentDate = DateTime.Now;

            if (currency.Equals("VND") || currency.Equals("IDR"))
            {
                amount = (requestAmount * 1000m).ToString("#.00");
            }

            System.Text.StringBuilder sbHashString = new System.Text.StringBuilder();
            sbHashString.Append(merchantAccount);
            sbHashString.Append(invId);
            sbHashString.Append(strMemberId);
            sbHashString.Append(amount);
            sbHashString.Append(currency);
            sbHashString.Append(currentDate.ToString("yyyyMMddHHmmss"));
            sbHashString.Append(merchantKey);

            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, sbHashString.ToString(), string.Empty, Convert.ToString(processSerialId), processId, false);

            md5 = SdPayUtility.getMd5Hash(sbHashString.ToString()).ToUpper();

            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, md5, string.Empty, Convert.ToString(processSerialId), processId, false);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<form id=""theForm"" name=""theForm"" target=""_self"" method=""post"" action='" + postUrl + "'>");
            sb.Append(@"<input type=""hidden"" id=""Merchant"" name=""Merchant"" value='" + merchantAccount + "'/>");
            sb.Append(@"<input type=""hidden"" id=""Currency"" name=""Currency"" value='" + currency + "'/>");
            sb.Append(@"<input type=""hidden"" id=""Customer"" name=""Customer"" value='" + strMemberId + "'/>");
            sb.Append(@"<input type=""hidden"" id=""Reference"" name=""Reference"" value='" + invId + "'/>");
            sb.Append(@"<input type=""hidden"" id=""Key"" name=""Key"" value='" + md5 + "'/>");
            sb.Append(@"<input type=""hidden"" id=""Amount"" name=""Amount"" value='" + amount + "'/>");
            sb.Append(@"<input type=""hidden"" id=""Note"" name=""Note"" value='" + note + "'/>");
            sb.Append(@"<input type=""hidden"" id=""Datetime"" name=""Datetime"" value='" + currentDate.ToString("yyyy-MM-dd hh:mm:sstt") + "'/>");
            sb.Append(@"<input type=""hidden"" id=""FrontURI"" name=""FrontURI"" value='" + frontReturnUrl + "'/>");
            sb.Append(@"<input type=""hidden"" id=""BackURI"" name=""BackURI"" value='" + serverReturnUrl + "'/>");
            sb.Append(@"<input type=""hidden"" id=""Language"" name=""Language"" value='" + language + "'/>");
            sb.Append(@"<input type=""hidden"" id=""Bank"" name=""Bank"" value='" + bankCode + "'/>");
            sb.Append(@"<input type=""hidden"" id=""TransferMethod"" name=""TransferMethod"" value='" + transferMethod + "'/>");
            sb.Append(@"</form>");

            context.Response.Write(sb.ToString());

            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "ok", sb.ToString(), Convert.ToString(processSerialId), processId, false);

            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(@"<script type='text/javascript'>");
            sb1.Append(@"var ctlForm = document.forms.namedItem('theForm');");
            sb1.Append(@"ctlForm.submit();");
            sb1.Append(@"</script>");

            context.Response.Write(sb1.ToString());
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "-99", ex.ToString(), Convert.ToString(processSerialId), processId, true);
            context.Response.Write("Post exception");
            return;
        }
        #endregion
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}