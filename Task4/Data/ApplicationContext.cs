using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task4.Data.Entities;

namespace Task4.Data
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _config;

        public ApplicationContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config.GetConnectionString("ConnetctionString"));
        }
    }
}
