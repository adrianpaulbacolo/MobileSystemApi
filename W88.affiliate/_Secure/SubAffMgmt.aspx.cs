using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;

public partial class _Secure_SubAffMgmt : System.Web.UI.Page
{
    protected System.Xml.Linq.XElement xeErrors = null;
    protected System.Xml.Linq.XElement xeResources = null;
    protected System.Xml.Linq.XElement xeResourcesSecQues = null;
    protected string strAlertCode = string.Empty;
    protected string strAlertMessage = string.Empty;

    protected void Page_Init(object sender, EventArgs e) { 
        if (string.IsNullOrEmpty(commonVariables.CurrentMemberSessionId)) 
        { 
            Response.Redirect("/Index",true); 
        }  
    }
  
    protected void Page_Load(object sender, EventArgs e)
    {
        string strOperatorId = commonVariables.OperatorId;
        string strAffiliateId = string.Empty;
        xeErrors = commonVariables.ErrorsXML;
    
        commonCulture.appData.getRootResource("/AccountInfo.aspx", out xeResources);
     
        customConfig.OperatorSettings opSettings = new customConfig.OperatorSettings("W88");

        //testing
        //System.Web.HttpContext.Current.Session["AffiliateId"] = "20264";

        if (!Page.IsPostBack)
        {
            try
            {
                using (wsAffiliateMS1.affiliateWSSoapClient wsInstanceAff = new wsAffiliateMS1.affiliateWSSoapClient("affiliateWSSoap"))
                {
                    DataSet ds = wsInstanceAff.GetSubAffiliateList(long.Parse((string)System.Web.HttpContext.Current.Session["AffiliateId"]));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ds.Tables[0].Rows[0]["ParentID"] = "0";

                        lblEmailAddress.Text = commonCulture.ElementValues.getResourceString("lblEmailAddress", xeResources);
                        lblMessage.Text = commonCulture.ElementValues.getResourceString("lblMessage", xeResources);

                        btnSubmit.Text = commonCulture.ElementValues.getResourceString("btnInvite", xeResources);
                        btnCancel.InnerText = commonCulture.ElementValues.getResourceString("btnCancel", xeResources);
                                                
                        string emailContent = "";
                        string emailSubject = "";
                        getEmail(ref emailSubject, ref emailContent);

                        mainContent.Text = Server.HtmlDecode(emailContent);
                    }
                }
            }
            catch (Exception) { }



        }

    }

    private void getEmail(ref string emailSubject, ref string emailContent)
    {
        WebRequest req;
        WebResponse res;
        try
        {



            req = WebRequest.Create(Server.MapPath("~").ToLower() + "/emailTemplate/1/SubAffiliateInvitation_" + commonVariables.SelectedLanguage + ".html");
            res = req.GetResponse();
        }
        catch (Exception ex)
        {


            req = WebRequest.Create(Server.MapPath("~").ToLower() + "/emailTemplate/1/SubAffiliateInvitation_en-us.html");
            res = req.GetResponse();
        }



        StreamReader sr = new StreamReader(res.GetResponseStream());


        string html = sr.ReadToEnd();
        bool utf8Content = false;
        bool utf8Subject = false;

        if (html != "")
        {




            string subjectStartTag = "<!--emailsubject=";
            string subjectEndTag = "=emailsubject-->";
            int startPosition = html.IndexOf(subjectStartTag);
            int endposition = html.IndexOf(subjectEndTag);

            if (startPosition >= 0 && endposition >= 0)
            {
                string domain = commonIp.DomainName;
                //string domain = "w88uat";

                emailSubject = html.Substring(startPosition + subjectStartTag.Length, endposition - startPosition - subjectStartTag.Length);
                emailContent = html.Replace(subjectStartTag + emailSubject + subjectEndTag, "");
                emailContent = emailContent.Replace("[affiliateid]", (string)System.Web.HttpContext.Current.Session["AffiliateId"]);
                //emailContent = emailContent.Replace(".w88.com/register", (string)Session["domain_1"] + "/register");
                //emailContent = emailContent.Replace(".w88.com/register", "." + domain + ".com/register");
                emailContent = emailContent.Replace(".w88.com/register", "." + domain + "/register");
                

                if (commonVariables.SelectedLanguage.ToString().ToLower() != "en-us")
                {
                    utf8Subject = true;
                    utf8Content = true;

                }
            }
        }
    }

    private void checkEmail(ref int result, string email)
    {
        //localResx = string.Format(localResx, (string)Session["language"]);
        if (!string.IsNullOrEmpty(email))
        {
            string resultEmail = "00";
            string emailContent = "";
            string emailSubject = "";
            getEmail(ref emailSubject, ref emailContent);

            //string emailContent = "";
            //string emailSubject = commonCulture.ElementValues.getResourceString("lblEmailAddress", xeResources);
          
            resultEmail = Email.sendInviteMemberEmail(commonVariables.SelectedLanguage.ToString(), email, emailSubject, emailContent);

            if (resultEmail == "00")
                result = 1;
            else
                result = -1;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int intProcessSerialId = 0;
        string strProcessId = Guid.NewGuid().ToString().ToUpper();
        string strPageName = "UpdatePassword";

        string strProcessCode = string.Empty;

        string strResultCode = string.Empty;
        string strResultDetail = string.Empty;
        string strErrorCode = string.Empty;
        string strErrorDetail = string.Empty;
        string strProcessRemark = string.Empty;
        bool isProcessAbort = false;
        bool isSystemError = false;

        long lngOperatorId = 1;
                
        int intResult = int.MinValue;

        #region populateVariables
      
        #endregion

        #region parametersValidation

        int result = -1;

        checkEmail(ref result, txtEmail1.Text);
        checkEmail(ref result, txtEmail2.Text);
        checkEmail(ref result, txtEmail3.Text);
        checkEmail(ref result, txtEmail4.Text);
        checkEmail(ref result, txtEmail5.Text);
       
        if (result == -1)
        {
            strAlertMessage = commonCulture.ElementValues.getResourceXPathString("SubAffiliate/MissingValidEmail", xeErrors);
            isProcessAbort = true;
        }
        

        #endregion

        if (!isProcessAbort)
        {

            switch (result)
            {
                case 1: 
                    strAlertCode = "1";
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("SubAffiliate/InvitationSuccess", xeErrors);

                    //clear data
                    txtEmail1.Text = "";
                    txtEmail2.Text = "";
                    txtEmail3.Text = "";
                    txtEmail4.Text = "";
                    txtEmail5.Text = "";
                    break;
                default: // general error
                    strAlertMessage = commonCulture.ElementValues.getResourceXPathString("SubAffiliate/InvitationFailed", xeErrors);
                    break;
            }
        }
    }
    
      
}