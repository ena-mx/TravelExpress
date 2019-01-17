namespace TravelExpress.Queries.Customers
{
    using System;

    public class OrderModel
    {
        public Guid OrderId { get; set; }
        public int OrderStatusId { get; set; }
        public DateTime OrderCreatedDate { get; set; }
        public Guid ExcursionId { get; set; }
        public Guid BillId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalDebit { get; set; }
        public string CustomerName { get; set; }
    }
}