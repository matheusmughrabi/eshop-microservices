using eShop.IdentityServer.DataAccess.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace eShop.IdentityServer.DataAccess
{
    public class IdentityServerDbContext : IdentityDbContext<ApplicationUserEntity>
    {
        public IdentityServerDbContext(DbContextOptions<IdentityServerDbContext> options) : base(options)   
        {
        }
    }
}
