using System.Net.Mail;

namespace SAPBO.JS.Model.Dto
{
    public class AppEmail
    {
        public MailAddress From { get; set; }

        public List<MailAddress> To { get; set; }

        public List<MailAddress> Cc { get; set; }

        public List<MailAddress> Co { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
