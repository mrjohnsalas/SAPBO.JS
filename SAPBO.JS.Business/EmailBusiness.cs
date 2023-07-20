using Microsoft.Extensions.Options;
using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.App;
using SAPBO.JS.Model.Dto;
using System.Net.Mail;
using System.Net;

namespace SAPBO.JS.Business
{
    public class EmailBusiness : SapB1GenericRepository<AppEmailGroupItem>, IEmailBusiness
    {
        private readonly IOptions<SmtpClientSetting> _smtpClientSetting;

        public EmailBusiness(SapB1Context context, ISapB1AutoMapper<AppEmailGroupItem> mapper, IOptions<SmtpClientSetting> smtpClientSetting) : base(context, mapper)
        {
            _smtpClientSetting = smtpClientSetting;
        }

        private List<MailAddress> ConvertToMailAddress(IEnumerable<AppEmailGroupItem> emailItems)
        {
            var emails = new List<MailAddress>();

            if (emailItems != null && emailItems.Any())
                foreach (var item in emailItems)
                    emails.Add(new MailAddress(item.EmailAddress));

            return emails;
        }

        public async Task<AppEmail> GetByGroupIdAsync(string id)
        {
            var emailitems = await GetAllAsync("GP_WEB_APP_054", new List<dynamic> { id });

            var appEmail = new AppEmail();

            if (emailitems != null && emailitems.Any())
            {
                var from = emailitems.FirstOrDefault(x => x.EmailToType == Enums.EmailToType.Fr);
                if (from != null)
                    appEmail.From = new MailAddress(from.EmailAddress);

                appEmail.To = ConvertToMailAddress(emailitems.Where(x => x.EmailToType == Enums.EmailToType.To));
                appEmail.Cc = ConvertToMailAddress(emailitems.Where(x => x.EmailToType == Enums.EmailToType.Cc));
                appEmail.Co = ConvertToMailAddress(emailitems.Where(x => x.EmailToType == Enums.EmailToType.Co));
            }

            return appEmail;
        }

        public void SendEmailAsync(AppEmail obj)
        {
            var smtp = new SmtpClient
            {
                Host = _smtpClientSetting.Value.Host,
                Port = _smtpClientSetting.Value.Port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpClientSetting.Value.EmailAddress, _smtpClientSetting.Value.Password)
            };

            var message = new MailMessage
            {
                From = obj.From ?? new MailAddress(_smtpClientSetting.Value.EmailFrom, _smtpClientSetting.Value.DisplayName),
                Subject = obj.Subject,
                Body = obj.Body,
                IsBodyHtml = true
            };

            if (obj.To != null && obj.To.Any())
                foreach (var address in obj.To)
                    message.To.Add(address);

            if (obj.Cc != null && obj.Cc.Any())
                foreach (var address in obj.Cc)
                    message.CC.Add(address);

            if (obj.Co != null && obj.Co.Any())
                foreach (var address in obj.Co)
                    message.Bcc.Add(address);

            if (obj.Attachments != null && obj.Attachments.Any())
                foreach (var attachment in obj.Attachments)
                    message.Attachments.Add(attachment);

            smtp.Send(message);
        }
    }
}
