using Supermarket.Models;

namespace Supermarket.Data.IRepository
{
    public interface IProductRepository:IRepository<Product>
    {
        void Update(Product obj);
        List<Product> FilterBy(int id);
     }
}
