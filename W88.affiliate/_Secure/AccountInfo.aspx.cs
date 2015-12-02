using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_AccountInfo : System.Web.UI.Page
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected System.Xml.Linq.XElement xeResources = null;
    protected System.Xml.Linq.XElement xeResourcesSecQues = null;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e) { 
        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) 
        { 
            Response.Redirect("../Index",true); 
        }  
    }
    //else { Response.Redirect(Request.RawUrl); }

    protected void Page_Load(object sender, EventArgs e)
    {
        string strOperatorId = commonVariables.OperatorId;
        string strAffiliateId = string.Empty;
        xeErrors = commonVariables.ErrorsXML;
        //System.Xml.Linq.XElement xeResources = null;
        //commonCulture.appData.getLocalResource(out xeResources);

        //xeErrors = commonVariables.ErrorsXML;
        commonCulture.appData.getRootResource("/AccountInfo.aspx", out xeResources);
        commonCulture.appData.getRootResource("/security_question.aspx", out xeResourcesSecQues);

        customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");

        //testing
        //System.Web.HttpContext.Current.Session["AffiliateId"] = "20264";

        if (!Page.IsPostBack)
        {
            using (wsAffiliateMS1.affiliateWSSoapClient wsInstanceAff = new wsAffiliateMS1.affiliateWSSoapClient("affiliateWSSoap"))
            {

                DataSet dsAffMember = wsInstanceAff.GetAffiliateMemberInfoByID(long.Parse(System.Web.HttpContext.Current.Session["AffiliateId"].ToString()));
                if (dsAffMember.Tables.Count > 0)
                {
                    if (dsAffMember.Tables[0].Rows.Count > 0)
                    {

                        //if (string.IsNullOrEmpty(commonVariables.GetSessionVariable("AffiliateId"))) { if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString.Get("AffiliateId"))) { commonVariables.SetSessionVariable("AffiliateId", HttpContext.Current.Request.QueryString.Get("AffiliateId")); } }
                        //strAffiliateId = string.IsNullOrEmpty(commonVariables.GetSessionVariable("AffiliateId")) ? string.Empty : Convert.ToString(commonVariables.GetSessionVariable("AffiliateId"));
            
                        lblFullName.Text = commonCulture.ElementValues.getResourceString("lblFullName", xeResources);
                        lblMemberFullName.Text = dsAffMember.Tables[0].Rows[0]["firstname"].ToString();

                        lblUsername.Text = commonCulture.ElementValues.getResourceString("lblUsername", xeResources);
                        lblMemberUsername.Text = dsAffMember.Tables[0].Rows[0]["affiliateUser"].ToString();

                        lblEmail.Text = commonCulture.ElementValues.getResourceString("lblEmailAddress", xeResources);
                        lblMemberEmail.Text = dsAffMember.Tables[0].Rows[0]["email"].ToString();

                        lblCurrency.Text = commonCulture.ElementValues.getResourceString("lblCurrency", xeResources);
                        lblMemberCurrency.Text = dsAffMember.Tables[0].Rows[0]["currency"].ToString();

                        lblDOB.Text = commonCulture.ElementValues.getResourceString("lblDOB", xeResources);

                        drpDay.SelectedValue = ((DateTime)dsAffMember.Tables[0].Rows[0]["dob"]).Year.ToString();
                        drpMonth.SelectedValue = ((DateTime)dsAffMember.Tables[0].Rows[0]["dob"]).Month.ToString();
                        drpYear.SelectedValue = ((DateTime)dsAffMember.Tables[0].Rows[0]["dob"]).Day.ToString();

                        lblContact.Text = commonCulture.ElementValues.getResourceString("lblContact", xeResources);
                        //txtContact.Attributes.Add("PLACEHOLDER", lblContact.Text);
                        //txtContact.Attributes.Add("PLACEHOLDER", lblContact.Text);
                        
                        string mobilno = dsAffMember.Tables[0].Rows[0]["mobileNo"].ToString();
                        if (mobilno.Contains("-"))
                        {
                            string[] mobilenosplit = mobilno.Split('-');
                            drpContactCountry.SelectedValue = mobilenosplit[0];
                            txtContact.Text = mobilenosplit[1];
                        }
                        else
                        {
                            drpContactCountry.SelectedValue = "-1";
                            txtContact.Text = mobilno;
                        }

                        lblCountry.Text = commonCulture.ElementValues.getResourceString("lblCountry", xeResources);
                        drpCountry.SelectedValue = dsAffMember.Tables[0].Rows[0]["countryCode"].ToString();

                        lblAccount.Text = commonCulture.ElementValues.getResourceString("lblAccount", xeResources);
                        txtAccount.Text = dsAffMember.Tables[0].Rows[0]["contactMessenger"].ToString();
                   
                        lblAddress.Text = commonCulture.ElementValues.getResourceString("lblAddress", xeResources);
                        txtAddress.Text = dsAffMember.Tables[0].Rows[0]["address"].ToString();
                     
                        lblCity.Text = commonCulture.ElementValues.getResourceString("lblCity", xeResources);
                        txtCity.Text = dsAffMember.Tables[0].Rows[0]["city"].ToString();
                        txtPostal.Text = dsAffMember.Tables[0].Rows[0]["postal"].ToString();
                 
                        lblWebsiteUrl.Text = commonCulture.ElementValues.getResourceString("lblWebsiteUrl", xeResources);

                        System.Web.HttpContext.Current.Session["urlID1"] = "";
                        System.Web.HttpContext.Current.Session["url1"] = "";

                        System.Web.HttpContext.Current.Session["urlID2"] = "";
                        System.Web.HttpContext.Current.Session["url2"] = "";

                        System.Web.HttpContext.Current.Session["urlID3"] = "";
                        System.Web.HttpContext.Current.Session["url3"] = "";


                        DataSet dsAffMemberWebSite = wsInstanceAff.GetAffiliateMemberWebsite(long.Parse(System.Web.HttpContext.Current.Session["AffiliateId"].ToString()));
                      
                            //check by individual

                        if (dsAffMemberWebSite.Tables[0].Rows.Count > 0)
                        {
                           
                            if (!string.IsNullOrEmpty(dsAffMemberWebSite.Tables[0].Rows[0]["AffiliateMemberURLID"].ToString()))
                            {
                                System.Web.HttpContext.Current.Session["urlID1"] = dsAffMemberWebSite.Tables[0].Rows[0]["AffiliateMemberURLID"].ToString();
                                System.Web.HttpContext.Current.Session["url1"] = dsAffMemberWebSite.Tables[0].Rows[0]["affiliateURL"].ToString();
                                txtURL1.Text = dsAffMemberWebSite.Tables[0].Rows[0]["affiliateURL"].ToString();
                            }
                            else
                            {
                                lblURL1.Text = commonCulture.ElementValues.getResourceString("lblURL1", xeResources);
                                txtURL1.Attributes.Add("PLACEHOLDER", lblURL1.Text);
                            }

                            if (dsAffMemberWebSite.Tables[0].Rows.Count >= 2 && !string.IsNullOrEmpty(dsAffMemberWebSite.Tables[0].Rows[1]["AffiliateMemberURLID"].ToString()))
                            {
                                System.Web.HttpContext.Current.Session["urlID2"] = dsAffMemberWebSite.Tables[0].Rows[1]["AffiliateMemberURLID"].ToString();
                                System.Web.HttpContext.Current.Session["url2"] = dsAffMemberWebSite.Tables[0].Rows[1]["affiliateURL"].ToString();
                                txtURL2.Text = dsAffMemberWebSite.Tables[0].Rows[1]["affiliateURL"].ToString();
                            }
                            else
                            {
                                lblURL2.Text = commonCulture.ElementValues.getResourceString("lblURL2", xeResources);
                                txtURL2.Attributes.Add("PLACEHOLDER", lblURL2.Text);
                            }

                            if (dsAffMemberWebSite.Tables[0].Rows.Count >= 3 && !string.IsNullOrEmpty(dsAffMemberWebSite.Tables[0].Rows[2]["AffiliateMemberURLID"].ToString()))
                            {
                                System.Web.HttpContext.Current.Session["urlID3"] = dsAffMemberWebSite.Tables[0].Rows[2]["AffiliateMemberURLID"].ToString();
                                System.Web.HttpContext.Current.Session["url3"] = dsAffMemberWebSite.Tables[0].Rows[2]["affiliateURL"].ToString();
                                txtURL3.Text = dsAffMemberWebSite.Tables[0].Rows[2]["affiliateURL"].ToString();
                            }
                            else
                            {
                                lblURL3.Text = commonCulture.ElementValues.getResourceString("lblURL3", xeResources);
                                txtURL3.Attributes.Add("PLACEHOLDER", lblURL3.Text);
                            }
                        }

                        else
                        {
                            lblURL1.Text = commonCulture.ElementValues.getResourceString("lblURL1", xeResources);
                            txtURL1.Attributes.Add("PLACEHOLDER", lblURL1.Text);

                            lblURL2.Text = commonCulture.ElementValues.getResourceString("lblURL2", xeResources);
                            txtURL2.Attributes.Add("PLACEHOLDER", lblURL2.Text);

                            lblURL3.Text = commonCulture.ElementValues.getResourceString("lblURL3", xeResources);
                            txtURL3.Attributes.Add("PLACEHOLDER", lblURL3.Text);
                        }

                        lblLanguage.Text = commonCulture.ElementValues.getResourceString("lblLanguage", xeResources);
                        lblCommissionType.Text = commonCulture.ElementValues.getResourceString("lblCommissionType", xeResources);

                        lblSecQues.Text = commonCulture.ElementValues.getResourceString("lblSecQues", xeResources);

                        lblSecAns.Text = commonCulture.ElementValues.getResourceString("lblSecAns", xeResources);
                        txtSecAns.Text = dsAffMember.Tables[0].Rows[0]["securityAnswer"].ToString();
        
                        lblBankAccName.Text = commonCulture.ElementValues.getResourceString("lblBankAccName", xeResources);
                        txtBankAccName.Text = dsAffMember.Tables[0].Rows[0]["BankAccName"].ToString();

                        lblBankAccNo.Text = commonCulture.ElementValues.getResourceString("lblBankAccNo", xeResources);
                        txtBankAccNo.Text = dsAffMember.Tables[0].Rows[0]["BankAccNumber"].ToString();

                        lblSwiftCode.Text = commonCulture.ElementValues.getResourceString("lblSwiftCode", xeResources);
                        txtSwiftCode.Text = dsAffMember.Tables[0].Rows[0]["BankSwiftCode"].ToString();
 
                        lblBankName.Text = commonCulture.ElementValues.getResourceString("lblBankName", xeResources);
                        txtBankName.Text = dsAffMember.Tables[0].Rows[0]["BankName"].ToString();
                        
                        lblBankAdd.Text = commonCulture.ElementValues.getResourceString("lblBankAdd", xeResources);
                        txtBankAdd.Text = dsAffMember.Tables[0].Rows[0]["BankAddress"].ToString();
                        
                        //lblCaptcha.Text = commonCulture.ElementValues.getResourceString("lblCaptcha", xeResources);
                        //txtCaptcha.Attributes.Add("PLACEHOLDER", lblCaptcha.Text);

                        //lblDisclaimer.InnerText = commonCulture.ElementValues.getResourceString("lblDisclaimer", xeResources);

                        btnUpdate.Text = commonCulture.ElementValues.getResourceString("lblUpdate", xeResources);
                        btnCancel.InnerText = commonCulture.ElementValues.getResourceString("btnCancel", xeResources);

                        #region PhoneCountryCode
                        System.Data.DataSet dsCountryInfo = null;

                        using (wsMemberMS1.memberWSSoapClient wsInstance = new wsMemberMS1.memberWSSoapClient())
                        {
                            dsCountryInfo = wsInstance.GetCountryInfo(Convert.ToInt64(strOperatorId));

                            if (dsCountryInfo.Tables[0].Rows.Count > 0)
                            {
                                foreach (System.Data.DataRow drPhoneCountryCode in dsCountryInfo.Tables[0].Select("", "countryPhoneCode ASC"))
                                {
                                    string strProcessRemark = "Register: GetCountryInfo" + strOperatorId;
                                    int intProcessSerialId = 0;
                                    intProcessSerialId += 1;
                                    commonAuditTrail.appendLog("system", "Register", "ParameterValidation", "DataBaseManager.DLL", "", "", "", "", strProcessRemark, Convert.ToString(intProcessSerialId), "", true);

                                    drpContactCountry.Items.Add(new ListItem(string.Format("+ {0}", Convert.ToString(drPhoneCountryCode["countryPhoneCode"])), Convert.ToString(drPhoneCountryCode["countryPhoneCode"])));
                                }
                            }


                        }
                        #endregion

                        #region Currencies
                        //string arrStrCurrencies = opSettings.Values.Get("Currencies");
                        //List<string> lstCurrencies = arrStrCurrencies.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();

                        //drpCurrency.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpCurrencySelect", xeResources), "-1"));

                        //foreach (string currency in lstCurrencies)
                        //{
                        //    string strProcessRemark = "currency: " + currency;
                        //    int intProcessSerialId = 0;
                        //    intProcessSerialId += 1;
                        //    commonAuditTrail.appendLog("system", "Register", "ParameterValidation", "DataBaseManager.DLL", "", "", "", "", strProcessRemark, Convert.ToString(intProcessSerialId), "", true);

                        //    drpCurrency.Items.Add(new ListItem(commonCulture.ElementValues.getResourceXPathString("Currency/" + currency, xeResources), currency));
                        //}
                        #endregion

                        #region Country
                        //using (wsAffiliateMS1.affiliateWSSoapClient wsInstanceAff = new wsAffiliateMS1.affiliateWSSoapClient("affiliateWSSoap"))
                        //{

                        System.Data.DataSet ds_country = wsInstanceAff.GetCountryList();

                        if (ds_country.Tables[0].Rows.Count > 0)
                        {
                            drpCountry.DataTextField = "countryName";
                            drpCountry.DataValueField = "countryCode";
                            drpCountry.DataSource = ds_country.Tables[0];
                            drpCountry.DataBind();

                            drpCountry.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpCountrySelect", xeResources), "-1"));
                        }
                        //}
                        #endregion

                        #region Language

                        string[] langcodes = System.Configuration.ConfigurationManager.AppSettings.Get("list_language_code").Split(',');
                        string[] langNames = System.Configuration.ConfigurationManager.AppSettings.Get("list_language_translation").Split(',');

                        drpLanguage.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("drpLanguageSelect", xeResources), "-1"));

                        for (int i = 0; i < langcodes.Length; i++)
                        {
                            drpLanguage.Items.Add(new ListItem(langNames[i], langcodes[i]));
                        }
                                             
                         drpLanguage.SelectedValue = dsAffMember.Tables[0].Rows[0]["languageCode"].ToString();
                       
                        #endregion

                        #region Commission Type
                      
                            drpCommissionType.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("lblCommissionType", xeResources), "-1"));
                            drpCommissionType.Items.Add(new ListItem(commonCulture.ElementValues.getResourceString("lblRevenueShare", xeResources).ToString(), "Revenue Share"));
                      
                            drpCommissionType.SelectedValue = dsAffMember.Tables[0].Rows[0]["comType"].ToString();
                       
                        #endregion

                        #region Security Question

                       
                            //drpSecQues.Items.Insert(0, new ListItem(commonCulture.ElementValues.getResourceString("lblSecQues", xeResources), "-1"));

                            for (int i = 1; i <= 6; i++)
                            {
                                drpSecQues.Items.Add(new ListItem(commonCulture.ElementValues.getResourceString("lblsecurityquestion" + i, xeResourcesSecQues), "security_question_" + i));
                            }
                        
                            drpSecQues.SelectedValue = dsAffMember.Tables[0].Rows[0]["securityQuestion"].ToString();
                       
                        #endregion

                        int intDay = 0;
                        foreach (int vintDay in new int[31]) { intDay++; drpDay.Items.Add(new ListItem((intDay).ToString("0#"), Convert.ToString(intDay))); }
                        foreach (System.Xml.Linq.XElement xeMonth in xeResources.Element("Calendar").Elements()) { drpMonth.Items.Add(new ListItem(xeMonth.Value, Convert.ToString(xeMonth.Name).Replace("m", ""))); }
                        for (int intYear = System.DateTime.Now.Year - 18; intYear >= System.DateTime.Now.Year - 99; intYear--) { drpYear.Items.Add(new ListItem(Convert.ToString(intYear))); }

                        //txtAffiliateID.Text = strAffiliateId;

                    }
                }
            }
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        string strAffiliateId = string.Empty;

        #region initialiseVariables
        int intProcessSerialId = 0;
        string strProcessId = Guid.NewGuid().ToString().ToUpper();
        string strPageName = "Register";

        string strProcessCode = string.Empty;

        string strResultCode = string.Empty;
        string strResultDetail = string.Empty;
        string strErrorCode = string.Empty;
        string strErrorDetail = string.Empty;
        string strProcessRemark = string.Empty;
        bool isProcessAbort = false;
        bool isSystemError = false;

        long lngOperatorId = 1;
        bool isTestAccount = false;
        //string strMemberCode = string.Empty;
        //string strPassword = string.Empty;
        //string strPasswordEncrypted = string.Empty;
        //string strEmail = string.Empty;
        string strContact = string.Empty;
        string strContactNumber = string.Empty;
        string strDOB = string.Empty;
        string strFName = string.Empty;
        //string strLName = string.Empty;
        //string strCurrencyCode = string.Empty;
        string strCountryCode = string.Empty;
        string strAccount = string.Empty;
        //string strReferralId = string.Empty;
        string strLanguageCode = string.Empty;
        string strCommissionType = string.Empty;
        string strAddress = string.Empty;
        string strCity = string.Empty;
        string strPostal = string.Empty;
        string strUrl1 = string.Empty;
        string strUrl2 = string.Empty;
        string strUrl3 = string.Empty;
        //string strDesc = string.Empty;

        string strIPAddress = string.Empty;
        string strSignUpUrl = string.Empty;
        string strVCode = string.Empty;
        string strSessionVCode = string.Empty;
        string strPermission = string.Empty;

        string strSecurityQues = string.Empty;
        string strSecurityAns = string.Empty;

        string strBankAccName = string.Empty;
        string strBankAccNo = string.Empty;
        string strSwiftCode = string.Empty;
        string strBankName = string.Empty;
        string strBankAdd = string.Empty;


        int intOddsType = 1;
        System.DateTime dtDOB = DateTime.MinValue;
        string strHiddenValues = hidValues.Value;

        List<string> lstValues = null;
        #endregion

        #region populateVariables

        strContact = txtContact.Text;
        strContactNumber = string.Format("{0}-{1}", drpContactCountry.SelectedValue, strContact);
        strDOB = string.Format("{0}-{1}-{2}", drpYear.SelectedValue, drpMonth.SelectedValue, drpDay.SelectedValue);
        strCountryCode = drpCountry.SelectedValue;
        strAccount = txtAccount.Text.Trim(); ;
        strLanguageCode = drpLanguage.SelectedValue;
        strCommissionType = drpCommissionType.SelectedValue;
        strAddress = txtAddress.Text.Trim();
        strCity = txtCity.Text.Trim();
        strPostal = txtPostal.Text.Trim();
        strUrl1 = txtURL1.Text.Trim();
        strUrl2 = txtURL2.Text.Trim();
        strUrl3 = txtURL3.Text.Trim();

        strSecurityQues = drpSecQues.SelectedValue;
        strSecurityAns = txtSecAns.Text.Trim();
        strBankAccName = txtBankAccName.Text.Trim();
        strBankAccNo = txtBankAccNo.Text.Trim();
        strSwiftCode = txtSwiftCode.Text.Trim();
        strBankName = txtBankName.Text.Trim();
        strBankAdd = txtBankAdd.Text.Trim();

        strSessionVCode = commonVariables.GetSessionVariable("vCode");
        strAlertCode = "-1";

        //strAffiliateId = txtAffiliateID.Text;

        System.Text.RegularExpressions.Regex rexContact = new System.Text.RegularExpressions.Regex("([0-9]{1,4})[-]([0-9]{6,12})$");
        #endregion

        #region parametersValidation

        strResultCode = "11";
        strResultDetail = "Error:ParameterValidation";

        //txtCaptcha.Text = string.Empty;

        if (string.IsNullOrEmpty(strContact))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingContact", xeErrors);
            isProcessAbort = true;
        }
        else if (!rexContact.IsMatch(strContactNumber))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strSecurityAns))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/MissingSecurityAnswer", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strBankAccName))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidBankAccName", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strBankAccNo))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidBankAccNo", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strBankName))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidBankName", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strBankAdd))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidBankAdd", xeErrors);
            isProcessAbort = true;
        }

        else if (string.IsNullOrEmpty(strCountryCode) || string.Compare(strCountryCode, "-1", true) == 0)
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingCountryCode", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strLanguageCode) || string.Compare(strLanguageCode, "-1", true) == 0)
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingLanguageCode", xeErrors);
            isProcessAbort = true;
        }
        else if (string.IsNullOrEmpty(strCommissionType) || string.Compare(strCommissionType, "-1", true) == 0)
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/MissingCommissionType", xeErrors);
            isProcessAbort = true;
        }

        else if (commonValidation.isInjection(strContact))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidContact", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strCountryCode))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidCountryCode", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strAccount))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidAccount", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strLanguageCode))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidLanguageCode", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strCommissionType))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidCommissionType", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strAddress))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidAddress", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strCity))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidCity", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strPostal))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidPostal", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strUrl1))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidUrl1", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strUrl2))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidUrl2", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strUrl3))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidUrl3", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strSecurityQues))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidSecurityQuestion", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strSecurityAns))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidSecurityAnswer", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strBankAccName))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidBankAccName", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strBankAccNo))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidBankAccNo", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strBankName))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidBankName", xeErrors);
            isProcessAbort = true;
        }
        else if (commonValidation.isInjection(strBankAdd))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/InvalidBankAdd", xeErrors);
            isProcessAbort = true;
        }


        else if (!DateTime.TryParse(strDOB, out dtDOB))
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/InvalidDOB", xeErrors);
            isProcessAbort = true;
        }
        else
        {
            strResultCode = "00";
            strResultDetail = "OK:ParameterValidation";

            strContact = strContact.TrimStart('+');
            
        }

        strErrorDetail = strAlertMessage;
        strProcessRemark = string.Format("strAlertMessage: {0} | HiddenValues: {1}", strAlertMessage, strHiddenValues);

        intProcessSerialId += 1;
        commonAuditTrail.appendLog("system", strPageName, "ParameterValidation", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);

        #endregion

        if (!isProcessAbort)
        {
            lstValues = strHiddenValues.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToList();

            if (lstValues.Count > 0)
            {
                //if (lstValues[0] != null) { strCountryCode = lstValues[0]; }
                //if (lstValues[1] != null) { strSignUpUrl = string.Format("m.{0}", lstValues[1]); }
                if (lstValues[2] != null) { strIPAddress = lstValues[2]; }
                if (lstValues[3] != null) { strPermission = lstValues[3]; }
            }

            strSignUpUrl = string.Format("m.{0}", commonIp.DomainName);
            strLanguageCode = commonVariables.SelectedLanguage;

            if (string.IsNullOrEmpty(strIPAddress)) { strIPAddress = commonIp.UserIP; }

            if (string.IsNullOrEmpty(strCountryCode) || string.Compare(strCountryCode, "-", true) == 0)
            {
                using (wsIP2Loc.ServiceSoapClient wsInstance = new wsIP2Loc.ServiceSoapClient())
                {
                    wsInstance.location(strIPAddress, ref strCountryCode, ref strPermission);
                }
            }

            switch (strCountryCode.ToUpper())
            {
                case "MY":
                case "TH":
                case "VN":
                case "KH":
                    intOddsType = 1;
                    break;
                case "CN":
                    intOddsType = 2;
                    break;
                case "IN":
                case "KR":
                case "JP":
                case "AU":
                    intOddsType = 3;
                    break;
                case "ID":
                    intOddsType = 4;
                    break;

                default:
                    intOddsType = 3;
                    break;
            }

            customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");

            if (opSettings.Values.Get("DemoDomains").IndexOf(commonIp.DomainName) >= 0) { isTestAccount = true; }
                                                                     
            int intAffiliateId = string.IsNullOrEmpty(commonVariables.GetSessionVariable("AffiliateId")) ? (string.IsNullOrEmpty(strAffiliateId) ? 0 : Convert.ToInt32(strAffiliateId)) : Convert.ToInt32(commonVariables.GetSessionVariable("AffiliateId"));
            string strReferBy = string.Empty;
            string strDeviceId = "Mobile";

           
            //System.Data.DataSet dsRegister = null;
            int result = 0;

            //using (wsMemberMS1.memberWSSoapClient svcInstance = new wsMemberMS1.memberWSSoapClient())
            using (wsAffiliateMS1.affiliateWSSoapClient svcInstance = new wsAffiliateMS1.affiliateWSSoapClient())
            {
                //result = svcInstance.MemberRegistration(strMemberCode, strPasswordEncrypted, strFName, strEmail, strContactNumber, strCountryCode, strLanguageCode, strCurrencyCode, lngOperatorId,
                //            strAddress, strCity, strPostal, strIPAddress, strDesc, referralid, strUrl1, strUrl2, strUrl3, dtDOB, strAccount, strCommissionType, strSignUpUrl,
                //            (string)System.Configuration.ConfigurationManager.AppSettings.Get("internal_affiliate"));

                //strProcessRemark = "exec spAffiliateMemberInsertWS " + "'" + strMemberCode + "'" + ",'" + strPasswordEncrypted + "'" + ",'" + strFName + "'" + ",'" + strEmail + "'" + ",'" + strContactNumber + "'" + ",'" + strCountryCode + "'" + ",'" + strLanguageCode + "'" + ",'" + strCurrencyCode + "'" + ",'" + lngOperatorId + "'" + ",'" + strAddress + "'" + ",'" + strCity + "'" + ",'" + strPostal + "'" + ",'" + strIPAddress + "'" + ",'" + strDesc + "'" + ",'" + referralid + "'" + ",'" + strUrl1 + "'" + ",'" + strUrl2 + "'" + ",'" + strUrl3 + "'" + ",'" + dtDOB + "'" + ",'" + strAccount + "'" + ",'" + strCommissionType + "'" + ",'" + strSignUpUrl + "'" + ",'" + (string)System.Configuration.ConfigurationManager.AppSettings.Get("internal_affiliate") + "'";
                try
                {
                    //result = svcInstance.UpdateAffiliateMemberInfo(long.Parse(System.Web.HttpContext.Current.Session["AffiliateId"].ToString()), DateTime.Parse(strDOB),
                    //strCountryCode, strAccount, strContactNumber, strAddress, strCity, strPostal, strLanguageCode, strCommissionType, strSecurityQues,
                    //strSecurityAns, strBankAccName, strBankAccNo, strSwiftCode, strBankName, strBankAdd);

                    result = svcInstance.UpdateAffiliateMemberInfo(long.Parse(System.Web.HttpContext.Current.Session["AffiliateId"].ToString()), DateTime.Parse(strDOB),
                    strCountryCode, strAccount, strContactNumber, strAddress, strCity, strPostal, strLanguageCode, strCommissionType, strSecurityQues,
                    strSecurityAns, strBankAccName, strBankAccNo, strSwiftCode, strBankName, strBankAdd,"");

                    if (result == 1)
                    {
                        //delete removed url
                        if (String.IsNullOrEmpty(txtURL1.Text) && (string)System.Web.HttpContext.Current.Session["url1"] != "")
                        {
                            result = svcInstance.DeleteWebsiteURL(long.Parse((string)System.Web.HttpContext.Current.Session["urlID1"]));
                        }

                        if (String.IsNullOrEmpty(txtURL2.Text) && (string)System.Web.HttpContext.Current.Session["url2"] != "")
                        {
                            result = svcInstance.DeleteWebsiteURL(long.Parse((string)System.Web.HttpContext.Current.Session["urlID2"]));
                        }

                        if (String.IsNullOrEmpty(txtURL3.Text) && (string)System.Web.HttpContext.Current.Session["url3"] != "")
                        {
                            result = svcInstance.DeleteWebsiteURL(long.Parse((string)System.Web.HttpContext.Current.Session["urlID3"]));
                        }

                        if ((string)System.Web.HttpContext.Current.Session["url1"] != txtURL1.Text && !String.IsNullOrEmpty((string)System.Web.HttpContext.Current.Session["url1"]))
                        {
                            result = svcInstance.DeleteWebsiteURL(long.Parse((string)System.Web.HttpContext.Current.Session["urlID1"]));
                        }

                        if ((string)System.Web.HttpContext.Current.Session["url2"] != txtURL2.Text && !String.IsNullOrEmpty((string)System.Web.HttpContext.Current.Session["url2"]))
                        {
                            result = svcInstance.DeleteWebsiteURL(long.Parse((string)System.Web.HttpContext.Current.Session["urlID2"]));
                        }

                        if ((string)System.Web.HttpContext.Current.Session["url3"] != txtURL3.Text && !String.IsNullOrEmpty((string)System.Web.HttpContext.Current.Session["url3"]))
                        {
                            result = svcInstance.DeleteWebsiteURL(long.Parse((string)System.Web.HttpContext.Current.Session["urlID3"]));
                        }

                        //add new url
                        if ((string)System.Web.HttpContext.Current.Session["url1"] != txtURL1.Text && txtURL1.Text != commonCulture.ElementValues.getResourceString("lblURL1", xeResources) && !String.IsNullOrEmpty(txtURL1.Text))
                        {
                            result = svcInstance.InsertWebsiteURL(long.Parse(System.Web.HttpContext.Current.Session["AffiliateId"].ToString()), txtURL1.Text);
                        }
                      
                        if ((string)System.Web.HttpContext.Current.Session["url2"] != txtURL2.Text && txtURL2.Text != commonCulture.ElementValues.getResourceString("lblURL2", xeResources) && !String.IsNullOrEmpty(txtURL2.Text))
                        {
                            result = svcInstance.InsertWebsiteURL(long.Parse(System.Web.HttpContext.Current.Session["AffiliateId"].ToString()), txtURL2.Text);
                        }

                        if ((string)System.Web.HttpContext.Current.Session["url3"] != txtURL3.Text && txtURL3.Text != commonCulture.ElementValues.getResourceString("lblURL3", xeResources) && !String.IsNullOrEmpty(txtURL3.Text))
                        {
                            result = svcInstance.InsertWebsiteURL(long.Parse(System.Web.HttpContext.Current.Session["AffiliateId"].ToString()), txtURL3.Text);
                        }


                    }
                 
                }
                catch (Exception)
                {
                    throw;
                }
                strProcessRemark = "exec spAffiliateMemberUpdatePublic " + "'" + long.Parse(System.Web.HttpContext.Current.Session["AffiliateId"].ToString()) + "'" + ",'" + DateTime.Parse(strDOB) + "'" + ",'" + strCountryCode + "'" + ",'" + strAccount + "'" + ",'" + strContactNumber + "'" + ",'" + strAddress + "'" + ",'" + strCity + "'" + ",'" + strPostal + "'" + ",'" + strLanguageCode + "'" + ",'" + strCommissionType + "'" + ",'" + strSecurityQues + "'" + ",'" + strSecurityAns + "'" + ",'" + strBankAccName + "'" + ",'" + strBankAccNo + "'" + ",'" + strSwiftCode + "'" + ",'" + strBankName + ",'" + strBankAdd + "'" + "'";

                intProcessSerialId += 1;
                commonAuditTrail.appendLog("system", strPageName, "RegistrationParameterValidation", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);

                strResultCode = "21";
                strResultDetail = "Error:MemberRegistrationNew";

                //if (dsRegister.Tables[0].Rows.Count > 0)
                //{
                //    strProcessCode = Convert.ToString(dsRegister.Tables[0].Rows[0]["RETURN_VALUE"]);

                //switch (strProcessCode)
                switch (result.ToString())
                {
                    case "0":
                        strAlertMessage = commonCulture.ElementValues.getResourceString("Exception", xeErrors);
                        break;

                    case "1":
                        strAlertCode = "1";
                        strAlertMessage = commonCulture.ElementValues.getResourceXPathString("UpdateProfile/Success", xeErrors);
                        break;

                    case "10":
                        strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/DuplicateUsername", xeErrors);
                        break;

                    case "11":
                        strAlertMessage = commonCulture.ElementValues.getResourceXPathString("Register/DuplicateEmail", xeErrors);
                        break;

                    default:
                        strAlertMessage = commonCulture.ElementValues.getResourceString("Exception", xeErrors);
                        break;
                }

                strErrorCode = strProcessCode;
                strErrorDetail = strAlertMessage;
            }

            intProcessSerialId += 1;
            commonAuditTrail.appendLog("system", strPageName, "MemberRegistrationNew", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
        }
    }
    

    protected void IovationSubmit(ref int intProcessSerialId, string strProcessId, string strPageName, string strUsername, string strIPAddress, string strPermission)
    {
        string strResultCode = string.Empty;
        string strResultDetail = string.Empty;
        string strErrorCode = string.Empty;
        string strErrorDetail = string.Empty;
        string strProcessRemark = string.Empty;
        bool isSystemError = false;

        customConfig.IovationSettings ioSettings = new customConfig.IovationSettings("W88");
        string strCheckTransactionUrl = ioSettings.Values.Get("CheckTransactionUrl");
        string strGetEvidenceUrl = ioSettings.Values.Get("GetEvidenceUrl");
        string strAccountPrefix = ioSettings.Values.Get("AccountPrefix");
        string strSubscriberID = ioSettings.Values.Get("SubscriberId");
        string strSubscriberAccount = ioSettings.Values.Get("SubscriberAccount");
        string strSubscriberPassCode = ioSettings.Values.Get("SubscriberPassCode");
        string strServiceEnabled = ioSettings.Values.Get("ServiceEnabled");
        string strUserAccountCode = string.Format("{0}{1}", strAccountPrefix, strUsername);
        string strExceptions = ioSettings.Values.Get("Exceptions");

        List<string> lstPermission = strExceptions.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        if (lstPermission.FindIndex(x => x.Trim().Equals(strPermission, StringComparison.OrdinalIgnoreCase)) >= 0)
        {
            return;
        }
        else if (string.Compare(strServiceEnabled, "true", true) != 0)
        {
            return;
        }

        strProcessRemark = string.Format("CheckTransactionURL: {0} | GetEvidenceURL: {1} | AccountPrefix: {2} | SubscriberID: {3} | SubscriberAccount: {4} | SubscriberPassCode: {5} | UserAccountCode : {6}",
            strCheckTransactionUrl, strGetEvidenceUrl, strAccountPrefix, strSubscriberID, strSubscriberAccount, strSubscriberPassCode, strUserAccountCode);

        try
        {
            using (CheckTransactionDetailsService ioInstance = new CheckTransactionDetailsService(strCheckTransactionUrl))
            {
                CheckTransactionDetails ioRequest = new CheckTransactionDetails();

                ioRequest.accountcode = strUserAccountCode;
                ioRequest.enduserip = strIPAddress;

                ioRequest.beginblackbox = HttpContext.Current.Request.Form.Get("ioBlackBox");
                ioRequest.subscriberid = strSubscriberID;
                ioRequest.subscriberpasscode = strSubscriberPassCode;
                ioRequest.subscriberaccount = strSubscriberAccount;
                ioRequest.type = "registration";

                CheckTransactionDetailsResponse ioResponse = new CheckTransactionDetailsResponse();

                ioResponse = ioInstance.CheckTransactionDetails(ioRequest);

                #region setIovationCookie
                HttpCookie cookie = new HttpCookie(strUsername);
                cookie.Value = ioResponse.result;
                if (!string.IsNullOrEmpty(commonIp.DomainName)) { cookie.Domain = commonIp.DomainName; }
                cookie.Expires = System.DateTime.Now.AddDays(Convert.ToInt32(ioSettings.Values.Get("ServiceDays")));
                HttpContext.Current.Response.Cookies.Add(cookie);
                #endregion
            }
        }
        catch (Exception ex)
        {
            strResultCode = "31";
            strResultDetail = "Error:Iovation";
            strErrorCode = Convert.ToString(ex.HResult);
            strErrorDetail = ex.Message;

            isSystemError = true;
        }

        intProcessSerialId += 1;
        commonAuditTrail.appendLog("system", strPageName, "Iovation", "DataBaseManager.DLL", strResultCode, strResultDetail, strErrorCode, strErrorDetail, strProcessRemark, Convert.ToString(intProcessSerialId), strProcessId, isSystemError);
    }
}