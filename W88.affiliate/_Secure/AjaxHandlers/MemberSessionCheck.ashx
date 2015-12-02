<%@ WebHandler Language="C#" Class="_Secure_AjaxHandlers_MemberSessionCheck" %>

using System;
using System.Web;

public class _Secure_AjaxHandlers_MemberSessionCheck : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
{    
    public void ProcessRequest (HttpContext context) 
    {
        System.Xml.Linq.XElement xeErrors = commonVariables.ErrorsXML;

        #region initialiseVariables
        int intProcessSerialId = 0;
        string strProcessId = Guid.NewGuid().ToString().ToUpper();
        string strPageName = "MemberSessionCheck";

        string strResultCode = string.Empty;
        string strResultDetail = string.Empty;
        string strErrorCode = string.Empty;
        string strErrorDetail = string.Empty;
        string strProcessRemark = string.Empty;
        bool isProcessAbort = false;
        bool isSystemError = false;

        //string strLanguage = string.Empty;
        string strLoginIp = string.Empty;
        string strSessionId = string.Empty;
        string strProcessCode = string.Empty;
        string strProcessMessage = string.Empty;

        #endregion

        #region populateVariables
        strSessionId = commonVariables.CurrentMemberSessionId;
        strLoginIp = commonIp.UserIP;

        if (string.IsNullOrEmpty(strSessionId))
        {
            isProcessAbort = true;
            //strLanguage = commonVariables.SelectedLanguage;
            commonVariables.ClearSessionVariables();
            commonCookie.ClearCookies();

            strProcessRemark = string.Format("strSessionId:{0} | isProcessAbort:{1}", strSessionId, isProcessAbort);
            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", "MemberSessionCheck", "EmptyStrSessionId", "DataBaseManager.DLL", "", "", "", "", strProcessRemark, Convert.ToString(intProcessSerialId), "", true);

        }

        strProcessRemark = string.Format("strSessionId:{0} | isProcessAbort:{1}", strSessionId, isProcessAbort);
        intProcessSerialId += 1;
        //commonAuditTrail.appendLog("system", "MemberSessionCheck", "ProcessRequest", "DataBaseManager.DLL", "", "", "", "", strProcessRemark, Convert.ToString(intProcessSerialId), "", true);
        
        #endregion

        #region initiateSessionCheck
        if (string.Compare(commonVariables.GetSessionVariable("LoginStatus"), "success", true) != 0)
        {
            strProcessCode = "-1";
            isProcessAbort = true;
        }
        else if (!isProcessAbort)
        {
            try
            {
                using (wsAffiliateMS1.affiliateWSSoapClient svcInstance = new wsAffiliateMS1.affiliateWSSoapClient())
                {
                    System.Data.DataSet dsSignin = null;
                    dsSignin = svcInstance.MemberSessionCheck(strSessionId, strLoginIp);

                    if (dsSignin.Tables[0].Rows.Count > 0)
                    {
                        strProcessCode = Convert.ToString(dsSignin.Tables[0].Rows[0]["RETURN_VALUE"]);
                        switch (strProcessCode)
                        {
                            case "0":
                                strProcessMessage = "Exception";
                                break;
                            case "1":
                                break;
                            case "10":
                                strProcessMessage = "NotLogin";
                                commonVariables.ClearSessionVariables();
                                commonCookie.ClearCookies();
                                break;
                            case "21":
                                strProcessMessage = "InvalidUsername";
                                commonVariables.ClearSessionVariables();
                                commonCookie.ClearCookies();
                                break;
                            case "22":
                                strProcessMessage = "InactiveAccount";
                                commonVariables.ClearSessionVariables();
                                commonCookie.ClearCookies();
                                break;
                            case "23":
                                strProcessMessage = "InvalidPassword";
                                commonVariables.ClearSessionVariables();
                                commonCookie.ClearCookies();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                strProcessCode = "0";
                strProcessMessage = ex.Message;
                commonVariables.ClearSessionVariables();
                commonCookie.ClearCookies();
            }
        }
        
        strProcessRemark = string.Format("SessionId: {0} | IPAddress: {1} | ProcessCode: {2} | ProcessMessage: {3}", strSessionId, strLoginIp, strProcessCode, strProcessMessage);

        intProcessSerialId += 1;
        //commonAuditTrail.appendLog("system", strPageName, "InitiateMemberSessionCheck", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        #endregion

        context.Response.Write(strProcessCode);
        context.Response.End();
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}