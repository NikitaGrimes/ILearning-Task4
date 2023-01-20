using Microsoft.AspNetCore.Identity;
using Task4.Data.Entities;

namespace Task4.Data
{
    public class ApplicationSeeder
    {
        private readonly ApplicationContext _context;

        public ApplicationSeeder(ApplicationContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();
        }
    }
}
