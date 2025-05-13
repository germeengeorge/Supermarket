using Supermarket.Data.IRepository;
using Supermarket.Models;

namespace Supermarket.Data.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private AppDbContext _db;
        public OrderDetailRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }



        public void Update(OrderDetail obj)
        {
            _db.OrderDetails.Update(obj);
        }
    }
}
