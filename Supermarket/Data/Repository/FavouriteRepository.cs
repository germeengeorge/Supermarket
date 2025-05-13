using Microsoft.EntityFrameworkCore;
using Supermarket.Data.IRepository;
using Supermarket.Models;
using System.Linq;
using System.Linq.Expressions;

namespace Supermarket.Data.Repository
{
    public class FavouriteRepository:Repository<Favourite>,IFavourite
    {
        private AppDbContext _db;
        public FavouriteRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public List<Favourite> FindAll(Expression<Func<Favourite, bool>> filter)
        {
            IQueryable<Favourite> query = dbSet;
            query = query.Where(filter);
            return query.ToList();
        }

        //public async Task<IEnumerable<Favourite>> FindAllAsync(Expression<Func<Favourite, bool>> filter)
        //{
        //    IQueryable<Favourite> query = dbSet;
        //    query = query.Where(filter);
        //    return await query.ToListAsync();
        //}

        public async Task<IEnumerable<Favourite>> FindAllAsync(Expression<Func<Favourite, bool>> filter, string includeProperties)
        {
            IQueryable<Favourite> query = dbSet;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property.Trim());
                }
            }

            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();
        }
    }
}
