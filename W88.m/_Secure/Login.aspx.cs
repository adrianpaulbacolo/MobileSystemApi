using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_Login : System.Web.UI.Page
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected string strRedirect = string.Empty;

    protected void Page_Init(object sender, EventArgs e)
    {
        string strLanguage = string.Empty;

        strLanguage = Request.QueryString.Get("lang");

        commonVariables.SelectedLanguage = string.IsNullOrEmpty(strLanguage) ? (string.IsNullOrEmpty(commonVariables.SelectedLanguage) ? "en-us" : commonVariables.SelectedLanguage) : strLanguage;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        xeErrors = commonVariables.ErrorsXML;
        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getLocalResource(out xeResources);

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

        if (!string.IsNullOrEmpty(Request.QueryString.Get("token")))
        {
            
            try
            {
                var cipherKey = commonEncryption.Decrypt(ConfigurationManager.AppSettings.Get("PrivateKeyToken"));
                strSessionId = decryptToken(Request.QueryString.Get("token"), cipherKey);
                

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
                commonAuditTrail.appendLog("system exception: ", strPageName, "CheckLoginToken", "", strResultCode, strProcessMessage, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
                commonVariables.ClearSessionVariables();
                commonCookie.ClearCookies();
            }

            Response.Redirect("/Deposit/Default_app.aspx");
        }

        if (string.IsNullOrEmpty(Request.QueryString.Get("redirect"))) { strRedirect = "/Index.aspx?lang=" + commonVariables.SelectedLanguage; }
        else { strRedirect = Request.QueryString.Get("redirect"); }

        if (!Page.IsPostBack)
        {
            lblUsername.Text = commonCulture.ElementValues.getResourceString("lblUsername", xeResources);
            lblPassword.Text = commonCulture.ElementValues.getResourceString("lblPassword", xeResources);
            lblCaptcha.Text = commonCulture.ElementValues.getResourceString("lblCaptcha", xeResources);
            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnLogin", xeResources);

            txtUsername.Focus();

            lblRegister.Text = commonCulture.ElementValues.getResourceString("btnRegister", xeResources);
        }
    }

    public static string decryptToken(string cipherString,string cipherKey)
    {
        byte[] keyArray;
        //get the byte code of the string

        byte[] toEncryptArray = Convert.FromBase64String(cipherString);

        System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();

        //if hashing was not implemented get the byte code of the key
        keyArray = UTF8Encoding.UTF8.GetBytes(cipherKey);

        TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
        //set the secret key for the tripleDES algorithm
        tdes.Key = keyArray;
        //mode of operation. there are other 4 modes.
        //We choose ECB(Electronic code Book)

        tdes.Mode = CipherMode.ECB;
        //padding mode(if any extra byte added)
        tdes.Padding = PaddingMode.PKCS7;

        ICryptoTransform cTransform = tdes.CreateDecryptor();
        byte[] resultArray = cTransform.TransformFinalBlock
                (toEncryptArray, 0, toEncryptArray.Length);
        //Release resources held by TripleDes Encryptor
        tdes.Clear();
        //return the Clear decrypted TEXT
        return UTF8Encoding.UTF8.GetString(resultArray);
    }
}
