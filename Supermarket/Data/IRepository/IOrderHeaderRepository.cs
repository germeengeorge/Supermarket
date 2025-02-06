using Supermarket.Models;

namespace Supermarket.Data.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
        void UpdateStripePaymentID(int id, string sessionId, string paymentIntentId);
    }
}
