using Supermarket.Data.IRepository;
using Supermarket.Models;
using System.Linq.Expressions;

namespace Supermarket.Data.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>,IApplicationUserRepository
    {
        private AppDbContext _db;
        public ApplicationUserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }  
    }
}
  
