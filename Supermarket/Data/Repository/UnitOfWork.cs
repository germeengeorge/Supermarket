using Microsoft.EntityFrameworkCore;
using Supermarket.Data.IRepository;

namespace Supermarket.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _db;
        public ICategoryRepository Categories { get; private set; }
        public IProductRepository Products { get; private set; }
        public ICartRepository Carts { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IFavourite Favorites { get; private set; }

        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            Carts = new CartRepository(_db);
            Categories = new CategoryRepository(_db);
            Products = new ProductRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            Favorites = new FavouriteRepository(_db);
            OrderHeader=new OrderHeaderRepository(_db);
            OrderDetail=new OrderDetailRepository(_db);

        }
        public void Save()
        {
            _db.SaveChanges();
        }

        public async Task SavaAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
