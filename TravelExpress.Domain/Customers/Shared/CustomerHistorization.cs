namespace TravelExpress.Domain.Customers.Shared
{
    using System;
    using System.Threading.Tasks;
    using TravelExpress.Enums.Customers;

    public abstract class CustomerHistorization
    {
        public abstract Task AddCustomerActivity(Guid customerId, CustomerActivityType customerActivityType, DateTime date, Guid userId);
    }
}