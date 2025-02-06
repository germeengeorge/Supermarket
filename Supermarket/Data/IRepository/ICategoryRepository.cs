using Supermarket.Models;

namespace Supermarket.Data.IRepository
{
    public interface ICategoryRepository:IRepository<Category>
    {
        void Update(Category obj);
    }
}
