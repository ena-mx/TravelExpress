namespace TravelExpress.Queries.Orders
{
    using System;
    using System.Threading.Tasks;

    public abstract class OrderModelQueryComponent
    {
        public abstract Task<OrderModel> OrderAsync(Guid orderId);
    }
}
