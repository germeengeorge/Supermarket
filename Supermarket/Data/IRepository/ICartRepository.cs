using Supermarket.Models;

namespace Supermarket.Data.IRepository
{
    public interface ICartRepository:IRepository<ShoppingCart>
    {
        void Update(ShoppingCart obj);
        List<ShoppingCart> GetShoppingCartListByUserId(string UserId);
    }
}
