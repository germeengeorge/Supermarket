using Supermarket.Models;

namespace Supermarket.ViewModel
{
    public class ShoppingCartVM
    {
        public List<ShoppingCart> ShoppingCartList { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
