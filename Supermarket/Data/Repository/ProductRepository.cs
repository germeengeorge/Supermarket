using Supermarket.Data.IRepository;
using Supermarket.Models;

namespace Supermarket.Data.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private AppDbContext _db;
        public ProductRepository(AppDbContext db):base(db)
        {
            _db=db;
        }

        public List<Product> FilterBy(int id)
        {
             return _db.Products.Where(x=>x.CategoryId == id).ToList();
        }

        public void Update(Product obj)
        {
            _db.Products.Update(obj);
        }
    }
}
