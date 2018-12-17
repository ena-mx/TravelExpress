namespace TravelExpress.Domain.Orders.Component
{
    using EnaBricks.Generics;
    using System;
    using System.Threading.Tasks;

    public abstract class OrderFactory
    {
        public abstract IOrder NewOrder(Guid customerId);
        public abstract Task<Option<IOrder>> OrderAsync(Guid orderId);
    }
}