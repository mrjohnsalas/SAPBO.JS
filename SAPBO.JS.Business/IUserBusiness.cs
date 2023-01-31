using SAPBO.JS.Model.Dto;

namespace SAPBO.JS.Business
{
    public interface IUserBusiness
    {
        Task<UserInfo> Login(UserInfo userInfo);

        Task<bool> CreateUser(UserInfo userInfo);

        Task<bool> CreateRole(RoleInfo roleInfo);

        Task<UserInfo> GetUserByEmail(string email);

        Task<List<RoleInfo>> GetRolesByUser(string userEmail);

        Task<List<UserInfo>> GetUsers(bool withRoles = false);

        Task<List<RoleInfo>> GetRoles();

        Task<bool> SetRoleToUser(string userEmail, RoleInfo roleInfo);

        Task<bool> SetRolesToUser(string userEmail, List<RoleInfo> roles);

        Task<bool> RemoveRoleToUser(string userEmail, RoleInfo roleInfo);
    }
}
