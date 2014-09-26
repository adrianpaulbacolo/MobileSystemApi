using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Secure_UpdateProfile_Address : System.Web.UI.Page
{
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Xml.Linq.XElement xeResources = null;
        commonCulture.appData.getRootResource("/_Secure/UpdateProfile", out xeResources);
        customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");

        System.Data.DataSet dsMemberProfile = null;

        string strOperatorId = string.Empty;
        string strMemberId = string.Empty;

        #region UneditableFieldsVariablesInitialise
        string strEmailAddress = string.Empty;
        string strContactNumber = string.Empty;
        string strMemberCode = string.Empty;
        string strFName = string.Empty;
        string strLName = string.Empty;
        string strDOB = string.Empty;
        string strCurrency = string.Empty;
        string strCountry = string.Empty;
        #endregion

        #region AddressDetailInitialisation
        string strAddress = string.Empty;
        string strCity = string.Empty;
        string strPostal = string.Empty;
        #endregion

        #region OtherDetails
        string strGender = string.Empty;
        string strOdds = string.Empty;
        string strLanguage = string.Empty;
        string strSecurityQuestion = string.Empty;
        string strSecurityAnswer = string.Empty;
        #endregion

        if (!Page.IsPostBack)
        {
            strOperatorId = commonVariables.OperatorId;
            strMemberId = commonVariables.GetSessionVariable("MemberId");

            lblAddress.Text = commonCulture.ElementValues.getResourceString("lblAddress", xeResources);
            lblCity.Text = commonCulture.ElementValues.getResourceString("lblCity", xeResources);
            lblPostal.Text = commonCulture.ElementValues.getResourceString("lblPostal", xeResources);

            btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnSubmit", xeResources);

            using (wsMemberMS1.memberWSSoapClient wsInstance = new wsMemberMS1.memberWSSoapClient())
            {
                dsMemberProfile = wsInstance.GetMemberInfo(Convert.ToInt64(strOperatorId), Convert.ToInt64(strMemberId));

                if (dsMemberProfile.Tables.Count > 0)
                {
                    if (dsMemberProfile.Tables[0].Rows.Count == 1)
                    {
                        #region UneditableFieldsVariablesPopulate
                        strEmailAddress = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["email"]);
                        strContactNumber = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["mobile"]);
                        strMemberCode = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["memberCode"]);
                        strFName = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["firstName"]);
                        strLName = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["lastName"]);
                        strDOB = Convert.ToDateTime(dsMemberProfile.Tables[0].Rows[0]["dob"]).ToString(commonVariables.DisplayDateFormat);
                        strCurrency = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["currencyCode"]);
                        strCountry = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["countryCode"]);
                        #endregion

                        #region AddressDetails
                        strAddress = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["address"]);
                        strCity = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["city"]);
                        strPostal = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["postal"]);
                        #endregion

                        #region OtherDetails
                        strGender = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["gender"]);
                        strOdds = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["oddsType"]);
                        strLanguage = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["languageCode"]);
                        strSecurityQuestion = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["securityQuestion"]);
                        strSecurityAnswer = Convert.ToString(dsMemberProfile.Tables[0].Rows[0]["securityAnswer"]);
                        #endregion
                    }
                }
            }
        }
    }
}