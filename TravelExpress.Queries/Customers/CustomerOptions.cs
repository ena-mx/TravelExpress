namespace TravelExpress.Queries.Customers
{
    using System;

    public class CustomerOptions
    {
        public Guid CustomerId { get; set; }
        public bool PhoneCallsEnabled { get; set; }
        public bool MailEnabled { get; set; }
    }
}
