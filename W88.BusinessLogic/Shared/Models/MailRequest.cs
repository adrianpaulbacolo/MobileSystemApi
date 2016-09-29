using System.Text;

namespace W88.BusinessLogic.Shared.Models
{
    public class MailRequest
    {
        public string From = string.Empty;

        public string SenderName = string.Empty;

        public string To = string.Empty;

        public string[] BccAddresses = null;

        public string Subject = string.Empty;

        public bool IsBodyHtml = true;

        public string Body = string.Empty;
        
        public Encoding BodyEncoding = Encoding.UTF8;

        public int Port = 0;

        public string Host = string.Empty;

        public string Username = string.Empty;

        public string Password = string.Empty;

        public bool UseDefaultCredentials = false;
    }
}
