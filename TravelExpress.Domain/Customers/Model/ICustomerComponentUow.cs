namespace TravelExpress.Domain.Customers.Model
{
    public interface ICustomerComponentUow: ICustomerComponent
    {
        void CommitChanges();
        void RollbackChanges();
    }
}
