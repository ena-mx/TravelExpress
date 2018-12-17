namespace TravelExpress.Queries.Customers
{
    using System;

    public class CustomerIndex
    {
        public Guid CustomerId { get; set; }
        public CustomerInfo CustomerInfo { get; set; }
        public CustomerOptions CustomerOptions { get; set; }
    }
}