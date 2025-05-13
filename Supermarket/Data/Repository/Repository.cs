using Microsoft.EntityFrameworkCore;
using Supermarket.Data.IRepository;
using System.Linq.Expressions;
using System.Linq;

namespace Supermarket.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(AppDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
            //_db.Categories == dbSet
            _db.Products.Include(u => u.Category).Include(u => u.CategoryId);
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _db.Set<T>().AddAsync(entity);
            return entity;
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query=dbSet;
           query = query.Where(filter);
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            
            return query.ToList();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);
            return await query.FirstOrDefaultAsync();
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public async Task<T> RemoveAsync(T entity)
        {
            _db.Set<T>().Remove(entity);    
            return entity;
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }

    }
}
