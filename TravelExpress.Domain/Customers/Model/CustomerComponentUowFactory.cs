namespace TravelExpress.Domain.Customers.Model
{
    using EnaBricks.Generics;
    using System;
    using System.Threading.Tasks;

    public abstract class CustomerComponentUowFactory
    {
        public abstract ICustomerComponentUow NewCustomerUow();
        public abstract Task<Option<ICustomerComponentUow>> ExistingCustomerUowAsync(Guid customerId);
    }
}