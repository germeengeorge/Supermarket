using Supermarket.Data.IRepository;
using Supermarket.Models;

namespace Supermarket.Data.Repository
{
    public class CategoryRepository:Repository<Category>,ICategoryRepository
    {
        private AppDbContext _db;
        public CategoryRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Category obj)
        {
            _db.categories.Update(obj);
        } 
    }
}
