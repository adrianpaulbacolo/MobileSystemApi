using System;
using W88.BusinessLogic.Shared.Models;
using W88.Utilities.Log.Helpers;

namespace W88.BusinessLogic.Shared.Helpers
{
    public class MailHelper
    {
        public static void SendMail(MailRequest mailRequest)
        {
            try
            {
                using (var message = new System.Net.Mail.MailMessage(mailRequest.From, mailRequest.To))
                {
                    var smtpClient = new System.Net.Mail.SmtpClient();
                    smtpClient.UseDefaultCredentials = mailRequest.UseDefaultCredentials;
                    if (!smtpClient.UseDefaultCredentials)
                    {
                        smtpClient.Port = mailRequest.Port;
                        smtpClient.Host = mailRequest.Host;

                        var credentials = new System.Net.NetworkCredential();
                        credentials.UserName = mailRequest.Username;
                        credentials.Password = mailRequest.Password;
                        smtpClient.Credentials = credentials;
                    }

                    if (mailRequest.BccAddresses != null)
                    {
                        foreach (var address in mailRequest.BccAddresses)
                        {
                            message.Bcc.Add(address);
                        }
                    }

                    message.From = new System.Net.Mail.MailAddress(mailRequest.From, mailRequest.SenderName);
                    message.BodyEncoding = mailRequest.BodyEncoding;
                    message.IsBodyHtml = mailRequest.IsBodyHtml;
                    message.Subject = mailRequest.Subject;
                    message.Body = mailRequest.Body;
                    smtpClient.Send(message);
                }
            }
            catch (Exception exception)
            {
                AuditTrail.AppendLog(mailRequest.To, string.Empty, "SendMail", "MailHelper", string.Empty, string.Empty, string.Empty,
                    exception.Message, Convert.ToString(Guid.NewGuid()), string.Empty, string.Empty, true);
            }
        }
    }
}
