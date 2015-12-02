<%@ WebHandler Language="C#" Class="SDPay" %>

using System;
using System.Web;

public class SDPay : IHttpHandler,System.Web.SessionState.IReadOnlySessionState {
    
    public void ProcessRequest (HttpContext context) 
    {
        //if (string.IsNullOrEmpty((string)HttpContext.Current.Session["user_MemberID"]))
        //    context.Response.Redirect((string)HttpContext.Current.Session["domain_Account"] + "/BankingOptions.aspx#logout");

        #region variables
        int processSerialId = 0;
        string processId = Guid.NewGuid().ToString().ToUpper();
        string pageName = "Pay120223Handler";
        string taskName = "ProcessRequest";
        string processDetail = string.Empty;
        decimal requestAmount = 0m;
        long invId = 0;

        //string memberId = (string)HttpContext.Current.Session["user_MemberID"];
        //string memberCode = (string)HttpContext.Current.Session["user_MemberCode"];
        //string memberCurrency = (string)HttpContext.Current.Session["user_Currency"];
        string strAmount = context.Request.Params["requestAmount"];

        string strOperatorId = commonVariables.OperatorId;
        string strMemberId = commonVariables.GetSessionVariable("MemberId");
        string strMemberCode = commonVariables.GetSessionVariable("MemberCode");
        string strCurrencyCode = commonVariables.GetSessionVariable("CurrencyCode");
        
        string parameters = strMemberCode + "|" + strAmount;
        #endregion

        #region log parameters
        processSerialId++;
        processDetail = "log parameters";

        try
        {
            
            string path = @"C:\temp\log.txt";

            if (!System.IO.File.Exists(path))
            {
                using (System.IO.StreamWriter sw = System.IO.File.CreateText(path))
                {
                    var log = string.Format("Operator: {0} Member: {1} Code:{2} Currency: {3}", strOperatorId, strMemberId, strMemberCode, strCurrencyCode);
                    sw.Write(log);
                    sw.Close();
                }
            }

            using (System.IO.StreamWriter sw = System.IO.File.AppendText(path))
            {
                var log = string.Format("Operator: {0} Member: {1} Code: {2} Currency: {3}", strOperatorId, strMemberId, strMemberCode, strCurrencyCode);
                sw.WriteLine(log);
                sw.Close();
            }

        }
        catch(Exception)
        { }
        
        commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "ok", parameters, Convert.ToString(processSerialId), processId, false);
        #endregion

        #region parameter validation
        processSerialId++;
        processDetail = "parameter validation";

        /*
        if (!commonValidation.IsValidString(strAmount, 16))
        {
            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "invalid strAmount", Convert.ToString(processSerialId), processId, false);
            context.Response.Write("参数错误");
            return;
        }

        if (!commonValidation.IsValidAmount(strAmount))
        {
            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "invalid requestAmount", Convert.ToString(processSerialId), processId, false);
            context.Response.Write("参数错误");
            return;
        }
        */
        commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "ok", string.Empty, Convert.ToString(processSerialId), processId, false);
        #endregion

        #region check payment setting
        processSerialId++;
        processDetail = "check payment setting";

        try
        {
            System.Data.DataTable dt;
            string statusCode, statusText;

            requestAmount = Convert.ToDecimal(strAmount);

            using (svcPayMember.MemberClient client = new svcPayMember.MemberClient())
            {
                dt = client.getMethodLimits(strOperatorId, strMemberCode, Convert.ToInt32(commonVariables.DepositMethod.SDPay).ToString(), "1", false, out statusCode, out statusText);
                client.Close();
            }

            if (dt == null)
            {
                commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "payment setting null", Convert.ToString(processSerialId), processId, false);
                context.Response.Write("限额错误");
                return;
            }

            if (dt.Rows.Count != 1)
            {
                commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "payment setting row count error", Convert.ToString(processSerialId), processId, false);
                context.Response.Write("限额错误");
                return;
            }

            System.Data.DataRow dr = dt.Rows[0];

            if (Convert.ToDecimal(dr["minDeposit"]) > requestAmount)
            {
                commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "Amount is less than min limit", Convert.ToString(processSerialId), processId, false);
                context.Response.Write("金额小于最低限额");
                return;
            }

            if (Convert.ToDecimal(dr["maxDeposit"]) < requestAmount)
            {
                commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "Amount is more than max limit", Convert.ToString(processSerialId), processId, false);
                context.Response.Write("金额大于最高限额");
                return;
            }

            if (Convert.ToDecimal(dr["totalAllowed"]) < requestAmount)
            {
                commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "Amount is more than total allowed", Convert.ToString(processSerialId), processId, false);
                context.Response.Write("金额大于总允许额度");
                return;
            }
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "-99", ex.ToString(), Convert.ToString(processSerialId), processId, true);
            context.Response.Write("出现错误");
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
                xElement = client.createOnlineDepositTransaction(Convert.ToInt64(strOperatorId), strMemberCode, Convert.ToInt64(commonVariables.DepositMethod.SDPay), strCurrencyCode, requestAmount, string.Empty);
                client.Close();
            }

            if (xElement == null)
            {
                commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "deposit result is null", Convert.ToString(processSerialId), processId, false);
                context.Response.Write("无法进行存款");
                return;
            }

            bool result = Convert.ToBoolean(xElement.Element("result").Value);
            invId = Convert.ToInt64(xElement.Element("invId").Value);

            if (!result)
            {
                commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "unable to create deposit", Convert.ToString(processSerialId), processId, false);
                context.Response.Write("无法进行存款");
                return;
            }
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "-99", ex.ToString(), Convert.ToString(processSerialId), processId, true);
            context.Response.Write("出现错误");
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
            //return opSettings.Values.Get("OperatorId");
            string payment_encryption_key = System.Configuration.ConfigurationManager.AppSettings.Get("PaymentPrivateKey");            
            string merchantAccount = commonEncryption.decrypting(opSettings.Values.Get("SDPay_MerchantAccount"), payment_encryption_key);
            string merchantKey1 = commonEncryption.decrypting(opSettings.Values.Get("SDPay_Key1"), payment_encryption_key);
            string merchantKey2 = commonEncryption.decrypting(opSettings.Values.Get("SDPay_Key2"), payment_encryption_key);
            string md5Key = commonEncryption.decrypting(opSettings.Values.Get("SDPay_MD5"), payment_encryption_key);
            string serverReturnUrl = opSettings.Values.Get("SDPay_ServerReturnUrl");
            string postUrl = opSettings.Values.Get("SDPay_PostUrl");
            string username = strMemberId;
            string md5, xml, d, des;

            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "MerchantKey1:" + merchantKey1 + "MerchantKey2:" + merchantKey2, string.Empty, Convert.ToString(processSerialId), processId, false);
            
            System.Text.StringBuilder sbXml = new System.Text.StringBuilder();
            sbXml.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sbXml.Append("<message>");
            sbXml.Append("<cmd>6009</cmd>");
            sbXml.AppendFormat("<merchantid>{0}</merchantid>", merchantAccount);
            sbXml.Append("<language>zh-cn</language>");
            sbXml.Append("<userinfo>");
            sbXml.AppendFormat("<order>{0}</order>", invId.ToString());
            sbXml.AppendFormat("<username>{0}</username>", username);
            sbXml.AppendFormat("<money>{0}</money>", requestAmount.ToString());
            sbXml.Append("<unit>1</unit>");
            sbXml.AppendFormat("<time>{0}</time>", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            sbXml.Append("<remark></remark>");
            sbXml.AppendFormat("<backurl>{0}</backurl>", serverReturnUrl);
            sbXml.Append("</userinfo>");
            sbXml.Append("</message>");

            xml = sbXml.ToString();
            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, xml, string.Empty, Convert.ToString(processSerialId), processId, false);

            md5 = SdPayUtility.getMd5Hash(xml + md5Key);
            d = xml + md5;
            des = SdPayUtility.EncryptData(d, merchantKey1, merchantKey2);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(@"<form id=""theForm"" name=""theForm"" target=""_self"" method=""post"" action='" + postUrl + "'>");
            sb.Append(@"<input type=""hidden"" id=""cmd"" name=""cmd"" value=""6009""/>");
            sb.Append(@"<input type=""hidden"" id=""pid"" name=""pid"" value='" + merchantAccount + "'/>");
            sb.Append(@"<input type=""hidden"" id=""des"" name=""des"" value='" + des + "'/>");
            sb.Append(@"</form>");

            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, sb.ToString(), string.Empty, Convert.ToString(processSerialId), processId, false);
            
            context.Response.Write(sb.ToString());

            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "ok", string.Empty, Convert.ToString(processSerialId), processId, false);

            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            sb1.Append(@"<script type='text/javascript'>");
            sb1.Append(@"var ctlForm = document.forms.namedItem('theForm');");
            sb1.Append(@"ctlForm.submit();");
            sb1.Append(@"</script>");

            context.Response.ContentType = "text/html";
            context.Response.Write(sb1.ToString());
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "-99", ex.ToString(), Convert.ToString(processSerialId), processId, true);
            context.Response.Write("出现错误");
            return;
        }
        #endregion
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}