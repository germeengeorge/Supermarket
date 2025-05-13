namespace Supermarket.Data.IRepository
{
    public interface IUnitOfWork 
    {
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        ICartRepository Carts { get; }
        IApplicationUserRepository ApplicationUser { get; }
        IFavourite Favorites { get; }

        IOrderDetailRepository OrderDetail { get; }
        IOrderHeaderRepository OrderHeader { get; }
        void Save ();
        Task SavaAsync();
    
    }
}
