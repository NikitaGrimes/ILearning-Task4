using Task4.Data.Entities;

namespace Task4.Data
{
    public class ApplicaionRepository : IApplicationRepository
    {
        private readonly ApplicationContext _context;

        public ApplicaionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public ApplicationUser GetUserByID(string id) 
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public ApplicationUser GetUserByUserName(string name)
        {
            return _context.Users.FirstOrDefault(x => x.UserName == name);
        }

        public ApplicationUser GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email);
        }

        public bool AddUser(ApplicationUser user)
        {
            _context.Users.Add(user);
            return SaveAll();
        }

        public bool UpdateLastLoginDateUser(string userName)
        {
            ApplicationUser user = GetUserByUserName(userName);
            user.LastLogInDate = DateTime.Now;
            return SaveAll();
        }

        public bool UpdateBlockedStatus(string userName, bool status)
        {
            ApplicationUser user = GetUserByUserName(userName);
            user.IsBlocked = status;
            return SaveAll();
        }

        public bool DeleteUser(string userName) 
        {
            ApplicationUser user = GetUserByUserName(userName);
            _context.Remove(user);
            return SaveAll();
        }

        public bool IsUserExist(string userName)
        {
            ApplicationUser user = GetUserByUserName(userName);
            if (user == null)
                return false;

            return true;
        }

        public bool IsUserBlocked(string userName)
        {
            ApplicationUser user = GetUserByUserName(userName);
            if (user == null)
                return true;

            return user.IsBlocked;
        }

        private bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
