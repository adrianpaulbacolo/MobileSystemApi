<%@ WebHandler Language="C#" Class="Login" %>

using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Models;

public class Login : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {

        if (!context.Request.ContentType.Contains("json")) return;

        var queryString = HttpUtility.ParseQueryString(context.Request.UrlReferrer.Query);
        commonCookie.CookieSubPlatform(queryString.Get("spfid"));

        var sJson = new StreamReader(context.Request.InputStream).ReadToEnd();
        var loginInfo = commonFunctions.Deserialize<LoginInfo>(sJson);

        if (loginInfo == null) return;

        var processId = Guid.NewGuid().ToString().ToUpper();
        var loginHelper = new LoginProcess(loginInfo);
        var process = loginHelper.ValidateData();
        
        loginInfo.Password = commonEncryption.Encrypt(loginInfo.Password);
        
        var processRemark = string.Format("MemberCode: {0} | Password: {1} | VCode: {2} | SVCode: {3} | IP: {4} ",
            loginInfo.Username, loginInfo.Password, loginInfo.Captcha, loginInfo.SessionCaptcha, commonIp.UserIP);
        
        int processSerialId = 1;
        commonAuditTrail.appendLog("system", loginInfo.PageName, "ParameterValidation", "DataBaseManager.DLL", string.Empty,
            string.Empty, string.Empty, string.Empty, processRemark, Convert.ToString(processSerialId), processId, false);

        if (!process.IsAbort)
        {
            using (var svcInstance = new wsMemberMS1.memberWSSoapClient())
            {
                try
                {
                    var dsSignin = svcInstance.MemberSignin(loginInfo.OperatorId, loginInfo.Username, loginInfo.Password, loginInfo.SiteUrl, commonIp.UserIP, loginInfo.DeviceId);
                    
                    if (dsSignin.Tables[0].Rows.Count > 0)
                    {
                        process.Code = Convert.ToString(dsSignin.Tables[0].Rows[0]["RETURN_VALUE"]);
                        loginHelper.CheckResult(ref process, dsSignin.Tables[0], ref processSerialId, processId);
                    }
                }
                catch (Exception ex)
                {
                    process.Code = "0";
                    process.Message = commonCulture.ElementValues.getResourceString("Exception", loginInfo.XeErrors);
                    processRemark = string.Format("{0} | Message: {1}", processRemark, ex.Message);
                }

                processRemark = string.Format("{0} | strProcessCode: {1}", processRemark, process.Code);

                processSerialId += 1;
                commonAuditTrail.appendLog("system", loginInfo.PageName, "MemberSignin", "DataBaseManager.DLL", string.Empty,
                    string.Empty, string.Empty, string.Empty, processRemark, Convert.ToString(processSerialId),
                    processId, false);
            }
        }

        var serializer = new JavaScriptSerializer();
        var json = serializer.Serialize(process);
        context.Response.ContentType = "text/json";
        context.Response.Write(json);
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

}