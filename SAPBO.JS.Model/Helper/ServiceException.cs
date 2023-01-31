using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPBO.JS.Model.Helper
{
    public class ServiceException
    {
        public bool Result => false;

        public string Message { get; set; }

        public string Description { get; set; }

        public string Function { get; set; }

        public DateTime Date => DateTime.Now;

        public string UserId { get; set; }
    }
}
