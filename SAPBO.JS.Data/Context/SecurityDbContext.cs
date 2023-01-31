using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SAPBO.JS.Model.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Data.Context
{
    public class SecurityDbContext : IdentityDbContext<ApplicationUser>
    {
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options) : base(options)
        {

        }
    }
}
