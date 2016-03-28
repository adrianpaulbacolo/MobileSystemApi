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
        string strSessionId = string.Empty;
        string strProcessCode = string.Empty;
        string strProcessMessage = string.Empty;

        #endregion

        #region populateVariables
        strSessionId = commonVariables.CurrentMemberSessionId;

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
                using (wsMemberMS1.memberWSSoapClient svcInstance = new wsMemberMS1.memberWSSoapClient())
                {
                    System.Data.DataSet dsSignin = null;
                    dsSignin = svcInstance.MemberSessionCheck(strSessionId, commonIp.UserIP);

                    if (dsSignin.Tables[0].Rows.Count > 0)
                    {
                        strProcessCode = Convert.ToString(dsSignin.Tables[0].Rows[0]["RETURN_VALUE"]);
                        switch (strProcessCode)
                        {
                            case "0":
                                strProcessMessage = commonCulture.ElementValues.getResourceString("Exception", xeErrors);
                                break;
                            case "1":
                                string strMemberSessionId = Convert.ToString(dsSignin.Tables[0].Rows[0]["memberSessionId"]);
                                HttpContext.Current.Session.Add("MemberSessionId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberSessionId"]));
                                HttpContext.Current.Session.Add("MemberId", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberId"]));
                                HttpContext.Current.Session.Add("MemberCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["memberCode"]));
                                HttpContext.Current.Session.Add("MemberName", Convert.ToString(dsSignin.Tables[0].Rows[0]["lastName"]) + Convert.ToString(dsSignin.Tables[0].Rows[0]["firstName"]));
                                HttpContext.Current.Session.Add("CountryCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["countryCode"]));
                                HttpContext.Current.Session.Add("CurrencyCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["currencyCode"]));
                                HttpContext.Current.Session.Add("LanguageCode", Convert.ToString(dsSignin.Tables[0].Rows[0]["languageCode"]));
                                HttpContext.Current.Session.Add("RiskId", Convert.ToString(dsSignin.Tables[0].Rows[0]["riskId"]));
                                //HttpContext.Current.Session.Add("PaymentGroup", "A"); //Convert.ToString(dsSignin.Tables[0].Rows[0]["paymentGroup"]));
                                HttpContext.Current.Session.Add("PartialSignup", Convert.ToString(dsSignin.Tables[0].Rows[0]["partialSignup"]));
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
                            case "13":
                                commonVariables.ClearSessionVariables();
                                commonCookie.ClearCookies();
                                break;
                            case "21":
                                strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/InvalidUsername", xeErrors);
                                break;
                            case "22":
                                strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/InactiveAccount", xeErrors);
                                break;
                            case "23":
                                strProcessMessage = commonCulture.ElementValues.getResourceXPathString("Login/InvalidPassword", xeErrors);
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

        strProcessRemark = string.Format("SessionId: {0} | IPAddress: {1} | ProcessCode: {2} | ProcessMessage: {3}", strSessionId, commonIp.UserIP, strProcessCode, strProcessMessage);

        intProcessSerialId += 1;
        commonAuditTrail.appendLog("system", strPageName, "InitiateProcessLogin", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        #endregion

        if (string.Compare(strProcessCode, "1", true) == 0) { Response.Write(commonVariables.SelectedLanguage); }
        else { Response.Write("0"); }
        Response.End();
    }
}