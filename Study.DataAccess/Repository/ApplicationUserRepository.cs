using Study.DataAccess.Repository.IRepository;
using Study.Models;
namespace Study.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public string Get(string UserName)
        {
            var User = _db.ApplicationUsers.FirstOrDefault(w => w.UserName == UserName);

            return User.Name;
        }
    }
}
