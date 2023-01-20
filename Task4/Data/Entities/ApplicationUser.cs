using Microsoft.AspNetCore.Identity;

namespace Task4.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime LastLogInDate { get; set; }

        public DateTime RegisterDate { get; set; }

        public bool IsBlocked { get; set; }

        public ApplicationUser()
        {

        }

        public ApplicationUser(string userName, string email)
        {
            UserName = userName;
            Email = email;
            RegisterDate = DateTime.Now;
            LastLogInDate = DateTime.Now;
            IsBlocked = false;
            Id = Guid.NewGuid().ToString();
        }
    }
}
