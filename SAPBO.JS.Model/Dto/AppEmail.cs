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

        public List<Attachment> Attachments { get; set; }
    }

    public class MailAddressDto
    {
        public string Address { get; set; }
        public string DisplayName { get; set; }
    }

    public class AppEmailDto
    {
        public List<MailAddressDto> To { get; set; }

        public List<MailAddressDto> Cc { get; set; }

        public List<MailAddressDto> Co { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
