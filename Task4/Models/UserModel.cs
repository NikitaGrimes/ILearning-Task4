using Task4.Data.Entities;

namespace Task4.Models
{
    public class UserModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime RegisterDate { get; set; }

        public DateTime LastLogInDate { get; set;}

        public bool IsBlocked { get; set; }

        public bool IsChecked { get; set; }

        public static implicit operator UserModel(ApplicationUser appUser)
        {
            return new UserModel
            {
                Id = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email,
                RegisterDate = appUser.RegisterDate,
                LastLogInDate = appUser.LastLogInDate,
                IsBlocked = appUser.IsBlocked,
                IsChecked = false
            };
        }
    }
}
