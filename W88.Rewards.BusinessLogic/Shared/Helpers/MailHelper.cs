using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
using W88.BusinessLogic.Base.Helpers;
using W88.BusinessLogic.Shared.Helpers;
using W88.WebRef.RewardsServices;

namespace W88.Rewards.BusinessLogic.Shared.Helpers
{
    public class MailHelper : BaseHelper
    {
        public void SendMail(string memberCode, string redemptionId)
        {
            var recipientAddress = string.Empty;
            var senderAddress = ConfigurationManager.AppSettings.Get("email_from");
            var senderName = ConfigurationManager.AppSettings.Get("senderName");
            var bccAddress = ConfigurationManager.AppSettings.Get("email_bcc");
            var smtpAlternative = ConfigurationManager.AppSettings.Get("smtpAlternative");
            var isAlternative = false;
            var localResxMail = "~/redemption_mail.{0}.aspx";
            var language = string.Empty;

            using (var sClientMember = new RewardsServicesClient())
            {
                var dataSet = sClientMember.getMemberInfo(OperatorId.ToString(CultureInfo.InvariantCulture), memberCode);

                foreach (DataRow dataRow in dataSet.Tables[0].Rows)
                {
                    recipientAddress = dataRow["email"].ToString();
                    language = dataRow["languageCode"].ToString();
                }
            }

            var smtpClient = new System.Net.Mail.SmtpClient();
            var credentials = new System.Net.NetworkCredential();

            using (var message = new System.Net.Mail.MailMessage(senderAddress, recipientAddress))
            {
                string[] splitChar = { "|" };
                string[] results = smtpAlternative.Split(splitChar, StringSplitOptions.None);
                    
                foreach (var r in results)
                {
                    var mail = r;
                    if (recipientAddress.Contains(mail))
                    {
                        isAlternative = true;
                        break;
                    }
                }

                if (isAlternative)
                {
                    smtpClient.Port = int.Parse(ConfigurationManager.AppSettings.Get("mail_port"));
                    smtpClient.Host = ConfigurationManager.AppSettings.Get("mail_host");
                    credentials.UserName = ConfigurationManager.AppSettings.Get("mail_username");
                    credentials.Password = ConfigurationManager.AppSettings.Get("mail_password");

                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = credentials;
                }
                else
                {
                    message.Bcc.Add(new System.Net.Mail.MailAddress(bccAddress));
                }

                localResxMail = string.Format(localResxMail, string.IsNullOrEmpty(language) ? LanguageHelpers.SelectedLanguage : language);
                var subject = HttpContext.GetLocalResourceObject(localResxMail, "lbl_subject") == null ? string.Empty : (string)HttpContext.GetLocalResourceObject(localResxMail, "lbl_subject");
                var body = HttpContext.GetLocalResourceObject(localResxMail, "lbl_body") == null ? string.Empty : string.Format((string)HttpContext.GetLocalResourceObject(localResxMail, "lbl_body"), memberCode.Trim(), redemptionId);           
                message.From = new System.Net.Mail.MailAddress(senderAddress, senderName);
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                message.Subject = subject;
                message.Body = body;
                smtpClient.Send(message);
            }
        }
    }
}