using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Model.App
{
    public class SmtpClientSetting
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public string EmailAddress { get; set; }

        public string DisplayName { get; set; }

        public string EmailFrom { get; set; }

        public string Password { get; set; }
    }
}
