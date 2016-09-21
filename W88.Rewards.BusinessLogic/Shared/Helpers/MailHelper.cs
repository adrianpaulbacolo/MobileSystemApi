using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
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
            var senderAddress = System.Configuration.ConfigurationManager.AppSettings.Get("email_from");
            var senderName = System.Configuration.ConfigurationManager.AppSettings.Get("senderName");
            var bccAddress = System.Configuration.ConfigurationManager.AppSettings.Get("email_bcc");
            var smtpAlternative = System.Configuration.ConfigurationManager.AppSettings.Get("smtpAlternative");
            var isAlternative = false;
            var localResxMail = "~/redemption_mail.{0}.aspx";
            var language = string.Empty;

            using (RewardsServicesClient sClientMember = new RewardsServicesClient())
            {
                System.Data.DataSet ds = sClientMember.getMemberInfo(base.OperatorId.ToString(CultureInfo.InvariantCulture), memberCode);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    recipientAddress = dr["email"].ToString();
                    language = dr["languageCode"].ToString();
                }
            }

            try
            {
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential();

                using (System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage(senderAddress, recipientAddress))
                {
                    string[] results;
                    string[] splitChar = { "|" };
                    results = smtpAlternative.Split(splitChar, StringSplitOptions.None);

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
                        smtpClient.Port = 25;
                        smtpClient.Host = "retail.smtp.com";

                        credentials.UserName = "dev@w88.com";
                        credentials.Password = "2NDbr0isFAT!";

                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = credentials;
                    }
                    else
                    {
                        message.Bcc.Add(new System.Net.Mail.MailAddress(bccAddress));
                    }

                    localResxMail = string.Format(localResxMail, string.IsNullOrEmpty(language) ? LanguageHelpers.SelectedLanguage : language);
                    message.From = new System.Net.Mail.MailAddress(senderAddress, senderName);
                    message.BodyEncoding = Encoding.UTF8;
                    message.IsBodyHtml = true;
                    message.Subject = (string)System.Web.HttpContext.GetLocalResourceObject(localResxMail, "lbl_subject");
                    message.Body = string.Format((string)System.Web.HttpContext.GetLocalResourceObject(localResxMail, "lbl_body"), memberCode.Trim(), redemptionId);
                    smtpClient.Send(message);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}