using Microsoft.AspNetCore.Identity;

namespace eShop.IdentityServer.DataAccess.Entity
{
    public class ApplicationUserEntity : IdentityUser
    {
        private string FirstName { get; set; }
        private string LastName { get; set; }
    }
}
