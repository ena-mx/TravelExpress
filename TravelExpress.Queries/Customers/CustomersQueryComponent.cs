namespace TravelExpress.Queries.Customers
{
    using System;
    using System.Threading.Tasks;
    using TravelExpress.Queries.Common;

    public abstract class CustomersQueryComponent
    {
        public abstract Task<GenericPage<CustomerIndex>> CustomersAsync(int offset, int limit, string searchValue);

        public abstract Task<Customer> CustomerAsync(Guid customerId);
    }
}