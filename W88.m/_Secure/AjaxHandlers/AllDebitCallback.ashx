<%@ WebHandler Language="C#" Class="AllDebitCallback" %>

using System;
using System.Web;
using System.Xml.Linq;
using System.Configuration;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using System.Web.SessionState;

public class AllDebitCallback : IHttpHandler, IRequiresSessionState
{
    private XDocument xDocumentResources = null;

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        #region variables
        int processSerialId = 0;
        string processId = Guid.NewGuid().ToString().ToUpper();
        string pageName = "AllDebitCallback";
        string taskName = "ProcessRequest";
        string processDetail = string.Empty;

        if (context.Request.HttpMethod != "POST")
        {
            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "invalid http method", Convert.ToString(processSerialId), processId, false);
            return;
        }

        string ip = context.Request.ServerVariables["REMOTE_ADDR"];
        string merNo = context.Request.Params["merNo"];
        string gatewayNo = context.Request.Params["gatewayNo"];
        string tradeNo = context.Request.Params["tradeNo"];
        string orderNo = context.Request.Params["orderNo"];
        string orderCurrency = context.Request.Params["orderCurrency"];
        string orderAmount = context.Request.Params["orderAmount"];
        string orderStatus = context.Request.Params["orderStatus"];
        string orderInfo = context.Request.Params["orderInfo"];
        string signInfo = context.Request.Params["signInfo"];
        string remark = context.Request.Params["remark"];

        string parameters = ip + "|" + merNo + "|" + gatewayNo + "|" + tradeNo + "|" + orderNo + "|" + orderCurrency + "|" + orderAmount + "|" + orderStatus + "|" +
            orderInfo + "|" + signInfo + "|" + remark;

        #endregion

        #region get gateway settings from xml
        processSerialId++;
        processDetail = "get gateway settings from xml";

        string accountInfo;
        string defaultMerchantId;

        string signKey = string.Empty;

        if (gatewayNo.Equals("20751003"))
        {
            signKey = commonEncryption.decrypting(ConfigurationManager.AppSettings["AllDebit_Visa"]);
        }
        else if (gatewayNo.Equals("20751004"))
        {
            signKey = commonEncryption.decrypting(ConfigurationManager.AppSettings["AllDebit_Master"]);
        }

        commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "ok", "signKey:" + signKey, Convert.ToString(processSerialId), processId, false);
        #endregion

        #region response validation
        processSerialId++;
        processDetail = "response validation";

        var signBuilder = new StringBuilder();
        signBuilder.AppendFormat("{0}{1}{2}{3}{4}{5}{6}{7}{8}", merNo, gatewayNo, tradeNo, orderNo, orderCurrency, orderAmount, orderStatus, orderInfo, signKey);

        commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "checkpoint", signBuilder.ToString(), Convert.ToString(processSerialId), processId, false);

        string generatedSign = commonEncryption.GetSHA256Hash(signBuilder.ToString()).ToUpper();

        if (signInfo != generatedSign)
        {
            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "Source SHA256:" + signInfo + "|Generated SHA256:" + generatedSign, Convert.ToString(processSerialId), processId, false);
            context.Response.Write(commonCulture.ElementValues.getResourceXPathString("Deposit/TransferFail", commonVariables.ErrorsXML));
            return;
        }

        commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "ok", string.Empty, Convert.ToString(processSerialId), processId, false);
        #endregion

        #region call external server
        processSerialId++;
        processDetail = "call external server";
        byte[] responseBytes;

        try
        {
            string postUrl = ConfigurationManager.AppSettings["AllDebit_callbackURL"];

            var postParams = new NameValueCollection()
                {
                    {"merNo", merNo},
                    {"gatewayNo", gatewayNo},
                    {"tradeNo", tradeNo},
                    {"orderNo", orderNo},
                    {"orderCurrency", orderCurrency},
                    {"orderAmount", orderAmount},
                    {"orderStatus", orderStatus},
                    {"orderInfo", orderInfo},
                    {"signInfo", signInfo},
                    {"remark", remark},
                };

            using (var client = new WebClient())
            {
                responseBytes = client.UploadValues(postUrl, postParams);
            }

            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "ok", string.Empty, Convert.ToString(processSerialId), processId, false);
        }
        catch (Exception ex)
        {
            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "-99", "Message: " + ex.Message + "|Stacktrace: " + ex.StackTrace, Convert.ToString(processSerialId), processId, true);
            context.Response.Write(commonCulture.ElementValues.getResourceXPathString("Deposit/Exception", commonVariables.ErrorsXML));
            return;
        }
        #endregion

        #region check response
        processSerialId++;
        processDetail = "check response";

        if (responseBytes.Length == 0)
        {
            commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "error", "response is empty", Convert.ToString(processSerialId), processId, false);
            context.Response.Write(commonCulture.ElementValues.getResourceXPathString("Deposit/TransferFail", commonVariables.ErrorsXML));
            return;
        }

        string responseStr = Encoding.UTF8.GetString(responseBytes);
        commonAuditTrail.appendLog("system", pageName, taskName, string.Empty, string.Empty, processDetail, string.Empty, "ok", "AllDebit Response: " + responseStr, Convert.ToString(processSerialId), processId, false);

        if (responseStr != "OK")
        {
            context.Response.Write(commonCulture.ElementValues.getResourceXPathString("Deposit/TransferFail", commonVariables.ErrorsXML));
            return;
        }
        else
        {
            if (orderStatus == "1")
                context.Response.Write(commonCulture.ElementValues.getResourceXPathString("Deposit/TransferSuccess", commonVariables.ErrorsXML));
            else
                context.Response.Write(commonCulture.ElementValues.getResourceXPathString("Deposit/Exception", commonVariables.ErrorsXML));
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