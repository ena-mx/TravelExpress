namespace TravelExpress.Domain.Orders
{
    using System;
    using System.Threading.Tasks;
    using TravelExpress.Enums.Orders;

    public abstract class OrderHistorization
    {
        public abstract Task AddOrderHistorizationAsync(Guid orderId, OrderStatus orderStatus, DateTime serverDate, Guid userId);
        public abstract Task AddBillingHistorizationAsync(Guid billId, BillActivityType billActivityType, DateTime serverDate, Guid userId);
    }
}