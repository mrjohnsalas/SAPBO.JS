using SAPBO.JS.Common;
using SAPBO.JS.Data.Context;
using SAPBO.JS.Data.Mappers;
using SAPBO.JS.Data.Repositories;
using SAPBO.JS.Model.Domain;
using SAPBO.JS.Model.Dto;
using System.Net.Mail;

namespace SAPBO.JS.Business
{
    public class ContactMessageBusiness : SapB1GenericRepository<ContactMessage>, IContactMessageBusiness
    {
        private readonly IEmailBusiness _emailBusinessRepository;

        public ContactMessageBusiness(
            SapB1Context context,
            ISapB1AutoMapper<ContactMessage> mapper,
            IEmailBusiness emailBusinessRepository) : base(context, mapper)
        {
            _emailBusinessRepository = emailBusinessRepository;
        }

        private EmailAlertTemplateModel0 GetBody(ContactMessage message, Enums.ContactMessageType type)
        {
            var currentDate = DateTime.Now;
            var body = new EmailAlertTemplateModel0();
            body.HeadTitle = "NUEVO";
            body.HeadSecondLine = "MENSAJE";
            body.AlertImageLink = AppMessages.ContactMessageImageLink;
            switch (type)
            {
                case Enums.ContactMessageType.Internal:
                    //body.AlertImageLink = AppMessages.ContactMessageImageLink;
                    body.AlertTitle = AppMessages.ContactMessageInternalTitle;
                    body.AlertText = AppMessages.ContactMessageInternalText;
                    break;
                case Enums.ContactMessageType.External:
                    //body.AlertImageLink = AppMessages.ContactMessageImageLink;
                    body.AlertTitle = AppMessages.ContactMessageExternalTitle;
                    body.AlertText = AppMessages.ContactMessageExternalText;
                    break;
            }

            body.Heads = new List<EmailAlertTemplateModel0Data>
            {
                new EmailAlertTemplateModel0Data { Name = "FECHA:", Value = currentDate.ToString(AppFormats.Date) },
                new EmailAlertTemplateModel0Data { Name = "NOMBRES:", Value = message.FirstName },
                new EmailAlertTemplateModel0Data { Name = "APELLIDOS:", Value = message.LastName },
                new EmailAlertTemplateModel0Data { Name = "CORREO:", Value = message.Email },
                new EmailAlertTemplateModel0Data { Name = "TELEFONO:", Value = message.Phone }
            };

            body.FooterTitle = "Mensaje:";
            body.FooterText = message.Message;

            body.FooterBlocks = new List<EmailAlertTemplateModel0Block>
            {
                new EmailAlertTemplateModel0Block
                {
                    LeftBlock = new EmailAlertTemplateModel0Data
                    {
                        Name = "Dirección:",
                        Value = "Carretera Central Km 19.5,<br>Ñaña, Chaclacayo 08 Lima, Perú"
                    },
                    RightBlock = new EmailAlertTemplateModel0Data
                    {
                        Name = "Contacto:",
                        Value = $"+51 (998) 119 946<br>cotizaciones@grafipapel.com.pe"
                    }
                }
            };

            return body;
        }

        public async Task SendContactMessageAsync(ContactMessage message)
        {
            //var internalEmail = await _emailBusinessRepository.GetByGroupIdAsync("ADM001");
            var internalEmail = await _emailBusinessRepository.GetByGroupIdAsync("WCM001");
            internalEmail.Subject = AppMessages.ContactMessageInternalSubject;
            internalEmail.Body = EmailAlertTemplateUtilities.EmailAlertTemplateModel0Builder(GetBody(message, Enums.ContactMessageType.Internal));
            _emailBusinessRepository.SendEmailAsync(internalEmail);

            //var externalEmail = await _emailBusinessRepository.GetByGroupIdAsync("ADM002");
            var externalEmail = await _emailBusinessRepository.GetByGroupIdAsync("WAP002");
            externalEmail.Subject = string.Format(AppMessages.ContactMessageExternalSubject, message.FirstName);
            externalEmail.Body = EmailAlertTemplateUtilities.EmailAlertTemplateModel0Builder(GetBody(message, Enums.ContactMessageType.External));
            externalEmail.To.Add(new MailAddress(message.Email, message.Email));
            _emailBusinessRepository.SendEmailAsync(externalEmail);
        }
    }
}
