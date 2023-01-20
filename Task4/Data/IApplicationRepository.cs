using Task4.Data.Entities;

namespace Task4.Data
{
    public interface IApplicationRepository
    {
        IEnumerable<ApplicationUser> GetAllUsers();

        ApplicationUser GetUserByID(string id);

        ApplicationUser GetUserByUserName(string name);

        ApplicationUser GetUserByEmail(string email);

        bool AddUser(ApplicationUser user);

        bool IsUserExist(string userName);

        bool IsUserBlocked(string userName);

        bool UpdateLastLoginDateUser(string userName);

        bool UpdateBlockedStatus(string userName, bool status);

        bool DeleteUser(string userName);
    }
}
