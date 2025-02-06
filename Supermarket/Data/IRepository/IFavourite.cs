using Supermarket.Models;
using System.Linq.Expressions;

namespace Supermarket.Data.IRepository
{
    public interface IFavourite:IRepository<Favourite>
    {
        List<Favourite> FindAll(Expression<Func<Favourite, bool>> filter);
        Task<IEnumerable<Favourite>> FindAllAsync(Expression<Func<Favourite, bool>> filter, string includeProperties);
    }
}
