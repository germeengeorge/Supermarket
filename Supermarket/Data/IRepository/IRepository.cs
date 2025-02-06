using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Supermarket.Data.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Remove(T entity);
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task<T> AddAsync(T entity);
        Task<T> RemoveAsync(T entity);
        void RemoveRange(IEnumerable<T> entity);


    }
}
