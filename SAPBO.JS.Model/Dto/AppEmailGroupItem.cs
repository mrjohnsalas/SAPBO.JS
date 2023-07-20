using SAPBO.JS.Common;
using System.Net.Mail;

namespace SAPBO.JS.Model.Dto
{
    public class AppEmailGroupItem
    {
        public int Id { get; set; }

        public string AppEmailGroupId { get; set; }

        public Enums.EmailToType EmailToType { get; set; }

        public string EmailAddress { get; set; }
    }
}
