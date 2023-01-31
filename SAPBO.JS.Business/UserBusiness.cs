using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SAPBO.JS.Common;
using SAPBO.JS.Model.Auth;
using SAPBO.JS.Model.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SAPBO.JS.Business
{
    public class UserBusiness : IUserBusiness
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly IEmployeeBusiness employeeRepository;
        private readonly IBusinessPartnerBusiness businessPartnerRepository;

        public UserBusiness(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IEmployeeBusiness employeeRepository,
            IBusinessPartnerBusiness businessPartnerRepository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.employeeRepository = employeeRepository;
            this.businessPartnerRepository = businessPartnerRepository;
        }

        public async Task<UserInfo> Login(UserInfo userInfo)
        {
            //GET USER BY BUSINESS
            if (string.IsNullOrEmpty(userInfo.Email))
            {
                var user = await userManager.Users.SingleOrDefaultAsync(x => x.BusinessPartnerId.Substring(2, 11).ToUpper().Equals(userInfo.BusinessPartnerId));
                if (user == null)
                    throw new Exception(AppMessages.UserNotFound);

                userInfo.Email = user.Email;
            }

            var result = await signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
                throw new Exception(AppMessages.InvalidLogin);

            userInfo = await GetUserByEmail(userInfo.Email);

            userInfo.Token = BuildToken(userInfo);

            if (userInfo.Roles.Exists(x => x.Name.Equals(RoleNames.Customer)) && !string.IsNullOrEmpty(userInfo.BusinessPartnerId))
            {
                userInfo.BusinessPartner = await businessPartnerRepository.GetAsync(userInfo.BusinessPartnerId);
            }
            else if (userInfo.Roles.Exists(x => x.Name.Equals(RoleNames.Carrier)) && !string.IsNullOrEmpty(userInfo.BusinessPartnerId))
            {
                userInfo.BusinessPartner = await businessPartnerRepository.GetCarrierAsync(userInfo.BusinessPartnerId);
            }
            else
            {
                userInfo.Employee = await employeeRepository.GetByEmailAsync(userInfo.Email, Enums.ObjectType.Full);
            }

            return userInfo;
        }

        public async Task<bool> CreateUser(UserInfo userInfo)
        {
            IdentityResult result = null;

            if (await userManager.FindByEmailAsync(userInfo.Email) != null)
                throw new Exception(AppMessages.UserAlreadyExists);

            result = await userManager.CreateAsync(new ApplicationUser
            {
                UserName = userInfo.Email,
                Email = userInfo.Email,
                BusinessPartnerId = userInfo.BusinessPartnerId,
                FirstName = "",
                LastName = ""
            }, userInfo.Password);

            if (!result.Succeeded)
                throw new Exception($"{AppMessages.CreateUserError} \nDetalle: {GetErrors(result)}");

            if (userInfo.Roles != null && userInfo.Roles.Any())
            {

            }

            return result.Succeeded;
        }

        public async Task<bool> CreateRole(RoleInfo roleInfo)
        {
            if (await roleManager.RoleExistsAsync(roleInfo.Name))
                throw new Exception(AppMessages.RoleAlreadyExists);

            var result = await roleManager.CreateAsync(new IdentityRole { Name = roleInfo.Name });
            if (!result.Succeeded)
                throw new Exception($"{AppMessages.UserNotFound} \nDetalle: {GetErrors(result)}");

            return result.Succeeded;
        }

        public async Task<UserInfo> GetUserByEmail(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                throw new Exception(AppMessages.UserNotFound);

            var result = new UserInfo
            {
                Email = user.Email,
                BusinessPartnerId = user.BusinessPartnerId
            };

            result.Roles = await GetRolesByUser(result.Email);
            return result;
        }

        public async Task<List<RoleInfo>> GetRolesByUser(string userEmail)
        {
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
                throw new Exception(AppMessages.UserNotFound);

            var roles = await userManager.GetRolesAsync(user);
            var result = new List<RoleInfo>();

            if (roles != null)
                foreach (var rol in roles)
                    result.Add(new RoleInfo { Name = rol });

            return result;
        }

        public async Task<List<UserInfo>> GetUsers(bool withRoles = false)
        {
            var users = await userManager.Users.ToListAsync();
            var result = new List<UserInfo>();

            if (users != null)
                foreach (var user in users)
                {
                    result.Add(new UserInfo
                    {
                        Email = user.Email
                    });
                }

            if (withRoles)
                foreach (var userInfo in result)
                    userInfo.Roles = await GetRolesByUser(userInfo.Email);

            return result;
        }

        public async Task<List<RoleInfo>> GetRoles()
        {
            var roles = await roleManager.Roles.ToListAsync();
            var result = new List<RoleInfo>();

            if (roles != null)
                foreach (var rol in roles)
                    result.Add(new RoleInfo { Name = rol.Name });

            return result;
        }

        public async Task<bool> SetRoleToUser(string userEmail, RoleInfo roleInfo)
        {
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
                throw new Exception(AppMessages.UserNotFound);

            if (!await roleManager.RoleExistsAsync(roleInfo.Name))
                throw new Exception(AppMessages.RoleNotFound);

            if (await userManager.IsInRoleAsync(user, roleInfo.Name))
                throw new Exception(AppMessages.RoleAlreadyAssignedToUser);

            var result = await userManager.AddToRoleAsync(user, roleInfo.Name);
            if (!result.Succeeded)
                throw new Exception($"{AppMessages.RoleCouldNotBeAssignedToUser} \nDetalle: {GetErrors(result)}");

            return result.Succeeded;
        }

        public async Task<bool> SetRolesToUser(string userEmail, List<RoleInfo> roles)
        {
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
                throw new Exception(AppMessages.UserNotFound);

            IdentityResult result = null;
            if (roles != null && roles.Any())
            {
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role.Name))
                        throw new Exception(AppMessages.RoleNotFound);

                    if (await userManager.IsInRoleAsync(user, role.Name))
                        throw new Exception(AppMessages.RoleAlreadyAssignedToUser);

                    result = await userManager.AddToRoleAsync(user, role.Name);
                    if (!result.Succeeded)
                        throw new Exception($"{AppMessages.RoleCouldNotBeAssignedToUser} \nDetalle: {GetErrors(result)}");
                }
            }

            return result.Succeeded;
        }

        public async Task<bool> RemoveRoleToUser(string userEmail, RoleInfo roleInfo)
        {
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
                throw new Exception(AppMessages.UserNotFound);

            if (!await roleManager.RoleExistsAsync(roleInfo.Name))
                throw new Exception(AppMessages.RoleNotFound);

            if (!await userManager.IsInRoleAsync(user, roleInfo.Name))
                throw new Exception(AppMessages.RoleIsNotAssignedToUser);

            var result = await userManager.RemoveFromRoleAsync(user, roleInfo.Name);
            if (!result.Succeeded)
                throw new Exception($"{AppMessages.RoleCouldNotBeRemovedFromUser} \nDetalle: {GetErrors(result)}");

            return result.Succeeded;
        }

        private UserToken BuildToken(UserInfo userInfo)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (userInfo.Roles != null)
                claims.AddRange(userInfo.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration[AppConfiguration.JwtKeyName]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                AppConfiguration.DomainName,
                AppConfiguration.DomainName,
                claims,
                expires: AppConfiguration.TokenExpiration,
                signingCredentials: creds);

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = AppConfiguration.TokenExpiration
            };
        }

        private string GetErrors(IdentityResult result)
        {
            var errors = string.Empty;
            foreach (var error in result.Errors)
                errors += $"{error} \n";

            return errors;
        }
    }
}
