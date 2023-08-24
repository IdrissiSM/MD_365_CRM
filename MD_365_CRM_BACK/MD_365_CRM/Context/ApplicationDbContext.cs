using MD_365_CRM.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MD_365_CRM.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<Otp> Otps { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Opportunity> Opportunities { get; set; }
        public DbSet<ProductOpportunity> ProductOpportunities { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<BlacklistedUser> BlacklistedUsers { get; set; }

    }
}
