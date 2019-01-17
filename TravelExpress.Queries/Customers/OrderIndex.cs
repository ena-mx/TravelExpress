namespace TravelExpress.Queries.Customers
{
    using System;

    public class OrderIndex : OrderModel
    {
        public string ExcursionDescription { get; set; }
        public DateTime ExcursionBeginDate { get; set; }
        public DateTime ExcursionEndDate { get; set; }
        
    }
}