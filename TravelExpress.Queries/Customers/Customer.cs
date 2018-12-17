namespace TravelExpress.Queries.Customers
{
    using System;

    public class Customer
    {
        public Guid CustomerId { get; set; }
        public CustomerInfo CustomerInfo { get; set; }
        public CustomerOptions CustomerOptions { get; set; }
        public CustomerHistorization[] Historization { get; set; }
        public CustomerPhoneCall[] Phonecalls { get; set; }
        public CustomerObservation[] Observations { get; set; }
    }
}