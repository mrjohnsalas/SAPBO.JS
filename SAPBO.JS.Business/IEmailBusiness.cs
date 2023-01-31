using SAPBO.JS.Model.Dto;

namespace SAPBO.JS.Business
{
    public interface IEmailBusiness
    {
        void SendEmailAsync(AppEmail obj);

        Task<AppEmail> GetByGroupIdAsync(string id);
    }
}
