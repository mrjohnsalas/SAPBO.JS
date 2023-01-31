using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SAPBO.JS.Business;
using SAPBO.JS.Common;
using SAPBO.JS.Model.Dto;
using SAPBO.JS.Model.Helper;

namespace SAPBO.JS.WebApi.Controllers
{
    [Route(AppConfiguration.WebApiRoutePath)]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUserBusiness repository;
        private readonly ILogger<AccountsController> logger;

        public AccountsController(IUserBusiness repository, ILogger<AccountsController> logger)
        {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserInfo>> Login([FromBody] UserInfo userInfo)
        {
            try
            {
                return await repository.Login(userInfo);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = e.Message });
            }
        }

        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserInfo>> GetCurrentUser()
        {
            try
            {
                return await repository.GetUserByEmail(User.Identity.Name);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = e.Message });
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.Admin)]
        [HttpPost("CreateUser")]
        public async Task<ActionResult<bool>> CreateUser([FromBody] UserInfo userInfo)
        {
            try
            {
                return await repository.CreateUser(userInfo);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = e.Message });
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.Admin)]
        [HttpGet("GetUsers/{withRoles}")]
        public async Task<ActionResult<ICollection<UserInfo>>> GetUsers(bool withRoles)
        {
            try
            {
                return await repository.GetUsers(withRoles);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = e.Message });
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.Admin)]
        [HttpGet("GetUserByEmail/{email}")]
        public async Task<ActionResult<UserInfo>> GetUserByEmail(string email)
        {
            try
            {
                return await repository.GetUserByEmail(email);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = e.Message });
            }
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.Admin)]
        [HttpPost("CreateRole")]
        public async Task<ActionResult<bool>> CreateRole([FromBody] RoleInfo roleInfo)
        {
            try
            {
                return await repository.CreateRole(roleInfo);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = e.Message });
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.Admin)]
        [HttpGet("GetRoles")]
        public async Task<ActionResult<ICollection<RoleInfo>>> GetRoles()
        {
            try
            {
                return await repository.GetRoles();
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = e.Message });
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.Admin)]
        [HttpGet("GetRolesByUser/{userEmail}")]
        public async Task<ActionResult<ICollection<RoleInfo>>> GetRolesByUser(string userEmail)
        {
            try
            {
                return await repository.GetRolesByUser(userEmail);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = e.Message });
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.Admin)]
        [HttpPost("SetRoleToUser/{userEmail}")]
        public async Task<ActionResult<bool>> SetRoleToUser(string userEmail, [FromBody] RoleInfo roleInfo)
        {
            try
            {
                return await repository.SetRoleToUser(userEmail, roleInfo);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = e.Message });
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleNames.Admin)]
        [HttpPost("RemoveRoleToUser/{userEmail}")]
        public async Task<ActionResult<bool>> RemoveRoleToUser(string userEmail, [FromBody] RoleInfo roleInfo)
        {
            try
            {
                return await repository.RemoveRoleToUser(userEmail, roleInfo);
            }
            catch (Exception e)
            {
                return BadRequest(new ServiceException { Message = e.Message });
            }
        }
    }
}
