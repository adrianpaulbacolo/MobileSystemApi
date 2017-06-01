using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_ProcessLoginBySessionId : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        System.Xml.Linq.XElement xeErrors = commonVariables.ErrorsXML;

        #region initialiseVariables
        int intProcessSerialId = 0;
        string strProcessId = Guid.NewGuid().ToString().ToUpper();
        string strPageName = "ProcessLoginBySessionId";

        string strResultCode = string.Empty;
        string strResultDetail = string.Empty;
        string strErrorCode = string.Empty;
        string strErrorDetail = string.Empty;
        string strProcessRemark = string.Empty;
        bool isProcessAbort = false;
        bool isSystemError = false;

        //string strLanguage = string.Empty;
        string strLoginIp = string.Empty;
        string strCountryCode = string.Empty;
        string strSessionId = string.Empty;
        string strProcessCode = string.Empty;
        string strProcessMessage = string.Empty;

        #endregion

        #region populateVariables
        strSessionId = commonVariables.CurrentMemberSessionId;
        strLoginIp = HttpContext.Current.Request.Form.Get("ip");
        strCountryCode = HttpContext.Current.Request.Form.Get("country");

        if (string.IsNullOrEmpty(strSessionId))
        {
            isProcessAbort = true;
            commonVariables.ClearSessionVariables();
            commonCookie.ClearCookies();
        }

        #endregion

        #region initiateSessionCheck
        if (!isProcessAbort)
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
                                string strMemberSessionId = Convert.ToString(dsSignin.Tables[0].Rows[0]["memberSessionId"]);
                                HttpContext.Current.Session.Add("MemberSessionId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberSessionId"]));
                                HttpContext.Current.Session.Add("MemberId", Convert.ToString(dsSignin.Tables[0].Rows[0]["affiliateID"]));
                                HttpContext.Current.Session.Add("MemberCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberCode"]));
                                HttpContext.Current.Session.Add("CountryCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["countryCode"]));
                                HttpContext.Current.Session.Add("CurrencyCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["currencyCode"]));
                                HttpContext.Current.Session.Add("LanguageCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["languageCode"]));
                                //HttpContext.Current.Session.Add("RiskId", Convert.ToString(dsSignin.Tables[0].Rows[0]["riskId"]));
                                //HttpContext.Current.Session.Add("PartialSignup", Convert.ToString(dsSignin.Tables[0].Rows[0]["partialSignup"]));
                                HttpContext.Current.Session.Add("ResetPassword", Convert.ToString(dsSignin.Tables[0].Rows[0]["resetPassword"]));

                                commonCookie.CookieS = strMemberSessionId;
                                commonCookie.CookieG = strMemberSessionId;
                                HttpContext.Current.Session.Add("LoginStatus", "success");
                                break;
                            case "10":
                                strProcessMessage = "NotLogin";
                                commonVariables.ClearSessionVariables();
                                commonCookie.ClearCookies();
                                break;
                            case "21":
                                strProcessMessage = "InvalidUsername";
                                break;
                            case "22":
                                strProcessMessage = "InactiveAccount";
                                break;
                            case "23":
                                strProcessMessage = "InvalidPassword";
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

        strProcessRemark = string.Format("SessionId: {0} | IPAddress: {1} | CountryCode: {2} | ProcessCode: {3} | ProcessMessage: {4}", strSessionId, strLoginIp, strCountryCode, strProcessCode, strProcessMessage);

        intProcessSerialId += 1;
        commonAuditTrail.appendLog("system", strPageName, "InitiateProcessLogin", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        #endregion

        if (string.Compare(strProcessCode, "1", true) == 0) { Response.Write(commonVariables.SelectedLanguage); }
        else { Response.Write("0"); }
        Response.End();
    }
}