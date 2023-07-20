using SAPBO.JS.Model.Dto;

namespace SAPBO.JS.Business
{
    public interface IContactMessageBusiness
    {
        Task SendContactMessageAsync(ContactMessage message);
    }
}
