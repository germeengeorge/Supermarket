using Microsoft.EntityFrameworkCore;
using Supermarket.Data.IRepository;
using Supermarket.Models;

namespace Supermarket.Data.Repository
{
    public class CartRepository : Repository<ShoppingCart>, ICartRepository
    {
        private AppDbContext _db;
        public CartRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public List<ShoppingCart> GetShoppingCartListByUserId(string UserId)
        {
            return _db.shoppingCarts.Where(x=>x.ApplicationUserId==UserId).Include(s=> s.Product).ToList();    
        }

        public void Update(ShoppingCart obj)
        {
            _db.shoppingCarts.Update(obj);
        }
    }
}
