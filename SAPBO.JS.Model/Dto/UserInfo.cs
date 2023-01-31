using SAPBO.JS.Model.Domain;

namespace SAPBO.JS.Model.Dto
{
    public class UserInfo
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public UserToken Token { get; set; }

        public List<RoleInfo> Roles { get; set; }

        public Employee Employee { get; set; }

        public string BusinessPartnerId { get; set; }

        public BusinessPartner BusinessPartner { get; set; }

        public ICollection<ShoppingCartItem> ShoppingCart { get; set; }
    }
}
