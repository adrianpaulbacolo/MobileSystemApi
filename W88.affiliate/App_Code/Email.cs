using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Net;
using System.Collections;


/// <summary>
/// Summary description for Email
/// </summary>
public class Email
{
    
	public Email()
	{
		//
		// TODO: Add constructor logic here
		//

    }


    public static bool checkFilterEmail(string receiverEmail)
    {
        bool chk = false; 

        ArrayList emailFilterList = new ArrayList();
        emailFilterList.AddRange(System.Configuration.ConfigurationManager.AppSettings["emailfilter"].Split(','));

        int count = emailFilterList.Count;

        for (int i = 0; i < count; i++)
        {

            if (receiverEmail.ToLower().Contains(emailFilterList[i].ToString()))
            {

                chk = true;
            }

        }

        return chk;

    }


    public static void Send(string senderEmail, string senderName, string receiverEmail, string subjectText, bool subjectUTF8,
                                   string bodyText, bool bodyUTF8, string cc, string BCC)
    {

        //create the mail message





        if (checkFilterEmail(receiverEmail))
        {

            try
            {
                System.Net.Mail.SmtpClient sClient = new System.Net.Mail.SmtpClient();
                System.Net.NetworkCredential nCredentials = new System.Net.NetworkCredential();

                using (MailMessage mail = new MailMessage())
                {
                    nCredentials.UserName = "dev@w88.com";
                    nCredentials.Password = "2NDbr0isFAT!";

                    sClient.UseDefaultCredentials = false;
                    sClient.Credentials = nCredentials;
                    sClient.Port = 25;
                    sClient.Host = "retail.smtp.com";


                    mail.From = new MailAddress(senderEmail, senderName);
                    mail.To.Add(receiverEmail);

                    //set the content
                    mail.Subject = subjectText;
                    mail.Body = bodyText;
                    if (subjectUTF8) mail.SubjectEncoding = System.Text.Encoding.UTF8;
                    if (bodyUTF8) mail.SubjectEncoding = System.Text.Encoding.UTF8;

                    mail.IsBodyHtml = true;

                    // mail.Bcc.Add(new System.Net.Mail.MailAddress("gb.vchen@gmail.com"));

                    sClient.Send(mail);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        else
        {

            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(senderEmail, senderName);
            mail.To.Add(receiverEmail);

            //set the content
            mail.Subject = subjectText;
            mail.Body = bodyText;
            if (subjectUTF8) mail.SubjectEncoding = System.Text.Encoding.UTF8;
            if (bodyUTF8) mail.SubjectEncoding = System.Text.Encoding.UTF8;

            mail.IsBodyHtml = true;
               mail.Bcc.Add(new System.Net.Mail.MailAddress("dev.debug@aquozsolutions.com"));
           // mail.Bcc.Add(new System.Net.Mail.MailAddress("vchenz@hotmail.com"));
            //send the message
            SmtpClient smtp = new SmtpClient(System.Configuration.ConfigurationManager.AppSettings["emailHost"], 25);
            try
            {
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

    }

    public static string sendWelcomeMemberEmail(string languagecode, string fullname, string membercode, string password, string email,
        string domainname)
    {

        string emailSubject = "";
        string emailContent = "";
        bool utf8Content = false;
        bool utf8Subject = false;
        try
        {
            WebRequest req;
            WebResponse res;
            try
            {
                req = WebRequest.Create(System.Web.HttpContext.Current.Session["affDomainHTTP"].ToString() + "/emailTemplate/" + System.Configuration.ConfigurationManager.AppSettings.Get("operator_id") + "/WelcomeMember_" + languagecode + ".html");
                res = req.GetResponse();
            }
            catch (Exception ex)
            {
                req = WebRequest.Create(System.Web.HttpContext.Current.Session["affDomainHTTP"].ToString() + "/emailTemplate/" + System.Configuration.ConfigurationManager.AppSettings.Get("operator_id") + "/WelcomeMember_en-us.html");
                res = req.GetResponse();
            }
            StreamReader sr = new StreamReader(res.GetResponseStream());
            string html = sr.ReadToEnd();

            if (html != "")
            {

                string subjectStartTag = "<!--emailsubject=";
                string subjectEndTag = "=emailsubject-->";
                int startPosition = html.IndexOf(subjectStartTag);
                int endposition = html.IndexOf(subjectEndTag);

                if (startPosition >= 0 && endposition >= 0)
                {
                    emailSubject = html.Substring(startPosition + subjectStartTag.Length, endposition - startPosition - subjectStartTag.Length).Replace("[fullname]", fullname);
                    emailContent = html.Replace(subjectStartTag + emailSubject + subjectEndTag, "");
                    emailContent = emailContent.Replace("[fullname]", fullname);
                    emailContent = emailContent.Replace("[membercode]", membercode);
                    emailContent = emailContent.Replace("[domain]", domainname);

                    if (languagecode.ToLower() != "en-us")
                    {
                        utf8Subject = true;
                        utf8Content = true;
                    }

                    //Email.Send(AppSettings.senderWelcomeMemberEmail, AppSettings.senderWelcomeMemberName, email, emailSubject, utf8Subject, emailContent, utf8Content, "", "");
                    //commonFunction.audit_trail("send email", "sendWelcomeMemberEmail", "", "", "-1", "00",
                    //"email:" + email + " - " +
                    //"senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
                    //"senderContactUsName:" + AppSettings.senderContactUsName + " - " +
                    //"emailSubject:" + emailSubject + " - " +
                    //"utf8Content:" + utf8Content + " - " +
                    //"00 - Successful", "-", "-", "", "0", true);

                    return "00";
                }
                else
                {
                    //commonFunction.audit_trail("send email", "sendWelcomeMemberEmail", "", "", "-1", "00",
                    //"email:" + email + " - " +
                    //"senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
                    //"senderContactUsName:" + AppSettings.senderContactUsName + " - " +
                    //"emailSubject:" + emailSubject + " - " +
                    //"utf8Content:" + utf8Content + " - " +
                    //" - Error:" +
                    //"01 - Error No Subject", "-", "-", "", "0", true);

                    return "01";
                }
            }
            else
            {
                //commonFunction.audit_trail("send email", "sendWelcomeMemberEmail", "", "", "-1", "00",
                //"email:" + email + " - " +
                //"senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
                //"senderContactUsName:" + AppSettings.senderContactUsName + " - " +
                //"emailSubject:" + emailSubject + " - " +
                //"utf8Content:" + utf8Content + " - " +
                //" - Error:" +
                //"02 - Error No Template", "-", "-", "", "0", true);

                return "02";
            }
        }
        catch (Exception ex)
        {
            //commonFunction.audit_trail("send email", "sendWelcomeMemberEmail", "", "", "-1", "00",
            //    "email:" + email + " - " +
            //    "senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
            //    "senderContactUsName:" + AppSettings.senderContactUsName + " - " +
            //    "emailSubject:" + emailSubject + " - " +
            //    "emailContent:" + emailContent + " - " +
            //    "utf8Content:" + utf8Content + " - " +
            //    " - Error:" +
            //    ex.Message, "-", "-", "", "0", true);

            return "99";
        }
    }


    public static string sendInviteMemberEmail(string languagecode, string email, string emailSubject, string emailContent)
    {
        bool utf8Content = false;
        bool utf8Subject = false;

        System.Xml.Linq.XElement xeResources = null;

        commonCulture.appData.getRootResource("/AccountInfo.aspx", out xeResources);

        //try
        //{
            //WebRequest req = WebRequest.Create(Server.MapPath("~").ToLower() +"/emailTemplate/" + AppSettings.operator_id + "/WelcomeMember_" + languagecode + ".html");
            //WebResponse res = req.GetResponse();
            //StreamReader sr = new StreamReader(res.GetResponseStream());
            //string html = sr.ReadToEnd();

            //if (html != "")
            //{

                if (languagecode.ToLower() != "en-us")
                {
                    utf8Subject = true;
                    utf8Content = true;
                }

                commonCulture.ElementValues.getResourceString("lblEmailAddress", xeResources);

                //Email.Send((string)System.Configuration.ConfigurationManager.AppSettings.Get("senderEmail"), (string)System.Configuration.ConfigurationManager.AppSettings.Get("senderName"), email, emailSubject, utf8Subject, emailContent, utf8Content, "", "");
                Email.Send((string)System.Configuration.ConfigurationManager.AppSettings.Get("senderEmail"), commonCulture.ElementValues.getResourceString("lblsenderName", xeResources), email, emailSubject, utf8Subject, emailContent, utf8Content, "", "");

                //commonFunction.audit_trail("send email", "sendInviteMemberEmail", "", "", "-1", "00",
                //"email:" + email + " - " +
                //"senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
                //"senderContactUsName:" + AppSettings.senderContactUsName + " - " +
                //"emailSubject:" + emailSubject + " - " +
                //"emailContent:" + emailContent + " - " +
                //"utf8Content:" + utf8Content + " - " +
                //"00 - Successful", "-", "-", "", "0", true);
                return "00";
            //}
            //else
            //{
            //    commonFunction.audit_trail("send email", "sendInviteMemberEmail", "", "", "-1", "00",
            //    "email:" + email + " - " +
            //    "senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
            //    "senderContactUsName:" + AppSettings.senderContactUsName + " - " +
            //    "emailSubject:" + emailSubject + " - " +
            //    "emailContent:" + emailContent + " - " +
            //    "utf8Content:" + utf8Content + " - " +
            //    " - Error:" +
            //    "02 - Error No Template", "-", "-", "", "0", true);

                //return "02";
            //}
        //}
        //catch (Exception ex)
        //{
            //commonFunction.audit_trail("send email", "sendInviteMemberEmail", "", "", "-1", "00",
            //    "email:" + email + " - " +
            //    "senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
            //    "senderContactUsName:" + AppSettings.senderContactUsName + " - " +
            //    "emailSubject:" + emailSubject + " - " +
            //    "emailContent:" + emailContent + " - " +
            //    "utf8Content:" + utf8Content + " - " +
            //    " - Error:" +
            //    ex.Message, "-", "-", "", "0",true);
        //    return "99";
        //}
    }



    public static string sendForgotPasswordEmail(long operatorId, string memberCode, string newPassword, string email, string languagecode,string domainname)
    {

        string emailSubject = "";
        string emailContent = "";
        bool utf8Content = false;
        bool utf8Subject = false;

        try
        {
            WebRequest req;
            WebResponse res;
            try
            {
                req = WebRequest.Create(System.Web.HttpContext.Current.Session["affDomainHTTP"].ToString() + "/emailTemplate/" + System.Configuration.ConfigurationManager.AppSettings.Get("operator_id") + "/ForgetPassword_" + languagecode + ".html");
                res = req.GetResponse();
            }
            catch (Exception ex)
            {
                req = WebRequest.Create(System.Web.HttpContext.Current.Session["affDomainHTTP"].ToString() + "/emailTemplate/" + System.Configuration.ConfigurationManager.AppSettings.Get("operator_id") + "/ForgetPassword_en-us.html");
                res = req.GetResponse();
            }
            StreamReader sr = new StreamReader(res.GetResponseStream());
            string html = sr.ReadToEnd();

            if (html != "")
            {

                string subjectStartTag = "<!--emailsubject=";
                string subjectEndTag = "=emailsubject-->";
                int startPosition = html.IndexOf(subjectStartTag);
                int endposition = html.IndexOf(subjectEndTag);

                if (startPosition >= 0 && endposition >= 0)
                {
                    emailSubject = html.Substring(startPosition + subjectStartTag.Length, endposition - startPosition - subjectStartTag.Length);
                    emailContent = html.Replace(subjectStartTag + emailSubject + subjectEndTag, "");
                    emailContent = emailContent.Replace("[membercode]", memberCode);
                    emailContent = emailContent.Replace("[password]", newPassword);

                    if (languagecode.ToLower() != "en-us")
                    {
                        utf8Subject = true;
                        utf8Content = true;
                    }

                    //Email.Send(AppSettings.senderForgotPasswordEmail, AppSettings.senderForgotPasswordName, email, emailSubject, utf8Subject, emailContent, utf8Content, "", "");
                    //commonFunction.audit_trail("send email", "sendForgotPasswordEmail", "", "", "-1", "00",
                    //"email:" + email + " - " +
                    //"senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
                    //"senderContactUsName:" + AppSettings.senderContactUsName + " - " +
                    //"emailSubject:" + emailSubject + " - " +
                    //"emailContent:" + emailContent + " - " +
                    //"utf8Content:" + utf8Content + " - " +
                    //" - Error:" +
                    //"00 - Successful", "-", "-", "", "0", true);
                    return "00";
                }
                else
                {
                    //commonFunction.audit_trail("send email", "sendForgotPasswordEmail", "", "", "-1", "00",
                    //"email:" + email + " - " +
                    //"senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
                    //"senderContactUsName:" + AppSettings.senderContactUsName + " - " +
                    //"emailSubject:" + emailSubject + " - " +
                    //"emailContent:" + emailContent + " - " +
                    //"utf8Content:" + utf8Content + " - " +
                    //" - Error:" +
                    //"01 - Error No Subject", "-", "-", "", "0", true);
                    return "01";
                }
            }
            else
            {
                //commonFunction.audit_trail("send email", "sendForgotPasswordEmail", "", "", "-1", "00",
                //"email:" + email + " - " +
                //"senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
                //"senderContactUsName:" + AppSettings.senderContactUsName + " - " +
                //"emailSubject:" + emailSubject + " - " +
                //"emailContent:" + emailContent + " - " +
                //"utf8Content:" + utf8Content + " - " +
                //" - Error:" +
                //"02 - Error No Template", "-", "-", "", "0", true);

                return "02";
            }
        }
        catch (Exception ex)
        {
            //commonFunction.audit_trail("send email", "sendForgotPasswordEmail", "", "", "-1", "00",
            //    "email:" + email + " - " +
            //    "senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
            //    "senderContactUsName:" + AppSettings.senderContactUsName + " - " +
            //    "emailSubject:" + emailSubject + " - " +
            //    "emailContent:" + emailContent + " - " +
            //    "utf8Content:" + utf8Content + " - " +
            //    " - Error:" +
            //    ex.Message, "-", "-", "", "0",true);
            return "99";
        }
    }
    public static string sendContactUsEmail(string name, string username, string email, string languagecode, string phonenumber, string message, string cc)
    {

        string emailSubject = "";
        string emailContent = "";
        bool utf8Content = false;
        bool utf8Subject = false;

        try
        {
            WebRequest req;
            WebResponse res;
            try
            {
                //req = WebRequest.Create(System.Web.HttpContext.Current.Session["affDomainHTTP"].ToString() + "/emailTemplate/" + AppSettings.operator_id + "/ContactUs_" + languagecode + ".html");
                req = WebRequest.Create(System.Web.HttpContext.Current.Session["affDomainHTTP"].ToString() + "/emailTemplate/" + System.Configuration.ConfigurationManager.AppSettings.Get("operator_id") + "/ContactUs_en-us.html");
                res = req.GetResponse();
            }
            catch (Exception ex)
            {
                req = WebRequest.Create(System.Web.HttpContext.Current.Session["affDomainHTTP"].ToString() + "/emailTemplate/" + System.Configuration.ConfigurationManager.AppSettings.Get("operator_id") + "/ContactUs_en-us.html");
                res = req.GetResponse();
            }
            StreamReader sr = new StreamReader(res.GetResponseStream());
            string html = sr.ReadToEnd();

            if (html != "")
            {

                string subjectStartTag = "<!--emailsubject=";
                string subjectEndTag = "=emailsubject-->";
                int startPosition = html.IndexOf(subjectStartTag);
                int endposition = html.IndexOf(subjectEndTag);

                if (startPosition >= 0 && endposition >= 0)
                {
                    emailSubject = html.Substring(startPosition + subjectStartTag.Length, endposition - startPosition - subjectStartTag.Length);
                    emailContent = html.Replace(subjectStartTag + emailSubject + subjectEndTag, "");
                    emailContent = emailContent.Replace("[url]", System.Web.HttpContext.Current.Session["affDomainHTTP"].ToString());
                    emailContent = emailContent.Replace("[name]", name);
                    emailContent = emailContent.Replace("[username]", username);
                    emailContent = emailContent.Replace("[email]", email);
                    emailContent = emailContent.Replace("[phonenumber]", phonenumber);
                    emailContent = emailContent.Replace("[message]", message);

                    if (languagecode.ToLower() != "en-us")
                    {
                        utf8Subject = true;
                        utf8Content = true;
                    }
                    //Email.Send(AppSettings.senderContactUsEmail, AppSettings.senderContactUsName, email, emailSubject, utf8Subject, emailContent, utf8Content, cc, "");
                    //commonFunction.audit_trail("send email", "sendContactUsEmail", "", "", "-1", "00",
                    //"email:" + email + " - " +
                    //"senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
                    //"senderContactUsName:" + AppSettings.senderContactUsName + " - " +
                    //"emailSubject:" + emailSubject + " - " +
                    //"emailContent:" + emailContent + " - " +
                    //"utf8Content:" + utf8Content + " - " +
                    //" - Error:" +
                    //"00 - Successful", "-", "-", "", "0", true);
                    return "00";
                }
                else
                {
                    //commonFunction.audit_trail("send email", "sendContactUsEmail", "", "", "-1", "00",
                    //"email:" + email + " - " +
                    //"senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
                    //"senderContactUsName:" + AppSettings.senderContactUsName + " - " +
                    //"emailSubject:" + emailSubject + " - " +
                    //"emailContent:" + emailContent + " - " +
                    //"utf8Content:" + utf8Content + " - " +
                    //"cc:" + cc + " - " + " - Error:" +
                    //"01 - Error No Subject", "-", "-", "", "0", true);

                    return "01";
                }
            }
            else
            {
                //commonFunction.audit_trail("send email", "sendContactUsEmail", "", "", "-1", "00",
                //"email:" + email + " - " +
                //"senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
                //"senderContactUsName:" + AppSettings.senderContactUsName + " - " +
                //"emailSubject:" + emailSubject + " - " +
                //"emailContent:" + emailContent + " - " +
                //"utf8Content:" + utf8Content + " - " +
                //"cc:" + cc + " - " + " - Error:" +
                //"02 - Error No Template", "-", "-", "", "0", true);

                return "02";
            }
        }
        catch (Exception ex)
        {
            //commonFunction.audit_trail("send email", "sendContactUsEmail", "", "", "-1", "00",
            //    "email:" + email + " - " +
            //    "senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
            //    "senderContactUsName:" + AppSettings.senderContactUsName + " - " +
            //    "emailSubject:" + emailSubject + " - " +
            //    "emailContent:" + emailContent + " - " +
            //    "utf8Content:" + utf8Content + " - " +
            //    "cc:" + cc + " - " + " - Error:" +
            //    ex.Message, "-", "-", "", "0",
            //           true);
            return "99";
        }
    }
    public static string sendContactUsAutoReplyEmail(string name, string username, string email, string languagecode, string phonenumber, string message, string cc)
    {

        string emailSubject = "";
        string emailContent = "";
        bool utf8Content = false;
        bool utf8Subject = false;

        try
        {
            WebRequest req;
            WebResponse res;
            try
            {
                req = WebRequest.Create(System.Web.HttpContext.Current.Session["affDomainHTTP"].ToString() + "/emailTemplate/" + System.Configuration.ConfigurationManager.AppSettings.Get("operator_id") + "/ContactUs_autoreply_" + languagecode + ".html");
                //req = WebRequest.Create(System.Web.HttpContext.Current.Session["affDomainHTTP"].ToString() + "/emailTemplate/" + AppSettings.operator_id + "/ContactUs_en-us.html");
                res = req.GetResponse();
            }
            catch (Exception ex)
            {
                req = WebRequest.Create(System.Web.HttpContext.Current.Session["affDomainHTTP"].ToString() + "/emailTemplate/" + System.Configuration.ConfigurationManager.AppSettings.Get("operator_id") + "/ContactUs_autoreply_en-us.html");
                res = req.GetResponse();
            }
            StreamReader sr = new StreamReader(res.GetResponseStream());
            string html = sr.ReadToEnd();

            if (html != "")
            {

                string subjectStartTag = "<!--emailsubject=";
                string subjectEndTag = "=emailsubject-->";
                int startPosition = html.IndexOf(subjectStartTag);
                int endposition = html.IndexOf(subjectEndTag);

                if (startPosition >= 0 && endposition >= 0)
                {
                    emailSubject = html.Substring(startPosition + subjectStartTag.Length, endposition - startPosition - subjectStartTag.Length);
                    emailContent = html.Replace(subjectStartTag + emailSubject + subjectEndTag, "");
                    emailContent = emailContent.Replace("[url]", System.Web.HttpContext.Current.Session["affDomainHTTP"].ToString());
                    emailContent = emailContent.Replace("[name]", name);
                    emailContent = emailContent.Replace("[username]", username);
                    emailContent = emailContent.Replace("[email]", email);
                    emailContent = emailContent.Replace("[phonenumber]", phonenumber);
                    emailContent = emailContent.Replace("[message]", message);

                    if (languagecode.ToLower() != "en-us")
                    {
                        utf8Subject = true;
                        utf8Content = true;
                    }
                    //Email.Send(AppSettings.senderContactUsEmail, AppSettings.senderContactUsName, email, emailSubject, utf8Subject, emailContent, utf8Content, cc, "");
                    //commonFunction.audit_trail("send email", "sendContactUsAutoReplyEmail", "", "", "-1", "00",
                    //"email:" + email + " - " +
                    //"senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
                    //"senderContactUsName:" + AppSettings.senderContactUsName + " - " +
                    //"emailSubject:" + emailSubject + " - " +
                    //"emailContent:" + emailContent + " - " +
                    //"utf8Content:" + utf8Content + " - " +
                    //" - Error:" +
                    //"00 - Successful", "-", "-", "", "0", true);
                    return "00";
                }
                else
                {
                    //commonFunction.audit_trail("send email", "sendContactUsAutoReplyEmail", "", "", "-1", "00",
                    //"email:" + email + " - " +
                    //"senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
                    //"senderContactUsName:" + AppSettings.senderContactUsName + " - " +
                    //"emailSubject:" + emailSubject + " - " +
                    //"emailContent:" + emailContent + " - " +
                    //"utf8Content:" + utf8Content + " - " +
                    //"cc:" + cc + " - " + " - Error:" +
                    //"01 - Error No Subject", "-", "-", "", "0", true);

                    return "01";
                }
            }
            else
            {
                //commonFunction.audit_trail("send email", "sendContactUsAutoReplyEmail", "", "", "-1", "00",
                //"email:" + email + " - " +
                //"senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
                //"senderContactUsName:" + AppSettings.senderContactUsName + " - " +
                //"emailSubject:" + emailSubject + " - " +
                //"emailContent:" + emailContent + " - " +
                //"utf8Content:" + utf8Content + " - " +
                //"cc:" + cc + " - " + " - Error:" +
                //"02 - Error No Template", "-", "-", "", "0", true);

                return "02";
            }
        }
        catch (Exception ex)
        {
            //commonFunction.audit_trail("send email", "sendContactUsAutoReplyEmail", "", "", "-1", "00",
            //    "email:" + email + " - " +
            //    "senderContactUsEmail:" + AppSettings.senderContactUsEmail + " - " +
            //    "senderContactUsName:" + AppSettings.senderContactUsName + " - " +
            //    "emailSubject:" + emailSubject + " - " +
            //    "emailContent:" + emailContent + " - " +
            //    "utf8Content:" + utf8Content + " - " +
            //    "cc:" + cc + " - " + " - Error:" +
            //    ex.Message, "-", "-", "", "0",
            //           true);
            return "99";
        }
    }

}