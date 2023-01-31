using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Common
{
    public class AppConfiguration
    {
        public const string SecurityConnectionName = "SecurityDbConnection";

        public const string JwtKeyName = "JWT:Key";

        public const string WebApiRoutePath = "api/[controller]";

        public const string DomainName = "grafipapel.com.pe";

        public static readonly DateTime TokenExpiration = DateTime.UtcNow.AddDays(15);

        public const string SmtpClientHost = "SmtpClientHost";
        public const string SmtpClientPort = "SmtpClientPort";
        public const string EmailUserId = "EmailUserId";
        public const string EmailPassword = "EmailPassword";
        public const string EmailFrom = "EmailFrom";
        public const string EmailName = "EmailName";
    }
}
