namespace TravelExpress.Domain.Orders
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public abstract class OrderStorage
    {
        public abstract Task<bool> CreateOrderHeaderAsync(Guid orderId, Guid customerId, Guid excursionId, DateTime serverDate);
        public abstract Task<bool> AddOrderDetailsAsync(Guid orderId, KeyValuePair<int, int>[] aditionalServices, KeyValuePair<int, int>[] orderDetails);
        public abstract Task<bool> AddBillingAsync(Guid orderId, Guid billId, Guid customerId);
        public abstract Task AddPaymentAsync(Guid orderId, Guid billId, decimal amount, DateTime serverDate);
        public abstract Task<bool> HasPaymentsAsync(Guid orderId);
        public abstract Task CancelOrderAsync(Guid orderId);
        public abstract Task<bool> IsPaidAsync(Guid orderId);
    }
}