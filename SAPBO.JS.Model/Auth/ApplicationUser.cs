using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Model.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string BusinessPartnerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
